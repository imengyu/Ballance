---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameLuaObjectParticleEventCaller
local GameLuaObjectParticleEventCaller={ }
---
---@public
---@return number 
function GameLuaObjectParticleEventCaller:GetEventType() end
---
---@public
---@return String[] 
function GameLuaObjectParticleEventCaller:GetSupportEvents() end
---
---@public
---@param host GameLuaObjectHost 
---@return void 
function GameLuaObjectParticleEventCaller:OnInitLua(host) end
---
Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjectParticleEventCaller = GameLuaObjectParticleEventCaller