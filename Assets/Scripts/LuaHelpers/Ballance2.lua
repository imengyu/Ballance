
---@class Ballance2GlobalNamespace
local Ballance2GlobalNamespace={ }
---@class UnityEngineGlobalNamespace
local UnityEngineGlobalNamespace={ }
local JetBrainsGlobalNamespace={ }
local UnityGlobalNamespace={ }

---GameLuaObjectHost的自定义lua类固定结构
---@class GameLuaObjectHostClass
---@field public transform Transform 获取Transform
---@field public monoBehaviour GameLuaObjectHost 获取当前GameLuaObjectHost
---@field public tableInstance table 获取当前GameLuaObjectHost上绑定的lua table
---@field public gameObject GameObject 获取GameObject
---@field public store Store 访问GlobalStore
---@field public actionStore GameActionStore 访问ActionStore
local GameLuaObjectHostClass={ }

---Ballance2 C# 全局命名空间
Ballance2 = Ballance2GlobalNamespace
UnityEngine = UnityEngineGlobalNamespace
JetBrains = JetBrainsGlobalNamespace
Unity = UnityGlobalNamespace