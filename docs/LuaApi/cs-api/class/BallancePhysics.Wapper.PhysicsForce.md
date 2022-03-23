# BallancePhysics.Wapper.PhysicsForce 
设置一个力，推动两个物体移动

## 注解

!> **提示：** Ballance中没有使用这个组件，这个力是会推动两个物体移动的，功能与 PhysicsConstraintForce 不一样，
如果需要恒力，请使用 [PhysicsConstraintForce](BallancePhysics.Wapper.PhysicsConstraintForce) 组件。

## 字段

|名称|类型|说明|
|---|---|---|
|Position1Ref|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) |物体1参照点|
|Position2Ref|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) |物体2参照点|
|Other|[PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md) |连接到的另外一个物体|
|PushObject2|boolean |是否推动物体2|
## 属性

|名称|类型|说明|
|---|---|---|
|Force|number [float](../types.md)|力的大小|

## 方法

