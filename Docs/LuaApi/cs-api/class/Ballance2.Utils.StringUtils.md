# Ballance2.Utils.StringUtils 
字符串工具类

## 注解

提供了许多字符串处理工具函数。


## 方法



### `静态` isNullOrEmpty(text)

检测字符串是否为空


#### 参数


`text` string <br/>要检测的字符串



#### 返回值

boolean <br/>如果字符串是否为空或者 null，则返回 true，否则返回 false


### `静态` IsNullOrWhiteSpace(text)

检测字符串是否为空或空白


#### 参数


`text` string <br/>要检测的字符串



#### 返回值

boolean <br/>如果字符串是否为空、空白，或者 null，则返回 true，否则返回 false


### `静态` IsUrl(text)

检测字符串是否是URL


#### 参数


`text` string <br/>要检测的字符串



#### 返回值

boolean <br/>如果是一个有效的 URL, 则返回 true，否则返回 false


### `静态` IsNumber(text)

检测字符串是否是整数


#### 参数


`text` string <br/>要检测的字符串



#### 返回值

boolean <br/>如果是一个有效的整数字符串, 则返回 true，否则返回 false


### `静态` IsFloatNumber(text)

检测字符串是否是浮点数


#### 参数


`text` string <br/>要检测的字符串



#### 返回值

boolean <br/>如果是一个有效的浮点数字符串, 则返回 true，否则返回 false


### `静态` IsPackageName(text)

检测字符串是否是包名


#### 参数


`text` string <br/>要检测的字符串



#### 返回值

boolean <br/>如果是一个有效的包名字符串, 则返回 true，否则返回 false


### `静态` CompareTwoVersion(version1, version2)

比较两个版本字符串的先后


#### 参数


`version1` string <br/>版本1

`version2` string <br/>版本2



#### 返回值

number [int](../types.md)<br/>1 小于 2 返回 -1 ，大于返回 1，等于返回 0

#### 注解

可以比较两个版本字符串，以 `.` 符号分隔的版本，支持位数不限，例如你可以比较 `1.0`、`1.2.3`、`1.3.3.12` 。


### `静态` ReplaceBrToLine(str)

替换字符串的 <br> 转为换行符


#### 参数


`str` string <br/>要替换的字符串



#### 返回值

string <br/>经过处理的字符串


### `静态` StringToColor(color)

颜色字符串转为 Color


#### 参数


`color` string <br/>要转换的颜色字符串



#### 返回值

[Color](https://docs.unity3d.com/ScriptReference/Color.html) <br/>返回转换的颜色，如果转换失败，默认返回 Color.black

#### 注解

转换的颜色字符串支持 Color 中定义的颜色名称，如下：
* black ： Color.black
* blue ： Color.blue
* clear ： Color.clear
* cyan ： Color.cyan
* gray ： Color.gray
* green ： Color.green
* magenta ： Color.magenta
* red ： Color.red
* white ： Color.white
* yellow ： Color.yellow

或者是十六进制颜色字符串，例如 `#ffffff` 格式，或者是 `255,255,255` 格式的颜色数值字符串。


### `静态` TryConvertStringArrayToValueArray(arr)

尝试把字符串数组转为参数数组


#### 参数


`arr` [String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>要转换的字符串数组



#### 返回值

[Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>转换的参数数组

#### 注解

此函数只能转换基本数据类型：float int bool string。


### `静态` TryConvertStringArrayToValueArray(arr, startIndex)

尝试把字符串数组转为参数数组


#### 参数


`arr` [String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>要转换的字符串数组

`startIndex` number [int](../types.md)<br/>数组转换起始索引



#### 返回值

[Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>转换的参数数组

#### 注解

此函数只能转换基本数据类型：float int bool string。


### `静态` TryConvertStringToValue(value)

尝试转换字符串为参数


#### 参数


`value` string <br/>



#### 返回值

[Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>如果转换失败则返回原字符串

#### 注解

此函数只能转换基本数据类型：float int bool string。


### `静态` ValueArrayToString(arr)

尝试把参数数组转为字符串


#### 参数


`arr` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>要转换的参数数组



#### 返回值

string <br/>转换的字符串


### `静态` TestBytesMatch(inV, outV)

比较Bytes


#### 参数


`inV` [Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>bytes数组1

`outV` [Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>bytes数组2



#### 返回值

boolean <br/>返回两个Bytes是否相等


### `静态` FixUtf8BOM(buffer)

修复UTF8的BOM头


#### 参数


`buffer` [Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>内容数组



#### 返回值

[Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>返回处理完成的数组

#### 注解

此函数会比较数据头是否是 UTF8 BOM 头, 如果是，返回移除头部BOM头后数据，否则返回原数据。


### `静态` MD5String(input)

计算字符串的MD5值


#### 参数


`input` string <br/>字符串



#### 返回值

string <br/>返回MD5值


### `静态` MD5(inputBytes)

计算字节数组的MD5值


#### 参数


`inputBytes` [Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>字节数组



#### 返回值

string <br/>返回MD5


### `静态` GetASCIIBytes(inputBytes)

Encoding.ASCII.GetString 函数包装


#### 参数


`inputBytes` [Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>字节数组



#### 返回值

string <br/>返回解码的字符串


### `静态` GetUtf8Bytes(inputBytes)

Encoding.UTF8.GetString 函数包装


#### 参数


`inputBytes` [Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>字节数组



#### 返回值

string <br/>返回解码的字符串


### `静态` GetUnicodeBytes(inputBytes)

Encoding.Unicode.GetString 函数包装


#### 参数


`inputBytes` [Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>字节数组



#### 返回值

string <br/>返回解码的字符串


### `静态` StringToASCIIBytes(input)

Encoding.ASCII.GetBytes 函数包装


#### 参数


`input` string <br/>输入字符串



#### 返回值

[Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>返回字节数组


### `静态` StringToUtf8Bytes(input)

Encoding.UTF8.GetBytes 函数包装


#### 参数


`input` string <br/>输入字符串



#### 返回值

[Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>返回字节数组


### `静态` StringToUnicodeBytes(input)

Encoding.Unicode.GetBytes 函数包装


#### 参数


`input` string <br/>输入字符串



#### 返回值

[Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>返回字节数组


### `静态` RemoveStringByStringStart(input, find)

在 input 查找 find ，如果找到，则从input找到find的末尾位置截取input至结尾。 


#### 参数


`input` string <br/>要截取的字符串

`find` string <br/>要查找的字符串



#### 返回值

string <br/>返回字节数组
