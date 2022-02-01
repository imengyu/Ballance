using System;
using UnityEngine;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [RequireComponent(typeof(PhysicsObject))]
  public class PhysicsConstraintBase : PhysicsComponent
  {
    protected virtual void DoCreateConstraint() {
    }

    /// <summary>
    /// 创建约束
    /// </summary>
    protected override void DoCreate() {
      DoCreateConstraint();
    }
    /// <summary>
    /// 销毁约束
    /// </summary>
    /// <param name="handle"></param>
    protected override void DoDestroy(IntPtr handle) {
      PhysicsApi.API.destroy_physics_constraint(handle);
    }
  }
}