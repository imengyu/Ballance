using UnityEngine;

namespace BallancePhysics.Wapper
{
  
  [AddComponentMenu("BallancePhysics/PhysicsBallJoint")]
  
  public class PhysicsBallJoint : PhysicsConstraintBase
  {
    [Tooltip("连接点位置参照")]
    
    public Transform JointPositionRef = null;
    [Tooltip("连接到的另外一个物理物体，如果为空，则连接到世界物体")]
    
    public PhysicsObject Other = null;

    protected override void DoCreateConstraint()
    {
      var obj = GetComponent<PhysicsObject>();
      if(!obj.IsPhysicalized)
        return;
      if(Other != null && !Other.IsPhysicalized) {
        Other.AddPendCreateComponent(this);
        return;
      }
      Handle = PhysicsApi.API.set_physics_ball_joint(obj.Handle, Other != null ? Other.Handle : System.IntPtr.Zero, JointPositionRef.transform.position);
    }
  }
}