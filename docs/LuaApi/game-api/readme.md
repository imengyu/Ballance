# Ballance 游戏 Lua API 总览

这一部分是游戏Lua部分专用的API, C#端是无法访问的。

## 全局对象

所有脚本API功能都是通过类成员实现的。因此，要使用API，您需要访问这些类的实例。

游戏提供了一些全局对象，以便你可快速访问某些组件。

* `Game` -> `table`

  Game对象包含了游戏常用管理器，可以在这里访问到。

  * `Manager` -> [GameManager](../cs-api/class/Ballance2.Services.GameManager) : 获取系统管理器（等同于 `GameManager.Instance`）
  * `Mediator` -> [GameMediator](../cs-api/class/Ballance2.Services.GameMediator) : 获取获取系统中介者
  * `PackageManager` -> [GamePackageManager](../cs-api/class/Ballance2.Services.GamePackageManager) : 获取系统包管理器
  * `UIManager` -> [GameUIManager](../cs-api/class/Ballance2.Services.GameUIManager) : 获取UI管理器
  * `SoundManager` -> [GameSoundManager](../cs-api/class/Ballance2.Services.GameSoundManager) : 获取声音管理器
  * `CorePackage` -> [GamePackage](../cs-api/class/Ballance2.Package.GamePackage) : 获取系统包
  * `GamePlay` -> table : 获取游戏玩模块（也可直接使用全局变量GamePlay获取）
  * `LevelBuilder` -> [LevelBuilder](/LuaApi/game-api/class/LevelBuilder) : 获取关卡加载器模块
  * `CommandServer` -> [GameDebugCommandServer](../cs-api/class/Ballance2.Services.GameDebugCommandServer) : 获取调试命令服务
  * `HighScoreManager` -> [HighScoreManager](/LuaApi/game-api/class/HighscoreManager) : 获取分数管理器

* `GamePlay` -> `table`

  游戏玩模块，是游戏的主要控制模块。

  ?> **注意** 此模块仅可在游玩关卡时访问，在初始化和主菜单中是无法访问到的。

  * `BallManager` -> [BallManager](/LuaApi/game-api/class/BallManager) : 球管理器，负责管理球的注册、运动控制、特殊效果等等。
  * `BallPiecesControll` -> [BallPiecesControll](/LuaApi/game-api/class/BallPiecesControll) : 默认的球碎片抛出和回收器。提供默认的球碎片抛出和回收效果控制。
  * `CamManager` -> [CamManager](/LuaApi/game-api/class/CamManager) : 摄像机管理器，负责游戏中的摄像机运动。
  * `GamePlayManager` -> [GamePlayManager](/LuaApi/game-api/class/GamePlayManager) : 游戏玩管理器，是游戏的主要控制管理器。
  * `GamePreviewManager` -> GamePreviewManager : 关卡预览管理器，负责关卡预览模式时的一些控制行为。
  * `SectorManager` -> [SectorManager](/LuaApi/game-api/class/SectorManager) : 节管理器，负责控制关卡游戏中每个小节机关的状态。
  * `MusicManager` -> [MusicManager](/LuaApi/game-api/class/MusicManager) : 背景音乐管理器，控制游戏中的背景音乐。
  * `TranfoManager` -> TranfoAminControl : 变球器动画控制器。
  * `UFOAnimController` -> UFOAnimController : 游戏结束时的UFO动画控制器。
  * `BallSoundManager` -> [BallSoundManager](/LuaApi/game-api/class/BallSoundManager) : 球声音管理器，负责管理球的滚动碰撞声音。

* `GameUI` -> `table`

  一些游戏内UI组件的全局索引。

## 说明

游戏内所有主模块代码存放在 `Assets\Game\Scripts` 目录下，如果您使用其他编辑器打开你的模组项目时，不要忘记将这个目录添加到你的编辑器Lua搜索目录下，这样在你写模组代码时就可以有代码提示，跳转查看功能。

### 源码目录说明

