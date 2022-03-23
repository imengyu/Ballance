using System;
using UnityEngine;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [AddComponentMenu("BallancePhysics/PhysicsForce")]
  [LuaApiDescription("设置一个力，推动两个物体移动")]
  [LuaApiNotes(@"!> **提示：** Ballance中没有使用这个组件，这个力是会推动两个物体移动的，功能与 PhysicsConstraintForce 不一样，
如果需要恒力，请使用 [PhysicsConstraintForce](BallancePhysics.Wapper.PhysicsConstraintForce) 组件。")]
  public class PhysicsForce : PhysicsComponent
  {
    /// <summary>
    /// 物体1参照点
    /// </summary>
    [Tooltip("物体1参照点")]
    [LuaApiDescription("物体1参照点")]
    public Transform Position1Ref;
    [Tooltip("物体2参照点")]
    [LuaApiDescription("物体2参照点")]
    /// <summary>
    /// 物体2参照点
    /// </summary>
    public Transform Position2Ref;

    [Tooltip("力的大小")]
    [SerializeField]
    private float _Force = 1.0f;

    [Tooltip("连接到的另外一个物体")]
    [LuaApiDescription("连接到的另外一个物体")]
    public PhysicsObject Other = null;

    /// <summary>
    /// 力的大小
    /// </summary>
    /// <value></value>
    [LuaApiDescription("力的大小")]
    public float Force { 
      get => _Force;
      set {
        _Force = value;
        if(Handle != IntPtr.Zero)
          PhysicsApi.API.set_physics_force_value(Handle, _Force);
      }
    }
    /// <summary>
    /// 是否推动物体2
    /// </summary>
    [Tooltip("是否推动物体2")]
    [LuaApiDescription("是否推动物体2")]
    public bool PushObject2 = false;

    protected override void DoCreate()
    {
      var obj = GetComponent<PhysicsObject>();
      if(!obj.IsPhysicalized)
        return;
      if(Other == null)
        return;
      if(!Other.IsPhysicalized) {
        Other.AddPendCreateComponent(this);
        return;
      }
      Handle = PhysicsApi.API.create_physics_force(
        obj.Handle, Other.Handle, Position1Ref.transform.position, 
        Position2Ref.transform.position, Force, PushObject2
      );
    }
    protected override void DoDestroy(IntPtr ptr)
    {
      PhysicsApi.API.destroy_physics_force(ptr);
      base.DoDestroy(ptr);
    }
  }
}