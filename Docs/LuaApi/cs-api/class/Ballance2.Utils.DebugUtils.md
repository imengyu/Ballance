# Ballance2.Utils.DebugUtils 
调试工具类

## 注解

提供了一些有用的调试工具方法。


## 方法



### `静态` GetStackTrace(skipFrame)

获取当前调用堆栈


#### 参数


`skipFrame` number [int](../types.md)<br/>要跳过的帧，为0不跳过



#### 返回值

string <br/>返回调用堆栈详细信息

#### 注解

注意：此函数只能获取 C# 的调用堆栈，不能获取Lua调用堆栈，lua 请使用 debug.traceback()


### `静态` PrintBytes(vs)

格式化的 byte 数组，以十六进制显示


#### 参数


`vs` [Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>byte 数组



#### 返回值

string <br/>返回字符串，请输出或者显示至控制台。


### `静态` PrintCodeWithLine(code)

格式化出带行号的代码


#### 参数


`code` string <br/>代码字符串



#### 返回值

string <br/>返回字符串，请输出或者显示至控制台。


### `静态` PrintCodeWithLine(code)

格式化出带行号的代码


#### 参数


`code` [Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>代码字符串



#### 返回值

string <br/>返回字符串，请输出或者显示至控制台。


### `静态` PrintArrVar(any)

格式化数组为字符串


#### 参数


`any` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>要转换的数组



#### 返回值

string <br/>返回字符串，请输出或者显示至控制台。


### `静态` PrintLuaVarAuto(any, max_level)

打印LUA变量


#### 参数


`any` [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>LUA变量

`max_level` number [int](../types.md)<br/>打印最大层级



#### 返回值

string <br/>返回字符串，请输出或者显示至控制台。

#### 注解

此函数会递归打印 Lua table，对于调试 Lua 是一个非常有用的方法。

提示：如果 lua 嵌套过深，可能会有性能问题，请设置 `max_level` 控制最大打印层级。


#### 示例

打印一个table：
```lua
local testTable = {
  a = 'test,
  b = {
    c = 1.06,
    d = 'test2'
  }
}
Log.D('Test', DebugUtils.PrintLuaVarAuto(testTable))
```



### `静态` CheckDebugParam(index, arr, value, required, defaultValue)

从用户输入的参数数组中检查并获取指定位的字符串参数。


#### 参数


`index` number [int](../types.md)<br/>设置当前需要获取的参数是参数数组的第几个, 索引从 0 开始

`arr` [String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>用户输入的参数数组

`value` [String&](https://docs.microsoft.com/zh-cn/dotnet/api/System.String&) <br/>获取到的参数

`required` boolean <br/>是否必填，必填则如果无输入参数，会返回false

`defaultValue` string <br/>默认值，若非必填且无输入参数，则会返回默认值



#### 返回值

boolean <br/>返回参数是否成功获取

#### 注解

此函数为自定义控制台调试命令获取参数而设计，如果你需要在控制台调试命令中获取参数，这一个非常有用的方法。

如果你设置了 required 必填，而用户没有输入参数，方法会自动输出错误信息。

提示：index 参数在 Lua 中是从 `0` 开始的，不是 `1` 。


#### 示例

注册一个测试控制台指令，获取用户输入的参数：
```lua
GameDebugCommandServer:RegisterCommand('gos', function (keyword, fullCmd, argsCount, args) 
  --第一个返回值表示返回参数是否成功获取
  --第二个返回值是参数的值
  local ox, nx = DebugUtils.CheckIntDebugParam(0, args, Slua.out, true, 0)
  if not ox then return false end
    print('用户输入了：'..tostring(nx))
  return true
end, 1, 'mycommand <count:number> > 测试控制台指令')
```



### `静态` CheckIntDebugParam(index, arr, value, required, defaultValue)

从用户输入的参数数组中检查并获取指定位的整形参数。


#### 参数


`index` number [int](../types.md)<br/>设置当前需要获取的参数是参数数组的第几个, 索引从 0 开始

`arr` [String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>用户输入的参数数组

`value` [Int32&](https://docs.microsoft.com/zh-cn/dotnet/api/System.Int32&) <br/>获取到的参数

`required` boolean <br/>是否必填，必填则如果无输入参数，会返回false

`defaultValue` number [int](../types.md)<br/>默认值，若非必填且无输入参数，则会返回默认值



#### 返回值

boolean <br/>返回参数是否成功获取


### `静态` CheckFloatDebugParam(index, arr, value, required, defaultValue)

从用户输入的参数数组中检查并获取指定位的浮点型参数。


#### 参数


`index` number [int](../types.md)<br/>设置当前需要获取的参数是参数数组的第几个, 索引从 0 开始

`arr` [String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>用户输入的参数数组

`value` [Single&](https://docs.microsoft.com/zh-cn/dotnet/api/System.Single&) <br/>获取到的参数

`required` boolean <br/>是否必填，必填则如果无输入参数，会返回false

`defaultValue` number [float](../types.md)<br/>默认值，若非必填且无输入参数，则会返回默认值



#### 返回值

boolean <br/>返回参数是否成功获取


### `静态` CheckBoolDebugParam(index, arr, value, required, defaultValue)

从用户输入的参数数组中检查并获取指定位的布尔型参数。


#### 参数


`index` number [int](../types.md)<br/>设置当前需要获取的参数是参数数组的第几个, 索引从 0 开始

`arr` [String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>用户输入的参数数组

`value` [Boolean&](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean&) <br/>获取到的参数

`required` boolean <br/>是否必填，必填则如果无输入参数，会返回false

`defaultValue` boolean <br/>默认值，若非必填且无输入参数，则会返回默认值



#### 返回值

boolean <br/>返回参数是否成功获取


### `静态` CheckDoubleDebugParam(index, arr, value, required, defaultValue)

从用户输入的参数数组中检查并获取指定位的双精浮点数类型参数。


#### 参数


`index` number [int](../types.md)<br/>设置当前需要获取的参数是参数数组的第几个, 索引从 0 开始

`arr` [String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>用户输入的参数数组

`value` [Double&](https://docs.microsoft.com/zh-cn/dotnet/api/System.Double&) <br/>获取到的参数

`required` boolean <br/>是否必填，必填则如果无输入参数，会返回false

`defaultValue` number [double](../types.md)<br/>默认值，若非必填且无输入参数，则会返回默认值



#### 返回值

boolean <br/>返回参数是否成功获取


### `静态` CheckStringDebugParam(index, arr, required)

从用户输入的参数数组中检查并获取指定位的字符串参数。


#### 参数


`index` number [int](../types.md)<br/>设置当前需要获取的参数是参数数组的第几个, 索引从 0 开始

`arr` [String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>用户输入的参数数组

`required` boolean <br/>是否必填，必填则如果无输入参数，会返回false



#### 返回值

boolean <br/>返回参数是否成功获取
