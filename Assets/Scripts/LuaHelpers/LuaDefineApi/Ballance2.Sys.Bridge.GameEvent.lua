---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameEvent
---@field public EventName string 获取事件名称
---@field public EventHandlers List`1 获取事件接收器
local GameEvent={ }
---释放
---@public
---@return void 
function GameEvent:Dispose() end
---
Ballance2.Sys.Bridge.GameEvent = GameEvent