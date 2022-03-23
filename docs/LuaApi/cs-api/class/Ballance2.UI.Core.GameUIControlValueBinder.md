# Ballance2.UI.Core.GameUIControlValueBinder 
UI控件数据绑定器。该绑定器用在需要绑定的UI控件上

## 字段

|名称|类型|说明|
|---|---|---|
|MessageCenterName|string |指定对应UI消息中心名字|
|Name|string |指定绑定器的名称，可在UI消息中心使用该名称查找|
|SolveInLua|boolean |指定当前绑定器是否在Lua中处理，需在UI消息中心设置Lua处理函数|
|UserUpdateCallbacks|table ||
|BinderSupplierCallback|`回调` GameUIControlValueBinderSupplierCallback(value: [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object)) -> [Boolean](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean) ||
|MessageCenter|[GameUIMessageCenter](./Ballance2.UI.Core.GameUIMessageCenter.md) ||

## 方法



### NotifyUserUpdate(newval)

通知UI更新事件


#### 参数


`newval` [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>新的数值


