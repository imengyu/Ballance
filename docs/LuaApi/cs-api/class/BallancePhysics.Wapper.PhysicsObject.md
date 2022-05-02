# BallancePhysics.Wapper.PhysicsObject 
Ballance 物理物体组件

## 字段

|名称|类型|说明|
|---|---|---|
|OnPhantomEnter|`回调` OnPhysicsPhantomEventCallback(self: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md), other: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md)) |物理对象进入幻影事件|
|OnPhantomLeave|`回调` OnPhysicsPhantomEventCallback(self: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md), other: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md)) |物理对象离开幻影事件|
|OnPhysicsCollision|`回调` OnPhysicsCollisionEventCallback(self: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md), other: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md), contact_point_ws: [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html), speed: [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html), surf_normal: [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html)) |物理对象碰撞事件|
|OnPhysicsFrictionCreated|`回调` OnPhysicsFrictionEventCallback(self: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md), other: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md), friction_handle: [IntPtr](https://docs.microsoft.com/zh-cn/dotnet/api/System.IntPtr), contact_point_ws: [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html), speed: [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html), surf_normal: [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html)) |物理对象摩擦创建事件|
|OnPhysicsFrictionDeleted|`回调` OnPhysicsFrictionEventCallback(self: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md), other: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md), friction_handle: [IntPtr](https://docs.microsoft.com/zh-cn/dotnet/api/System.IntPtr), contact_point_ws: [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html), speed: [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html), surf_normal: [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html)) |物理对象摩擦删除事件|
|OnPhysicsCollDetection|`回调` OnPhysicsCollDetectionEventCallback(self: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md), col_id: [Int32](https://docs.microsoft.com/zh-cn/dotnet/api/System.Int32), speed_precent: [Single](https://docs.microsoft.com/zh-cn/dotnet/api/System.Single)) |物理对象工具碰撞事件|
|OnPhysicsContactOn|`回调` OnPhysicsContactEventCallback(self: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md), col_id: [Int32](https://docs.microsoft.com/zh-cn/dotnet/api/System.Int32)) |物理对象开始接触物体事件|
|OnPhysicsContactOff|`回调` OnPhysicsContactEventCallback(self: [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md), col_id: [Int32](https://docs.microsoft.com/zh-cn/dotnet/api/System.Int32)) |物理对象停止接触物体事件|
## 属性

|名称|类型|说明|
|---|---|---|
|Id|number [int](../types.md)|获取此物体的唯一ID|
|Handle|[IntPtr](https://docs.microsoft.com/zh-cn/dotnet/api/System.IntPtr) |获取此物体的底层引擎指针|
|Friction|number [float](../types.md)|物体的m摩擦力。0表示完全没有摩擦力。0.1=冰。3=非常粗糙。为每个物理对象定义了摩擦力。因此，物体A和物体B之间的滑动运动将取决于两者的摩擦力。对于实际摩擦力，该值必须与其他对象的摩擦系数相乘。|
|Elasticity|number [float](../types.md)|物体的弹力。>1.0意味着永不停止的跳跃。<0.2模拟需要额外的CPU。注意不要给弹性赋予不切实际的值：过高的值（如10）会使模拟在一段时间后变得非常不稳定。|
|LinearSpeedDamping|number [float](../types.md)|0.0表示线性速度（对象的平移）不受影响，1.0表示每秒减少30%，0.1表示90%，等等，可以用作空气阻力。过低：对象可能会不切实际地加速。|
|RotSpeedDamping|number [float](../types.md)|0.0表示旋转速度（物体的旋转）不受影响，1.0表示每秒减少30%，0.1表示90%，等等，可以用作某种空气阻力。过低：对象可能会不切实际地加速。|
|BallRadius|number [float](../types.md)|球体碰撞器的半径。|
|UseBall|boolean |是否使用球体碰撞器。|
|BuildRootConvexHull|boolean |如果为true，该算法将在对象周围生成一个额外的凸包。对于可移动对象，此外壳可以将性能提高到与凸面对象相同的水平。对于巨大的景观来说，这种优化毫无意义，因为所有有趣的对象都一直在穿透凸面外壳。|
|EnableCollision|boolean |设置当前物体是否启用碰撞。|
|StartFrozen|boolean |如果为true，则对象不会受到重力的影响，直到某个事件将其唤醒（如与另一个对象的碰撞、脉冲或连接到其上的弹簧的创建）。例如，当您需要物理化地板上已经存在的许多对象，并且不希望物理引擎通过计算所有需要的碰撞来稳定对象，从而减慢合成速度时，这非常有用。|
|ShiftMassCenter|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) |物体的质心|
|DoNotAutoCreateAtAwake|boolean |指定是否在 Awake 时不自动创建刚体|
|AutoMassCenter|boolean |是否自动计算 CenterOfMass |
|AutoControlActive|boolean |是否在gameObject激活时自动切换刚体的激活状态|
|Layer|number [int](../types.md)|物体的碰撞层|
|SystemGroupName|string |指定当前碰撞组的名称，为空则不设置。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效|
|SubSystemId|number [int](../types.md)|指定当前碰撞子组的ID，同一个碰撞组中子组的ID不能重复，默认为0。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效|
|SubSystemDontCollideWith|number [int](../types.md)|指定当前碰撞组不与那个子组碰撞，默认为0。创建之后再修改则必须调用 ForceUpdateCollisionFilterInfo 才能生效|
|Convex|table |指定此物体的凸体网格|
|Concave|table |指定此物体的凹体网格，凹体网格将会销毁更多计算时间，所以推荐使用多个拆分的凸体构成一个凹体|
|SurfaceName|string |当前物体物理网格生成的碰撞层名称，名称不能重复，否则当前碰撞层名称将不能复用|
|UseExistsSurface|boolean |当前物体物理网格是否使用已存在的碰撞层，如果为true，则不创建碰撞层，而是使用已有的碰撞层|
|ExtraRadius|number [float](../types.md)|当使用“幻影事件”构建块时，此值延迟“离开对象”输出激活。“离开对象”输出仅在对象离开幻影加上此额外半径时激活。|
|CollisionEventCallSleep|number [float](../types.md)|设置当前物体碰撞事件调用的休息时间（秒）|
|CollisionID|number [int](../types.md)|指定当前自定义碰撞组ID, 用于声音组碰撞事件的判断|
|IsPhysicalized|boolean |获取当前物体是否已物理化|
|Speed|number [float](../types.md)|获取当前物体的速度|
|SpeedVector|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) |获取当前物体的速度向量|
|RotSpeed|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) |获取当前物体的旋转速度（物体坐标）|
|Mass|number [float](../types.md)|获取或设置当前物体的质量（kg）|
|Fixed|boolean |获取当前物体是是固定物体|
|EnableGravity|boolean |获取当前物体上是否启用了重力|
|EnableCollisionEvent|boolean |获取或设置当前物体上是否启用碰撞事件|
|EnableConstantForce|boolean |是否启用施加在这个物体上的恒力|
|ContractCacheString|string ||

## 方法



### Physicalize()

物理化当前物体



### UnPhysicalize(silently)

取消物理化当前物体


#### 参数


`silently` boolean <br/>是否静默取消物理化，否则此操作将使周围物体激活




### WakeUp()

唤醒物体



### Freeze()

立即冻结对象，忽略速度


#### 返回值

boolean <br/>如果成功则返回true


### IsContact(other)

取消物理化当前物体


#### 参数


`other` [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md) <br/>某个物体



#### 返回值

boolean <br/>如果碰撞返回true，否则返回false


### EnableCollisionDetection()

启用当前物体的碰撞检测



### DisableCollisionDetection()

禁用当前物体的碰撞检测



### ForceUpdateCollisionFilterInfo()

强制更新物体的碰撞信息



### BeamObjectToNewPosition(optimize_for_repeated_calls)

将当前物体的坐标（Unity）设置至物理引擎


#### 参数


`optimize_for_repeated_calls` boolean <br/>如果当前调用是需要重复调用的，例如每帧更新坐标，请设置为true让物理引擎进行优化




### BeamObjectToNewPosition(pos, rot, optimize_for_repeated_calls)

设置物体在物理引擎中的位置


#### 参数


`pos` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>新的位置坐标（世界坐标）

`rot` [Quaternion](https://docs.unity3d.com/ScriptReference/Quaternion.html) <br/>新的旋转

`optimize_for_repeated_calls` boolean <br/>如果当前调用是需要重复调用的，例如每帧更新坐标，请设置为true让物理引擎进行优化




### AddConstantForce(value, dircetion)

添加施加在这个物体上的恒力


#### 参数


`value` number [float](../types.md)<br/>力大小（N）

`dircetion` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>力方向



#### 返回值

[PhysicsConstantForceData](./BallancePhysics.Wapper.PhysicsConstantForceData.md) <br/>返回恒力ID，可使用DeleteConstantForce删除恒力


### AddConstantForceLocalCenter(value, dircetion)

添加施加在这个物体上的恒力(自动以当前物体为位置参照)


#### 参数


`value` number [float](../types.md)<br/>力大小（N）

`dircetion` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>力方向



#### 返回值

[PhysicsConstantForceData](./BallancePhysics.Wapper.PhysicsConstantForceData.md) <br/>返回恒力ID，可使用DeleteConstantForce删除恒力


### AddConstantForceWithPosition(value, dircetion, postion)

取消物理化当前物体


#### 参数


`value` number [float](../types.md)<br/>力大小（N）

`dircetion` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>力方向

`postion` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>施加力的位置



#### 返回值

[PhysicsConstantForceData](./BallancePhysics.Wapper.PhysicsConstantForceData.md) <br/>返回恒力ID，可使用DeleteConstantForce删除恒力


### AddConstantForceWithPositionAndRef(value, dircetion, postion, directionRef, positionRef)

添加施加在这个物体上的恒力


#### 参数


`value` number [float](../types.md)<br/>力大小（N）

`dircetion` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>力方向

`postion` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>施加力的位置

`directionRef` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>力方向参考物体

`positionRef` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>力的位置考物体



#### 返回值

[PhysicsConstantForceData](./BallancePhysics.Wapper.PhysicsConstantForceData.md) <br/>返回恒力ID，可使用DeleteConstantForce删除恒力


### GetConstantForceByID(forceId)

通过恒力ID获取恒力对象


#### 参数


`forceId` number [int](../types.md)<br/>恒力ID



#### 返回值

[PhysicsConstantForceData](./BallancePhysics.Wapper.PhysicsConstantForceData.md) <br/>


### DeleteConstantForce(forceId)

删除施加在这个物体上的恒力


#### 参数


`forceId` number [int](../types.md)<br/>AddConstantForce 返回的ID




### ClearConstantForce()

清除所有施加在这个物体上的恒力



### Impluse(impluse)

给物体中心坐标处施加一个推动


#### 参数


`impluse` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>力的方向和大小（世界坐标系）




### Impluse(pos, impluse)

给物体施加一个推动


#### 参数


`pos` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>推动坐标（世界坐标系）

`impluse` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>力的方向和大小（世界坐标系）




### Torque(rotVec)

给物体施加一个旋转推动


#### 参数


`rotVec` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>该矢量的每个分量表示施加在该对象相关核心轴上的旋转力。




### AddSpeed(speed)

为物体添加速度


#### 参数


`speed` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>速度（世界坐标系）




### ConvertToPhantom()

将物理对象转换为幻影体积



### IsPhantom()

获取当前物体是否是幻影


#### 返回值

boolean <br/>


### IsInsidePhantom(other)

检查某个物体是否在当前幻影物体中


#### 参数


`other` [PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md) <br/>某个物体



#### 返回值

boolean <br/>


### IsMotionEnabled()

获取当前物体上是否启用了motion控制器


#### 返回值

boolean <br/>


### SetMotionEnabled(eanble)

设置motion控制器的启用状态


#### 参数


`eanble` boolean <br/>是否启用




### `静态` GetIdByHandle(handle)

通过物理物体底层引擎指针获取物理物体ID


#### 参数


`handle` [IntPtr](https://docs.microsoft.com/zh-cn/dotnet/api/System.IntPtr) <br/>底层引擎指针



#### 返回值

number [int](../types.md)<br/>如果获取失败，则返回0


### EnableContractEventCallback()

启用当前物体上的接触工具事件发生器



### DisableContractEventCallback()

禁用当前物体上的接触工具事件发生器



### AddCollDetection(col_id, min_speed, max_speed, sleep_afterwards, speed_threadhold)

添加指定层的碰撞工具事件处理


#### 参数


`col_id` number [int](../types.md)<br/>自定义碰撞层ID

`min_speed` number [float](../types.md)<br/>最小速度（m/s）

`max_speed` number [float](../types.md)<br/>最大速度（m/s）

`sleep_afterwards` number [float](../types.md)<br/>碰撞延迟时间（秒），在指定的时间内碰撞不会被重复触发

`speed_threadhold` number [float](../types.md)<br/>速度变换阈值（m/s）




### DeleteCollDetection(col_id)

移除指定层的碰撞工具事件处理


#### 参数


`col_id` number [int](../types.md)<br/>自定义碰撞层ID




### DeleteAllCollDetection()

移除全部碰撞工具事件处理



### AddContractDetection(col_id, time_delay_start, time_delay_end)

添加指定层的接触工具事件处理


#### 参数


`col_id` number [int](../types.md)<br/>自定义碰撞层ID

`time_delay_start` number [float](../types.md)<br/>接触前延时（秒）

`time_delay_end` number [float](../types.md)<br/>接触后延时（秒）




### DeleteContractDetection(col_id)

移除指定层的接触工具事件处理


#### 参数


`col_id` number [int](../types.md)<br/>自定义碰撞层ID




### DeleteAllContractDetection()

移除全部接触工具事件处理

