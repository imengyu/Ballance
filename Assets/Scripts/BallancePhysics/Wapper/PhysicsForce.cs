using System;
using UnityEngine;

namespace BallancePhysics.Wapper
{
  [AddComponentMenu("BallancePhysics/PhysicsForce")]
  [SLua.CustomLuaClass]
  public class PhysicsForce : PhysicsComponent
  {
    [Tooltip("物体1参照点")]
    public Transform Position1Ref;
    [Tooltip("物体2参照点")]
    public Transform Position2Ref;

    [Tooltip("力的大小")]
    [SerializeField]
    private float _Force = 1.0f;

    [Tooltip("连接到的另外一个物体")]
    public PhysicsObject Other = null;
    public float Force { 
      get => _Force;
      set {
        _Force = value;
        if(Handle != IntPtr.Zero)
          PhysicsApi.API.set_physics_force_value(Handle, _Force);
      }
    }
    [Tooltip("是否推动物体2")]
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
    protected override void DoDestry(IntPtr ptr)
    {
      PhysicsApi.API.destroy_physics_force(ptr);
      base.DoDestry(ptr);
    }
  }
}