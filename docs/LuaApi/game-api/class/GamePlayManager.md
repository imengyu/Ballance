# GamePlayManager

游戏玩管理器，是游戏的主要控制管理器。

## 属性

|名称|类型|说明|
|---|---|---|
|StartLife|number|获取当前关卡起始生命|
|StartPoint|number|获取当前关卡起始分数|
|LevelScore|number|获取当前关卡的基础分数|
|StartBall|string|获取当前的球名称|
|NextLevelName|string|获取当前的球名称|
|CurrentLevelName|string|获取当前的球名称|
|CurrentPoint|number|获取或者设置当前时间点数|
|CurrentLife|number|获取或者设置当前生命数|
|CurrentSector|number|获取当前小节|
|CurrentLevelPass|noolean|获取是否过关|
|CurrentEndWithUFO|noolean|获取当前关卡是否以UFO结尾|
|CanEscPause|noolean|获取当前是否可以按ESC暂停|

## 事件

* EventStart 关卡开始事件
* EventQuit 关卡退出事件
* EventFall 玩家球掉落事件
* EventDeath 关卡球掉落并且没有生命游戏结束事件（不会发出Fall）
* EventResume 继续事件
* EventPause 暂停事件
* EventRestart 重新开始关卡事件
* EventUfoAnimFinish UFO动画完成事件
* EventPass 过关事件
* EventHideBalloonEnd 过关后飞船隐藏事件
* EventAddLife 生命增加事件
* EventAddPoint 分数增加事件

## 方法

### GamePlayManager:NextLevel()

加载下一关

### GamePlayManager:RestartLevel()

重新开始关卡

### GamePlayManager:QuitLevel()

退出关卡

### GamePlayManager:PauseLevel(showPauseUI)

暂停关卡

#### 参数

`showPauseUI` boolean <br/>是否显示暂停界面

### GamePlayManager:ResumeLevel(forceRestart)

继续关卡

#### 参数

`forceRestart` boolean <br/>是否强制重置，会造成当前小节重置，默认false

### GamePlayManager:Fall()

触发球坠落

### GamePlayManager:Pass()

触发过关

### GamePlayManager:ActiveTranfo(tranfo, targetType, color)

激活变球序列

#### 参数

`tranfo` P_Trafo_Base <br/>变球器实例

`targetType` string <br/>要变成的目标球类型

`color` Color <br/>变球器颜色

### GamePlayManager:AddLife()

添加生命

### GamePlayManager:AddPoint(count)

添加时间点数

#### 参数

`count` number|nil <br/>时间点数，默认为10

### GamePlayManager:CreateSkyAndLight(skyBoxPre, customSkyMat, lightColor)

初始化灯光和天空盒

#### 参数

`skyBoxPre` string <br/>A-K 或者空，为空则使用 customSkyMat 材质

`customSkyMat` Material <br/>自定义天空盒材质

`lightColor` Color <br/>灯光颜色

### GamePlayManager:HideSkyAndLight()

隐藏天空盒和关卡灯光
