# Ballance2.Game.DistanceChecker 
距离检查器

## 注解

距离检查器，用于测量两个物体的距离，以在指定范围内触发自定义事件。

## 字段

|名称|类型|说明|
|---|---|---|
|Object1|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) |物体1|
|Object2|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) |物体2|
|Diatance|number [float](../types.md)|检查距离|
|CheckEnabled|boolean |是否开启检查|
|CheckTickMin|number [int](../types.md)|每隔多少Tick进行一次检查|
|CheckTickMax|number [int](../types.md)|最大检查Tick|
|OnEnterRange|`回调` GameObjectDelegate(go: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)) |进入范围事件|
|OnLeaveRange|`回调` GameObjectDelegate(go: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)) |离开范围事件|
