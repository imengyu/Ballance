# BallSoundManager

球声音管理器，负责管理球的滚动碰撞声音。

## 定义

### BallSoundCollData 球声音碰撞信息定义

|名称|类型|说明|
|---|---|---|
|MinSpeed|number|碰撞检测最低速度|
|MaxSpeed|number|碰撞检测最高速度|
|SleepAfterwards|number|碰撞检测延时|
|SpeedThreadhold|number|碰撞检测最低速度阈值|
|TimeDelayStart|number|滚动检测起始延时|
|TimeDelayEnd|number|滚动检测末尾延时|
|HasRollSound|boolean|是否有滚动声音|
|RollSoundName|string|滚动声音名称，放在球的RollSound信息中|
|HitSoundName|string|撞击声音名称，放在球的HitSound信息中|

## 方法

### BallSoundManager:AddSoundCollData(colId, data)

添加球碰撞声音组

#### 参数

`colId` number <br/>自定义碰撞层ID，为防止重复，请使用 `GetSoundCollIDByName` 使用名称获取ID

`data` BallSoundCollData <br/>碰撞数据

#### 返回

BallSoundCollData<br/>返回碰撞数据

### BallSoundManager:RemoveSoundCollData(colId) 

移除球碰撞声音组

?> 注意，不会移除激活中球的声音组，需要等到球下一次激活时生效，因此不建议在游戏中使用此函数

#### 参数

`colId` number <br/>自定义碰撞层ID

### 通过名称分配一个可用的声音组ID, 如果名称存在，则返回同样的ID

#### 参数

`name` string <br/>自定义声音组名称

#### 返回

number<br/>声音组ID

### BallSoundManager:StopAllSound()

强制停止所有球声音
