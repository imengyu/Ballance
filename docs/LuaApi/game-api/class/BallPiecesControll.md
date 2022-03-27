# BallPiecesControll

默认的球碎片抛出和回收器。提供默认的球碎片抛出和回收效果控制。

## 定义

### BallPiecesData 球碎片数据结构定义

|名称|类型|说明|
|---|---|---|
|bodys|`PhysicsObject[]`|所有的碎片物理体|
|parent|GameObject|父级游戏对象|
|fadeOutTimerID|`number or nil`|淡出延时定时器|
|delayHideTimerID|`number or nil`|隐藏延时定时器|
|throwed|boolean|获取碎片是否已经抛出了|
|fadeObjects|`FadeObject[]`|淡出控制对象|

## 方法

### BallPiecesControll:ThrowPieces(data, pos, minForce, maxForce, timeLive)

开始抛出碎片

#### 参数

`data` BallPiecesData <br/>碎片数据集

`pos` Vector3 <br/>抛出的位置

`minForce` number <br/>推动最小力

`maxForce` number <br/>推动最大力

`timeLive` number <br/>碎片存活时间（30个tick为单位）

### BallPiecesControll:ResetPieces(data)

回收碎片

#### 参数

`data` BallPiecesData <br/>碎片数据集