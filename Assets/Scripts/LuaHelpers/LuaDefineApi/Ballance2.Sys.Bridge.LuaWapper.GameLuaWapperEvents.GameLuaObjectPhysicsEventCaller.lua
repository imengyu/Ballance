---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameLuaObjectPhysicsEventCaller
local GameLuaObjectPhysicsEventCaller={ }
---
---@public
---@return number 
function GameLuaObjectPhysicsEventCaller:GetEventType() end
---
---@public
---@return String[] 
function GameLuaObjectPhysicsEventCaller:GetSupportEvents() end
---
---@public
---@param host GameLuaObjectHost 
---@return void 
function GameLuaObjectPhysicsEventCaller:OnInitLua(host) end
---
Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjectPhysicsEventCaller = GameLuaObjectPhysicsEventCaller