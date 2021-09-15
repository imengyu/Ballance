using System.Collections.Generic;
using UnityEngine;

namespace BallancePhysics.Wapper
{
  [AddComponentMenu("BallancePhysics/PhysicsObject")]
  [DefaultExecutionOrder(20)]
  [DisallowMultipleComponent]
  [SLua.CustomLuaClass]
  public class PhysicsObject : MonoBehaviour
  {
    [Tooltip("物体的质量（kg）")]
    [SerializeField]
    private float m_Mass = 1.0f;
    [Tooltip("物体的m摩擦力。0表示完全没有摩擦力。0.1=冰。3=非常粗糙。为每个物理对象定义了摩擦力。因此，物体A和物体B之间的滑动运动将取决于两者的摩擦力。对于实际摩擦力，该值必须与其他对象的摩擦系数相乘。")]
    [SerializeField]
    private float m_Friction = 0.7f;
    [Tooltip("物体的弹力。>1.0意味着永不停止的跳跃。<0.2模拟需要额外的CPU。注意不要给弹性赋予不切实际的值：过高的值（如10）会使模拟在一段时间后变得非常不稳定。")]
    [SerializeField]
    private float m_Elasticity = 0.4f;

    [Tooltip("0.0表示线性速度（对象的平移）不受影响，1.0表示每秒减少30%，0.1表示90%，等等，可以用作空气阻力。过低：对象可能会不切实际地加速。")]
    [SerializeField]
    private float LinearSpeedDamping;
    [Tooltip("0.0表示旋转速度（物体的旋转）不受影响，1.0表示每秒减少30%，0.1表示90%，等等，可以用作某种空气阻力。过低：对象可能会不切实际地加速。")]
    [SerializeField]
    private float RotSpeedDamping;
    [Tooltip("球体碰撞器的半径。")]
    [SerializeField]
    private float m_BallRadius;

    [Tooltip("是否使用球体碰撞器。")]
    [SerializeField]
    private bool m_UseBall = false;
    [Tooltip("如果为true，该算法将在对象周围生成一个额外的凸包。对于可移动对象，此外壳可以将性能提高到与凸面对象相同的水平。对于巨大的景观来说，这种优化毫无意义，因为所有有趣的对象都一直在穿透凸面外壳。")]
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

    [Tooltip("在 Awake 时不自动创建刚体，设置为 false 后您需要手动调用 ForceReCreateShape 来创建刚体")]
    [SerializeField]
    private bool m_DoNotAutoCreateAtAwake = false;
    [Tooltip("自动计算 CenterOfMass ")]
    [SerializeField]
    private bool m_AutoMassCenter = true;
    [Tooltip("是否在gameObject激活时自动切换刚体的激活状态")]
    [SerializeField]
    private bool m_AutoControlActive = true;
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

    [Tooltip("")]
    [SerializeField]
    private List<Mesh> m_Convex = new List<Mesh>();
    [Tooltip("")]
    [SerializeField]
    private List<Mesh> m_Concave = new List<Mesh>();

    
     
  }
}