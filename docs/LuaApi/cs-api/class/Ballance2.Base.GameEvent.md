# Ballance2.Base.GameEvent 
全局事件存储类

## 属性

|名称|类型|说明|
|---|---|---|
|EventName|string |获取事件名称|
|EventHandlers|table |获取事件的接收器列表|

## 方法



### Delete()

取消注册此事件。同 GameMediator.UnRegisterGlobalEvent。



### Dispatch(param)

分发此事件。同 GameMediator.DispatchGlobalEvent。


#### 参数


`param` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>


