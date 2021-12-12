using System;
using Ballance2.LuaHelpers;
using UnityEngine;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [RequireComponent(typeof(PhysicsObject))]
  public class PhysicsConstraintBase : PhysicsComponent
  {
    protected virtual void DoCreateConstraint() {
    }

    [LuaApiDescription("创建约束")]
    protected override void DoCreate() {
      DoCreateConstraint();
    }
    [LuaApiDescription("销毁约束")]
    protected override void DoDestroy(IntPtr handle) {
      PhysicsApi.API.destroy_physics_constraint(handle);
      base.DoDestroy(handle);
    }
  }
}