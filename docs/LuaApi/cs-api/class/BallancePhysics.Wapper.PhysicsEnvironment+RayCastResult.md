# BallancePhysics.Wapper.PhysicsEnvironment+RayCastResult 
射线碰撞结果

## 字段

|名称|类型|说明|
|---|---|---|
|HitObjects|table |碰撞的物体|
|HitDistances|table |碰撞的物体距离射线发射原点的位置|

## 方法



### GetHitObjectsCount()

获取碰撞的物体数量


#### 返回值

number [int](../types.md)<br/>


### GetHitObjectsAt(index)

获取第几个碰撞的物体


#### 参数


`index` number [int](../types.md)<br/>



#### 返回值

[PhysicsObject](./BallancePhysics.Wapper.PhysicsObject.md) <br/>


### GetHitDistancesCount()

获取碰撞的距离射线发射原点的位置信息数量


#### 返回值

number [int](../types.md)<br/>


### GetHitDistancesAt(index)

获取指定第几个碰撞的距离射线发射原点的位置信息


#### 参数


`index` number [int](../types.md)<br/>



#### 返回值

number [float](../types.md)<br/>
