# Ballance2.Base.GameEventEmitterStorage 
事件发射器的事件存储类

## 属性

|名称|类型|说明|
|---|---|---|
|Name|string |当前事件的名称|

## 方法



### Emit(obj)

发射当前事件


#### 参数


`obj` [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>事件的参数




### On(fn)

增加事件侦听


#### 参数


`fn` `回调` GameEventEmitterDelegate(obj: [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object)) <br/>事件侦听回调



#### 返回值

[GameEventEmitterHandler](./Ballance2.Base.GameEventEmitterHandler.md) <br/>


### OnWithTag(fn, tag)

增加事件侦听并且设置标签，可以使用标签取消事件侦听


#### 参数


`fn` `回调` GameEventEmitterDelegate(obj: [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object)) <br/>事件侦听回调

`tag` string <br/>指定事件标签



#### 返回值

[GameEventEmitterHandler](./Ballance2.Base.GameEventEmitterHandler.md) <br/>


### Once(fn)

增加单次事件侦听


#### 参数


`fn` `回调` GameEventEmitterDelegate(obj: [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object)) <br/>事件侦听回调



#### 返回值

[GameEventEmitterHandler](./Ballance2.Base.GameEventEmitterHandler.md) <br/>


### Off(fn)

移除事件侦听


#### 参数


`fn` `回调` GameEventEmitterDelegate(obj: [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object)) <br/>事件侦听回调




### OffAllTag(tag)

移除事件指定标签的侦听


#### 参数


`tag` string <br/>指定事件标签




### Clear()

清空当前事件的所有事件侦听

