# Ballance2.Game.Utils.SmoothFly 
平滑移动脚本

## 注解

平滑移动脚本，可将物体平滑移动至指定目标

## 示例


你可以先在需要平滑移动的物体上绑定此脚本，然后设置平滑移动目标，开启，随后在 `ArrivalCallback` 中就可以接收移动完成事件，例如：
```lua
local smoothFly = gameObject:AddComponent(SmoothFly)
smoothFly.TargetTransform = 移动目标
smoothFly.Fly = true --开启移动
smoothFly.ArrivalDiatance = function() 
  --移动结束时执行某些功能
end
```


## 字段

|名称|类型|说明|
|---|---|---|
|TargetTransform|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) |设置平滑移动目标变换|
|TargetPos|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) |设置平滑移动目标位置|
|Fly|boolean |设置是否开启平滑移动|
|Time|number [float](../types.md)|设置平滑移动时间（秒）|
|MaxSpeed|number [float](../types.md)|设置最大速度|
|CurrentVelocity|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) |获取当前的速度|
|StopWhenArrival|boolean |设置是否达到目标时停止|
|ArrivalDiatance|number [float](../types.md)|设置达到目标判断阈值|
|ArrivalCallback|`回调` CallbackDelegate(fly: [SmoothFly](./Ballance2.Game.Utils.SmoothFly.md)) |设置达到目标时的事件回调|
|Type|number [SmoothFlyType](./Ballance2.Game.Utils.SmoothFlyType.md)|设置平滑移动的类型|
