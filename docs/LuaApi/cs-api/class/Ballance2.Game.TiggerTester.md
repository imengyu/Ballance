# Ballance2.Game.TiggerTester 
Tigger检查脚本

## 注解

此脚本是对 OnTriggerEnter 和 OnTriggerExit 的封装，方便在 Lua 中的触发器检测。

## 字段

|名称|类型|说明|
|---|---|---|
|onTriggerEnter|`回调` OnTiggerTesterEventCallback(self: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html), other: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)) |当有碰撞体进入当前触发器时，发送此事件（OnTriggerEnter）|
|onTriggerExit|`回调` OnTiggerTesterEventCallback(self: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html), other: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)) |当有碰撞体离开当前触发器时，发送此事件（OnTriggerExit）|
