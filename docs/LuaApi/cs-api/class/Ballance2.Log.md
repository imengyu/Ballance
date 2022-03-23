# Ballance2.Log 
基础日志类

## 注解

基础日志静态类。此类提供一些静态方可输出日志至控制台或文件，或可注册日志观察者以获取系统输出的日志，供自己输出或处理。

日志观察者使用方法：
```csharp
Log.RegisterLogObserver((level, tag, message, stackTrace) => {
   //捕获Warning和Error等级的日志信息
}, LogLevel.Warning | LogLevel.Error);
```
```lua
Log.RegisterLogObserver(function (level, tag, message, stackTrace)
   --捕获Warning和Error等级的日志信息
end, LuaUtils.Or(LogLevel.Warning, LogLevel.Error));
```

日志使用方法：
* Log.V(tag, message, ...) 打印一些最为繁琐、意义不大的日志信息 
* Log.D(tag, message, ...) 打印一些调试信息
* Log.I(tag, message, ...) 打印一些比较重要的数据，可帮助你分析用户行为数据
* Log.W(tag, message, ...) 打印一些警告信息
* Log.E(tag, message, ...) 打印错误信息

日志格式化输出：

格式化输出是使用 C# `string.Format` 的，参数一致，使用 `{x}` 来代表参数，lua中需要这样调用：
```lua
Log.D('Tesr', 'Test log {0} {1:0.0} {2}', { MyVar1, 0.333333, 'String value' }) 
```

或者你也可以直接在 lua 直接格式化字符串再输出至控制台：
```lua
Log.D('Tesr', string.format('Test log %s %0.2f %s', MyVar1, 0.333333, 'String value')) 
```



## 方法



### `静态` V(tag, message)

打印一些最为繁琐、意义不大的日志信息


#### 参数


`tag` string <br/>标签

`message` string <br/>要打印的日志信息




### `静态` V(tag, format, param)

打印可格式化字符串的日志信息


#### 参数


`tag` string <br/>标签

`format` string <br/>格式化字符串，此字符串格式与 string.Format 格式相同

`param` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>格式化参数, lua 需要传入数组 {} 




### `静态` D(tag, message)

打印一些调试信息


#### 参数


`tag` string <br/>标签

`message` string <br/>要打印的日志信息




### `静态` D(tag, format, param)

打印可格式化字符串的调试信息


#### 参数


`tag` string <br/>标签

`format` string <br/>格式化字符串，此字符串格式与 string.Format 格式相同

`param` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>格式化参数, lua 需要传入数组 {} 




### `静态` I(tag, message)

打印一些信息字符串


#### 参数


`tag` string <br/>标签

`message` string <br/>要打印的日志信息




### `静态` I(tag, format, param)

打印可格式化字符串的信息


#### 参数


`tag` string <br/>标签

`format` string <br/>格式化字符串，此字符串格式与 string.Format 格式相同

`param` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>格式化参数, lua 需要传入数组 {} 




### `静态` W(tag, message)

打印一些警告信息


#### 参数


`tag` string <br/>标签

`message` string <br/>




### `静态` W(tag, format, param)

打印可格式化字符串的警告信息


#### 参数


`tag` string <br/>标签

`format` string <br/>格式化字符串，此字符串格式与 string.Format 格式相同

`param` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>格式化参数, lua 需要传入数组 {} 




### `静态` E(tag, message)

打印错误信息


#### 参数


`tag` string <br/>标签

`message` string <br/>要打印的日志信息




### `静态` E(tag, format, param)

打印可格式化字符串的错误信息


#### 参数


`tag` string <br/>标签

`format` string <br/>格式化字符串，此字符串格式与 string.Format 格式相同

`param` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>格式化参数, lua 需要传入数组 {} 




### `静态` LogWrite(level, tag, message, stackTrace)

手动写入日志


#### 参数


`level` number [LogLevel](./Ballance2.LogLevel.md)<br/>日志等级

`tag` string <br/>标签

`message` string <br/>信息

`stackTrace` string <br/>堆栈信息




### `静态` SendLogsInTemporary()

重新发送暂存区中的日志条目



### `静态` RegisterLogObserver(observer, acceptLevel)

注册日志观察者


#### 参数


`observer` `回调` LogObserver(level: [LogLevel](./Ballance2.LogLevel.md), tag: [String](https://docs.microsoft.com/zh-cn/dotnet/api/System.String), message: [String](https://docs.microsoft.com/zh-cn/dotnet/api/System.String), stackTrace: [String](https://docs.microsoft.com/zh-cn/dotnet/api/System.String)) <br/>观察者回调

`acceptLevel` number [LogLevel](./Ballance2.LogLevel.md)<br/>指定观察者要捕获的日志等级



#### 返回值

number [int](../types.md)<br/>返回大于0的数字表示观察者ID，返回-1表示错误

#### 示例

日志观察者使用方法：
```csharp
Log.RegisterLogObserver((level, tag, message, stackTrace) => {
   //捕获Warning和Error等级的日志信息
}, LogLevel.Warning | LogLevel.Error);
```
```lua
Log.RegisterLogObserver(function (level, tag, message, stackTrace)
   --捕获Warning和Error等级的日志信息
end, LuaUtils.Or(LogLevel.Warning, LogLevel.Error));
```



### `静态` UnRegisterLogObserver(id)

取消注册日志观察者


#### 参数


`id` number [int](../types.md)<br/>观察者ID（由 RegisterLogObserver 返回）




### `静态` GetLogObserver(id)

获取日志观察者


#### 参数


`id` number [int](../types.md)<br/>观察者ID（由 RegisterLogObserver 返回）



#### 返回值

`回调` LogObserver(level: [LogLevel](./Ballance2.LogLevel.md), tag: [String](https://docs.microsoft.com/zh-cn/dotnet/api/System.String), message: [String](https://docs.microsoft.com/zh-cn/dotnet/api/System.String), stackTrace: [String](https://docs.microsoft.com/zh-cn/dotnet/api/System.String)) <br/>如果找到则返回观察者，如果找不到则返回null-1表示错误


### `静态` LogLevelToString(logLevel)

日志等级转为对应字符串


#### 参数


`logLevel` number [LogLevel](./Ballance2.LogLevel.md)<br/>日志等级



#### 返回值

string <br/>


### `静态` GetLogColor(level)

日志等级转为对应字符串


#### 参数


`level` number [LogLevel](./Ballance2.LogLevel.md)<br/>日志等级



#### 返回值

string <br/>返回十六进制颜色字符串，例如 ffffff
