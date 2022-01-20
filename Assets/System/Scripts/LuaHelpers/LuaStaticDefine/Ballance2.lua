
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
UnityEngine.Yield = function (wait) end
JetBrains = JetBrainsGlobalNamespace
Unity = UnityGlobalNamespace
---创建类接口
CreateClass = {}
BallancePhysics = {}
BallancePhysics.Wapper = {}
Ballance2.Base = {}
Ballance2.Log = {}
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