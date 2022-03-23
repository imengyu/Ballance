# Ballance2.UI.Core.GameUIMessageCenter 
UI 消息中心，方便Lua层处理UI事件

## 注解

此类提供了简单事件的绑定、数值同步绑定两个功能。

## 字段

|名称|类型|说明|
|---|---|---|
|Name|string ||

## 方法



### `静态` FindGameUIMessageCenter(name)

查找系统中的 UI 消息中心


#### 参数


`name` string <br/>名字



#### 返回值

[GameUIMessageCenter](./Ballance2.UI.Core.GameUIMessageCenter.md) <br/>找到则返回 UI 消息中心实例，否则返回null


### RegisterValueBinder(binder)

注册数据更新器（该方法无需手动调用）


#### 参数


`binder` [GameUIControlValueBinder](./Ballance2.UI.Core.GameUIControlValueBinder.md) <br/>



#### 返回值

boolean <br/>


### UnRegisterValueBinder(binder)

取消注册数据更新器（该方法无需手动调用）


#### 参数


`binder` [GameUIControlValueBinder](./Ballance2.UI.Core.GameUIControlValueBinder.md) <br/>



#### 返回值

boolean <br/>


### SubscribeValueBinder(binderName, callbackFun)

订阅数据更新器


#### 参数


`binderName` string <br/>数据更新器名称

`callbackFun` `回调` GameUIControlValueBinderUserUpdateCallback(value: [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object)) <br/>数据更新回调



#### 返回值

`回调` GameUIControlValueBinderSupplierCallback(value: [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object)) -> [Boolean](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean) <br/>返回一个可供更新数据的回调，调用此回调更新控件上的数据


### GetComponentInstance(binderName)

使用数据更新器获取控件实例


#### 参数


`binderName` string <br/>数据更新器名称



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>


### UnSubscribeValueBinder(binderName, callbackFun)

取消订阅数据更新器


#### 参数


`binderName` string <br/>数据更新器名称

`callbackFun` `回调` GameUIControlValueBinderUserUpdateCallback(value: [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object)) <br/>数据更新回调



#### 返回值

boolean <br/>返回是否成功


### SubscribeEvent(evtName, callBack)

订阅单一消息


#### 参数


`evtName` string <br/>消息名称

`callBack` `回调` VoidDelegate() <br/>消息回调




### UnSubscribeEvent(evtName, callBack)

取消订阅单一消息


#### 参数


`evtName` string <br/>消息名称

`callBack` `回调` VoidDelegate() <br/>消息回调



#### 返回值

boolean <br/>


### NotifyEvent(evtName)

发送单一消息


#### 参数


`evtName` string <br/>消息名称


