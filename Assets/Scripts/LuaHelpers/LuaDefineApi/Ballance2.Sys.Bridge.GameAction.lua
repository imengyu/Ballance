---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameAction
---@field public Store GameActionStore 所属模块
---@field public Package GamePackage 所属模块
---@field public Name string 操作名称
---@field public GameHandler GameHandler 操作接收器
---@field public CallTypeCheck String[] 操作类型检查
---@field public Empty GameAction 空操作
local GameAction={ }
---
---@public
---@return void 
function GameAction:Dispose() end
---全局操作
Ballance2.Sys.Bridge.GameAction = GameAction