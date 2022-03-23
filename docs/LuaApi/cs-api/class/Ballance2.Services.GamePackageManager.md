# Ballance2.Services.GamePackageManager 
框架模块管理器

## 注解

框架包管理器，用于管理模块包的注册、加载、卸载等流程。一并提供了从模块包中读取资源的相关API。

## 字段

|名称|类型|说明|
|---|---|---|
|SYSTEM_PACKAGE_NAME|string |系统核心模块的包名。系统核心模块包存放了一些系统初始化脚本、工具脚本等等，是 Ballance 核心模块包的依赖。|
|CORE_PACKAGE_NAME|string |获取 Ballance 核心的模块包的包名。Ballance 核心模块包是 Ballance 游戏的主要模块，所有游戏代码与资源均在这个包中，是所有模组的依赖。|
## 属性

|名称|类型|说明|
|---|---|---|
|NoPackageMode|boolean |是否是无模块模式。无模块模式开启时不会加载第三方模组。|

## 方法



### RegisterPackage(packageName)

注册模块


#### 参数


`packageName` string <br/>包名



#### 返回值

table <br/>返回是否加载成功。要获得错误代码，请获取 GameErrorChecker.LastError


### FindRegisteredPackage(packageName)

查找已注册的模块


#### 参数


`packageName` string <br/>包名



#### 返回值

[GamePackage](./Ballance2.Package.GamePackage.md) <br/>返回模块实例，如果未找到，则返回null


### IsPackageEnableLoad(packageName)

检测模块是否用户选择了启用


#### 参数


`packageName` string <br/>包名



#### 返回值

boolean <br/>


### UnRegisterPackage(packageName, unLoadImmediately)

取消注册模块


#### 参数


`packageName` string <br/>包名

`unLoadImmediately` boolean <br/>是否立即卸载



#### 返回值

boolean <br/>返回是否成功


### IsPackageLoading(packageName)

获取模块是否正在加载


#### 参数


`packageName` string <br/>包名



#### 返回值

boolean <br/>


### IsPackageRegistering(packageName)

获取模块是否正在注册


#### 参数


`packageName` string <br/>包名



#### 返回值

boolean <br/>


### IsPackageRegistered(packageName)

获取模块是否注册


#### 参数


`packageName` string <br/>包名



#### 返回值

boolean <br/>


### IsPackageLoaded(packageName)

获取模块是否已加载


#### 参数


`packageName` string <br/>包名



#### 返回值

boolean <br/>


### NotifyAllPackageRun(packageNameFilter)

通知模块运行


#### 参数


`packageNameFilter` string <br/>包名筛选，为“*”时表示所有包，为正则表达式时使用正则匹配包。




### LoadPackage(packageName)

加载模块


#### 参数


`packageName` string <br/>模块包名



#### 返回值

table <br/>返回加载是否成功


### UnLoadPackage(packageName, unLoadImmediately)

卸载模块


#### 参数


`packageName` string <br/>模块包名

`unLoadImmediately` boolean <br/>是否立即卸载，如果为false，此模块将等待至依赖它的模块全部卸载之后才会卸载



#### 返回值

boolean <br/>返回加载是否成功


### FindPackage(packageName)

查找已加载的模块


#### 参数


`packageName` string <br/>模块包名



#### 返回值

[GamePackage](./Ballance2.Package.GamePackage.md) <br/>返回模块实例，如果未找到，则返回null


### CheckRequiredPackage(packageName, ver)

检查指定版本的模块是否加载


#### 参数


`packageName` string <br/>模块包名

`ver` number [int](../types.md)<br/>模块所须最小版本



#### 返回值

boolean <br/>如果已加载并且版本符合，则返回 true，否则返回false


### GetCodeAsset(pathorname, package)

全局读取资源包中的代码资源


#### 参数


`pathorname` string <br/>资源路径

`package` [GamePackage&](./Ballance2.Package.GamePackage&.md) <br/>



#### 返回值

[CodeAsset](./Ballance2.Package.GamePackage+CodeAsset.md) <br/>返回CodeAsset实例，如果未找到，则返回null

RequireFailedException

未找到指定的模块包。

#### 注解

?> 全局读取时您需要填写指定包名，例如 `__com.mymod__/test.lua` 是读取 `com.mymod` 模组下的 test.lua 资源。


### GetTextAsset(pathorname)

读取模块资源包中的文字资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[TextAsset](https://docs.unity3d.com/ScriptReference/TextAsset.html) <br/>返回TextAsset实例，如果未找到，则返回null

RequireFailedException

未找到指定的模块包。

#### 注解

?> 全局读取时您需要填写指定包名，例如 `__com.mymod__/test.txt` 是读取 `com.mymod` 模组下的 test.txt 资源。下方几个读取方法参数也是一样的，不再赘述。


### GetPrefabAsset(pathorname)

读取模块资源包中的 Prefab 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回资源实例，如果未找到，则返回null

RequireFailedException

未找到指定的模块包。


### GetTextureAsset(pathorname)

读取模块资源包中的 Texture 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[Texture](https://docs.unity3d.com/ScriptReference/Texture.html) <br/>返回资源实例，如果未找到，则返回null

RequireFailedException

未找到指定的模块包。


### GetTexture2DAsset(pathorname)

读取模块资源包中的 Texture2D 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[Texture2D](https://docs.unity3d.com/ScriptReference/Texture2D.html) <br/>返回资源实例，如果未找到，则返回null

RequireFailedException

未找到指定的模块包。


### GetSpriteAsset(pathorname)

读取模块资源包中的 Sprite 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[Sprite](https://docs.unity3d.com/ScriptReference/Sprite.html) <br/>返回资源实例，如果未找到，则返回null

RequireFailedException

未找到指定的模块包。


### GetMaterialAsset(pathorname)

读取模块资源包中的 Material 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[Material](https://docs.unity3d.com/ScriptReference/Material.html) <br/>返回资源实例，如果未找到，则返回null

RequireFailedException

未找到指定的模块包。


### GetAudioClipAsset(pathorname)

读取模块资源包中的 AudioClip 资源


#### 参数


`pathorname` string <br/>资源路径



#### 返回值

[AudioClip](https://docs.unity3d.com/ScriptReference/AudioClip.html) <br/>返回资源实例，如果未找到，则返回null

RequireFailedException

未找到指定的模块包。
