using System;
using System.Collections.Generic;
using UnityEngine;
using BallancePhysics.Api;
using System.Runtime.InteropServices;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [AddComponentMenu("BallancePhysics/PhysicsObject")]
  [DefaultExecutionOrder(20)]
  [DisallowMultipleComponent]
  /// <summary>
  /// Ballance 物理物体组件
  /// </summary>
  public class PhysicsObject : MonoBehaviour
  {
    #region 属性

    [Tooltip("物体的质量（kg）")]
    [SerializeField]
    private float m_Mass = 1.0f;
    [Tooltip("物体的m摩擦力。0表示完全没有摩擦力。0.1=冰。3=非常粗糙。为每个物理对象定义了摩擦力。因此，物体A和物体B之间的滑动运动将取决于两者的摩擦力。对于实际摩擦力，该值必须与其他对象的摩擦系数相乘。")]
    [SerializeField]
    [Range(0, 3f)]
    private float m_Friction = 0.7f;
    [Tooltip("物体的弹力。>1.0意味着永不停止的跳跃。<0.2模拟需要额外的CPU。注意不要给弹性赋予不切实际的值：过高的值（如10）会使模拟在一段时间后变得非常不稳定。")]
    [SerializeField]
    [Range(0, 2f)]
    private float m_Elasticity = 0.4f;
    [Tooltip("0.0表示线性速度（对象的平移）不受影响，1.0表示每秒减少30%，0.1表示90%，等等，可以用作空气阻力。过低：对象可能会不切实际地加速。")]
    [SerializeField]
    private float m_LinearSpeedDamping = 0.1f;
    [Tooltip("0.0表示旋转速度（物体的旋转）不受影响，1.0表示每秒减少30%，0.1表示90%，等等，可以用作某种空气阻力。过低：对象可能会不切实际地加速。")]
    [SerializeField]
    private float m_RotSpeedDamping = 0.1f;
    [Tooltip("球体碰撞器的半径。")]
    [SerializeField]
    private float m_BallRadius = 1;
    [Tooltip("是否使用球体碰撞器。")]
    [SerializeField]
    private bool m_UseBall = false;
    [Tooltip("如果为true，该算法将在对象周围生成一个额外的凸包。对于可移动对象，此外壳可以将性能提高到与凸面对象相同的水平。对于巨大的静态物体，这种优化毫无意义，因为所有可移动的对象都一直在穿透凸面外壳。")]
    [SerializeField]
    private bool m_EnableConvexHull = true;
    [Tooltip("设置当前物体是否启用碰撞。")]
    [SerializeField]
    private bool m_EnableCollision = true;
    [Tooltip("如果为true，则对象不会受到重力的影响，直到某个事件将其唤醒（如与另一个对象的碰撞、脉冲或连接到其上的弹簧的创建）。例如，当您需要物理化地板上已经存在的许多对象，并且不希望物理引擎通过计算所有需要的碰撞来稳定对象，从而减慢合成速度时，这非常有用。")]
    [SerializeField]
    private bool m_StartFrozen = false;
    [Tooltip("如果为true，该对象将被视为不可移动（重力和力不会对其产生任何影响）。")]
    [SerializeField]
    private bool m_Fixed = false;
    [Tooltip("物体的质心")]
    [SerializeField]
    private Vector3 m_ShiftMassCenter = Vector3.zero;

    [Tooltip("在 Awake 时不自动创建刚体")]
    [SerializeField]
    private bool m_DoNotAutoCreateAtAwake = true;
    [Tooltip("自动计算 CenterOfMass ")]
    [SerializeField]
    private bool m_AutoMassCenter = true;
    [Tooltip("是否在gameObject激活时自动切换刚体的激活状态")]
    [SerializeField]
    private bool m_AutoControlActive = true;
    [Tooltip("是否启用施加在这个物体上的恒力")]
    [SerializeField]
    private bool m_EnableConstantForce = true;
    [Tooltip("物体的碰撞层")]
    [SerializeField]
    private int m_Layer = -1;
    [Tooltip("指定当前碰撞组的名称，为空则不设置。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效")]
    [SerializeField]
    private string m_SystemGroupName = "";
    [Tooltip("指定当前碰撞子组的ID，同一个碰撞组中子组的ID不能重复，默认为0。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效")]
    [SerializeField]
    private int m_SubSystemId = 0;
    [Tooltip("指定当前碰撞组不与那个子组碰撞，默认为0。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效")]
    [SerializeField]
    private int m_SubSystemDontCollideWith = 0;
    [Tooltip("指定当前碰撞组ID, 用于碰撞事件的判断")]
    [SerializeField]
    private int m_CollisionID = 0;

    [Tooltip("指定此物体的凸体网格")]
    [SerializeField]
    private List<Mesh> m_Convex = new List<Mesh>();
    [Tooltip("指定此物体的凹体网格，凹体网格将会销毁更多计算时间，所以推荐使用多个拆分的凸体构成一个凹体")]
    [SerializeField]
    private List<Mesh> m_Concave = new List<Mesh>();
    [Tooltip("当前物体物理网格生成的碰撞层名称，名称不能重复，否则当前碰撞层名称将不能复用")]
    [SerializeField]
    private string m_SurfaceName = "";
    [Tooltip("当前物体物理网格是否使用已存在的碰撞层，如果为true，则不创建碰撞层，而是使用已有的碰撞层")]
    [SerializeField]
    private bool m_UseExistsSurface = false;
    [SerializeField]
    private float m_ExtraRadius = 0.0f;
    [Tooltip("设置当前物体是否启用重力")]
    [SerializeField]
    private bool m_EnableGravity = true;
    [Tooltip("设置当前物体上是否启用碰撞事件")]
    [SerializeField]
    private bool m_EnableCollisionEvent = false;
    [Tooltip("设置当前物体碰撞事件调用的休息时间（秒）")]
    [SerializeField]
    private float m_CollisionEventCallSleep = 0.5f;
    [Tooltip("设置静态恒力恒力方向")]
    [SerializeField]
    private Vector3 m_StaticConstantForceDirection = Vector3.forward;
    [Tooltip("设置静态恒力恒力")]
    [SerializeField]
    private float m_StaticConstantForce = 0;
    [Tooltip("设置静态恒力恒力方向参考")]
    [SerializeField]
    private Transform m_ConstantForceDirectionRef = null;

    #endregion

    /// <summary>
    /// 获取此物体的唯一ID
    /// </summary>
    /// <value></value>
    public int Id { get; private set; } = 0;
    /// <summary>
    /// 获取此物体的指针
    /// </summary>
    /// <value></value>
    public IntPtr Handle { get; private set; } = IntPtr.Zero;
    /// <summary>
    /// 物体的m摩擦力。0表示完全没有摩擦力。0.1=冰。3=非常粗糙。为每个物理对象定义了摩擦力。因此，物体A和物体B之间的滑动运动将取决于两者的摩擦力。对于实际摩擦力，该值必须与其他对象的摩擦系数相乘。
    /// </summary>
    /// <value></value>
    public float Friction { get => m_Friction; set => m_Friction = value; }
    /// <summary>
    /// 物体的弹力。>1.0意味着永不停止的跳跃。<0.2模拟需要额外的CPU。注意不要给弹性赋予不切实际的值：过高的值（如10）会使模拟在一段时间后变得非常不稳定。
    /// </summary>
    /// <value></value>
    public float Elasticity { get => m_Elasticity; set => m_Elasticity = value; }
    /// <summary>
    /// 0.0表示线性速度（对象的平移）不受影响，1.0表示每秒减少30%，0.1表示90%，等等，可以用作空气阻力。过低：对象可能会不切实际地加速。
    /// </summary>
    /// <value></value>
    public float LinearSpeedDamping { get => m_LinearSpeedDamping; set => m_LinearSpeedDamping = value; }
    /// <summary>
    /// 0.0表示旋转速度（物体的旋转）不受影响，1.0表示每秒减少30%，0.1表示90%，等等，可以用作某种空气阻力。过低：对象可能会不切实际地加速。
    /// </summary>
    /// <value></value>
    public float RotSpeedDamping { get => m_RotSpeedDamping; set => m_RotSpeedDamping = value; }
    /// <summary>
    /// 球体碰撞器的半径。
    /// </summary>
    /// <value></value>
    public float BallRadius { get => m_BallRadius; set => m_BallRadius = value; }
    /// <summary>
    /// 是否使用球体碰撞器。
    /// </summary>
    /// <value></value>
    public bool UseBall { get => m_UseBall; set => m_UseBall = value; }
    /// <summary>
    /// 如果为true，该算法将在对象周围生成一个额外的凸包。对于可移动对象，此外壳可以将性能提高到与凸面对象相同的水平。对于巨大的景观来说，这种优化毫无意义，因为所有有趣的对象都一直在穿透凸面外壳。
    /// </summary>
    /// <value></value>
    public bool EnableConvexHull { get => m_EnableConvexHull; set => m_EnableConvexHull = value; }
    /// <summary>
    /// 设置当前物体是否启用碰撞。
    /// </summary>
    /// <value></value>
    public bool EnableCollision { get => m_EnableCollision; set => m_EnableCollision = value; }
    /// <summary>
    /// 如果为true，则对象不会受到重力的影响，直到某个事件将其唤醒（如与另一个对象的碰撞、脉冲或连接到其上的弹簧的创建）。例如，当您需要物理化地板上已经存在的许多对象，并且不希望物理引擎通过计算所有需要的碰撞来稳定对象，从而减慢合成速度时，这非常有用。
    /// </summary>
    /// <value></value>
    public bool StartFrozen { get => m_StartFrozen; set => m_StartFrozen = value; }
    /// <summary>
    /// 物体的质心
    /// </summary>
    /// <value></value>
    public Vector3 ShiftMassCenter { get => m_ShiftMassCenter; set => m_ShiftMassCenter = value; }
    /// <summary>
    /// 在 Awake 时不自动创建刚体
    /// </summary>
    /// <value></value>
    public bool DoNotAutoCreateAtAwake { get => m_DoNotAutoCreateAtAwake; set => m_DoNotAutoCreateAtAwake = value; }
    /// <summary>
    /// 自动计算 CenterOfMass 
    /// </summary>
    /// <value></value>
    public bool AutoMassCenter { get => m_AutoMassCenter; set => m_AutoMassCenter = value; }
    /// <summary>
    /// 是否在gameObject激活时自动切换刚体的激活状态
    /// </summary>
    /// <value></value>
    public bool AutoControlActive { get => m_AutoControlActive; set => m_AutoControlActive = value; }
    /// <summary>
    /// 物体的碰撞层
    /// </summary>
    /// <value></value>
    public int Layer { get => m_Layer; set => m_Layer = value; }
    /// <summary>
    /// 指定当前碰撞组的名称，为空则不设置。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效
    /// </summary>
    /// <value></value>
    public string SystemGroupName { get => m_SystemGroupName; set => m_SystemGroupName = value; }
    /// <summary>
    /// 指定当前碰撞子组的ID，同一个碰撞组中子组的ID不能重复，默认为0。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效
    /// </summary>
    /// <value></value>
    public int SubSystemId { get => m_SubSystemId; set => m_SubSystemId = value; }
    /// <summary>
    /// 指定当前碰撞组不与那个子组碰撞，默认为0。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效
    /// </summary>
    /// <value></value>
    public int SubSystemDontCollideWith{ get => m_SubSystemDontCollideWith; set => m_SubSystemDontCollideWith = value; }
    /// <summary>
    /// 指定此物体的凸体网格
    /// </summary>
    public List<Mesh> Convex => m_Convex;
    /// <summary>
    /// 指定此物体的凹体网格，凹体网格将会销毁更多计算时间，所以推荐使用多个拆分的凸体构成一个凹体
    /// </summary>
    public List<Mesh> Concave => m_Concave;
    /// <summary>
    /// 当前物体物理网格生成的碰撞层名称，名称不能重复，否则当前碰撞层名称将不能复用
    /// </summary>
    /// <value></value>
    public string SurfaceName { get => m_SurfaceName; set => m_SurfaceName = value; }
    /// <summary>
    /// 当前物体物理网格是否使用已存在的碰撞层，如果为true，则不创建碰撞层，而是使用已有的碰撞层
    /// </summary>
    /// <value></value>
    public bool UseExistsSurface { get => m_UseExistsSurface; set => m_UseExistsSurface = value; }
    /// <summary>
    /// 当使用“幻影事件”构建块时，此值延迟“离开对象”输出激活。“离开对象”输出仅在对象离开幻影加上此额外半径时激活。
    /// </summary>
    /// <value></value>
    public float ExtraRadius { get => m_ExtraRadius; set => m_ExtraRadius = value; }
    /// <summary>
    /// 设置当前物体碰撞事件调用的休息时间（秒）
    /// </summary>
    /// <value></value>
    public float CollisionEventCallSleep { get => m_CollisionEventCallSleep; set => m_CollisionEventCallSleep = value; }
    /// <summary>
    /// 指定当前自定义碰撞组ID, 用于声音组碰撞事件的判断
    /// </summary>
    /// <value></value>
    public int CollisionID { 
      get => m_CollisionID; 
      set {
        m_CollisionID = value;
        if(IsPhysicalized)
          PhysicsApi.API.physics_set_col_id(Handle, m_CollisionID);
      }
    }

    private PhysicsEnvironment currentEnvironment = null;
    
    /// <summary>
    /// 物理化当前物体
    /// </summary>
    public void Physicalize()
    {
      if (Handle == IntPtr.Zero)
      {
        currentEnvironment = PhysicsEnvironment.GetCurrentScensePhysicsWorld();
        if (currentEnvironment == null)
        {
          Debug.LogWarning("Not found PhysicsEnvironment, please add it");
          return;
        }

        string surfaceName = m_SurfaceName;
        bool doNotCreateSurface = false;

        if (m_UseBall)
          doNotCreateSurface = true; //球体就不生成网格
        else if (surfaceName == "") {
          //根据当前物体的Mesh自动生成相对的名字
          string scaleName = gameObject.transform.lossyScale != Vector3.one ? ("_" + gameObject.transform.lossyScale.ToString()) : "";
          if(m_Convex.Count == 1)
            surfaceName = m_Convex[0].name + scaleName;
          else if(m_Concave.Count == 1) 
            surfaceName = m_Concave[0].name + scaleName;
          else {
            if(m_Concave.Count == 0 && m_Convex.Count == 0) {
              var meshFilter = GetComponent<MeshFilter>();
              if(meshFilter != null && meshFilter.sharedMesh != null) {
                var mesh = meshFilter.sharedMesh;
                surfaceName = mesh.name + scaleName;
              }
            } else
              surfaceName = gameObject.name + scaleName;
          }
        } 
        else if(PhysicsApi.API.surface_exist_by_name(currentEnvironment.Handle, surfaceName) || m_UseExistsSurface) {
          //检查 SurfaceName 是否存在，否则不生成
          Debug.LogWarning("Surface " + surfaceName + " already exist");
          doNotCreateSurface = true;
        }

        List<IntPtr> pConvexs = new List<IntPtr>();
        List<IntPtr> pConcavies = new List<IntPtr>();
        if (!doNotCreateSurface)
        {
          foreach (var mesh in m_Convex)
            pConvexs.Add(PhysicsApi.API.create_points_buffer(mesh.vertices, transform.lossyScale));
          foreach (var mesh in m_Concave)
            pConcavies.Add(PhysicsApi.API.create_polyhedron(mesh.vertices, mesh.triangles, transform.lossyScale));

          //如果不使用球并且没有指定的凹凸网格，则尝试自动获取网格
          if(pConvexs.Count == 0 && pConcavies.Count == 0) {
            var meshFilter = GetComponent<MeshFilter>();
            if(meshFilter == null || meshFilter.sharedMesh == null) {
              Debug.LogWarning("Object " + name + " create failed, No mesh was provided");
              return;
            }
            var mesh = meshFilter.sharedMesh;
            //if(m_Fixed) 
            //  pConcavies.Add(PhysicsApi.API.create_polyhedron(mesh.vertices, mesh.triangles, transform.lossyScale));
            //else 
              pConvexs.Add(PhysicsApi.API.create_points_buffer(mesh.vertices, transform.lossyScale));
          }
        }

        Handle = PhysicsApi.API.physicalize(currentEnvironment.Handle, name, m_Layer, currentEnvironment.GetSystemGroup(m_SystemGroupName),
         m_SubSystemId, m_SubSystemDontCollideWith, m_Mass, m_Friction, m_Elasticity, m_LinearSpeedDamping,
         m_RotSpeedDamping, m_BallRadius, m_UseBall, m_EnableConvexHull, m_AutoMassCenter, m_EnableCollision, m_StartFrozen,
         m_Fixed, transform.position, m_ShiftMassCenter, transform.rotation, m_UseExistsSurface, surfaceName,
         pConvexs.Count, pConvexs, pConcavies.Count, pConcavies, m_ExtraRadius, m_CollisionID);
        
        foreach (var ptr in pConvexs)
          PhysicsApi.API.delete_points_buffer(ptr);
        foreach (var ptr in pConcavies)
          PhysicsApi.API.delete_points_buffer(ptr);

        if(Handle == IntPtr.Zero)
          return;

        Id = PhysicsApi.API.physics_get_id(Handle);

        currentEnvironment.AddPhysicsObject(Id, this);

        CreateComponents();

        EnableGravity = m_EnableGravity;
        EnableCollisionEvent = m_EnableCollisionEvent;
      }
    }
    /// <summary>
    /// 取消物理化当前物体
    /// </summary>
    /// <param name="silently">是否静默取消物理化，否则此操作将使周围物体激活</param>
    public void UnPhysicalize(bool silently)
    {
      if (Handle != IntPtr.Zero)
      {
        DestroyComponents();
        Debug.Assert(currentEnvironment != null);
        PhysicsApi.API.unphysicalize(currentEnvironment.Handle, Handle, silently);
        Handle = IntPtr.Zero;
        currentEnvironment.RemovePhysicsObject(this);
      }
    }
    /// <summary>
    /// 获取当前物体是否已物理化
    /// </summary>
    /// <value></value>
    public bool IsPhysicalized { get => Handle != IntPtr.Zero; }

    /// <summary>
    /// 唤醒物体
    /// </summary>
    public void WakeUp() {
      checkPhysicalized();
      PhysicsApi.API.physics_wakeup(Handle);
    }
    /// <summary>
    /// 立即冻结对象，忽略速度
    /// </summary>
    /// <returns>如果成功则返回true</returns>
    public bool Freeze() {
      checkPhysicalized();
      return PhysicsApi.API.physics_freeze(Handle);
    } 

    /// <summary>
    /// 获取当前物体的速度
    /// </summary>
    /// <value></value>
    public float Speed
    {
      get
      {
        checkPhysicalized();
        return PhysicsApi.API.physics_get_speed(Handle);
      }
    }
    /// <summary>
    /// 获取当前物体的速度向量
    /// </summary>
    /// <value></value>
    public Vector3 SpeedVector
    {
      get
      {
        checkPhysicalized();
        PhysicsApi.API.physics_get_speed_vec(Handle, out var v);
        return v;
      }
    }
    /// <summary>
    /// 获取当前物体的旋转速度（物体坐标）
    /// </summary>
    /// <value></value>
    public Vector3 RotSpeed
    {
      get
      {
        checkPhysicalized();
        PhysicsApi.API.physics_get_rot_speed(Handle, out var v);
        return v;
      }
    }
    /// <summary>
    /// 获取或设置当前物体的质量（kg）
    /// </summary>
    /// <value></value>
    public float Mass {
      get { return m_Mass; }
      set {
        m_Mass = value;
        if(IsPhysicalized)
          PhysicsApi.API.physics_change_mass(Handle, value);
      }
    } 
    /// <summary>
    /// 获取当前物体是是固定物体
    /// </summary>
    public bool Fixed {
      get {
        //if(IsPhysicalized)
        //  return PhysicsApi.API.physics_is_fixed(Handle);
        return m_Fixed;
      }
      set {
        m_Fixed = value;
        if(IsPhysicalized)
          PhysicsApi.API.physics_change_unmovable_flag(Handle, value);
      }
    }
    /// <summary>
    /// 获取当前物体上是否启用了重力
    /// </summary>
    public bool EnableGravity {
      get {
        if(IsPhysicalized)
          return PhysicsApi.API.physics_is_gravity_enabled(Handle);
        return m_EnableGravity;
      }
      set {
        m_EnableGravity = value;
        if(IsPhysicalized)
          PhysicsApi.API.physics_enable_gravity(Handle, m_EnableGravity);
      }
    }  

    #region 碰撞

    /// <summary>
    /// 检查当前物体是否与其他某个物体相碰撞
    /// </summary>
    /// <param name="other">某个物体</param>
    /// <returns>如果碰撞返回true，否则返回false</returns>
    public bool IsContact(PhysicsObject other) {
      if(other == null)
        throw new Exception("other is null!");
      checkPhysicalized();
      other.checkPhysicalized();
      return PhysicsApi.API.physics_is_contact(Handle, other.Handle);
    }
    /// <summary>
    /// 启用当前物体的碰撞检测
    /// </summary>
    public void EnableCollisionDetection() {
      checkPhysicalized();
      PhysicsApi.API.physics_enable_collision_detection(Handle);
    }
    /// <summary>
    /// 禁用当前物体的碰撞检测
    /// </summary>
    public void DisableCollisionDetection() {
      checkPhysicalized();
      PhysicsApi.API.physics_enable_collision_detection(Handle);
    }
    /// <summary>
    /// 强制更新物体的碰撞信息
    /// </summary>
    public void ForceUpdateCollisionFilterInfo()
    {
      checkPhysicalized();
      PhysicsApi.API.physics_set_layer(
        currentEnvironment.Handle,
        Handle,
        m_Layer,
        currentEnvironment.GetSystemGroup(m_SystemGroupName),
        m_SubSystemId,
        m_SubSystemDontCollideWith);
    }  
    /// <summary>
    /// 获取或设置当前物体上是否启用碰撞事件
    /// </summary>
    public bool EnableCollisionEvent {
      get => m_EnableCollisionEvent;
      set {
        m_EnableCollisionEvent = value;
        if(IsPhysicalized) {
          if(m_EnableCollisionEvent) {
            collisionEventCallback = OnCollisionEvent;
            frictionEventCallback = OnFrictionEvent;
            PhysicsApi.API.physics_set_collision_listener(Handle, collisionEventCallback, m_CollisionEventCallSleep, frictionEventCallback);
          }
          else
            PhysicsApi.API.physics_remove_collision_listener(Handle);
        }
      }
    }  

    #endregion

    #region 更改坐标

    /// <summary>
    /// 将当前物体的坐标（Unity）设置至物理引擎
    /// </summary>
    /// <param name="optimize_for_repeated_calls">如果当前调用是需要重复调用的，例如每帧更新坐标，请设置为true让物理引擎进行优化</param>
    public void BeamObjectToNewPosition(bool optimize_for_repeated_calls) {
      BeamObjectToNewPosition(transform.position, transform.rotation, optimize_for_repeated_calls);
    }
    /// <summary>
    /// 设置物体在物理引擎中的位置
    /// </summary>
    /// <param name="pos">新的位置坐标（世界坐标）</param>
    /// <param name="rot">新的旋转</param>
    /// <param name="optimize_for_repeated_calls">如果当前调用是需要重复调用的，例如每帧更新坐标，请设置为true让物理引擎进行优化</param>
    public void BeamObjectToNewPosition(Vector3 pos, Quaternion rot, bool optimize_for_repeated_calls) {
      checkPhysicalized();
      PhysicsApi.API.physics_beam_object_to_new_position(Handle, rot, pos, optimize_for_repeated_calls);
    }

    #endregion

    #region 推动

    /// <summary>
    /// 是否启用施加在这个物体上的恒力
    /// </summary>
    /// <value></value>
    public bool EnableConstantForce { get => m_EnableConstantForce; set => m_EnableConstantForce = value; }
    /// <summary>
    /// 设置静态恒力恒力数值
    /// </summary>
    /// <value></value>
    public float StaticConstantForce { get => m_StaticConstantForce; set => m_StaticConstantForce = value; }
    /// <summary>
    /// 设置静态恒力恒力方向
    /// </summary>
    /// <value></value>
    public Vector3 StaticConstantForceDirection { get => m_StaticConstantForceDirection; set => m_StaticConstantForceDirection = value; }
    /// <summary>
    /// 施加在这个物体上的恒力方向参考
    /// </summary>
    /// <value></value>
    public Transform ConstantForceDirectionRef { get => m_ConstantForceDirectionRef; set => m_ConstantForceDirectionRef = value; }

    private int m_ConstantForce_ID = 0;
    private Dictionary<int, Vector3> m_ConstantForces = new Dictionary<int, Vector3>();

    /// <summary>
    /// 添加施加在这个物体上的恒力
    /// </summary>
    /// <param name="force">推动施加力的向量（世界坐标系）</param>
    /// <returns>返回恒力ID，可使用DeleteConstantForce删除恒力</returns>
    public int AddConstantForce(Vector3 force) {
      if(m_ConstantForce_ID < int.MaxValue - 1) m_ConstantForce_ID++;
      else m_ConstantForce_ID = 0;

      m_ConstantForces.Add(m_ConstantForce_ID, force);
      return m_ConstantForce_ID;
    }
    /// <summary>
    /// 删除施加在这个物体上的恒力
    /// </summary>
    /// <param name="forceId">AddConstantForce 返回的ID</param>
    public void DeleteConstantForce(int forceId) {
      m_ConstantForces.Remove(forceId);
    }
    /// <summary>
    /// 清除所有施加在这个物体上的恒力
    /// </summary>
    public void ClearConstantForce() {
      m_ConstantForces.Clear();
      m_ConstantForce_ID = 0;
    }

    private void doApplyConstantForce() {
      Transform dref = ConstantForceDirectionRef;
      foreach(var f in m_ConstantForces)
        Impluse(((ConstantForceDirectionRef == null ? f.Value : dref.TransformVector(f.Value))));
      if(m_StaticConstantForce > 0)
        Impluse((ConstantForceDirectionRef == null ? m_StaticConstantForceDirection : dref.TransformVector(m_StaticConstantForceDirection)) * m_StaticConstantForce);
    }

    /// <summary>
    /// 给物体施加一个推动
    /// </summary>
    /// <param name="pos">推动坐标（世界坐标系）</param>
    /// <param name="impluse">力的方向和大小（世界坐标系）</param>
    public void Impluse(Vector3 impluse) {
      Impluse(transform.position, impluse);
    }

    /// <summary>
    /// 给物体施加一个推动
    /// </summary>
    /// <param name="pos">推动坐标（世界坐标系）</param>
    /// <param name="impluse">力的方向和大小（世界坐标系）</param>
    public void Impluse(Vector3 pos, Vector3 impluse) {
      checkPhysicalized();
      PhysicsApi.API.physics_impluse(Handle, pos, impluse);
    }
    /// <summary>
    /// 给物体施加一个旋转推动
    /// </summary>
    /// <param name="rotVec">该矢量的每个分量表示施加在该对象相关核心轴上的旋转力。</param>
    public void Torque(Vector3 rotVec) {
      checkPhysicalized();
      PhysicsApi.API.physics_torque(Handle, rotVec);
    }
    /// <summary>
    /// 为物体添加速度
    /// </summary>
    /// <param name="speed">速度（世界坐标系）</param>
    public void AddSpeed(Vector3 speed) {
      checkPhysicalized();
      PhysicsApi.API.physics_add_speed(Handle, speed);
    }
  
    #endregion

    #region 幻影

    /// <summary>
    /// 将物理对象转换为幻影体积
    /// </summary>
    public void ConvertToPhantom() {
      if(IsPhantom()) 
        Debug.LogWarning("Object " + name + " is already phantom");
      else {
        phantomEventCallback = OnPhantomEvent;
        PhysicsApi.API.physics_convert_to_phantom(Handle, m_ExtraRadius, phantomEventCallback);
      }
    }
    /// <summary>
    /// 获取当前物体是否是幻影
    /// </summary>
    public bool IsPhantom() {
      checkPhysicalized();
      return PhysicsApi.API.physics_is_phantom(Handle);
    }
    /// <summary>
    /// 检查某个物体是否在当前幻影物体中
    /// </summary>
    /// <param name="other">某个物体</param>
    /// <returns></returns>
    public bool IsInsidePhantom(PhysicsObject other) {
      if(other == null)
        throw new Exception("other is null!");
      checkPhysicalized();
      other.checkPhysicalized();
      return PhysicsApi.API.physics_is_inside_phantom(Handle, other.Handle);
    }
    
    #endregion

    #region motion控制器

    /// <summary>
    /// 获取当前物体上是否启用了motion控制器
    /// </summary>
    /// <returns></returns>
    public bool IsMotionEnabled() {
      checkPhysicalized();
      return PhysicsApi.API.physics_is_motion_enabled(Handle);
    }
    /// <summary>
    /// 设置motion控制器的启用状态
    /// </summary>
    /// <param name="eanble">是否启用</param>
    public void SetMotionEnabled(bool eanble) {
      checkPhysicalized();
      PhysicsApi.API.physics_enable_motion(Handle, eanble);
    }

    #endregion

    #region 其他工具方法

    /// <summary>
    /// 通过物理物体指针获取物理物体ID
    /// </summary>
    /// <param name="handle"></param>
    /// <returns></returns>
    public static int GetIdByHandle(IntPtr handle) {
      return PhysicsApi.API.physics_get_id(handle);
    }

    private void checkPhysicalized()
    {
      if (!IsPhysicalized)
        throw new Exception("Object" + name + " not physicalized");
    }

    #endregion

    #region 事件

    public delegate void OnPhysicsPhantomEventCallback(PhysicsObject self, PhysicsObject other);
    public delegate void OnPhysicsCollisionEventCallback(PhysicsObject self, PhysicsObject other, Vector3 contact_point_ws, Vector3 speed, Vector3 surf_normal);
    public delegate void OnPhysicsFrictionEventCallback(PhysicsObject self, PhysicsObject other, IntPtr friction_handle, Vector3 contact_point_ws, Vector3 speed, Vector3 surf_normal);
    public delegate void OnPhysicsCallback(PhysicsObject self);
    
    public delegate void OnPhysicsCollDetectionEventCallback(PhysicsObject self, int col_id, float speed_precent);
    public delegate void OnPhysicsContactEventCallback(PhysicsObject self, int col_id);

    /// <summary>
    /// 物理对象进入幻影事件
    /// </summary>
    public OnPhysicsPhantomEventCallback OnPhantomEnter;
    /// <summary>
    /// 物理对象离开幻影事件
    /// </summary>
    public OnPhysicsPhantomEventCallback OnPhantomLeave;
    /// <summary>
    /// 物理对象碰撞事件
    /// </summary>
    public OnPhysicsCollisionEventCallback OnPhysicsCollision;
    /// <summary>
    /// 物理对象摩擦创建事件
    /// </summary>
    public OnPhysicsFrictionEventCallback OnPhysicsFrictionCreated;
    /// <summary>
    /// 物理对象摩擦删除事件
    /// </summary>
    public OnPhysicsFrictionEventCallback OnPhysicsFrictionDeleted;
    /// <summary>
    /// 物理对象工具碰撞事件
    /// </summary>
    public OnPhysicsCollDetectionEventCallback OnPhysicsCollDetection;
    /// <summary>
    /// 物理对象开始接触事件
    /// </summary>
    public OnPhysicsContactEventCallback OnPhysicsContactOn;
    /// <summary>
    /// 物理对象停止接触事件
    /// </summary>
    public OnPhysicsContactEventCallback OnPhysicsContactOff;

    #endregion

    #region 碰撞处理

    private PhantomEventCallback phantomEventCallback = null;
    private CollisionEventCallback collisionEventCallback = null;
    private FrictionEventCallback frictionEventCallback = null;
    private ContractEventCallback contractEventCallback = null;

    private void OnCollisionEvent(IntPtr self, IntPtr other, IntPtr contact_point_ws, IntPtr speed, IntPtr surf_normal) {
      if(self == Handle && OnPhysicsCollision != null) {
        var id = GetIdByHandle(other);
        var obj = currentEnvironment.GetObjectById(GetIdByHandle(other));
        OnPhysicsCollision.Invoke(
          this, 
          obj,
          PhysicsApi.API.ptr_to_vec3(contact_point_ws),
          PhysicsApi.API.ptr_to_vec3(speed),
          PhysicsApi.API.ptr_to_vec3(surf_normal)
        );
      }
    }
    private void OnFrictionEvent(int create, IntPtr self, IntPtr other, IntPtr friction_handle, IntPtr contact_point_ws, IntPtr speed, IntPtr surf_normal) {
      if(self == Handle) {
        var obj = currentEnvironment.GetObjectById(GetIdByHandle(other));
        var vspeed = PhysicsApi.API.ptr_to_vec3(speed);
        if(create > 0) {
          //摩擦事件
          if(OnPhysicsFrictionCreated != null) {
            OnPhysicsFrictionCreated.Invoke(
              this, 
              obj,
              friction_handle,
              PhysicsApi.API.ptr_to_vec3(contact_point_ws),
              vspeed,
              PhysicsApi.API.ptr_to_vec3(surf_normal)
            );
          } 

        } else {
          //摩擦事件
          if(OnPhysicsFrictionDeleted != null) {
            OnPhysicsFrictionDeleted.Invoke(
              this, 
              obj,
              friction_handle,
              PhysicsApi.API.ptr_to_vec3(contact_point_ws),
              vspeed,
              PhysicsApi.API.ptr_to_vec3(surf_normal)
            );
          }
        }
      }
    }
    private void OnPhantomEvent(int enter, IntPtr self, IntPtr other) {
      if(self == Handle) {
        if(enter > 0)
          OnPhantomEnter?.Invoke(this, currentEnvironment.GetObjectById(GetIdByHandle(other)));
        else 
          OnPhantomLeave?.Invoke(this, currentEnvironment.GetObjectById(GetIdByHandle(other)));
      }
    }
    private void OnContractEvent(IntPtr self, int col_id, short type, float speed_precent, short isOn) {
      if(type == 1) {
        OnPhysicsCollDetection?.Invoke(this, col_id, speed_precent);
      } else if(type == 2) {
        if(isOn == 1) 
          OnPhysicsContactOn?.Invoke(this, col_id);
        else
          OnPhysicsContactOff?.Invoke(this, col_id);
      } 
    }

    /// <summary>
    /// 启用当前物体上的接触工具事件发生器
    /// </summary>
    public void EnableContractEventCallback() {
      contractEventCallback = OnContractEvent;
      PhysicsApi.API.physics_set_contract_listener(Handle, Marshal.GetFunctionPointerForDelegate(contractEventCallback));
    }
    /// <summary>
    /// 禁用当前物体上的接触工具事件发生器
    /// </summary>
    public void DisableContractEventCallback() {
      PhysicsApi.API.physics_disable_collision_detection(Handle);
      contractEventCallback = null;
    }

    private Dictionary<int, IntPtr> onCollDetection = new Dictionary<int, IntPtr>();
    private Dictionary<int, IntPtr> onContractDetection = new Dictionary<int, IntPtr>();

    /// <summary>
    /// 添加指定层的碰撞工具事件处理
    /// </summary>
    /// <param name="col_id">自定义碰撞层ID</param>
    /// <param name="min_speed">最小速度</param>
    /// <param name="max_speed">最大速度</param>
    /// <param name="sleep_afterwards">延迟</param>
    /// <param name="speed_threadhold">速度变换阈值</param>
    public void AddCollDetection(int col_id, float min_speed, float max_speed, float sleep_afterwards, float speed_threadhold) {
      if(onCollDetection.ContainsKey(col_id))
        return;
      var ptr = PhysicsApi.API.physics_coll_detection(Handle, col_id, min_speed, max_speed, sleep_afterwards, speed_threadhold);
      onCollDetection.Add(col_id, ptr);
    }
    /// <summary>
    /// 移除指定层的碰撞工具事件处理
    /// </summary>
    /// <param name="col_id">自定义碰撞层ID</param>
    public void DeleteCollDetection(int col_id) {
      if(onCollDetection.ContainsKey(col_id)) {
        PhysicsApi.API.destroy_physics_coll_detection(onCollDetection[col_id]);
        onCollDetection.Remove(col_id);
      }
    }

    /// <summary>
    /// 添加指定层的接触工具事件处理
    /// </summary>
    /// <param name="col_id">自定义碰撞层ID</param>
    /// <param name="time_delay_start">接触前延时</param>
    /// <param name="time_delay_end">接触后延时</param>
    public void AddContractDetection(int col_id, float time_delay_start, float time_delay_end) {
      if(onContractDetection.ContainsKey(col_id))
        return;
      var ptr = PhysicsApi.API.physics_contract_detection(Handle, col_id, time_delay_start, time_delay_end);
      onContractDetection.Add(col_id, ptr);
    }
    /// <summary>
    /// 移除指定层的接触工具事件处理
    /// </summary>
    /// <param name="col_id">自定义碰撞层ID</param>
    public void DeleteContractDetection(int col_id) {
      if(onContractDetection.ContainsKey(col_id)) {
        PhysicsApi.API.destroy_physics_contract_detection(onContractDetection[col_id]);
        onContractDetection.Remove(col_id);
      }
    }

    #endregion

    #region 约束和力和弹簧
    
    public bool ComponentsCreated { get; private set; }

    private List<PhysicsComponent> thisObjectComponents = new List<PhysicsComponent>();
    private List<PhysicsComponent> pendingCreateComponents = new List<PhysicsComponent>();

    private void DestroyComponents() {
      ComponentsCreated = false;
      foreach(var c in thisObjectComponents)
        c.Destroy();
    }
    private void CreateComponents() {
      var c = new List<PhysicsComponent>(GetComponents<PhysicsComponent>());
      thisObjectComponents.Clear();
      c.ForEach((a) => thisObjectComponents.Add(a));
      pendingCreateComponents.ForEach((e) => c.Add(e));
      pendingCreateComponents.Clear();

      foreach(var e in c)
        e.Create();

      ComponentsCreated = true;
    }
    
    public void AddPendCreateComponent(PhysicsComponent c) { pendingCreateComponents.Add(c); }

    #endregion

    private int createTick = 0;

    private void OnDestroy() {
      m_ConstantForces.Clear();
      if(IsPhysicalized)
        UnPhysicalize(true);
    }
    private void Awake() {
      createTick = 2;
    }
    private void FixedUpdate() {
      if(createTick > 0) {
        createTick--;
        if(createTick == 0) {
          if(!m_DoNotAutoCreateAtAwake) 
            Physicalize();
        }
      }
      if(EnableConstantForce && Handle != IntPtr.Zero) doApplyConstantForce();
    }
  }
}