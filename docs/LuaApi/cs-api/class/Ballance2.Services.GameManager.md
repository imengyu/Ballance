# Ballance2.Services.GameManager 
游戏管理器

## 注解


游戏管理器是整个游戏框架最基础的管理组件，负责游戏的启动、初始化、退出功能。当然，它也提供了很多方便的API供游戏脚本调用。

要获取游戏管理器实例，可以使用
```lua
GameManager.Instance
```


## 属性

|名称|类型|说明|
|---|---|---|
|Instance|[GameManager](./Ballance2.Services.GameManager.md) |获取当前 GameManager 实例|
|GameMediator|[GameMediator](./Ballance2.Services.GameMediator.md) |获取中介者 GameMediator 实例|
|GameSettings|[GameSettingsActuator](./Ballance2.Services.GameSettingsActuator.md) |获取系统设置实例。这是 core 模块的设置实例，与 `GameSettingsManager.GetSettings("core")` 获取的是相同的。|
|GameBaseCamera|[Camera](https://docs.unity3d.com/ScriptReference/Camera.html) |获取基础摄像机。基础摄像机是游戏摄像机还未加载时激活的摄像机，当游戏摄像机加载完成需要切换时，本摄像机应该禁用。|
|GameCanvas|[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) |获取根Canvas|
|GameMainEnv|`SLua.LuaSvr` |获取主虚拟机|
|GameMainLuaState|`SLua.LuaSvr+MainState` |获取游戏全局Lua虚拟机|
|GameDebugCommandServer|[GameDebugCommandServer](./Ballance2.Services.GameDebugCommandServer.md) |获取调试命令控制器。可以获取全局 GameDebugCommandServer 单例，你可以通过它来注册你的调试命令。|
|GameLight|[Light](https://docs.unity3d.com/ScriptReference/Light.html) |获取全局灯光实例。这是一个全局照亮的环境光, 与游戏内的主光源是同一个。|
|GameTimeMachine|[GameTimeMachine](./Ballance2.Services.GameTimeMachine.md) |GameTimeMachine 的一个实例。|
|DebugMode|boolean |获取或者设置当前是否处于开发者模式|

## 方法



### Initialize()




#### 返回值

boolean <br/>


### QuitGame()

请求开始退出游戏流程


#### 注解

调用此 API 将立即开始退出游戏流程，无法取消。


### RestartGame()

重启游戏


#### 注解

**不推荐**使用，本重启函数只能关闭框架然后重新运行一次，会出现很多不可预料的问题，应该退出游戏让用户手动重启。


### `静态` GetSystemService(name)

获取系统服务


#### 参数


`name` string <br/>服务名称



#### 返回值

[GameService](./Ballance2.Services.GameService.md) <br/>返回服务实例，如果没有找到，则返回null

#### 注解

使用 `GameManager.GetSystemService(name)` 来获取其他游戏基础服务组件的实例。

#### 示例


下面的示例演示了如何获取一些系统服务。
```lua
local PackageManager = GameManager.GetSystemService('GamePackageManager') ---@type GamePackageManager
local UIManager = GameManager.GetSystemService('GameUIManager') ---@type GameUIManager
local SoundManager = GameManager.GetSystemService('GameSoundManager') ---@type GameSoundManager
```



### CaptureScreenshot()

进行截图


#### 返回值

string <br/>返回截图保存的路径

#### 注解

截图默认保存至 `{Application.persistentDataPath}/Screenshot/` 目录下。


### SetGameBaseCameraVisible(visible)

设置基础摄像机状态


#### 参数


`visible` boolean <br/>是否显示



#### 注解

基础摄像机是游戏摄像机还未加载时激活的摄像机，当游戏摄像机加载完成需要切换时，应该调用此API禁用本摄像机。


### `静态` EnvBindingCheckCallback()

Lua绑定检查回调


#### 返回值

number [int](../types.md)<br/>


### HideGlobalStartLoading()

隐藏全局初始Loading动画



### ShowGlobalStartLoading()

显示全局初始Loading动画



### InstancePrefab(prefab, name)

实例化预制体


#### 参数


`prefab` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>预制体

`name` string <br/>新对象名称



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回新对象


### InstancePrefab(prefab, parent, name)

实例化预制体


#### 参数


`prefab` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>预制体

`parent` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>父级

`name` string <br/>新对象名称



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回新对象


### InstanceNewGameObject(parent, name)

新建一个新的 GameObject 


#### 参数


`parent` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>父级

`name` string <br/>新对象名称



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回新对象


### InstanceNewGameObject(name)

新建一个新的空 GameObject 


#### 参数


`name` string <br/>新对象名称



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回新对象


### WriteFile(path, append, data)

写入字符串至指定文件


#### 参数


`path` string <br/>要写入的文件路径

`append` boolean <br/>是否追加写入文件，否则为覆盖写入

`data` string <br/>要写入的文件数据



#### 注解

注意：此 API 不能读取用户个人的本地文件。你只能读取游戏目录、游戏数据目录下的文件。


### FileExists(path)

检查文件是否存在


#### 参数


`path` string <br/>要检查的文件路径



#### 返回值

boolean <br/>返回文件是否存在

#### 注解

注意：此 API 不能读取用户个人的本地文件。你只能读取游戏目录、游戏数据目录下的文件。


### DirectoryExists(path)

检查文件是否存在


#### 参数


`path` string <br/>要读取的文件路径



#### 返回值

boolean <br/>返回文件是否存在

#### 注解

注意：此 API 不能读取用户个人的本地文件。你只能读取游戏目录、游戏数据目录下的文件。


### CreateDirectory(path)

创建目录


#### 参数


`path` string <br/>创建目录的路径



#### 注解

注意：此 API 不能读取用户个人的本地文件。你只能读取游戏目录、游戏数据目录下的文件。


### ReadFile(path)

读取文件至字符串


#### 参数


`path` string <br/>要读取的文件路径



#### 返回值

string <br/>返回文件路径

#### 注解

注意：此 API 不能读取用户个人的本地文件。你只能读取游戏目录、游戏数据目录下的文件。


### RemoveFile(path)

删除指定的文件或目录


#### 参数


`path` string <br/>要删除文件的路径




### RemoveDirectory(path)

删除指定的目录


#### 参数


`path` string <br/>要删除目录的路径



#### 注解

注意：此 API 不能读取用户个人的本地文件。你只能读取游戏目录、游戏数据目录下的文件。


### Delay(sec, callback)

延时执行回调


#### 参数


`sec` number [float](../types.md)<br/>延时时长，秒

`callback` `回调` VoidDelegate() <br/>回调



#### 注解

Lua中不推荐使用这个函数，请使用 LuaTimer 

#### 示例


下面的示例演示了演示的使用：
```lua
GameManager.Instance:Delay(1.5, function() 
  --此回调将在1.5秒后被调用。
end)
```



### RequestEnterLogicScense(name)

进入指定逻辑场景


#### 参数


`name` string <br/>场景名字



#### 返回值

boolean <br/>返回是否成功

#### 注解


逻辑场景是类似于 Unity 场景的功能，但是它无需频繁加载卸载。切换逻辑场景实际上是在同一个 Unity 场景中操作，因此速度快。

切换逻辑场景将发出 EVENT_LOGIC_SECNSE_QUIT 与 EVENT_LOGIC_SECNSE_ENTER 全局事件，子模块根据这两个事件来隐藏或显示自己的东西，从而达到切换场景的效果。


#### 示例


下面的示例演示了如何监听进入离开逻辑场景，并显示或隐藏自己的内容：
```lua
GameManager.GameMediator:RegisterEventHandler(thisGamePackage, GameEventNames.EVENT_LOGIC_SECNSE_ENTER, 'Intro', function (evtName, params)
  local scense = params[1]
  if(scense == 'Intro') then 
    --进入场景时显示自己的东西
  end
  return false
end)    
GameManager.GameMediator:RegisterEventHandler(thisGamePackage, GameEventNames.EVENT_LOGIC_SECNSE_QUIT, 'Intro', function (evtName, params)
  local scense = params[1]
  if(scense == 'Intro') then 
    --离开场景时隐藏自己的东西
  end
  return false
end)
```



### GetLogicScenses()

获取所有逻辑场景


#### 返回值

[String[]](https://docs.microsoft.com/zh-cn/dotnet/api/System.String[]) <br/>
