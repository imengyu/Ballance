# Ballance2.Services.GameSettingsActuator 
设置执行器，用于管理具体的设置条目

## 字段

|名称|类型|说明|
|---|---|---|
|ACTION_UPDATE|number [int](../types.md)|指示当前设置执行器执行更新操作|
|ACTION_LOAD|number [int](../types.md)|指示当前设置执行器执行加载操作|

## 方法



### SetInt(key, value)

设置整型设置条目


#### 参数


`key` string <br/>设置键

`value` number [int](../types.md)<br/>设置值




### GetInt(key, defaultValue)

获取整型设置条目


#### 参数


`key` string <br/>设置键

`defaultValue` number [int](../types.md)<br/>未找到设置时返回的默认值



#### 返回值

number [int](../types.md)<br/>


### SetString(key, value)

设置字符串型设置条目


#### 参数


`key` string <br/>设置键

`value` string <br/>设置值




### GetString(key, defaultValue)

获取字符串型设置条目


#### 参数


`key` string <br/>设置键

`defaultValue` string <br/>未找到设置时返回的默认值



#### 返回值

string <br/>


### SetFloat(key, value)

设置浮点型设置条目


#### 参数


`key` string <br/>设置键

`value` number [float](../types.md)<br/>设置值




### GetFloat(key, defaultValue)

获取浮点型设置条目


#### 参数


`key` string <br/>设置键

`defaultValue` number [float](../types.md)<br/>未找到设置时返回的默认值



#### 返回值

number [float](../types.md)<br/>


### SetBool(key, value)

设置布尔型设置条目


#### 参数


`key` string <br/>设置键

`value` boolean <br/>设置值




### GetBool(key, defaultValue)

获取布尔型设置条目


#### 参数


`key` string <br/>设置键

`defaultValue` boolean <br/>未找到设置时返回的默认值



#### 返回值

boolean <br/>


### RequireSettingsLoad(groupName)

通知设置组加载更新


#### 参数


`groupName` string <br/>组名称，为*表示所有




### NotifySettingsUpdate(groupName)

通知设置组更新


#### 参数


`groupName` string <br/>组名称，为*表示所有




### RegisterSettingsUpdateCallback(groupName, callback)

注册设置组更新回调


#### 参数


`groupName` string <br/>组名称

`callback` `回调` GameSettingsCallback(groupName: [String](https://docs.microsoft.com/zh-cn/dotnet/api/System.String), action: [Int32](https://docs.microsoft.com/zh-cn/dotnet/api/System.Int32)) -> [Boolean](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean) <br/>



#### 返回值

number [int](../types.md)<br/>返回回调ID,可用于取消注册回调


### UnRegisterSettingsUpdateCallback(id)

取消注册设置组更新回调


#### 参数


`id` number [int](../types.md)<br/>回调ID


