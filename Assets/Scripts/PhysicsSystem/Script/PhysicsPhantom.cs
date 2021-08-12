using System;
using System.Collections;
using UnityEngine;

namespace PhysicsRT
{
  [SLua.CustomLuaClass]
    public enum PhysicsPhantomType {
        Aabb
    }
    [AddComponentMenu("PhysicsRT/Physics Phantom")]
    [DefaultExecutionOrder(250)]
    [DisallowMultipleComponent]
    [SLua.CustomLuaClass]
    public class PhysicsPhantom : MonoBehaviour
    {
        [SerializeField]
        private PhysicsPhantomType m_Type = PhysicsPhantomType.Aabb;
        [SerializeField]
        [Tooltip("The minimum boundary of the AABB (local)")]
        private Vector3 m_Min = Vector3.zero;
        [SerializeField]
        [Tooltip("The maximum boundary of the AABB (local)")]
        private Vector3 m_Max = Vector3.one;
        [SerializeField]
        private int m_Layer = 0;
        [Tooltip("是否添加 Listener，只有开启了才可使用OnCollision事件")]
        [SerializeField]
        private bool m_EnableListener = false;
        [Tooltip("在 Awake 时不自动创建幻影，设置为 false 后您需要手动调用 ForceReCreate 来创建")]
        [SerializeField]
        private bool m_DoNotAutoCreateAtAwake = false;
        private IntPtr ptr = IntPtr.Zero;

        /// <summary>
        /// 在 Awake 时不自动创建幻影，设置为 false 后您需要手动调用 ForceReCreate 来创建
        /// </summary>
        public bool DoNotAutoCreateAtAwake { get => m_DoNotAutoCreateAtAwake; set { m_DoNotAutoCreateAtAwake = value; } }
        /// <summary>
        /// 获取或设置幻影碰撞层
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
        public PhysicsPhantomType Type {
            get => m_Type; 
            set => m_Type = value;
        }
        /// <summary>
        /// 是否添加ContactListener，只有添加了ContactListener才可使用OnCollision事件（创建刚体后设置无效）
        /// </summary>
        public bool EnableListener { 
            get => m_EnableListener; 
            set { 
                if(ptr != IntPtr.Zero)
                    throw new Exception("Body is created, do not modify this after creation");
                m_EnableListener = value; 
            } 
        }
        /// <summary>
        /// 获取幻影包围盒最小坐标。创建以后设置请使用 SetAabb 方法
        /// </summary>
        /// <value></value>
        public Vector3 Min { get => m_Min; set => m_Max = value; }
        /// <summary>
        /// 获取幻影包围盒最大坐标。创建以后设置请使用 SetAabb 方法
        /// </summary>
        /// <value></value>
        public Vector3 Max { get => m_Max; set => m_Max = value; }

        /// <summary>
        /// ID
        /// </summary>
        /// <value></value>
        public int Id { get; private set; }

        public PhysicsBody prev { get; set; }
        public PhysicsBody next { get; set; }

        /// <summary>
        /// 重新创建幻影
        /// </summary>
        public void ForceReCreate()
        {
            Destroy();
            Create();
        }

        private void Awake() {
            CurrentPhysicsWorld = PhysicsWorld.GetCurrentScensePhysicsWorld();
            StartCoroutine(LateCreate());
        }
        private void OnDestroy() {
            Destroy();
        }
        private IEnumerator LateCreate() {
            yield return new WaitForSeconds(0.05f); 
            if(!m_DoNotAutoCreateAtAwake)
                Create();
        }

        private PhysicsWorld CurrentPhysicsWorld = null;

        public IntPtr GetPtr() { return ptr; }
        /// <summary>
        /// 更新当前幻影的Aabb包围盒（相对）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void SetAabb(Vector3 min, Vector3 max) {
            m_Max = max;
            m_Min = min;
            if(ptr != IntPtr.Zero) 
                PhysicsApi.API.SetAabbPhantomMinMax(ptr, transform.TransformPoint(m_Min), transform.TransformPoint(m_Max));
        }
        /// <summary>
        /// 获取当前幻影相交的刚体
        /// </summary>
        /// <returns></returns>
        public PhysicsBody[] GetOverlappingBodies() {
            if(ptr != IntPtr.Zero) {
                var outLen = 0;
                var rs = PhysicsApi.API.GetAabbPhantomOverlappingCollidables(ptr, ref outLen);
                
                PhysicsBody[] result = new PhysicsBody[outLen];
                for (int i = 0; i < rs.Length; i++)
                    result[i] = CurrentPhysicsWorld.GetBodyById(rs[i]);
                return result;
            }
            throw new Exception("Phantom not create!");
        }

        private void Create() {
            if(ptr != IntPtr.Zero) {
                return;
            }
            if(CurrentPhysicsWorld == null) {
                Debug.LogWarning("Not found PhysicsWorld in this scense, please add it before use PhysicsBody.");
                return;
            }

            ptr = PhysicsApi.API.CreateAabbPhantom(
                CurrentPhysicsWorld.GetPtr(),
                transform.TransformPoint(m_Min), transform.TransformPoint(m_Max),
                m_EnableListener,
                m_Layer);


            Id = PhysicsApi.API.GetPhantomId(ptr);
            CurrentPhysicsWorld.AddPhantom(Id, this);
        }
        private void Destroy() {
            if(CurrentPhysicsWorld == null || ptr == IntPtr.Zero)
                return;

            CurrentPhysicsWorld.RemovePhantom(this);
            PhysicsApi.API.DestroyPhantom(ptr); 
            ptr = IntPtr.Zero;
        }

        [SLua.CustomLuaClass]
        public delegate void OnPhantomOverlappingCollidableCallback(PhysicsPhantom self, PhysicsBody other);

        /// <summary>
        /// 刚体进入幻影时的事件
        /// </summary>
        public OnPhantomOverlappingCollidableCallback onOverlappingCollidableAdd;
        /// <summary>
        /// 刚体离开幻影时的事件
        /// </summary>
        public OnPhantomOverlappingCollidableCallback onOverlappingCollidableRemove;

        public void OnPhantomOverlapCallback(PhysicsBody sbodyOther, int ty) {
            if(ty == 1) onOverlappingCollidableAdd?.Invoke(this, sbodyOther);
            else onOverlappingCollidableRemove?.Invoke(this, sbodyOther);
        }
    
    
        private Vector3 oldPosition = Vector3.zero;
        private Vector3 oldMin = Vector3.zero;
        private Vector3 oldMax = Vector3.zero;

        [SLua.DoNotToLua]
        public void BackUpRuntimeCanModifieProperties() {
            oldPosition = transform.position;
            oldMin = m_Min;
            oldMax = m_Max;
        }
        [SLua.DoNotToLua]
        public void ApplyModifiedProperties() {
            if(oldPosition != transform.position || oldMin != m_Min || oldMax != m_Max) {
                BackUpRuntimeCanModifieProperties();
                SetAabb(m_Min, m_Max);
            }
        }
    }
}