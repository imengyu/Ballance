
using PhysicsRT.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsRT
{ 
    [SLua.CustomLuaClass]
    public enum MotionType {
        /// <summary>
        /// A fully-simulated, movable rigid body. At construction time the engine checks
        /// the input inertia and selects MOTION_SPHERE_INERTIA or MOTION_BOX_INERTIA as
        /// appropriate.
        /// </summary>
        Dynamic,
        /// <summary>
        /// Simulation is performed using a sphere inertia tensor. (A multiple of the
        /// Identity matrix). The highest value of the diagonal of the rigid body's
        /// inertia tensor is used as the spherical inertia.
        /// </summary>
        SphereInertia,
        /// <summary>
        /// Simulation is performed using a box inertia tensor. The non-diagonal elements
        /// of the inertia tensor are set to zero. This is slower than the
        /// MOTION_SPHERE_INERTIA motions, however it can produce more accurate results,
        /// especially for long thin objects.
        /// </summary>
        BoxInertia,
        /// <summary>
        /// Simulation is not performed as a normal rigid body. During a simulation step,
        /// the velocity of the rigid body is used to calculate the new position of the
        /// rigid body, however the velocity is NOT updated. The user can keyframe a rigid
        /// body by setting the velocity of the rigid body to produce the desired keyframe
        /// positions. The hkpKeyFrameUtility class can be used to simply apply keyframes
        /// in this way. The velocity of a keyframed rigid body is NOT changed by the
        /// application of impulses or forces. The keyframed rigid body has an infinite
        /// mass when viewed by the rest of the system.
        /// </summary>
        Keyframed,
        /// <summary>
        /// This motion type is used for the static elements of a game scene, e.g., the
        /// landscape. Fixed rigid bodies are treated in a special way by the system. They
        /// have the same effect as a rigid body with a motion of type MOTION_KEYFRAMED
        /// and velocity 0, however they are much faster to use, incurring no simulation
        /// overhead, except in collision with moving bodies.
        /// </summary>
        Fixed,

        /// <summary>
        /// A box inertia motion which is optimized for thin boxes and has less stability problems
        /// </summary>
        ThinBoxInertia,
        /// <summary>
        /// A specialized motion used for character controllers
        /// </summary>
        Character,
    }
    [SLua.CustomLuaClass]
    public enum CollidableQualityType
    {    
        Default = -1,
        /// Use this for fixed bodies.
        Fixed = 0,
        /// Use this for moving objects with infinite mass.
        Keyframed,
        /// Use this for all your debris objects.
        Debris,
        /// Use this for debris objects that should have simplified TOI collisions with fixed/landscape objects.
        DebrisSimpleTOI,
        /// Use this for moving bodies, which should not leave the world,
        /// but you rather prefer those objects to tunnel through the world than
        /// dropping frames because the engine .
        Moving,
        /// Use this for all objects, which you cannot afford to tunnel through
        /// the world at all.
        Critical,
        /// Use this for very fast objects.
        Bullet,
        /// For user. If you want to use this, you have to modify hkpCollisionDispatcher::initCollisionQualityInfo()
        User,
        /// Use this for rigid body character controllers.
        Character,
        /// Use this for moving objects with infinite mass which should report contact points and TOI-collisions against all other bodies, including other fixed and keyframed bodies.
        ///
        /// Note that only non-TOI contact points are reported in collisions against debris-quality objects.
        KeyframedReporting,
    };

    [AddComponentMenu("PhysicsRT/Physics Body")]
    [DefaultExecutionOrder(250)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PhysicsShape))]
    [SLua.CustomLuaClass]
    public class PhysicsBody : MonoBehaviour, LinkedListItem<PhysicsBody>
    {
        const float MinimumMass = 0.001f;
    
        private int Convert(MotionType m) {
            switch(m) {
                case MotionType.Dynamic: return 1;
                case MotionType.SphereInertia: return 2;
                case MotionType.BoxInertia: return 3;
                case MotionType.Keyframed: return 4;
                case MotionType.Fixed: return 5;
                case MotionType.ThinBoxInertia: return 6;
                case MotionType.Character: return 7;
            }
            return 0;
        }

        [SerializeField]
        private MotionType m_MotionType = MotionType.Fixed;
        [SerializeField]
        [Tooltip("The quality type, used to specify when to use continuous physics.")]
        private CollidableQualityType m_CollidableQualityType = CollidableQualityType.Default;
        [SerializeField]
        private float m_Mass = 1.0f;
        [SerializeField]
        [Tooltip("这适用于物体的线速度，随时间减小。")]
        private float m_LinearDamping = 0.0f;
        [SerializeField]
        [Tooltip("这适用于物体的角速度，随时间减小角速度。")]
        private float m_AngularDamping = 0.05f;
        [SerializeField]
        [Tooltip("物体在世界空间中的初始线速度")]
        private Vector3 m_InitialLinearVelocity = Vector3.zero;
        [SerializeField]
        [Tooltip("这表示在身体的局部运动空间（即围绕质心）中围绕每个轴的初始旋转速度")]
        private Vector3 m_InitialAngularVelocity = Vector3.zero;
        [SerializeField]
        [Tooltip("此实体的重力量缩放系数。")]
        private float m_GravityFactor = 1f;
        [SerializeField]
        private Vector3 m_CenterOfMass;
        [SerializeField]
        private CustomPhysicsBodyTags m_CustomTags = CustomPhysicsBodyTags.Nothing;
        [SerializeField]
        [Tooltip("使用此值可以指定刚体的初始摩擦力值。实体的“摩擦力”值指示其表面有多光滑，从而指示它沿其他实体滑动的容易程度。一般摩擦力值的范围在0和1之间，但可以更高（最大值为255）。默认值为0.5。")]
        private float m_Friction = 0.5f;
        [Range(0, 1.99f)]
        [Tooltip("这表明物体有多“弹性”——换句话说，物体与物体碰撞后有多少能量。值为1表示对象在碰撞后恢复其所有能量，值为0表示对象将完全停止移动。默认值为0.4。恢复的实现只是一个粗略的近似值，因此您可能希望在游戏中使用不同的值进行实验，以获得所需的效果。")]
        [SerializeField]
        private float m_Restitution = 0.4f;
        [SerializeField]
        private int m_Layer = -1;
        [SerializeField]
        private Matrix4x4 m_InertiaTensor = Matrix4x4.identity;
        [SerializeField]
        [Tooltip("The maximum angular velocity of the body (in rad/s).")]
        private float m_MaxAngularVelocity = 200;
        [SerializeField]
        [Tooltip("The maximum linear velocity of the body (in m/s).")]
        private float m_MaxLinearVelocity = 200;
        [SerializeField]
        [Tooltip("Is this body is Tigger ?")]
        private bool m_IsTigger = false;
        [Tooltip("是否添加ContactListener，只有添加了ContactListener才可使用OnCollision事件")]
        [SerializeField]
        private bool m_AddContactListener = false;
        [Tooltip("在 Awake 时不自动创建刚体，设置为 false 后您需要手动调用 ForceReCreateShape 来创建刚体")]
        [SerializeField]
        private bool m_DoNotAutoCreateAtAwake = false;
        [Tooltip("自动计算 CenterOfMass ")]
        [SerializeField]
        private bool m_AutoComputeCenterOfMass = true;
        [Tooltip("是否在gameObject激活时自动切换刚体的激活状态")]
        [SerializeField]
        private bool m_AutoControlActive = true;
        [Tooltip("指定当前碰撞组的名称，为空则不设置。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效")]
        [SerializeField]
        private string m_SystemGroupName = "";
        [Tooltip("指定当前碰撞子组的ID，同一个碰撞组中子组的ID不能重复，默认为0。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效")]
        [SerializeField]
        private int m_SubSystemId = 0;
        [Tooltip("指定当前碰撞组不与那个子组碰撞，默认为0。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效")]
        [SerializeField]
        private int m_SubSystemDontCollideWith = 0;

        private IntPtr ptr = IntPtr.Zero;

        public string SystemGroupName  { get => m_SystemGroupName; set { m_SystemGroupName = value; } }
        public int SubSystemId  { get => m_SubSystemId; set { m_SubSystemId = value; } }
        public int SubSystemDontCollideWith  { get => m_SubSystemDontCollideWith; set { m_SubSystemDontCollideWith = value; } }

        /// <summary>
        /// 是否在gameObject激活时自动切换刚体的激活状态
        /// </summary>
        public bool AutoControlActive { get => m_AutoControlActive; set { m_AutoControlActive = value; } }
        /// <summary>
        /// 在 Awake 时不自动创建刚体，设置为 false 后您需要手动调用 ForceReCreateShape 来创建刚体
        /// </summary>
        public bool DoNotAutoCreateAtAwake { get => m_DoNotAutoCreateAtAwake; set { m_DoNotAutoCreateAtAwake = value; } }
        /// <summary>
        /// 自动计算CenterOfMass（创建刚体后设置无效）
        /// </summary>
        public bool AutoComputeCenterOfMass { get => m_AutoComputeCenterOfMass; set { m_AutoComputeCenterOfMass = value; } }
        /// <summary>
        /// 获取或设置刚体碰撞层
        /// </summary>
        public int Layer {
            get => m_Layer; 
            set {
                if(m_Layer != value) {
                    m_Layer = value; 
                    if(ptr != IntPtr.Zero)
                        PhysicsApi.API.SetRigidBodyLayer(ptr, m_Layer);
                }
            }
        }  
        /// <summary>
        /// 获取或设置刚体的类型
        /// </summary>
        public MotionType MotionType {
            get => m_MotionType; 
            set {
                if(m_MotionType != value) {
                    m_MotionType = value; 
                    if(ptr != IntPtr.Zero)
                        PhysicsApi.API.SetRigidBodyMotionType(ptr, (int)Convert(m_MotionType));
                }
            }
        }
        /// <summary>
        /// 获取或设置刚体的碰撞质量（该值在创建刚体之后不能更改）
        /// </summary>
        public CollidableQualityType CollidableQualityType {
            get => m_CollidableQualityType; 
            set => m_CollidableQualityType = value;
        }
        /// <summary>
        /// 获取或设置刚体的初始初始旋转速度（该值在创建刚体之后不能更改）
        /// </summary>
        public Vector3 InitialAngularVelocity
        {
            get => m_InitialAngularVelocity;
            set => m_InitialAngularVelocity = value;
        }
        /// <summary>
        /// 获取或设置刚体的初始线速度（该值在创建刚体之后不能更改）
        /// </summary>
        public Vector3 InitialLinearVelocity
        {
            get => m_InitialLinearVelocity;
            set => m_InitialLinearVelocity = value;
        }
        /// <summary>
        /// 获取或设置刚体的质量
        /// </summary>
        public float Mass {
            get => m_Mass; 
            set {
                if(m_Mass != value) {
                    m_Mass = value; 
                    if(ptr != IntPtr.Zero)
                        PhysicsApi.API.SetRigidBodyMass(ptr, value);
                }
            }
        }
        /// <summary>
        /// 获取或设置刚体的自定义标签
        /// </summary>
        public CustomPhysicsBodyTags CustomTags { get => m_CustomTags; set { m_CustomTags = value; } }
        /// <summary>
        /// 获取或设置刚体是否是触发器，触发器才可以接受碰撞事件（创建刚体后设置无效）
        /// </summary>
        public bool IsTigger { get => m_IsTigger; set { m_IsTigger = value; } }
        /// <summary>
        /// 是否添加ContactListener，只有添加了ContactListener才可使用OnCollision事件（创建刚体后设置无效）
        /// </summary>
        public bool AddContactListener { 
            get => m_AddContactListener; 
            set { 
                if(ptr != IntPtr.Zero)
                    throw new Exception("Body is created, do not modify this after creation");
                m_AddContactListener = value; 
            } 
        }
        /// <summary>
        /// 获取或设置刚体的质心
        /// </summary>
        public Vector3 CenterOfMass {
            get => m_CenterOfMass; 
            set {
                if(m_CenterOfMass != value) {
                    m_CenterOfMass = value; 
                    if(ptr != IntPtr.Zero) {
                        PhysicsApi.API.SetRigidBodyCenterOfMass(ptr, value);
                    }
                }
            }
        }
        /// <summary>
        /// 获取或设置刚体的惯性张量
        /// </summary>
        public Matrix4x4 InertiaTensor {
            get => m_InertiaTensor; 
            set {
                if(m_InertiaTensor != value) {
                    m_InertiaTensor = value; 
                    if(ptr != IntPtr.Zero) 
                        PhysicsApi.API.SetRigidBodyInertiaTensor(ptr, value);
                }
            }
        }
        /// <summary>
        /// 获取或设置刚体的重力系数
        /// </summary>
        public float GravityFactor {
            get => m_GravityFactor; 
            set {
                if(m_GravityFactor != value) {
                    m_GravityFactor = value; 
                    if(ptr != IntPtr.Zero)
                        PhysicsApi.API.SetRigidBodyGravityFactor(ptr, m_GravityFactor);
                }
            }
        }
        /// <summary>
        /// 获取或设置刚体的线速度
        /// </summary>
        public float LinearDamping {
            get => m_LinearDamping; 
            set {
                if(m_LinearDamping != value) {
                    m_LinearDamping = value; 
                    if(ptr != IntPtr.Zero)
                        PhysicsApi.API.SetRigidBodyLinearDampin(ptr, m_LinearDamping);
                }
            }
        }
        /// <summary>
        /// 获取或设置刚体的角速度
        /// </summary>
        public float AngularDamping {
            get => m_AngularDamping; 
            set {
                if(m_AngularDamping != value) {
                    m_AngularDamping = value; 
                    if(ptr != IntPtr.Zero)
                        PhysicsApi.API.SetRigidBodyAngularDamping(ptr, m_AngularDamping);
                }
            }
        }
        /// <summary>
        /// 获取或设置刚体的摩擦力
        /// </summary>
        public float Friction {
            get => m_Friction; 
            set {
                if(m_Friction != value) {
                    m_Friction = value;
                    if(ptr != IntPtr.Zero)
                        PhysicsApi.API.SetRigidBodyFriction(ptr, m_Friction);
                }
            }
        }
        /// <summary>
        /// 获取或设置刚体的弹力
        /// </summary>
        public float Restitution {
            get => m_Restitution; 
            set {
                if(m_Restitution != value) {
                    m_Restitution = value;
                    if(ptr != IntPtr.Zero)
                        PhysicsApi.API.SetRigidBodyRestitution(ptr, m_Restitution);
                }
            }
        }
        /// <summary>
        /// ID
        /// </summary>
        /// <value></value>
        public int Id { get; private set; }

        [SLua.DoNotToLua]
        public PhysicsBody prev { get; set; }
        [SLua.DoNotToLua]
        public PhysicsBody next { get; set; }

        /// <summary>
        /// 重新创建物理刚体，这将导致当前物理状态丢失
        /// </summary>
        public void ForceReCreateShape()
        {
            nextCreateForce = true;
            DestroyBody();
            CreateBody();
        }

        private void Awake() {
            CurrentPhysicsWorld = PhysicsWorld.GetCurrentScensePhysicsWorld();
            StartCoroutine(LateCreate());
        }
        private void OnDestroy() {
            if(ptr != IntPtr.Zero) 
                DestroyBody();
        }
        private IEnumerator LateCreate() {
            yield return new WaitForSeconds(0.05f); 
            if(!m_DoNotAutoCreateAtAwake)
                CreateBody();
        }

        /// <summary>
        /// 强制激活刚体
        /// </summary>
        public void ForceActive() {
            if(ptr != IntPtr.Zero)
               PhysicsApi.API.ActiveRigidBody(ptr);
        }
        /// <summary>
        /// 强制设置刚体非激活态（不设置GameObject）
        /// </summary>
        public void ForceDeactive() {
            if(ptr != IntPtr.Zero) 
               PhysicsApi.API.DeactiveRigidBody(ptr);
        }
        /// <summary>
        /// 强制从物理世界中创建刚体
        /// </summary>
        public void ForcePhysics() {
            if(ptr == IntPtr.Zero)
                CreateBody();
        }
        /// <summary>
        /// 强制从物理世界中移除刚体
        /// </summary>
        public void ForceDePhysics() {
            if(ptr != IntPtr.Zero) 
               DestroyBody(false);
        }
        /// <summary>
        /// 检查当前刚体是否创建
        /// </summary>
        /// <returns></returns>
        public bool IsPhysicsed() {
            return (ptr != IntPtr.Zero);
        }
        /// <summary>
        /// 强制更新刚体的碰撞信息
        /// </summary>
        public void ForceUpdateCollisionFilterInfo() {
            if(ptr != IntPtr.Zero) 
                PhysicsApi.API.SetRigidBodyCollisionFilterInfo(
                    ptr,
                    m_Layer,
                    CurrentPhysicsWorld.GetSystemGroup(m_SystemGroupName),
                    m_SubSystemId,
                    m_SubSystemDontCollideWith);
        }

        private void OnEnable()
        {
            if(m_AutoControlActive) ForceActive();
        }
        private void OnDisable() 
        {
            if(m_AutoControlActive) ForceDeactive();
        }
        private void OnValidate()
        {
            Mass = Mathf.Max(MinimumMass, Mass);
            m_LinearDamping = Mathf.Max(m_LinearDamping, 0f);
            m_AngularDamping = Mathf.Max(m_AngularDamping, 0f);
        }

        private PhysicsWorld CurrentPhysicsWorld = null;
        private IntPtr currentShapeMassProperties = IntPtr.Zero;
        private IntPtr shapeBodyPtr = IntPtr.Zero;
        private bool nextCreateForce = false;

        public IntPtr GetPtr() { return ptr; }
        private IntPtr GetShapeBody() {
            if(shapeBodyPtr == IntPtr.Zero) {
                var shape = GetComponent<PhysicsShape>();
                if(shape == null)
                {
                    Debug.LogWarning("Not found PhysicsShape on this gameObject, physical function has been disabled.");
                    return IntPtr.Zero;
                }

                shapeBodyPtr = shape.GetShapeBody(nextCreateForce, m_Layer);
                if(CenterOfMass != Vector3.zero)
                    currentShapeMassProperties = shape.ComputeMassProperties(m_Mass);
            }
            return shapeBodyPtr;
        }
        private void ReleaseShapeBody() {
            var shape = GetComponent<PhysicsShape>();
            if(shape != null)
                shape.ReleaseShapeBody();
            shapeBodyPtr = IntPtr.Zero;
        }
        private void CreateBody() {
            if(ptr != IntPtr.Zero) {
                return;
            }
            if(CurrentPhysicsWorld == null) {
                Debug.LogWarning("Not found PhysicsWorld in this scense, please add it before use PhysicsBody.");
                return;
            }

            ptr = PhysicsApi.API.CreateRigidBody(
                CurrentPhysicsWorld.GetPtr(),
                GetShapeBody(), 
                transform.position, 
                transform.rotation,
                gameObject.name,
                Convert(m_MotionType),
                (int)m_CollidableQualityType,
                m_Friction,
                m_Restitution,
                m_Mass, 
                PhysicsApi.API.BoolToInt(gameObject.activeSelf), 
                m_Layer,
                CurrentPhysicsWorld.GetSystemGroup(m_SystemGroupName),
                m_SubSystemId,
                m_SubSystemDontCollideWith,
                m_IsTigger,
                m_AddContactListener,
                m_GravityFactor,
                m_LinearDamping,
                m_AngularDamping,
                m_CenterOfMass,
                m_InertiaTensor,
                m_InitialLinearVelocity,
                m_InitialAngularVelocity,
                m_MaxLinearVelocity,
                m_MaxAngularVelocity,
                currentShapeMassProperties);

            TryCreateSpring();
            TryCreateConstant();

            Id = PhysicsApi.API.GetRigidBodyId(ptr);
            CurrentPhysicsWorld.AddBody(Id, this);
            
            ReApplyForce();

            nextCreateForce = false;
        }
        private void DestroyBody(bool destroyShape = true) {
            if(CurrentPhysicsWorld == null || ptr == IntPtr.Zero)
                return;

            if(currentShapeMassProperties != IntPtr.Zero)
            {
                PhysicsApi.API.CommonDelete(currentShapeMassProperties);
                currentShapeMassProperties = IntPtr.Zero;
            }

            TryDestroyConstant();
            TryDestroySpring();

            if(destroyShape)
                ReleaseShapeBody();

            CurrentPhysicsWorld.RemoveBody(this);
            PhysicsApi.API.DestroyRigidBody(ptr);
            ptr = IntPtr.Zero;
        }

        //创建当前刚体的约束
        private void TryCreateConstant() {
            var constants = GetComponents<PhysicsConstraint>();
            for(int i = 0; i < constants.Length; i++) 
                constants[i].TryCreate();
            foreach(PhysicsConstraint c in pendingCreateConstant)
                c.TryCreate();
            pendingCreateConstant.Clear();
        }
        private void TryDestroyConstant() {
            var constants = GetComponents<PhysicsConstraint>();
            for(int i = 0; i < constants.Length; i++) 
                constants[i].Destroy();
        }
        
        private void TryCreateSpring() {
            var constants = GetComponents<PhysicsSpring>();
            for(int i = 0; i < constants.Length; i++) 
                if(!constants[i].DoNotAutoCreateAtAwake)
                    constants[i].Create();
        }
        private void TryDestroySpring() {
            var constants = GetComponents<PhysicsSpring>();
            for(int i = 0; i < constants.Length; i++) 
                constants[i].Destroy();
        }

        private List<PhysicsConstraint> pendingCreateConstant = new List<PhysicsConstraint>();
        internal void AddPendingCreateConstant(PhysicsConstraint c) { pendingCreateConstant.Add(c); }

        private MotionType oldMotionType = MotionType.Fixed;
        private float oldMass = 0;
        private float oldLinearDamping =  0;
        private float oldAngularDamping = 0;
        private float oldGravityFactor =  0;
        private Vector3 oldCenterOfMass = Vector3.zero;
        private float oldFriction =  0;
        private float oldRestitution = 0;
        private int oldLayer = 0;
        private Vector3 oldPosition = Vector3.zero;
        private Quaternion oldRotation = Quaternion.identity;

        [SLua.DoNotToLua]
        public void BackUpRuntimeCanModifieProperties() {
            oldMotionType = m_MotionType;
            oldMass = m_Mass;
            oldLinearDamping = m_LinearDamping;
            oldAngularDamping = m_AngularDamping;
            oldGravityFactor = m_GravityFactor;
            oldCenterOfMass = m_CenterOfMass;
            oldFriction = m_Friction;
            oldRestitution = m_Restitution;
            oldLayer = m_Layer;
            oldPosition = transform.position;
            oldRotation = transform.rotation;
        }
        [SLua.DoNotToLua]
        public void ApplyModifiedProperties() {
            if(oldMotionType != m_MotionType) {
                var newVal = m_MotionType; m_MotionType = oldMotionType;
                MotionType = newVal;
            }
            if(oldMass != m_Mass) {
                var newVal = m_Mass; m_Mass = oldMass;
                Mass = newVal;
            }
            if(oldLinearDamping != m_LinearDamping) {
                var newVal = m_LinearDamping; m_LinearDamping = oldLinearDamping;
                LinearDamping = newVal;
            }
            if(oldAngularDamping != m_AngularDamping) {
                var newVal = m_AngularDamping; m_AngularDamping = oldAngularDamping;
                AngularDamping = newVal;
            }
            if(oldGravityFactor != m_GravityFactor) {
                var newVal = m_GravityFactor; m_GravityFactor = oldGravityFactor;
                GravityFactor = newVal;
            }
            if(oldCenterOfMass != m_CenterOfMass) {
                var newVal = m_CenterOfMass; m_CenterOfMass = oldCenterOfMass;
                CenterOfMass = newVal;
            }
            if(oldFriction != m_Friction) {
                var newVal = m_Friction; m_Friction = oldFriction;
                Friction = newVal;
            }
            if(oldRestitution != m_Restitution) {
                var newVal = m_Restitution; m_Restitution = oldRestitution;
                Restitution = newVal;
            }
            if(oldLayer != m_Layer) {
                var newVal = m_Layer; m_Layer = oldLayer;
                Layer = newVal;
            }
            if(oldRotation != transform.rotation)
                UpdateTransformToPhysicsEngine();
            else if(oldPosition != transform.position)
                UpdatePositionToPhysicsEngine();
        }
        
        /// <summary>
        /// 同步位置和旋转至物理引擎
        /// </summary>
        public void UpdateTransformToPhysicsEngine() {
             if(ptr != IntPtr.Zero)
                PhysicsApi.API.SetRigidBodyPositionAndRotation(ptr, transform.position, transform.rotation);
        }
        /// <summary>
        /// 同步位置
        /// </summary>
        public void UpdatePositionToPhysicsEngine() {
            if(ptr != IntPtr.Zero)
                PhysicsApi.API.SetRigidBodyPosition(ptr, transform.position);
        }

        /// <summary>
        /// The velocity of the rigidbody at the point worldPoint in global space.
        /// </summary>
        /// <param name="worldPoint"></param>
        /// <returns></returns>
        public Vector3 GetPointVelocity(Vector3 worldPoint) {
            if(ptr == IntPtr.Zero) throw new PhysicsBodyNotCreateException();
            
            PhysicsApi.API.GetRigidBodyPointVelocity(ptr, worldPoint, out var v);
            return v;
        }
        /// <summary>
        /// The velocity relative to the rigidbody at the point relativePoint.
        /// </summary>
        /// <param name="relativePoint"></param>
        /// <returns></returns>
        public Vector3 GetRelativePointVelocity(Vector3 relativePoint) {
            return GetPointVelocity(transform.position + relativePoint);
        }
        /// <summary>
        /// 获取或设置刚体角速度
        /// </summary>
        /// <value></value>
        public Vector3 AngularVelocity {
            get {
                if(ptr == IntPtr.Zero)
                    return InitialAngularVelocity;
                else {
                    PhysicsApi.API.GetRigidBodyAngularVelocity(ptr, out var v);
                    return v;
                }
            }
            set {
                if(ptr == IntPtr.Zero) throw new PhysicsBodyNotCreateException();
                PhysicsApi.API.SetRigidBodyAngularVelocity(ptr, value);
            }
        }
        /// <summary>
        /// 获取或设置刚体线速度
        /// </summary>
        /// <value></value>
        public Vector3 LinearVelocity {
            get {
                if(ptr == IntPtr.Zero)
                    return InitialLinearVelocity;
                else {
                    PhysicsApi.API.GetRigidBodyLinearVelocity(ptr, out var v);
                    return v;
                }
            }
            set {
                if(ptr == IntPtr.Zero) throw new PhysicsBodyNotCreateException();
                PhysicsApi.API.SetRigidBodyLinearVelocity(ptr, value);
            }
        }
        /// <summary>
        /// The maximum angular velocity of the body (in rad/s).
        /// </summary>
        /// <value></value>
        public float MaxAngularVelocity {
            get => m_MaxAngularVelocity;
            set {
                if(ptr == IntPtr.Zero) throw new PhysicsBodyNotCreateException();
                m_MaxAngularVelocity = value;
                PhysicsApi.API.SetRigidBodyMaxAngularVelocity(ptr, value);
            }
        }
        /// <summary>
        /// The maximum linear velocity of the body (in rad/s).
        /// </summary>
        /// <value></value>
        public float MaxLinearVelocity {
            get => m_MaxLinearVelocity;
            set {
                if(ptr == IntPtr.Zero) throw new PhysicsBodyNotCreateException();
                m_MaxLinearVelocity = value;
                PhysicsApi.API.SetRigidBodyMaxLinearVelocity(ptr, value);
            }
        }
    
        //暂时存储刚体还未创建时用户设置的力，创建后统一设置
        private enum StartTemForceType {
           Force,
           ForceAtPoint,
           Torque,
           LinearImpulse,
           PointImpulse,
           AngularImpulse
        }
        private class StartTemForce {
            public StartTemForceType type;
            public Vector3 force;
            public Vector3 point;
            public StartTemForce(StartTemForceType type, Vector3 force, Vector3 point) {
                this.type = type;
                this.force = force;
                this.point = point;
            }

        }
        private List<StartTemForce> fTemp = new List<StartTemForce>();
        private void ReApplyForce() {
            if(ptr != IntPtr.Zero) {
                fTemp.ForEach((a) => {
                    switch(a.type) {
                        case StartTemForceType.Force: 
                            ApplyForce(a.force);
                            break;
                        case StartTemForceType.ForceAtPoint: 
                            ApplyForceAtPoint(a.force, a.point);
                            break;
                        case StartTemForceType.Torque: 
                            ApplyTorque(a.force);
                            break;
                        case StartTemForceType.LinearImpulse: 
                            ApplyForce(a.force);
                            break;
                        case StartTemForceType.PointImpulse: 
                            ApplyPointImpulse(a.force, a.point);
                            break;
                        case StartTemForceType.AngularImpulse: 
                            ApplyAngularImpulse(a.force);
                            break;
                    }
                });
                fTemp.Clear();
            }
        }

        /// <summary>
        /// Applies a force to the rigid body. The force is applied to the center of mass.
        /// </summary>
        /// <param name="force"></param>
        public void ApplyForce(Vector3 force) {
            if(ptr != IntPtr.Zero)
                PhysicsApi.API.RigidBodyApplyForce(ptr, Time.deltaTime, force);
            else
                fTemp.Add(new StartTemForce(StartTemForceType.Force, force, Vector3.zero));
        }
        /// <summary>
        /// Applies a force (in world space) to the rigid body at the point p in world space.
        /// </summary>
        /// <param name="force"></param>
        /// <param name="point"></param>
        public void ApplyForceAtPoint(Vector3 force, Vector3 point) {
            if(ptr != IntPtr.Zero)
                PhysicsApi.API.RigidBodyApplyForceAtPoint(ptr, Time.deltaTime, force, point);
            else
                fTemp.Add(new StartTemForce(StartTemForceType.ForceAtPoint, force, point));
        }
        /// <summary>
        /// Applies the specified torque (in world space) to the rigid body.
        /// Specify the torque as an Vector3. The direction of the vector indicates the axis (in world space) that you want the body to rotate around, and the magnitude of the vector indicates the strength of the force applied. The change in the body's angular velocity after torques are applied is proportional to the simulation delta time value and inversely proportional to the body's  inertia. 
        /// </summary>
        /// <param name="torque"></param>
        public void ApplyTorque(Vector3 torque) {
            if(ptr != IntPtr.Zero)
                PhysicsApi.API.RigidBodyApplyTorque(ptr, Time.deltaTime, torque);
            else
                fTemp.Add(new StartTemForce(StartTemForceType.Torque, torque, Vector3.zero));
        }
        /// <summary>
        /// Applies an impulse (in world space) to the center of mass.
        /// </summary>
        /// <param name="imp"></param>
        public void ApplyLinearImpulse(Vector3 imp) {
            if(ptr != IntPtr.Zero)
                PhysicsApi.API.RigidBodyApplyLinearImpulse(ptr, imp);
            else
                fTemp.Add(new StartTemForce(StartTemForceType.LinearImpulse, imp, Vector3.zero));
        }
        /// <summary>
        /// Apply an impulse at the point p in world space.
        /// </summary>
        /// <param name="imp"></param>
        /// <param name="point"></param>
        public void ApplyPointImpulse(Vector3 imp, Vector3 point) {
            if(ptr != IntPtr.Zero)
                PhysicsApi.API.RigidBodyApplyPointImpulse(ptr, imp, point);
            else
                fTemp.Add(new StartTemForce(StartTemForceType.PointImpulse, imp, point));
        }
        /// <summary>
        /// Apply an instantaneous change in angular velocity around center of mass.
        /// </summary>
        /// <param name="imp"></param>
        public void ApplyAngularImpulse(Vector3 imp) {
            if(ptr != IntPtr.Zero)
                PhysicsApi.API.RigidBodyApplyAngularImpulse(ptr, imp);
            else
                fTemp.Add(new StartTemForce(StartTemForceType.AngularImpulse, imp, Vector3.zero));
        }

        [SLua.CustomLuaClass]
        public delegate void OnBodyTiggerCollCallback(PhysicsBody body, PhysicsBody other);
        [SLua.CustomLuaClass]
        public delegate void OnBodyCollisionCallback(PhysicsBody body, PhysicsBody other, PhysicsBodyCollisionInfo info);
        [SLua.CustomLuaClass]
        public delegate void OnBodyCollisionLeaveCallback(PhysicsBody body, PhysicsBody other);

        /// <summary>
        /// Tigger中刚体进入时的事件（为Tigger时有效）
        /// </summary>
        public OnBodyTiggerCollCallback onTiggerEnter;
        /// <summary>
        /// Tigger中刚体离开时的事件（为Tigger时有效）
        /// </summary>
        public OnBodyTiggerCollCallback onTiggerLeave;

        /// <summary>
        /// 刚体碰撞进入时的事件（AddContactListener为true时有效）
        /// </summary>
        public OnBodyCollisionCallback onCollisionEnter;
        /// <summary>
        /// 刚体碰撞离开时的事件（AddContactListener为true时有效）
        /// </summary>
        public OnBodyCollisionLeaveCallback onCollisionLeave;
        /// <summary>
        /// 刚体碰撞离开时的事件（AddContactListener为true时有效）
        /// </summary>
        public OnBodyCollisionCallback onCollisionStay;

        private enum PhysicsBodyContactDataState {
            NEW_ADD,
            TWICE_ADD,
            END,
        }
        private class PhysicsBodyContactData : LinkedListItem<PhysicsBodyContactData> {
            public PhysicsBody body;
            public sPhysicsBodyContactData data;
            public PhysicsBodyContactDataState state;
            public bool entered = false;
            public bool needLeave = false;
            public PhysicsBodyContactData(PhysicsBody body, sPhysicsBodyContactData data) {
                this.body = body;
                this.data = data;
                state = PhysicsBodyContactDataState.NEW_ADD;
            }

            public PhysicsBodyContactData prev { get; set; }
            public PhysicsBodyContactData next { get; set; }
        }
        private Dictionary<int, PhysicsBodyContactData> currentFramEnterBodies = new Dictionary<int, PhysicsBodyContactData>();
        private SimpleLinkedList<PhysicsBodyContactData> currentFramEnterBodiesList = new SimpleLinkedList<PhysicsBodyContactData>();

        internal void FlushPhysicsBodyContactDataTick() {
            List<int> needRemoveData = new List<int>();
            PhysicsBodyContactData d = currentFramEnterBodiesList.getBegin();
            while(d != null) {
                switch(d.state) {
                    case PhysicsBodyContactDataState.NEW_ADD: 
                        onCollisionEnter?.Invoke(this, d.body, new PhysicsBodyCollisionInfo(d.data));
                        d.state = d.needLeave ? PhysicsBodyContactDataState.END : PhysicsBodyContactDataState.TWICE_ADD;
                        d.entered = true;
                        break;
                    case PhysicsBodyContactDataState.TWICE_ADD: 
                        onCollisionStay?.Invoke(this, d.body, new PhysicsBodyCollisionInfo(d.data));
                        break;
                    case PhysicsBodyContactDataState.END: 
                        onCollisionLeave?.Invoke(this, d.body);
                        currentFramEnterBodies.Remove(d.body.Id);
                        currentFramEnterBodiesList.remove(d);
                        break;
                }
                d = d.next;
            }
        }      
        internal void OnBodyPointContactCallback(PhysicsBody other, sPhysicsBodyContactData data) {
            if(currentFramEnterBodies.TryGetValue(other.Id, out var d)) {
                if(data.isRemoved == 1) {
                    if(d.entered) d.state = PhysicsBodyContactDataState.END;
                    else d.needLeave = true;
                } else {
                    d.data = data;
                    d.state = PhysicsBodyContactDataState.TWICE_ADD;
                }
            } else {
                d = new PhysicsBodyContactData(other, data);
                currentFramEnterBodies.Add(other.Id, d);
                currentFramEnterBodiesList.add(d);
            }
        }
    }
    
    [SLua.CustomLuaClass]
    public class PhysicsBodyCollisionInfo {
        public float distance { get; private set; }
        public Vector3 position { get; private set; }
        public Vector3 normal { get; private set; }
        public Vector3 separatingNormal { get; private set; }
        public float separatingVelocity { get; private set; }

        internal PhysicsBodyCollisionInfo(sPhysicsBodyContactData data) {
            distance = data.distance;
            separatingVelocity = data.separatingVelocity;
            position = new Vector3(data.pos[0], data.pos[1], data.pos[2]);
            separatingNormal = new Vector3(data.separatingNormal[0], data.separatingNormal[1], data.separatingNormal[2]);
            normal = new Vector3(data.normal[0], data.normal[1], data.normal[2]);
        }
    }
    public class PhysicsBodyNotCreateException : Exception {
        public PhysicsBodyNotCreateException() : base("Body is not created yet.") {}
    }
}