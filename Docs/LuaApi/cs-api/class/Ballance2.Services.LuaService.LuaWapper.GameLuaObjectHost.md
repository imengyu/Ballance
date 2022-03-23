# Ballance2.Services.LuaService.LuaWapper.GameLuaObjectHost 
Lua 脚本承载组件

## 注解


[完整文档请参考这里](SystemModding/lua-class.md)

* 使用方法：
  * 可以直接绑定此组件至你的 Prefab 上，填写 LuaClassName 与 LuaPackageName，Instantiate Prefab 后 GameLuaObjectHost 会自动找到模块并加载 Lua 文件并执行。如果找不到模块或Lua 文件，将会抛出错误。
  * 也可以在 GameMod 中直接调用 RegisterLuaObject 注册一个 Lua 对象
  * 以上两种方法都可以在 GamePackage 中使用 FindLuaObject 找到你注册的 Lua 对象

* 参数引入
  可以在编辑器中设置 LuaInitialVars 添加你想引入的参数，承载组件会自动将的参数设置到你的 Lua脚本 self 上，通过 self.参数名 就可以访问了。
  
* Unity消息
  * ★ 你可以在 Lua 类中添加 Start、Update、Awake、OnDestroy等等方法（也可以不写），承载组件会自动调用这些方法，Lua使用起来就像在C#中使用MonoBehaviour一样, 无需你写其他代码。
  * ★ GameLuaObjectHost默认实现 Awake Start Update FixedUpdate LateUpdate GUI Destroy Enable Disable 这几个事件，如果需要更多事件，在编辑器中 “Lua 类 On * 事件接收器” 这个设置中添加你需要的事件类别，编辑器中下方会显示可用的事件。
  
* 你可以使用这几个变量来访问当前对象上的固定变量：
  * `self.transform` 访问当前对象的 `transform`;
  * `self.monoBehaviour` 访问当前 `GameLuaObjectHost`;
  * `self.gameObject` 访问 `gameObject`;


## 字段

|名称|类型|说明|
|---|---|---|
|TAG|string ||
|Name|string |Lua 对象名字，用于 FindLuaObject 查找|
|LuaClassName|string |获取 Lua的类名（eg MenuLevel）|
|LuaFileName|string |获取 Lua类的文件名（eg MenuLevel.lua）|
|LuaPackageName|string |获取 Lua 类所在的模块包名|
|LuaInitialVars|table |Lua 初始参数|
|LuaPublicVars|table ||
|UpdateDelta|number [int](../types.md)|Update和LateUpdate函数调用的间隔，为0时则不限制，小于0时禁用，大于0时每指定的Tick调用一次|
|FixUpdateDelta|number [int](../types.md)|FixUpdate函数调用的间隔，为0时则不限制，小于0时禁用，大于0时每指定的Tick调用一次|
|LuaInitFinished|`回调` Action() ||
|OnInitLua|`回调` VoidDelegate() ||
## 属性

|名称|类型|说明|
|---|---|---|
|LuaSelf|`SLua.LuaTable` |获取lua self|
|Package|[GamePackage](./Ballance2.Package.GamePackage.md) |获取对应模块包|
|PackageName|string |获取该 Lua 脚本所属包包名|

## 方法



### CreateClass()

立即创建类，如果类已创建，则返回已创建的类


#### 返回值

`SLua.LuaTable` <br/>返回类的table，如果创建失败，则返回nil


### `静态` GetLuaClassFromGameObject(go)

从已附加 GameLuaObjectHost 的 GameObject 上获取Lua类table


#### 参数


`go` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>已附加 GameLuaObjectHost 的 GameObject



#### 返回值

`SLua.LuaTable` <br/>如果找到，则返回类table，如果没找到则返回nIl


### UpdateAllVarToLua()

更新 lua 脚本的所有 InitialVars 至 lua table上



### UpdateAllVarFromLua()

更新所有  lua table上的 InitialVars 至当前脚本上



### UpdateVarToLua(v)

更新指定的初始变量 LuaVarObjectInfo 至 lua 脚本上


#### 参数


`v` [LuaVarObjectInfo](./Ballance2.Services.LuaService.LuaWapper.LuaVarObjectInfo.md) <br/>初始变量名称




### UpdateVarFromLua(v)

从 lua 脚本上获取 lua 变量更新至 LuaVarObjectInfo 


#### 参数


`v` [LuaVarObjectInfo](./Ballance2.Services.LuaService.LuaWapper.LuaVarObjectInfo.md) <br/>初始变量名称




### UpdateVarFromLua(paramName)

将指定名字的 lua 变量更新至 LuaVarObjectInfo 


#### 参数


`paramName` string <br/>变量名称



#### 返回值

boolean <br/>如果没有找到变量，则返回false，否则返回true


### GetLuaClass()

获取当前 Lua 类(等于LuaSelf)


#### 返回值

`SLua.LuaTable` <br/>


### GetLuaFun(funName)

获取当前 Object 的指定函数


#### 参数


`funName` string <br/>函数名



#### 返回值

`SLua.LuaFunction` <br/>返回函数，未找到返回null


### CallLuaFun(funName)

调用lua无参函数


#### 参数


`funName` string <br/>lua函数名称




### CallLuaFunWithParam(funName, pararms)

调用lua函数


#### 参数


`funName` string <br/>lua函数名称

`pararms` [Object[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object[]) <br/>参数



#### 返回值

[Object](https://docs.microsoft.com/zh-cn/dotnet/api/System.Object) <br/>Lua函数返回的对象，如果调用该函数失败，则返回null
