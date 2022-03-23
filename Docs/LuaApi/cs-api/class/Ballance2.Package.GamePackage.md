# Ballance2.Package.GamePackage 
模块包实例

## 注解

这是游戏 Lua 模组的主要承载类，主要负责：模块运行环境初始化卸载、资源读取相关。

## 字段

|名称|类型|说明|
|---|---|---|
|FLAG_CODE_BASE_LOADED|number [int](../types.md)||
|FLAG_CODE_LUA_PACK|number [int](../types.md)||
|FLAG_CODE_CS_PACK|number [int](../types.md)||
|FLAG_CODE_ENTRY_CODE_RUN|number [int](../types.md)||
|FLAG_CODE_UNLOD_CODE_RUN|number [int](../types.md)||
|FLAG_PACK_NOT_UNLOADABLE|number [int](../types.md)||
|FLAG_PACK_SYSTEM_PACKAGE|number [int](../types.md)||
## 属性

|名称|类型|说明|
|---|---|---|
|TAG|string |标签|
|CSharpAssembly|[Assembly](https://docs.microsoft.com/zh-cn/dotnet/api/System.Reflection.Assembly) |C# 程序集, 如果当前模组设置加载了 C# 模块，则可以在这里访问程序集。|
|PackageFilePath|string |获取模块文件路径|
|PackageName|string |获取模块包名|
|PackageVersion|number [int](../types.md)|获取模块版本号|
|BaseInfo|[GamePackageBaseInfo](./Ballance2.Package.GamePackageBaseInfo.md) |获取基础信息|
|UpdateTime|[DateTime](https://docs.microsoft.com/zh-cn/dotnet/api/System.DateTime) |获取模块更新时间|
|SystemPackage|boolean |获取获取是否是系统必须包|
|LoadError|string |获取模块加载错误|
|PackageDef|[XmlDocument](https://docs.microsoft.com/zh-cn/dotnet/api/System.Xml.XmlDocument) |获取模块PackageDef文档|
|AssetBundle|[AssetBundle](https://docs.unity3d.com/ScriptReference/AssetBundle.html) |获取模块AssetBundle|
|TargetVersion|number [int](../types.md)|获取表示模块目标游戏内核版本|
|MinVersion|number [int](../types.md)|获取表示模块可以正常使用的最低游戏内核版本|
|IsCompatible|boolean |获取模块是否兼容当前内核|
|EntryCode|string |获取模块入口代码|
|Type|number [GamePackageType](./Ballance2.Package.GamePackageType.md)|获取模块类型|
|ContainCSharp|boolean |指示本模组是否要加载 CSharp 代码|
|Status|number [GamePackageStatus](./Ballance2.Package.GamePackageStatus.md)|获取模块加载状态|

## 方法



### `静态` GetCorePackage()

获取 Ballance 核心的模块包。核心模块包包名是 core。


#### 返回值

[GamePackage](./Ballance2.Package.GamePackage.md) <br/>

#### 注解

Ballance 核心模块包是 Ballance 游戏的主要模块，所有游戏代码与资源均在这个包中，是所有模组的依赖。


### `静态` GetSystemPackage()

获取系统核心的模块包，包名是 system 。


#### 返回值

[GamePackage](./Ballance2.Package.GamePackage.md) <br/>

#### 注解

系统核心模块包存放了一些系统初始化脚本、工具脚本等等，是 Ballance 核心模块包的依赖。


### IsNotUnLoadable()

获取是否可以卸载


#### 返回值

boolean <br/>


### IsSystemPackage()

获取是否是系统包


#### 返回值

boolean <br/>


### IsEntryCodeExecuted()

获取入口代码是否已经运行过


#### 返回值

boolean <br/>


### IsUnloadCodeExecuted()

获取出口代码是否已经运行过


#### 返回值

boolean <br/>


### SetFlag(flag)

设置当前模块的标志位


#### 参数


`flag` number [int](../types.md)<br/>标志位（GamePackage.FLAG_*）




### GetFlag()

获取当前模块的标志位


#### 返回值

number [int](../types.md)<br/>


### RunPackageExecutionCode()

运行模块初始化代码，模块的 初始化代码 只能运行一次，不能重复运行。


#### 返回值

boolean <br/>返回是否成功


### RunPackageBeforeUnLoadCode()

运行模块卸载回调，模块的 卸载回调 只能运行一次，不能重复运行。


#### 返回值

boolean <br/>返回是否成功


### RequireLuaClass(className)

导入 Lua 类到当前模块虚拟机中


#### 参数


`className` string <br/>类名



#### 返回值

`SLua.LuaFunction` <br/>类创建函数

MissingReferenceException

如果没有在当前模块包中找到类文件或是类创建函数 CreateClass:* ，则抛出 MissingReferenceException 异常。

Exception

如果Lua执行失败，则抛出此异常。

#### 注解

注意，类函数以 `CreateClass:类名` 开头，关于 Lua 类的说明，请参考 [LuaClass](SystemModding/lua-class.md) 。


### RequireLuaFile(fileName)

导入Lua文件到当前模块虚拟机中。不重复导入


#### 参数


`fileName` string <br/>LUA文件名



#### 返回值

[Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>返回执行结果

MissingReferenceException

如果没有在当前模块包中找到Lua文件，则抛出 MissingReferenceException 异常。

Exception

如果Lua执行失败，则抛出此异常。


### RequireLuaFileNoOnce(fileName)

导入Lua文件到当前模块虚拟机中，允许重复导入执行。


#### 参数


`fileName` string <br/>LUA文件名



#### 返回值

[Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>返回执行结果

MissingReferenceException

如果没有在当前模块包中找到Lua文件，则抛出 MissingReferenceException 异常。

Exception

如果Lua执行失败，则抛出此异常。


### RequireLuaFile(otherPack, fileName)

从其他模块导入Lua文件到当前模块虚拟机中。


#### 参数


`otherPack` [GamePackage](./Ballance2.Package.GamePackage.md) <br/>要导入Lua文件所属模块实例

`fileName` string <br/>LUA文件名



#### 返回值

[Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>返回执行结果

MissingReferenceException

如果没有在当前模块包中找到Lua文件，则抛出 MissingReferenceException 异常。

Exception

如果Lua执行失败，则抛出此异常。


### RequireLuaFileNoOnce(otherPack, fileName)

从其他模块导入Lua文件到当前模块虚拟机中，允许重复导入


#### 参数


`otherPack` [GamePackage](./Ballance2.Package.GamePackage.md) <br/>要导入Lua文件所属模块实例

`fileName` string <br/>LUA文件名



#### 返回值

[Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>返回执行结果

MissingReferenceException

如果没有在当前模块包中找到Lua文件，则抛出 MissingReferenceException 异常。

Exception

如果Lua执行失败，则抛出此异常。


### GetLuaFun(funName)

获取当前 模块主代码 的指定函数


#### 参数


`funName` string <br/>函数名



#### 返回值

`SLua.LuaFunction` <br/>返回函数，未找到返回null


### CallLuaFun(funName)

调用模块主代码的lua无参函数


#### 参数


`funName` string <br/>lua函数名称




### TryCallLuaFun(funName)

尝试调用模块主代码的lua无参函数


#### 参数


`funName` string <br/>lua函数名称



#### 返回值

boolean <br/>如果调用成功则返回true，否则返回false


### CallLuaFun(funName, pararms)

调用模块主代码的lua函数


#### 参数


`funName` string <br/>lua函数名称

`pararms` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>参数数组




### CallLuaFun(luaObjectName, funName)

调用指定的GameLuaObjectHost脚本中的lua无参函数


#### 参数


`luaObjectName` string <br/>GameLuaObjectHost脚本名称

`funName` string <br/>lua函数名称




### CallLuaFunWithParam(luaObjectName, funName, pararms)

调用指定的GameLuaObjectHost脚本中的lua函数


#### 参数


`luaObjectName` string <br/>GameLuaObjectHost脚本名称

`funName` string <br/>lua函数名称

`pararms` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>参数



#### 返回值

[Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>Lua函数返回的对象，如果调用该函数失败，则返回null


### RegisterLuaObject(name, gameObject, className)

注册GameLuaObjectHost脚本到物体上


#### 参数


`name` string <br/>GameLuaObjectHost脚本的名称

`gameObject` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>要附加的物体

`className` string <br/>目标代码类名



#### 返回值

[GameLuaObjectHost](./Ballance2.Services.LuaService.LuaWapper.GameLuaObjectHost.md) <br/>返回新注册的 GameLuaObjectHost 实例


### FindLuaObject(name, gameLuaObjectHost)

查找GameLuaObjectHost脚本


#### 参数


`name` string <br/>GameLuaObjectHost脚本的名称

`gameLuaObjectHost` [GameLuaObjectHost&](./Ballance2.Services.LuaService.LuaWapper.GameLuaObjectHost&.md) <br/>输出GameLuaObjectHost脚本



#### 返回值

boolean <br/>返回是否找到对应脚本


### GetTextAsset(pathorname)

读取模块资源包中的文字资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[TextAsset](https://docs.unity3d.com/ScriptReference/TextAsset.html) <br/>返回TextAsset实例，如果未找到，则返回null


### GetPrefabAsset(pathorname)

读取模块资源包中的 Prefab 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回 GameObject 实例，如果未找到，则返回null


### GetTextureAsset(pathorname)

读取模块资源包中的 Texture 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[Texture](https://docs.unity3d.com/ScriptReference/Texture.html) <br/>返回资源实例，如果未找到，则返回null


### GetTexture2DAsset(pathorname)

读取模块资源包中的 Texture2D 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[Texture2D](https://docs.unity3d.com/ScriptReference/Texture2D.html) <br/>返回资源实例，如果未找到，则返回null


### GetSpriteAsset(pathorname)

读取模块资源包中的 Sprite 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[Sprite](https://docs.unity3d.com/ScriptReference/Sprite.html) <br/>返回资源实例，如果未找到，则返回null


### GetMaterialAsset(pathorname)

读取模块资源包中的 Material 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[Material](https://docs.unity3d.com/ScriptReference/Material.html) <br/>返回资源实例，如果未找到，则返回null


### GetPhysicMaterialAsset(pathorname)

读取模块资源包中的 PhysicMaterial 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[PhysicMaterial](https://docs.unity3d.com/ScriptReference/PhysicMaterial.html) <br/>返回资源实例，如果未找到，则返回null


### GetAudioClipAsset(pathorname)

读取模块资源包中的 AudioClip 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[AudioClip](https://docs.unity3d.com/ScriptReference/AudioClip.html) <br/>返回资源实例，如果未找到，则返回null


### GetCodeAsset(pathorname)

读取模块资源包中的Lua代码资源


#### 参数


`pathorname` string <br/>文件名称或路径



#### 返回值

[CodeAsset](./Ballance2.Package.GamePackage+CodeAsset.md) <br/>如果读取成功则返回代码内容，否则返回null


### LoadCodeCSharp(pathorname)

加载模块资源包中的c#代码资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[Assembly](https://docs.microsoft.com/zh-cn/dotnet/api/System.Reflection.Assembly) <br/>如果加载成功则返回已加载的Assembly，否则将抛出异常，若当前环境并不支持加载，则返回null


### AddCustomProp(name, data)

添加自定义数据


#### 参数


`name` string <br/>数据名称

`data` [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>数据值



#### 返回值

[Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>返回数据值


### GetCustomProp(name)

获取自定义数据


#### 参数


`name` string <br/>数据名称



#### 返回值

[Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>返回数据值


### SetCustomProp(name, data)

设置自定义数据


#### 参数


`name` string <br/>数据名称

`data` [Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>数据值



#### 返回值

[Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>返回旧的数据值，如果之前没有该数据，则返回null


### RemoveCustomProp(name)

清除自定义数据


#### 参数


`name` string <br/>数据名称



#### 返回值

boolean <br/>返回是否成功
