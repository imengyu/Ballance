---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameLuaObjectPhysics2DEventCaller
local GameLuaObjectPhysics2DEventCaller={ }
---
---@public
---@return number 
function GameLuaObjectPhysics2DEventCaller:GetEventType() end
---
---@public
---@return String[] 
function GameLuaObjectPhysics2DEventCaller:GetSupportEvents() end
---
---@public
---@param host GameLuaObjectHost 
---@return void 
function GameLuaObjectPhysics2DEventCaller:OnInitLua(host) end
---
Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjectPhysics2DEventCaller = GameLuaObjectPhysics2DEventCaller