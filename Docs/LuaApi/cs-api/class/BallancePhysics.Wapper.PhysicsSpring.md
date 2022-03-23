# BallancePhysics.Wapper.PhysicsSpring 
在两个对象之间创建一个物理弹簧

## 字段

|名称|类型|说明|
|---|---|---|
|Position1Ref|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) |物体1参照点|
|Position2Ref|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) |物体2参照点|
|Other|[PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md) |连接到的另外一个物体|
|length|number [float](../types.md)|弹簧松弛时弹簧的长度。|
|constant|number [float](../types.md)|定义弹簧的硬度。刚性弹簧：应在[0.001f..1]范围内。0.001=非常软。1=非常难。正常弹簧：（单位：牛顿/米）。应在范围[0..infinite]内。0=非常软。10=硬。|
|spring_damping|number [float](../types.md)|弹簧松弛时弹簧的长度。|
|global_damping|number [float](../types.md)|阻尼系数，包括弹簧旋转的阻尼。刚性弹簧：未实施；对刚性弹簧没有影响。正常弹簧：该系数有助于最小化弹簧的数量。|
|UseStiffSpring|boolean |如果为true，则创建的弹簧将是刚性弹簧。然而，刚性弹簧的工作原理与普通弹簧类似-弹簧常数应在[0.001f..1]范围内-不会出现积分器问题->非常稳定。-可以使其比真正的弹簧更硬。-阻尼应在[0..1]范围内|
|ValuesAreRelative|boolean |Set this to TRUE, if spring values should be multiplied with the average virtual mass of both objects|
|ForceOnlyOnStretch|boolean |Set this to TRUE, if spring values should only be applied when the length exceeds spring_len|
