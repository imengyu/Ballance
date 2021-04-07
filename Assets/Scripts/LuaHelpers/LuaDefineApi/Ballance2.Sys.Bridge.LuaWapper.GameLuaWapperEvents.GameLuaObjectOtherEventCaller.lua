---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameLuaObjectOtherEventCaller
local GameLuaObjectOtherEventCaller={ }
---
---@public
---@return number 
function GameLuaObjectOtherEventCaller:GetEventType() end
---
---@public
---@return String[] 
function GameLuaObjectOtherEventCaller:GetSupportEvents() end
---
---@public
---@param host GameLuaObjectHost 
---@return void 
function GameLuaObjectOtherEventCaller:OnInitLua(host) end
---
Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjectOtherEventCaller = GameLuaObjectOtherEventCaller