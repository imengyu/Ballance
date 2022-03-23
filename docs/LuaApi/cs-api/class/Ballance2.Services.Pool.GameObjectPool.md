# Ballance2.Services.Pool.GameObjectPool 
游戏对象池


## 方法



### NextAvailableObject()

获取一个可用的物体


#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>游戏物体实例，如果没有可用物体，则返回空


### ReturnObjectFromParent(objName)

回退当前池所控根的指定名称子物体至池中


#### 参数


`objName` string <br/>对象名称




### ContainsObjectInParent(objName)

获取当前池所控根的是否存在指定名称子物体


#### 参数


`objName` string <br/>对象名称



#### 返回值

boolean <br/>


### Clear()

清除当前池所控根的子物体



### ReturnObjectToPool(po)

回退物体至池中，注意，只能回退当前池所控根的子物体


#### 参数


`po` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>物体




### ReturnObjectToPool(pool, po)

回退物体至池中


#### 参数


`pool` string <br/>池的名称

`po` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>物体


