# Ballance2.Res.GamePathManager 
路径管理器

## 注解

游戏外部资源文件的路径配置与路径转换工具类。

## 字段

|名称|类型|说明|
|---|---|---|
|DEBUG_PACKAGE_FOLDER|string |调试模组包存放路径|
|DEBUG_LEVEL_FOLDER|string |调试关卡存放路径|
## 属性

|名称|类型|说明|
|---|---|---|
|DEBUG_PATH|string |调试路径（输出目录）|
|DEBUG_OUTPUT_PATH|string |输出目录（您在调试时请点击菜单 "Ballance">"开发设置">"Debug Settings" 将其更改为自己调试输出存放目录）|
|DEBUG_PACKAGES_PATH|string |调试路径（模组目录）|
|DEBUG_LEVELS_PATH|string |调试路径（关卡目录）|

## 方法



### `静态` GetResRealPath(type, pathorname, replacePlatform, withFileSheme)

将资源的相对路径转为资源真实路径


#### 参数


`type` string <br/>资源种类（gameinit、core: 核心文件、level：关卡、package：模块）

`pathorname` string <br/>相对路径或名称

`replacePlatform` boolean <br/>是否替换文件路径中的 [Platform] 字符串

`withFileSheme` boolean <br/>



#### 返回值

string <br/>返回资源真实路径


### `静态` GetLevelRealPath(pathorname, withFileSheme)

将关卡资源的相对路径转为关卡资源真实路径


#### 参数


`pathorname` string <br/>关卡的相对路径或名称

`withFileSheme` boolean <br/>



#### 返回值

string <br/>返回资源真实路径


### `静态` ReplacePathInResourceIdentifier(newPath, buf)

Replace Path In Resource Identifier (Identifier:Path:Arg0:Arg1)


#### 参数


`newPath` string <br/>

`buf` [String[]&](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]&) <br/>



#### 返回值

string <br/>

#### 注解

!> 这个函数没有用了。


### `静态` SplitResourceIdentifier(oldIdentifier, outPath)

分割资源标识符


#### 参数


`oldIdentifier` string <br/>资源标识符

`outPath` [String&](https://docs.microsoft.com/zh-cn/dotnet/api/System.String&) <br/>输出资源路径



#### 返回值

[String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>

#### 注解

!> 这个函数没有用了。
