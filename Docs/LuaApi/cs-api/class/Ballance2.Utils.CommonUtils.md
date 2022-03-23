# Ballance2.Utils.CommonUtils 
通用帮助类，提供了一些工具方法

## 属性

|名称|类型|说明|
|---|---|---|
|NetAvailable|boolean |判断网络是否可用|
|IsWifi|boolean |判断网络是否是无线|

## 方法



### `静态` GenRandomID()

生成随机ID


#### 返回值

number [int](../types.md)<br/>


### `静态` GenAutoIncrementID()

生成自增长ID


#### 返回值

number [int](../types.md)<br/>


### `静态` GenNonDuplicateID()

生成不重复ID


#### 返回值

number [int](../types.md)<br/>


### `静态` RandomFloat(min, max)

生成自增长ID


#### 参数


`min` number [float](../types.md)<br/>最小值

`max` number [float](../types.md)<br/>最大值



#### 返回值

number [float](../types.md)<br/>


### `静态` RandomFloat(max)




#### 参数


`max` number [float](../types.md)<br/>



#### 返回值

number [float](../types.md)<br/>


### `静态` IsArrayNullOrEmpty(arr)

检查数组是否为空


#### 参数


`arr` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>要检查的数组



#### 返回值

boolean <br/>如果数组为null或长度为0，则返回true，否则返回false


### `静态` IsDictionaryNullOrEmpty(arr)

检查 Dictionary 是否为空


#### 参数


`arr` [IDictionary](https://docs.microsoft.com/zh-cn/dotnet/api/System.Collections.IDictionary) <br/>要检查的 Dictionary



#### 返回值

boolean <br/>如果Dictionary为null或长度为0，则返回true，否则返回false


### `静态` GenSameStringArray(val, count)

生成相同的字符串数组


#### 参数


`val` string <br/>字符串

`count` number [int](../types.md)<br/>数组长度



#### 返回值

[String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>


### `静态` CheckParam(param, index, typeName)

检查可变参数


#### 参数


`param` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>可变参数数组

`index` number [int](../types.md)<br/>要检查的参数索引

`typeName` string <br/>目标类型



#### 返回值

boolean <br/>检查类型一致则返回true，否则返回false


### `静态` GetStringArrayFromDictionary(keyValuePairs)

获取 Dictionary 里的string值数组（低性能！）


#### 参数


`keyValuePairs` table <br/>Dictionary



#### 返回值

[String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>


### `静态` ChangeColorAlpha(o, a)

更改颜色Alpha值，其他不变


#### 参数


`o` [Color](https://docs.unity3d.com/ScriptReference/Color.html) <br/>原颜色

`a` number [float](../types.md)<br/>Alpha值



#### 返回值

[Color](https://docs.unity3d.com/ScriptReference/Color.html) <br/>新生成的颜色对象


### `静态` DistanceBetweenTwoObjects(o1, o2)

计算两个物体的距离


#### 参数


`o1` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>物体1

`o2` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>物体2



#### 返回值

number [float](../types.md)<br/>


### `静态` DistanceBetweenTwoPos(o1, o2)

计算两个坐标的距离


#### 参数


`o1` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>坐标1

`o2` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>坐标2



#### 返回值

number [float](../types.md)<br/>


### `静态` IsObjectNull(o)

Lua 判断 UnityEngine.Object 是否为空


#### 参数


`o` [Object](https://docs.unity3d.com/ScriptReference/Object.html) <br/>对象



#### 返回值

boolean <br/>


### `静态` IsValid(v)

Lua 判断 UnityEngine.Vector3 是否有效


#### 参数


`v` [Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) <br/>对象



#### 返回值

boolean <br/>


### `静态` IsValid(v)

Lua 判断 UnityEngine.Vector2 是否有效


#### 参数


`v` [Vector2](https://docs.unity3d.com/ScriptReference/Vector2.html) <br/>对象



#### 返回值

boolean <br/>
