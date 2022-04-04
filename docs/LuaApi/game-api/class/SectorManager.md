# SectorManager

节管理器，负责控制关卡游戏中每个小节机关的状态。

## 属性

|名称|类型|说明|
|---|---|---|
|CurrentLevelSectorCount|number|获取当前关卡的小节数|
|CurrentLevelModulCount|number|获取当前关卡的机关数|
|CurrentLevelSectors|`SectorDataStorage[]`|获取当前关卡的小节机关数据|
|CurrentLevelRestPoints|`RestPointsDataStorage[]`|获取当前关卡的出生点数据|

## 事件

* EventSectorChanged 小节更改事件
  * 参数 table
  |名称|类型|说明|
  |---|---|---|
  |sector|number|当前的小节|
  |oldSector|number|之前的小节|
* EventSectorDeactive 小节结束事件
  * 参数 table
  |名称|类型|说明|
  |---|---|---|
  |oldSector|number|结束的小节|
* EventSectorActive 小节激活事件
  * 参数 table
  |名称|类型|说明|
  |---|---|---|
  |sector|number|当前的小节|
  |playCheckPointSound|boolean|是否播放节点音乐（可用于区分是否是用户还是程序激活节）|
* EventResetAllSector 所有节重置事件
  * 参数 table
  |名称|类型|说明|
  |---|---|---|
  |active|boolean|重置机关后是否重新激活|

## 定义

### SectorDataStorage 小节数据存储结构

|名称|值|说明|
|---|---|---|
|moduls|`ModulBase[]`|当前小节的所有机关实例|

### RestPointsDataStorage 出生点数据存储结构

|名称|值|说明|
|---|---|---|
|point|GameObject|出生点占位符对象|
|flame|PC_TwoFlames|火焰机关|

## 方法

### SectorManager:NextSector()

进入下一小节

### SectorManager:SetCurrentSector(sector)

设置当前激活的小节

#### 参数

`sector` number <br/>

### SectorManager:ActiveCurrentSector(playCheckPointSound)

激活当前节的机关

#### 参数

`playCheckPointSound` boolean <br/>是否播放节点音乐

### SectorManager:DeactiveCurrentSector()

禁用当前节的机关

### SectorManager:ResetCurrentSector(active)

重置当前节的机关

#### 参数

`active` boolean <br/>重置机关后是否重新激活

### SectorManager:ResetAllSector(active)

重置所有机关

#### 参数

`active` boolean <br/>重置机关后是否重新激活
