---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameLuaObjectMouseEventCaller
local GameLuaObjectMouseEventCaller={ }
---
---@public
---@return number 
function GameLuaObjectMouseEventCaller:GetEventType() end
---
---@public
---@return String[] 
function GameLuaObjectMouseEventCaller:GetSupportEvents() end
---
---@public
---@param host GameLuaObjectHost 
---@return void 
function GameLuaObjectMouseEventCaller:OnInitLua(host) end
---
Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjectMouseEventCaller = GameLuaObjectMouseEventCaller