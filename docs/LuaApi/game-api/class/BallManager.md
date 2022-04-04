# BallManager

球管理器，负责管理球的注册、运动控制、特殊效果等等。

## 属性

|名称|类型|说明|
|---|---|---|
|CurrentBallName|string|获取当前的球名称|
|CurrentBall|Ball|获取当前的球|
|PushType|number|获取或者设置当前球的推动方向|
|PosFrame|Transform|获取当前球的位置变换实例|
|CanControll|boolean|获取或者设置当前用户是否可以控制球|
|CanControllCamera|boolean|获取或者设置当前用户是否可以控制摄像机|
|KeyStateUp|boolean|获取上升按键状态|
|KeyStateDown|boolean|获取下降按键状态|
|KeyStateForward|boolean|获取前进按键状态|
|KeyStateBack|boolean|获取后退按键状态|
|KeyStateLeft|boolean|获取左按键状态|
|KeyStateRight|boolean|获取右按键状态|

## 事件

* EventBallRegistered 新球注册事件
  * 参数 table
  |名称|类型|说明|
  |---|---|---|
  |ball|[Ball](Ball.md)|球的实例|
  |body|[PhysicsObject](../../cs-api/class/BallancePhysics.Wapper.PhysicsObject.md)|这个球的刚体组件|
  |speedMeter|[SpeedMeter](../../cs-api/class/Ballance2.Game.SpeedMeter.md)|这个球的速度计组件|
* EventBallUnRegister 球删除注册事件
  * 参数 string 取消注册球的名称
* EventCurrentBallChanged 当前球变化事件
  * 参数 string 当前球的名称
* EventNextRecoverPosChanged 球的下一个出生位置变化事件
  * 参数 Vector3 球的下一个出生位置
* EventControllingStatusChanged 球控制状态变化事件
* EventPlaySmoke 播放烟雾事件
* EventPlayLighting 播放闪电事件
* EventFlushBallPush 刷新球推动力状态事件
* EventSetBallPushValue 球推动力数值手动更新事件
  * 参数 table
  |名称|类型|说明|
  |---|---|---|
  |x|number|X轴推动力百分比（[-1,1]）|
  |y|number|Y轴推动力百分比（[-1,1]）|
* EventRemoveAllBallPush 清除球推动力事件

## 定义

### BallPushType 球推动定义

|名称|值|说明|
|---|---|---|
|None|0|无推动|
|Forward|0x2|前|
|Back|0x4|后|
|Left|0x8|左|
|Right|0x10|右|
|Up|0x20|上升|
|Down|0x40|下降|

### BallControlStatus 指定球的控制状态

|名称|值|说明|
|---|---|---|
|NoControl|0|没有控制（无控制、无物理效果，无摄像机跟随）|
|Control|1|正常控制（可控制、物理效果，摄像机跟随）|
|UnleashingMode|2|释放模式（例如球坠落，球仍然有物理效果，但无法控制，摄像机不跟随，但看着球）|
|LockMode|3|锁定模式（例如变换球时，无物理效果，无法控制，但摄像机跟随）|
|FreeMode|4|释放模式2（球仍然有物理效果，但无法控制，摄像机跟随看着球）|
|LockLookMode|5|无物理效果，无法控制，但摄像机不跟随，但看着球|

## 方法

### BallManager:RegisterBall(name, gameObject)

注册一个球，名称不能重复。

?> 在注册球之前请设置球的物理参数，参见 [GamePhysBall](./GamePhysBall)。

#### 参数

`name` string <br/>球名称

`gameObject` GameObject <br/>球游戏对象，必须已经添加到场景中

### BallManager:UnRegisterBall(name)

取消注册球

#### 参数

`name` string <br/>球名称

### BallManager:GetRegisterBall(name)

获取注册了的球信息。

#### 参数

`name` string <br/>球名称

#### 返回

table <br/> 

|名称|类型|说明|
|---|---|---|
|name|string|球名称|
|ball|[Ball](./Ball)|球类实例|
|speedMeter|[SpeedMeter](../../cs-api/class/Ballance2.Game.SpeedMeter)|球的速度计|

### BallManager:GetCurrentBall()

获取当前激活的球

#### 返回

table <br/> 返回结构与 `GetRegisterBall` 一致。

### BallManager:SetCurrentBall(name, status)

#### 参数

`name` string <br/>球名称，不可为空

`status` number|nil  <br/>同时设置新的控制状态,（BallControlStatus） 如果为 nil，则保持之前的控制状态

### BallManager:SetNoCurrentBall()

设置禁用当前正在控制的球

### BallManager:SetControllingStatus(status)

设置当前球的控制的状态

`status` number(BallControlStatus)|nil  <br/>新的状态值（BallControlStatus）, 为 nil 时不改变状态只刷新

### BallManager:SetNextRecoverPos(pos)

设置下一次球出生位置

#### 参数

`pos` Vector3 <br/>出生位置

### BallManager:ResetPeices(typeName)

重置指定球的碎片

#### 参数

`typeName` string <br/>球名称，不可为空

### BallManager:ThrowPeices(typeName)

抛出指定球的碎片

#### 参数

`typeName` string <br/>球名称，不可为空

### BallManager:PlaySmoke(pos)

播放球出生时的烟雾

#### 参数

`pos` Vector3 <br/>放置位置

### BallManager:PlayLighting(pos, smallToBig, lightAnim, callback)

#### 参数

`pos` Vector3 <br/>放置位置

`smallToBig` boolean <br/>是否由小到大

`lightAnim` boolean <br/>是否同时播放灯光效果

`callback` function <br/>播放完成后，会调用这个完成回调

### BallManager:IsLighting()

获取当前是否正在运行球出生闪电效果

#### 返回

boolean <br/>

### BallManager:FastMoveTo(pos, time, callback)

快速将球锁定并移动至目标位置

#### 参数

`pos` Vector3 <br/>目标位置

`time` number <br/>时间 (秒)

`callback` function <br/>移动完成后会调用此回调

### BallManager:FlushBallPush()

刷新球推动方向按键

### BallManager:SetBallPushValue(x, y)

设置球推动方向数值，此函数可以用于摇杆控制

#### 参数

`x` number <br/>X轴推动力百分比（[-1,1]）

`y` number <br/> Y轴推动力百分比（[-1,1]）

### BallManager:RemoveAllBallPush()

去除当前球所有推动方向
