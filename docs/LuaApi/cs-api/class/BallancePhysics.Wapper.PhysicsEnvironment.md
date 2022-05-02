# BallancePhysics.Wapper.PhysicsEnvironment 
物理世界承载组件

## 字段

|名称|类型|说明|
|---|---|---|
|Gravity|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) |世界的引力。默认值是 (0, -9.8, 0). (模拟开始后更改此值无效)|
|DePhysicsFall|number [float](../types.md)|指定y坐标低于这个值的时候自动失活物体. 大于 0 的时候不启用|
|SimulationRate|number [int](../types.md)|模拟速率（10-100，一秒钟进行物理模拟的速率）(模拟开始后更改此值无效)|
|TimeFactor|number [int](../types.md)|用于物理对象模拟的时间乘以的因子。因此，如果“物理时间因子”为2.0，而不是1.0（正常速度），则物理对象下落的速度将加倍。|
|DeleteAllSurfacesWhenDestroy|boolean |是否在销毁环境时自动删除所有碰撞层|
|Simulate|boolean |是否启用模拟|
|AutoCreate|boolean |是否自动创建|
|PhysicsFactorFinalValue|number [float](../types.md)|获取物理力大小系数。|
## 属性

|名称|类型|说明|
|---|---|---|
|PhysicsWorlds|table |所有物理环境索引|
|Handle|[IntPtr](https://docs.microsoft.com/zh-cn/dotnet/api/System.IntPtr) |获取当前物理环境的底层指针|
|PhysicsTime|number [float](../types.md)|获取上一帧的物理执行时间 (秒)|
|PhysicsActiveBodies|number [int](../types.md)|获取当前激活的物理对象个数|
|PhysicsBodies|number [int](../types.md)|获取当前所有的物理对象个数|
|PhysicsSimuateTime|number [double](../types.md)|获取当前模拟的物理时间|
|PhysicsConstantPushBodies|number [int](../types.md)|获取当前正在恒力推动的物理对象个数|
|PhysicsFallCollectBodies|number [int](../types.md)|获取当前坠落回收物理对象个数|
|PhysicsFixedBodies|number [int](../types.md)|获取当前固定物理对象个数|
|PhysicsUpdateBodies|number [int](../types.md)|获取当前更新物理对象个数|

## 方法



### `静态` GetCurrentScensePhysicsWorld()

获取当前场景的物理环境


#### 返回值

[PhysicsEnvironment](./BallancePhysics.Wapper.PhysicsEnvironment.md) <br/>如果当前场景没有创建物理环境，则返回null


### Create()

手动创建物理环境



### Destroy()

手动销毁物理环境



### GetSystemGroup(name)

获取上一帧的物理执行时间 (秒)


#### 参数


`name` string <br/>子组名称



#### 返回值

number [int](../types.md)<br/>返回碰撞组子ID


### DeleteAllSurfaces()

删除物理系统中的所有碰撞层



### GetObjectById(id)

通过ID查找世界中的物理物体


#### 参数


`id` number [int](../types.md)<br/>ID



#### 返回值

[PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md) <br/>如果未找到则返回null


### RaycastingOne(startPoint, dirction, rayLength, distance)

从指定位置发射射线，返回第一个碰撞物体。


#### 参数


`startPoint` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>射线发射位置

`dirction` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>射线方向向量

`rayLength` number [float](../types.md)<br/>射线长度

`distance` [Single&](https://docs.microsoft.com/zh-cn/dotnet/api/System.Single&) <br/>第一个碰撞物体的距离



#### 返回值

[PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md) <br/>如果有物体碰撞，则返回第一个物体，否则返回null


### RaycastingObject(obj, startPoint, dirction, rayLength, distance)

从指定位置发射射线，获取射线是否与指定物体碰撞。


#### 参数


`obj` [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md) <br/>指定物体

`startPoint` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>射线发射位置

`dirction` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>射线方向向量

`rayLength` number [float](../types.md)<br/>射线长度

`distance` [Single&](https://docs.microsoft.com/zh-cn/dotnet/api/System.Single&) <br/>第一个碰撞物体的距离



#### 返回值

boolean <br/>如果射线有和物体碰撞，则返回true，否则返回false


### Raycasting(flags, startPoint, dirction, rayLength)

从指定位置发射射线，获取射线碰撞的全部物体


#### 参数


`flags` number [RaySolverFlag](./BallancePhysics.Wapper.PhysicsEnvironment+RaySolverFlag.md)<br/>射线处理标志

`startPoint` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>射线发射位置

`dirction` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>射线方向向量

`rayLength` number [float](../types.md)<br/>射线长度



#### 返回值

[RayCastResult](./BallancePhysics.Wapper.PhysicsEnvironment+RayCastResult.md) <br/>返回碰撞信息
