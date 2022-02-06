using UnityEngine;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [AddComponentMenu("BallancePhysics/PhysicsFixedConstraint")]
  public class PhysicsFixedConstraint : PhysicsConstraintBase
  {
    [Tooltip("连接到的另外一个物体，如果为空，则连接到世界")]
    /// <summary>
    /// 连接到的另外一个物体，如果为空，则连接到世界
    /// </summary>
    public PhysicsObject Other = null;

    protected override void DoCreateConstraint()
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
      Handle = PhysicsApi.API.set_physics_fixed_constraint(obj.Handle, Other.Handle);
    }
  }
}