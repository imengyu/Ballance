# Ballance2.Base.GameEventEmitter 
游戏事件发射器

## 注解

游戏事件发射器可以让某个类发送一组事件，让许多接受方订阅事件。 

此类非常像 Nodejs 的 EventEmitter，此类功能也是启发自它。


## 字段

|名称|类型|说明|
|---|---|---|
## 属性

|名称|类型|说明|
|---|---|---|
|Name|string |当前事件发射器的名称|

## 方法



### GetEvent(name)

获取指定的事件


#### 参数


`name` string <br/>事件名称



#### 返回值

[GameEventEmitterStorage](./Ballance2.Base.GameEventEmitterStorage.md) <br/>返回事件存储实例


### RegisterEvent(name)

注册一个事件存储实例，如果已经注册，则返回已有实例


#### 参数


`name` string <br/>事件名称



#### 返回值

[GameEventEmitterStorage](./Ballance2.Base.GameEventEmitterStorage.md) <br/>返回事件存储实例


### EmitEvent(name, param)

发射指定名称的事件


#### 参数


`name` string <br/>事件名称

`param` [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>事件的参数




### DeleteEvent(name)

删除指定事件，此操作会删除所有订阅此事件的接收回调


#### 参数


`name` string <br/>事件名称


