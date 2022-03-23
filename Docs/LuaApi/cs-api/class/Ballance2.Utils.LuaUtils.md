# Ballance2.Utils.LuaUtils 
Lua 工具类。部分函数为 C# 设计，在Lua端调用可能会有性能问题。


## 方法



### `静态` BooleanToString(param)

布尔值转为字符串


#### 参数


`param` boolean <br/>布尔值



#### 返回值

string <br/>字符串 "true" 或者 "false"


### `静态` Vector3ToString(param)

Vector3转为字符串表示


#### 参数


`param` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>



#### 返回值

string <br/>


### `静态` Vector4ToString(param)

Vector4转为字符串表示


#### 参数


`param` [Vector4](https://docs.unity3d.com/ScriptReference/Vector4.html) <br/>



#### 返回值

string <br/>


### `静态` Vector2ToString(param)

Vector2转为字符串表示


#### 参数


`param` [Vector2](https://docs.unity3d.com/ScriptReference/Vector2.html) <br/>



#### 返回值

string <br/>


### `静态` StringToBool(param)

字符串转为布尔值


#### 参数


`param` string <br/>字符串 "true" 或者 "false"



#### 返回值

boolean <br/>


### `静态` StringToKeyCode(param)

字符串转为KeyCode


#### 参数


`param` string <br/>字符串



#### 返回值

number [KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html)<br/>


### `静态` HTMLStringToColor(htmlColor)

将十六进制颜色字符串转为颜色实例


#### 参数


`htmlColor` string <br/>



#### 返回值

[Color](https://docs.unity3d.com/ScriptReference/Color.html) <br/>如果转换失败，则返回黑色。

#### 注解

?> 此函数是 `ColorUtility.TryParseHtmlString` 函数的封装。


### `静态` And(a, b)

Lua按位与函数


#### 参数


`a` number [int](../types.md)<br/>左值

`b` number [int](../types.md)<br/>右值



#### 返回值

number [int](../types.md)<br/>

#### 注解

?> 此函数存在跨语言调用，建议不要在 Update 中频繁调用，可能会存在性能问题。


### `静态` Or(a, b)

Lua按位或函数


#### 参数


`a` number [int](../types.md)<br/>左值

`b` number [int](../types.md)<br/>右值



#### 返回值

number [int](../types.md)<br/>

#### 注解

?> 此函数存在跨语言调用，建议不要在 Update 中频繁调用，可能会存在性能问题。


### `静态` Xor(a, b)

Lua按位异或函数


#### 参数


`a` number [int](../types.md)<br/>左值

`b` number [int](../types.md)<br/>右值



#### 返回值

number [int](../types.md)<br/>

#### 注解

?> 此函数存在跨语言调用，建议不要在 Update 中频繁调用，可能会存在性能问题。


### `静态` Not(a)

Lua按位非函数


#### 参数


`a` number [int](../types.md)<br/>左值



#### 返回值

number [int](../types.md)<br/>

#### 注解

?> 此函数存在跨语言调用，建议不要在 Update 中频繁调用，可能会存在性能问题。


### `静态` LeftMove(a, b)

Lua按左移函数


#### 参数


`a` number [int](../types.md)<br/>左值

`b` number [int](../types.md)<br/>右值



#### 返回值

number [int](../types.md)<br/>

#### 注解

?> 此函数存在跨语言调用，建议不要在 Update 中频繁调用，可能会存在性能问题。


### `静态` RightMove(a, b)

Lua按右移函数


#### 参数


`a` number [int](../types.md)<br/>左值

`b` number [int](../types.md)<br/>右值



#### 返回值

number [int](../types.md)<br/>

#### 注解

?> 此函数存在跨语言调用，建议不要在 Update 中频繁调用，可能会存在性能问题。


### CreateWaitUntil(f)

创建 WaitUntil 实例


#### 参数


`f` `回调` BoolDelegate() -> [Boolean](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean) <br/>回调



#### 返回值

[WaitUntil](https://docs.unity3d.com/ScriptReference/WaitUntil.html) <br/>

#### 注解

?> Lua 中使用 WaitUntil 可能存在性能问题，不推荐使用。


### CreateWaitWhile(f)

创建 WaitWhile 实例


#### 参数


`f` `回调` BoolDelegate() -> [Boolean](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean) <br/>回调



#### 返回值

[WaitWhile](https://docs.unity3d.com/ScriptReference/WaitWhile.html) <br/>

#### 注解

?> Lua 中使用 WaitWhile 可能存在性能问题，不推荐使用。
