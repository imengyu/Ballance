# Ballance2.UI.Core.GameUIControlMessageSender 
UI事件发送器。该绑定器用在需要发送事件的UI控件上

## 注解

使用方法，此类用于绑定在某个需要发送简单事件的控件上，例如某个按钮，先设置 `MessageCenterName` 也就是消息中心的名称，脚本启动后会自动查找。或者直接填写MessageCenter实例，二选一即可。然后你可以在编辑器中设置onClick的接收器，在里面填写调用函数，对象是GameUIControlMessageSender，调用 `NotifyEvent`，参数填写你在 GameUIMessageCenter 中由代码订阅过的事件名称（SubscribeEvent）。在以后，点击按钮将会自动把事件发送至 MessageCenter，代码就可以接受了。

## 字段

|名称|类型|说明|
|---|---|---|
|MessageCenterName|string |指定对应UI消息中心名字|
|MessageCenter|[GameUIMessageCenter](./Ballance2.UI.Core.GameUIMessageCenter.md) ||

## 方法



### NotifyEvent(name)

发送事件至消息中心


#### 参数


`name` string <br/>事件名称