* Scripts 源码目录
  * Consts 常量定义
  * Data 数据定义
  * Debug 调试用的入口脚本
  * GamePlay 游戏玩模块主要代码
    * Balls 游戏内置球脚本，和碎片控制器
    * ModulBase 游戏机关基类
    * Tranfo 变球器动画控制脚本
  * Highscore 分数管理器代码
  * Intro 游戏进入动画
  * LevelBuilder 关卡加载器
  * MenuLevel 主菜单控制器
  * Moduls 游戏的所有内置机关
  * UI 游戏的UI控制脚本
  * Native C#代码

## Lua改动与函数

因为要适合游戏框架与架构，框架向Lua环境中添加了一些在标准Lua中找不到的函数和库。此外，部分Lua函数有修改，参见下方。

### 不可用的函数

因为安全原因，不能使用如下函数：

* `dofile`
* `getfenv`
* `load`
* `loadfile`
* `loadstring`
* `setfenv`

因为安全原因，不能使用如下模块的某些功能：

* `os.execute`
* `os.getenv`
* `os.setlocale`
* `package.loadlib`
* `package.seeall`
* `package.loaded`
* `package.loaders`

### 有改动的函数和模块

* `require`

    为了适配游戏的模组系统，`require()` 的功能也发生了更改。现在 require 函数只能在MOD或者游戏内核加载代码，**无法直接访问文件系统加载**。

    使用绝对路径时 (“以 `/` 开头”)，默认是加载当前代码所在模组包的代码资源；使用相对路径时，以当前代码脚本路径计算出绝对路径，在模组包中寻找代码资源。这意味着无法从mod之外加载任意文件。

    **不过，框架提供了从其他MOD加载文件的方法：**

    如果代码路径以 `__mod.package.name__/` 开头，例如 `__com.test.mod__/scripts/myscript`, 那么会导入 com.test.mod 模组下的 `/scripts/myscript` 代码资源。

    **另外，一些模块是游戏内置处理的**，无论你的模组中是否有这些同名文件，加载的都是游戏内核的模块，因此，切记不要在你的模组包中使用以下名称的代码文件。

  * 核心模块，Lua 的核心模块由Lua加载，例如使用 `require('io')` 导入 Lua 的 io 模块。核心模块有如下： "string","utf8","io","package","table","math","os","debug", "socket.core", "socket", "table", "string", "coroutine"。
  * 游戏框架所提供模块："json","classic","debugger","vscode-debuggee","mobdebug","dkjson", "Table"
    * json [json.lua](https://github.com/rxi/json.lua) A lightweight JSON library for Lua
    * classic [classic](https://github.com/rxi/classic) Tiny class module for Lua
    * mobdebug [mobdebug](https://github.com/pkulchenko/MobDebug) Remote debugger for Lua.
    * dkjson [dkjson](https://github.com/LuaDist/dkjson)

* `os.exit`

    调用 `os.exit` 相当于调用  `GameManager.Instance:QuitGame()`。

* `io.*`
* `os.remove`
* `os.rename`

    文件操作相关API为例保证安全性，文件路径只能访问私有目录，不能访问玩家的个人文件夹。参见下方Lua文件访问安全。

### 新增的函数

* `loadAsset`

    此函数用于方便您加载模组包中的游戏资源，这个函数与 `GamePackageManager.Get*Asset` 功能基本相同。

    如果代码路径以 `__mod.package.name__/` 开头，例如 `__com.test.mod__/textures/logo.png`, 那么会导入 com.test.mod 模组下的 `/textures/logo.png` 资源。

    否则函数将会在调用代码所属模组中加载资源文件。

?> 注意，返回默认是 `UnityEngine.Object` 类型，需要使用 `SLua.As` 函数转换类型。

## Lua文件访问安全

Lua 中不允许直接访问文件系统，因此游戏提供了一些方法来允许Lua读写本地配置文件, 操作或删除本地目录等。

文件API位于 [GameManager](../cs-api/class/Ballance2.Services.GameManager) 和 [FileUtils](../cs-api/class/Ballance2.Utils.FileUtils) ，其中 GameManager 提供的文件API其实是对 FileUtils 的封装，二者是一样的。

但注意，这些API不允许访问用户文件，只允许访问以下目录：

* 游戏主目录（Windows/linux exe同级与子目录）
* `Application.dataPath`
* `Application.persistentDataPath`
* `Application.temporaryCachePath`
* `Application.streamingAssetsPath`

尝试访问不可访问的文件或者目录将会抛出异常。
