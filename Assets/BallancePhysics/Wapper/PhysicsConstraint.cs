using UnityEditor;
using UnityEngine;

namespace BallancePhysics.Wapper
{
  [SLua.CustomLuaClass]
  [AddComponentMenu("BallancePhysics/PhysicsConstraint")]
  [LuaApiDescription("在两个物理对象之间设置常规物理约束")]
  public class PhysicsConstraint : PhysicsConstraintBase
  {
    [LuaApiDescription("连接到的另外一个物体，如果为空，则连接到世界")]
    [Tooltip("连接到的另外一个物体，如果为空，则连接到世界")]
    /// <summary>
    /// 连接到的另外一个物体，如果为空，则连接到世界
    /// </summary>
    public PhysicsObject Other;
    
    [SLua.CustomLuaClass]
    [LuaApiDescription("物理约束组件的约束轴")]
    public enum CoordinateIndex {
      [LuaApiDescription("限制X轴")]
      X = 0,
      [LuaApiDescription("限制Y轴")]
      Y = 1,
      [LuaApiDescription("限制Z轴")]
      Z = 2 
    };

    [Tooltip("“力系数”是指向所需位置的系数。当“力系数”=0.5时，约束尝试仅到达当前位置和所需位置之间的中间位置。动作更流畅。")]
    [LuaApiDescription("“力系数”是指向所需位置的系数。当“力系数”=0.5时，约束尝试仅到达当前位置和所需位置之间的中间位置。动作更流畅。")]
    /// <summary>
    /// “力系数”是指向所需位置的系数。当“力系数”=0.5时，约束尝试仅到达当前位置和所需位置之间的中间位置。动作更流畅。
    /// </summary>
    public float force_factor = 1.0f;
    [Tooltip("较高的“阻尼系数”也意味着运动控制器BB尝试以更慢的速度到达其所需位置。“力系数/阻尼系数”是最重要的值。当该值低于1.0时，运动会更加平滑，不会跳跃或有弹性。")]
    [LuaApiDescription("较高的“阻尼系数”也意味着运动控制器BB尝试以更慢的速度到达其所需位置。“力系数/阻尼系数”是最重要的值。当该值低于1.0时，运动会更加平滑，不会跳跃或有弹性。")]
    /// <summary>
    /// 较高的“阻尼系数”也意味着运动控制器BB尝试以更慢的速度到达其所需位置。“力系数/阻尼系数”是最重要的值。当该值低于1.0时，运动会更加平滑，不会跳跃或有弹性。
    /// </summary>
    public float damp_factor = 1.0f;

    [EnumFlagProperty]
    [Tooltip("设置当前约束启用的移动约束轴")]
    [LuaApiDescription("设置当前约束启用的移动约束轴")]
    public CoordinateIndex translation_limit;
    
    [Tooltip("此参数细分为3对。每对定义一个平移轴的自由度最小值。")]
    [LuaApiDescription("此参数细分为3对。每对定义一个平移轴的自由度最小值。")]
    public Vector3 translation_freedom_min;
    [Tooltip("此参数细分为3对。每对定义一个平移轴的自由度最大值。")]
    [LuaApiDescription("此参数细分为3对。每对定义一个平移轴的自由度最大值。")]
    public Vector3 translation_freedom_max;
    
    [EnumFlagProperty]
    [Tooltip("设置当前约束启用的旋转约束轴")]
    [LuaApiDescription("设置当前约束启用的旋转约束轴")]
    public CoordinateIndex rotation_limit;

    [Tooltip("此参数细分为3对。每对定义一个旋转轴的自由度最小值。")]
    [LuaApiDescription("此参数细分为3对。每对定义一个旋转轴的自由度最小值。")]
    public Vector3 rotation_freedom_min;
    [Tooltip("此参数细分为3对。每对定义一个旋转轴的自由度最大值。")]
    [LuaApiDescription("此参数细分为3对。每对定义一个旋转轴的自由度最大值。")]
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
        force_factor, damp_factor, 
        (int)translation_limit, 
        translation_freedom_min, 
        translation_freedom_max,
        (int)rotation_limit, 
        rotation_freedom_min, 
        rotation_freedom_max);
    }
  }
}