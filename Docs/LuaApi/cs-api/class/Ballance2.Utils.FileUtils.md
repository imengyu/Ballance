# Ballance2.Utils.FileUtils 
文件工具类

## 注解

文件工具类。提供了文件操作相关工具方法。

Lua 中不允许直接访问文件系统，因此此处提供了一些方法来允许Lua读写本地配置文件,操作或删除本地目录等。

但注意，这些API不允许访问用户文件，只允许访问以下目录：
* 游戏主目录（Windows/linux exe同级与子目录）
* Application.dataPath
* Application.persistentDataPath
* Application.temporaryCachePath
* Application.streamingAssetsPath

尝试访问不可访问的目录将会抛出异常。



## 方法



### `静态` TestFileIsZip(file)

检测文件头是不是zip


#### 参数


`file` string <br/>要检测的文件路径



#### 返回值

boolean <br/>如果文件头匹配则返回true，否则返回false

FileAccessException

尝试访问不可访问的目录将会抛出异常。


### `静态` TestFileIsAssetBundle(file)

检测文件头是不是unityFs


#### 参数


`file` string <br/>要检测的文件路径



#### 返回值

boolean <br/>如果文件头匹配则返回true，否则返回false

FileAccessException

尝试访问不可访问的目录将会抛出异常。


### `静态` TestFileHead(file, head)

检测自定义文件头


#### 参数


`file` string <br/>要检测的文件路径

`head` [Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>自定义文件头



#### 返回值

boolean <br/>如果文件头匹配则返回true，否则返回false

FileAccessException

尝试访问不可访问的目录将会抛出异常。


### `静态` WriteFile(path, append, data)

写入字符串至指定文件


#### 参数


`path` string <br/>文件路径

`append` boolean <br/>是否追加写入文件，否则为覆盖写入

`data` string <br/>要写入的文件



FileAccessException

尝试访问不可访问的目录将会抛出异常。


### `静态` FileExists(path)

检查文件是否存在


#### 参数


`path` string <br/>文件路径



#### 返回值

boolean <br/>返回文件是否存在


### `静态` DirectoryExists(path)

检查文件是否存在


#### 参数


`path` string <br/>文件路径



#### 返回值

boolean <br/>返回文件是否存在


### `静态` CreateDirectory(path)

创建目录


#### 参数


`path` string <br/>目录路径



FileAccessException

尝试访问不可访问的目录将会抛出异常。


### `静态` ReadFile(path)

读取文件至字符串


#### 参数


`path` string <br/>文件路径



#### 返回值

string <br/>返回文件内容

FileAccessException

尝试访问不可访问的目录将会抛出异常。


### `静态` ReadAllToBytes(file)

读取文件所有内容为字节数组。注意：此 API 不能读取用户个人的本地文件。


#### 参数


`file` string <br/>文件路径



#### 返回值

[Byte[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Byte[]) <br/>返回字节数组

FileAccessException

尝试访问不可访问的目录将会抛出异常。


### `静态` RemoveFile(path)

删除指定的文件或目录》注意：此 API 不能删除用户个人的本地文件。


#### 参数


`path` string <br/>文件



FileAccessException

尝试访问不可访问的目录将会抛出异常。


### `静态` RemoveDirectory(path)

删除指定的目录》注意：此 API 不能删除用户个人的本地文件。


#### 参数


`path` string <br/>目录的路径



FileAccessException

尝试访问不可访问的目录将会抛出异常。


### `静态` GetBetterFileSize(longFileSize)

把文件大小（字节）按单位转换为可读的字符串


#### 参数


`longFileSize` number [long](../types.md)<br/>文件大小（字节）



#### 返回值

string <br/>可读的字符串，例如2.5M
