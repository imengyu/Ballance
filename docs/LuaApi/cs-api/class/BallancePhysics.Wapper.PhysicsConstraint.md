# BallancePhysics.Wapper.PhysicsConstraint 
在两个物理对象之间设置常规物理约束

## 字段

|名称|类型|说明|
|---|---|---|
|Other|[PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md) |连接到的另外一个物体，如果为空，则连接到世界|
|force_factor|number [float](../types.md)|“力系数”是指向所需位置的系数。当“力系数”=0.5时，约束尝试仅到达当前位置和所需位置之间的中间位置。动作更流畅。|
|damp_factor|number [float](../types.md)|较高的“阻尼系数”也意味着运动控制器BB尝试以更慢的速度到达其所需位置。“力系数/阻尼系数”是最重要的值。当该值低于1.0时，运动会更加平滑，不会跳跃或有弹性。|
|translation_limit|number [CoordinateIndex](./BallancePhysics.Wapper.PhysicsConstraint+CoordinateIndex.md)|设置当前约束启用的移动约束轴|
|translation_freedom_min|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) |此参数细分为3对。每对定义一个平移轴的自由度最小值。|
|translation_freedom_max|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) |此参数细分为3对。每对定义一个平移轴的自由度最大值。|
|rotation_limit|number [CoordinateIndex](./BallancePhysics.Wapper.PhysicsConstraint+CoordinateIndex.md)|设置当前约束启用的旋转约束轴|
|rotation_freedom_min|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) |此参数细分为3对。每对定义一个旋转轴的自由度最小值。|
|rotation_freedom_max|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) |此参数细分为3对。每对定义一个旋转轴的自由度最大值。|
