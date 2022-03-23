# Ballance2.Services.GameSettingsManager 
系统设置管理器

## 注解

游戏设置管理器，用于管理整个游戏的设置

## 示例

使用方法：
1. 通过 `GameSettingsManager.GetSettings('com.your.packagename')` 获取设置执行器。
2. 设置执行器可调用 `RegisterSettingsUpdateCallback` 订阅用户将设置更改的信息，将设置保存至本地。
3. UI可以通过 `NotifySettingsUpdate` 通知用户更改了设置信息。



## 方法



### `静态` GetSettings(packageName)

获取设置执行器


#### 参数


`packageName` string <br/>设置执行器所使用包名



#### 返回值

[GameSettingsActuator](./Ballance2.Services.GameSettingsActuator.md) <br/>返回设置执行器实例


### `静态` ResetDefaultSettings()

还原默认设置

