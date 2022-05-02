# Ballance2.Game.SpeedMeter 
速度计

## 注解

速度计组件，用于测量物体的移动速度。

## 字段

|名称|类型|说明|
|---|---|---|
|Target|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) |测速的目标，如果为空，则对当前物体测速|
|MaxSpeed|number [float](../types.md)|相对速度计算的最大值|
|MinSpeed|number [float](../types.md)|相对速度计算的最小值|
|NowRelativeSpeed|number [float](../types.md)|相对速度 0-1, 即 (NowAbsoluteSpeed - MinSpeed) / (MaxSpeed - MinSpeed)|
|NowAbsoluteSpeed|number [float](../types.md)|绝对速度 m/s |
|Callback|`回调` SpeedMeterDelegate(meter: [SpeedMeter](./Ballance2.Game.SpeedMeter.md)) |检查回调, 通过设置这个回调，可以每帧获取一次速度值|
## 属性

|名称|类型|说明|
|---|---|---|
|Enabled|boolean |是否要开始测速|

## 方法



### TestOnce(callback)

手动调用检查一次速度


#### 参数


`callback` `回调` SpeedMeterDelegate(meter: [SpeedMeter](./Ballance2.Game.SpeedMeter.md)) <br/>速度计算回调


