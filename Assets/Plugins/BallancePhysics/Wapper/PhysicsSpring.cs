using System;
using UnityEngine;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [AddComponentMenu("BallancePhysics/PhysicsSpring")]
  public class PhysicsSpring : PhysicsComponent
  {
    [Tooltip("物体1参照点")]
    /// <summary>
    /// 物体1参照点
    /// </summary>
    public Transform Position1Ref;
    [Tooltip("物体2参照点")]
    /// <summary>
    /// 物体2参照点
    /// </summary>
    public Transform Position2Ref;
    [Tooltip("连接到的另外一个物体")]
    /// <summary>
    /// 连接到的另外一个物体
    /// </summary>
    public PhysicsObject Other = null;
     
    [Tooltip("弹簧松弛时弹簧的长度。")]
    /// <summary>
    /// 弹簧松弛时弹簧的长度。
    /// </summary>
    public float length = 1;
    [Tooltip("定义弹簧的硬度。刚性弹簧：应在[0.001f..1]范围内。0.001=非常软。1=非常难。正常弹簧：（单位：牛顿/米）。应在范围[0..infinite]内。0=非常软。10=硬。")]
    /// <summary>
    /// 定义弹簧的硬度。刚性弹簧：应在[0.001f..1]范围内。0.001=非常软。1=非常难。正常弹簧：（单位：牛顿/米）。应在范围[0..infinite]内。0=非常软。10=硬。
    /// </summary>
    public float constant = 0.002f;
    [Tooltip("弹簧的恒定阻尼。刚性弹簧：应在范围[0..1]内。最好将弹簧阻尼值保持在与弹簧常数相同的范围内（例如，s_常数0.1f->s_阻尼[0.05f..0.2f]）。正常弹簧：应在范围[0..infinite]内。")]
    /// <summary>
    /// 弹簧的恒定阻尼。刚性弹簧：应在范围[0..1]内。最好将弹簧阻尼值保持在与弹簧常数相同的范围内（例如，s_常数0.1f->s_阻尼[0.05f..0.2f]）。正常弹簧：应在范围[0..infinite]内。
    /// </summary>
    public float spring_damping = 0.1f;
    [Tooltip("阻尼系数，包括弹簧旋转的阻尼。刚性弹簧：未实施；对刚性弹簧没有影响。正常弹簧：该系数有助于最小化弹簧的数量。")]
    /// <summary>
    /// 阻尼系数，包括弹簧旋转的阻尼。刚性弹簧：未实施；对刚性弹簧没有影响。正常弹簧：该系数有助于最小化弹簧的数量。
    /// </summary>
    public float global_damping = 0.1f;
    [Tooltip("如果为true，则创建的弹簧将是刚性弹簧。然而，刚性弹簧的工作原理与普通弹簧类似-弹簧常数应在[0.001f..1]范围内-不会出现积分器问题->非常稳定。-可以使其比真正的弹簧更硬。-阻尼应在[0..1]范围内")]
    /// <summary>
    /// 如果为true，则创建的弹簧将是刚性弹簧。然而，刚性弹簧的工作原理与普通弹簧类似-弹簧常数应在[0.001f..1]范围内-不会出现积分器问题->非常稳定。-可以使其比真正的弹簧更硬。-阻尼应在[0..1]范围内
    /// </summary>
    public bool UseStiffSpring = false;
    [Tooltip("Set this to TRUE, if spring values should be multiplied with the average virtual mass of both objects")]
    /// <summary>
    /// Set this to TRUE, if spring values should be multiplied with the average virtual mass of both objects
    /// </summary>
    public bool ValuesAreRelative = false;
    [Tooltip("Set this to TRUE, if spring values should only be applied when the length exceeds spring_len")]
    /// <summary>
    /// Set this to TRUE, if spring values should only be applied when the length exceeds spring_len
    /// </summary>
    public bool ForceOnlyOnStretch = false;

    protected override void DoCreate()
    {
      var obj = GetComponent<PhysicsObject>();
      if(!obj.IsPhysicalized)
        return;
      if(Other != null && !Other.IsPhysicalized) {
        Other.AddPendCreateComponent(this);
        return;
      }
      Handle = PhysicsApi.API.create_physics_spring(
        obj.Handle, Other != null ? Other.Handle : IntPtr.Zero, Position1Ref.transform.position, Position2Ref.transform.position, length, constant, spring_damping, 
        global_damping, UseStiffSpring, ValuesAreRelative, ForceOnlyOnStretch
      );
    }
    protected override void DoDestroy(IntPtr ptr)
    {
      PhysicsApi.API.destroy_physics_force(ptr);
      base.DoDestroy(ptr);
    }
  }
}