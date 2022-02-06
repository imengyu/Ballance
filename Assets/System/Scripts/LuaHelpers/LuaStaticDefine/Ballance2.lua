
---@class Ballance2GlobalNamespace
local Ballance2GlobalNamespace={ }
---@class UnityEngineGlobalNamespace
local UnityEngineGlobalNamespace={ }
---@class UnityEngineUIGlobalNamespace
local UnityEngineUIGlobalNamespace={ }
local JetBrainsGlobalNamespace={ }
local UnityGlobalNamespace={ }

---GameLuaObjectHost的自定义lua类固定结构
---@class GameLuaObjectHostClass : ClassicObject
---@field public transform Transform 获取Transform
---@field public monoBehaviour GameLuaObjectHost 获取当前GameLuaObjectHost
---@field public luaClass table 获取当前GameLuaObjectHost上绑定的lua类table
---@field public gameObject GameObject 获取GameObject
---@field public actionStore GameActionStore 访问ActionStore
---@field public package GamePackage 访问当前脚本所在模块包
local GameLuaObjectHostClass={ }

---Ballance2 C# 全局命名空间
Ballance2 = Ballance2GlobalNamespace
UnityEngine = UnityEngineGlobalNamespace
UnityEngine.UI = UnityEngineUIGlobalNamespace

---等待协程
---@param wait any
UnityEngine.Yield = function (wait) end

JetBrains = JetBrainsGlobalNamespace
Unity = UnityGlobalNamespace
---创建类接口
CreateClass = {}
BallancePhysics = {}
BallancePhysics.Wapper = {}
Ballance2.Base = {}
Ballance2.Log = {}
Ballance2.Entry = {}
Ballance2.Services = {}
Ballance2.Services.Debug = {}
Ballance2.Services.I18N = {}
Ballance2.Services.LuaService = {}
Ballance2.Services.LuaService.LuaWapper = {}
Ballance2.Services.LuaService.LuaWapper.GameLuaWapperEvents = {}
Ballance2.Package = {}
Ballance2.Res = {}
Ballance2.Config.Settings = {}
Ballance2.UI = {}
Ballance2.UI.Core = {}
Ballance2.UI.Core.Controls = {}
Ballance2.UI.Utils.Core.Controls = {}
Ballance2.Utils = {}
Ballance2.Config = {}
Ballance2.Game = {}
Ballance2.Game.Utils = {}
Ballance2.Utils = {}
---@diagnostic disable-next-line: lowercase-global
winapi={}

---模块入口结构
---@class PackageEntryStruct
---@field public PackageVersion number 返回当前模块版本
---@field public PackageBeforeUnLoad function 模块卸载事件回调
---@field public PackageEntry function 模块初始化事件回调
local PackageEntryStruct={ }

---调用 Ballance 加载全局加载资源
---@param pathFile string 资源路径。可以是 `__包名__/path/to/res` 表示加载指定模块的资源。不加包名则认为是加载当前脚本所在包的内置资源。
---@diagnostic disable-next-line: lowercase-global
function loadAsset(pathFile) end 

---宏定义

UNITY_EDITOR = false
UNITY_EDITOR_64 = false
UNITY_EDITOR_WIN = false
UNITY_EDITOR_OSX = false
UNITY_STANDALONE_OSX = false
UNITY_STANDALONE_WIN = false
UNITY_STANDALONE_LINUX = false
UNITY_EDITOR_OSX2 = false
UNITY_ANDROID = false
UNITY_IOS = false
UNITY_PS4 = false
UNITY_XBOXONE = false
UNITY_WSA = false
UNITY_WEBGL = false

---指定当前是不是机关迷你调试环境中
BALLANCE_MODUL_DEBUG = false