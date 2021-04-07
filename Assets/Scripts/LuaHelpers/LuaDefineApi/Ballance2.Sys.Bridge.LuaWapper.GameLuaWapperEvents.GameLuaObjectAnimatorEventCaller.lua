---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameLuaObjectAnimatorEventCaller
local GameLuaObjectAnimatorEventCaller={ }
---
---@public
---@return number 
function GameLuaObjectAnimatorEventCaller:GetEventType() end
---
---@public
---@return String[] 
function GameLuaObjectAnimatorEventCaller:GetSupportEvents() end
---
---@public
---@param host GameLuaObjectHost 
---@return void 
function GameLuaObjectAnimatorEventCaller:OnInitLua(host) end
---
Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjectAnimatorEventCaller = GameLuaObjectAnimatorEventCaller