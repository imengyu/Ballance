# Ballance2.Services.GameMediator 
游戏中介者

## 注解

游戏中介者管理器，用于游戏中央事件的转发与处理。

中介者提供了事件交互方法：

* 全局事件：发送整体全局事件，让多个接受方接受事件。
* 单一事件：发送单向全局事件，让一个接受方接受事件。 
* 事件发射器：某个类发送一组事件，让许多接受方订阅事件。 

?> **提示：** 单一事件与全局数据无须手动使用 `RegisterGlobalEvent`/`RegisterSingleEvent` 注册，你可以直接执行相关方法，例如 `DispatchGlobalEvent` 等等，
如果事件没有注册，它会自动调用注册。



## 属性

|名称|类型|说明|
|---|---|---|
|Events|table ||

## 方法



### RegisterEventEmitter(name)

注册事件发射器


#### 参数


`name` string <br/>事件发射器名称



#### 返回值

[GameEventEmitter](./Ballance2.Base.GameEventEmitter.md) <br/>


### UnRegisterEventEmitter(name)

取消注册事件发射器


#### 参数


`name` string <br/>事件发射器名称




### RegisterSingleEvent(evtName)

注册单一事件


#### 参数


`evtName` string <br/>事件名称



#### 返回值

boolean <br/>


### UnRegisterSingleEvent(evtName)

取消注册单一事件


#### 参数


`evtName` string <br/>事件名称



#### 返回值

boolean <br/>


### IsSingleEventRegistered(evtName)

获取单一事件是否注册


#### 参数


`evtName` string <br/>事件名称



#### 返回值

boolean <br/>是否注册


### CheckSingleEventAttatched(evtName)

检测单一事件是否被接收者附加


#### 参数


`evtName` string <br/>事件名称



#### 返回值

boolean <br/>返回是否附加


### DelayedNotifySingleEvent(evtName, delayeSecond, pararms)

延时通知单一事件


#### 参数


`evtName` string <br/>事件名称

`delayeSecond` number [float](../types.md)<br/>延时时长，单位秒

`pararms` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>事件参数



#### 返回值

boolean <br/>返回是否成功


### NotifySingleEvent(evtName, pararms)

通知单一事件


#### 参数


`evtName` string <br/>事件名称

`pararms` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>事件参数



#### 返回值

boolean <br/>返回是否成功


### SubscribeSingleEvent(package, evtName, name, gameHandlerDelegate)

订阅全局单一事件


#### 参数


`package` [GamePackage](./Ballance2.Package.GamePackage.md) <br/>所属包

`evtName` string <br/>事件名称

`name` string <br/>服务名称

`gameHandlerDelegate` `回调` GameEventHandlerDelegate(evtName: [String](https://docs.microsoft.com/zh-cn/dotnet/api/System.String), pararms: [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[])) -> [Boolean](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean) <br/>回调



#### 返回值

[GameHandler](./Ballance2.Base.Handler.GameHandler.md) <br/>返回接收器实例，如果失败，则返回null，具体请查看LastError


### UnsubscribeSingleEvent(package, evtName, gameHandler)

取消订阅全局单一事件


#### 参数


`package` [GamePackage](./Ballance2.Package.GamePackage.md) <br/>所属包

`evtName` string <br/>事件名称

`gameHandler` [GameHandler](./Ballance2.Base.Handler.GameHandler.md) <br/>注册的处理器实例



#### 返回值

boolean <br/>返回是否成功


### RegisterGlobalEvent(evtName)

注册事件


#### 参数


`evtName` string <br/>事件名称



#### 返回值

[GameEvent](./Ballance2.Base.GameEvent.md) <br/>


### UnRegisterGlobalEvent(evtName)

取消注册事件


#### 参数


`evtName` string <br/>事件名称



#### 返回值

boolean <br/>


### IsGlobalEventRegistered(evtName)

获取事件是否注册


#### 参数


`evtName` string <br/>事件名称



#### 返回值

boolean <br/>是否注册


### IsGlobalEventRegistered(evtName, e)

获取事件是否注册，如果已注册，则返回实例


#### 参数


`evtName` string <br/>事件名称

`e` [GameEvent&](./Ballance2.Base.GameEvent&.md) <br/>返回的事件实例



#### 返回值

boolean <br/>是否注册


### GetRegisteredGlobalEvent(evtName)

获取事件实例


#### 参数


`evtName` string <br/>事件名称



#### 返回值

[GameEvent](./Ballance2.Base.GameEvent.md) <br/>返回的事件实例


### DelayedDispatchGlobalEvent(evtName, delayeSecond, pararms)

延时执行事件分发


#### 参数


`evtName` string <br/>

`delayeSecond` number [float](../types.md)<br/>延时时长，单位秒

`pararms` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>事件参数



#### 返回值

boolean <br/>返回已经发送的接收器个数


### DispatchGlobalEvent(gameEvent, pararms)

执行事件分发


#### 参数


`gameEvent` [GameEvent](./Ballance2.Base.GameEvent.md) <br/>事件实例

`pararms` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>事件参数



#### 返回值

number [int](../types.md)<br/>返回已经发送的接收器个数


### DispatchGlobalEvent(evtName, pararms)

执行事件分发


#### 参数


`evtName` string <br/>事件名称

`pararms` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>事件参数



#### 返回值

number [int](../types.md)<br/>返回已经发送的接收器个数


### RegisterEventHandler(package, evtName, name, gameHandlerDelegate)

游戏中注册全局事件接收器介者


#### 参数


`package` [GamePackage](./Ballance2.Package.GamePackage.md) <br/>所属包

`evtName` string <br/>事件名称

`name` string <br/>接收器名字

`gameHandlerDelegate` `回调` GameEventHandlerDelegate(evtName: [String](https://docs.microsoft.com/zh-cn/dotnet/api/System.String), pararms: [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[])) -> [Boolean](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean) <br/>



#### 返回值

[GameHandler](./Ballance2.Base.Handler.GameHandler.md) <br/>返回注册的处理器，可使用这个处理器取消注册对应事件


### UnRegisterEventHandler(evtName, handler)

取消注册全局事件接收器


#### 参数


`evtName` string <br/>事件名称

`handler` [GameHandler](./Ballance2.Base.Handler.GameHandler.md) <br/>注册的处理器实例


