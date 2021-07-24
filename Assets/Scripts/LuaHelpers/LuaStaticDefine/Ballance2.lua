
---@class Ballance2GlobalNamespace
local Ballance2GlobalNamespace={ }
---@class UnityEngineGlobalNamespace
local UnityEngineGlobalNamespace={ }
---@class UnityEngineUIGlobalNamespace
local UnityEngineUIGlobalNamespace={ }
local JetBrainsGlobalNamespace={ }
local UnityGlobalNamespace={ }

---GameLuaObjectHost的自定义lua类固定结构
---@class GameLuaObjectHostClass
---@field public transform Transform 获取Transform
---@field public monoBehaviour GameLuaObjectHost 获取当前GameLuaObjectHost
---@field public luaClass table 获取当前GameLuaObjectHost上绑定的lua类table
---@field public gameObject GameObject 获取GameObject
---@field public store Store 访问GlobalStore
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