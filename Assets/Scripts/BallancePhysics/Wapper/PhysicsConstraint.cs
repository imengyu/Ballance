using UnityEditor;
using UnityEngine;

namespace BallancePhysics.Wapper
{
  [AddComponentMenu("BallancePhysics/PhysicsConstraint")]
  [SLua.CustomLuaClass]
  public class PhysicsConstraint : PhysicsConstraintBase
  {
    [Tooltip("位置和轴参照")]
    public Transform HingeRef;
    [Tooltip("连接到的另外一个物体，如果为空，则连接到世界")]
    public PhysicsObject Other;

    [SLua.CustomLuaClass]
    public enum CoordinateIndex {
      X = 0,
      Y = 1,
      Z = 2 
    };

    [Tooltip("“力系数”是指向所需位置的系数。当“力系数”=0.5时，运动控制器BB尝试仅到达当前位置和所需位置之间的中间位置。动作更流畅。")]
    public float force_factor = 1.0f;
    [Tooltip("较高的“阻尼系数”也意味着运动控制器BB尝试以更慢的速度到达其所需位置。“力系数/阻尼系数”是最重要的值。当该值低于1.0时，运动会更加平滑，不会跳跃或有弹性。")]
    public float damp_factor = 1.0f;

    [EnumFlagProperty]
    public CoordinateIndex translation_limit;
    
    [Tooltip("此参数细分为3对。每对定义一个平移轴的自由度最小值。")]
    public Vector3 translation_freedom_min;
    [Tooltip("此参数细分为3对。每对定义一个平移轴的自由度最大值。")]
    public Vector3 translation_freedom_max;
    
    [EnumFlagProperty]
    public CoordinateIndex rotation_limit;

    [Tooltip("此参数细分为3对。每对定义一个旋转轴的自由度最小值。")]
    public Vector3 rotation_freedom_min;
    [Tooltip("此参数细分为3对。每对定义一个旋转轴的自由度最大值。")]
    public Vector3 rotation_freedom_max;

    protected override void DoCreateConstraint()
    {
      var obj = GetComponent<PhysicsObject>();
      if(!obj.IsPhysicalized)
        return;
      if(Other != null && !Other.IsPhysicalized) {
        Other.AddPendCreateComponent(this);
        return;
      }
      Handle = PhysicsApi.API.set_physics_constraint(obj.Handle, Other != null ? Other.Handle : System.IntPtr.Zero, 
        force_factor, damp_factor, (int)translation_limit, translation_freedom_min, translation_freedom_max,
        (int)rotation_limit, rotation_freedom_min, rotation_freedom_max);
    }
  }
}