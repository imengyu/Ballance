# Ballance2.Services.Debug.GameErrorChecker 
错误检查器

## 注解

使用错误检查器获取游戏API的调用错误。

错误检查器还可负责弹出全局错误窗口以检查BUG.

## 属性

|名称|类型|说明|
|---|---|---|
|LastError|number [GameError](./Ballance2.Services.Debug.GameError.md)|获取或设置上一个操作的错误|
|StrictMode|boolean |获取当前是否是严格模式|

## 方法



### `静态` ThrowGameError(code, message)

抛出游戏异常，此操作会直接停止游戏。类似于 Windows 蓝屏功能。


#### 参数


`code` number [GameError](./Ballance2.Services.Debug.GameError.md)<br/>错误代码

`message` string <br/>关于错误的异常信息




### `静态` GetLastErrorMessage()

获取上一个操作的错误说明文字


#### 返回值

string <br/>


### `静态` EnterStrictMode()

进入严格模式。严格模式中如果Lua代码出现异常，则将立即弹出错误提示并停止游戏。



### `静态` QuitStrictMode()

退出严格模式



### `静态` SetLastErrorAndLog(code, tag, message, param)

设置错误码并打印日志


#### 参数


`code` number [GameError](./Ballance2.Services.Debug.GameError.md)<br/>错误代码

`tag` string <br/>日志标签

`message` string <br/>日志信息格式化字符串

`param` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>日志信息格式化参数




### `静态` SetLastErrorAndLog(code, tag, message)

设置错误码并打印日志


#### 参数


`code` number [GameError](./Ballance2.Services.Debug.GameError.md)<br/>错误代码

`tag` string <br/>日志标签

`message` string <br/>日志信息




### `静态` ShowSystemErrorMessage(message)

显示系统错误信息提示提示对话框


#### 参数


`message` string <br/>错误信息




### `静态` ShowScriptErrorMessage(fileName, packName, message)

显示脚本错误信息提示提示对话框


#### 参数


`fileName` string <br/>

`packName` string <br/>

`message` string <br/>错误信息


