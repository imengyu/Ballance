# LevelBuilder

关卡加载器。

## 属性

|名称|类型|说明|
|---|---|---|
|IsPreviewMode|boolean|是否是预览模式|

## 全局事件

全局事件可以使用 `Game.Mediator:RegisterEventHandler` 接受。

* EVENT_LEVEL_BUILDER_JSON_LOADED 关卡的 Level.json 加载完毕事件
* EVENT_LEVEL_BUILDER_MAIN_PREFAB_STANDBY 关卡的主要元件加载完毕事件
* EVENT_LEVEL_BUILDER_START 开始加载关卡事件
* EVENT_LEVEL_BUILDER_UNLOAD_START 开始卸载关卡事件

## 方法

### LevelBuilder:RegisterModul(name, basePrefab)

注册机关

#### 参数

`name` number <br/>机关名称

`basePrefab` GameObject <br/>机关的基础Prefab

### LevelBuilder:UnRegisterModul(name)

取消注册机关

#### 参数

`name` number <br/>机关名称

### LevelBuilder:FindRegisterModul(name)

获取注册的机关，如果没有注册，则返回nil

#### 参数

`name` number <br/>机关名称

#### 返回

`name` table <br/>机关注册信息

|名称|类型|说明|
|---|---|---|
|name|string|机关名称|
|basePrefab|GameObject|机关的基础Prefab|

### LevelBuilder:UpdateLoadProgress(precent)

设置进度条百分比

#### 参数

`precent` number <br/>

### LevelBuilder:UpdateErrStatus(err, statuaCode, errMessage)

设置加载失败状态

#### 参数

`err` boolean <br/>是否失败

`statuaCode` string <br/>状态码

`errMessage` string <br/>错误信息

### LevelBuilder:LoadLevel(name, preview)

开始加载关卡序列

#### 参数

`name` number <br/>关卡文件名

`preview` boolean <br/>是否是预览模式

### LevelBuilder:UnLoadLevel(endCallback)

卸载当前加载的关卡

#### 参数

`endCallback` function|nil <br/>完成回调

### LevelBuilder:RegisterLoadStep(name, callback, type)

注册自定义加载步骤

#### 参数

`name` number <br/>名称

`name` function(levelBuilder, type) <br/>回调， 第一个参数为 levelBuilder 实例；第二个参数是当前步骤的类型。

`type` "pre"|"modul"|"last"|"unload" <br/>回调类型

### LevelBuilder:UnRegisterLoadStep(name)

取消注册自定义加载步骤

#### 参数

`name` number <br/>名称
