# Ballance2.Utils.PathUtils 
路径字符串工具类, 提供了一些路径的处理工具方法。


## 方法



### `静态` IsAbsolutePath(path)

检测一个路径是否是绝对路径


#### 参数


`path` string <br/>路径



#### 返回值

boolean <br/>是否是绝对路径


### `静态` ReplaceAbsolutePathToRelativePath(path)

替换绝对路径至当前项目相对路径


#### 参数


`path` string <br/>绝对路径



#### 返回值

string <br/>


### `静态` JoinTwoPath(path1, path2)

合并两个相对路径为完整路径，它会处理当前目录（./）和上级目录（../）


#### 参数


`path1` string <br/>主路径

`path2` string <br/>副路径



#### 返回值

string <br/>处理完成的完整路径


### `静态` GetFileNameWithoutExt(path)

获取路径中的文件名（不包括后缀）


#### 参数


`path` string <br/>路径



#### 返回值

string <br/>


### `静态` GetFileName(path)

获取路径中的文件名（包括后缀）


#### 参数


`path` string <br/>路径



#### 返回值

string <br/>


### `静态` Exists(path)

检查文件是否存在


#### 参数


`path` string <br/>文件路径



#### 返回值

boolean <br/>


### `静态` FixFilePathScheme(path)

修复文件路径的 file:/// 前缀


#### 参数


`path` string <br/>路径



#### 返回值

string <br/>返回不带 file:/// 前缀的路径
