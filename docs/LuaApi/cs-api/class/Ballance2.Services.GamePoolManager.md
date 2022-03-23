# Ballance2.Services.GamePoolManager 
对象池管理器，可以注册、管理对象池


## 方法



### CreatePool(poolName, initSize, maxSize, prefab)

创建游戏对象池


#### 参数


`poolName` string <br/>池名称

`initSize` number [int](../types.md)<br/>

`maxSize` number [int](../types.md)<br/>最大大小

`prefab` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>



#### 返回值

[GameObjectPool](./Ballance2.Services.Pool.GameObjectPool.md) <br/>返回游戏资源池


### GetPool(poolName)

获取指定名称游戏对象池


#### 参数


`poolName` string <br/>池名称



#### 返回值

[GameObjectPool](./Ballance2.Services.Pool.GameObjectPool.md) <br/>返回游戏资源池，如果未找到，则返回null


### Get(poolName)

在指定名称游戏对象池中获取可用对象


#### 参数


`poolName` string <br/>池名称



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回游戏资源，如果未找到，或者无可用实例，则返回null


### Release(poolName, go)

在指定名称游戏对象池中回退对象


#### 参数


`poolName` string <br/>池名称

`go` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>对象


