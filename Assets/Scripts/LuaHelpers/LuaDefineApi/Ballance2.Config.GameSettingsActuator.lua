---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameSettingsActuator
---@field public ACTION_UPDATE number 
---@field public ACTION_LOAD number 
local GameSettingsActuator={ }
---
---@public
---@param key string 
---@param value number 
---@return void 
function GameSettingsActuator:SetInt(key, value) end
---
---@public
---@param key string 
---@param defaultValue number 
---@return number 
function GameSettingsActuator:GetInt(key, defaultValue) end
---
---@public
---@param key string 
---@param value string 
---@return void 
function GameSettingsActuator:SetString(key, value) end
---
---@public
---@param key string 
---@param defaultValue string 
---@return string 
function GameSettingsActuator:GetString(key, defaultValue) end
---
---@public
---@param key string 
---@param value number 
---@return void 
function GameSettingsActuator:SetFloat(key, value) end
---
---@public
---@param key string 
---@param defaultValue number 
---@return number 
function GameSettingsActuator:GetFloat(key, defaultValue) end
---
---@public
---@param key string 
---@param value boolean 
---@return void 
function GameSettingsActuator:SetBool(key, value) end
---
---@public
---@param key string 
---@param defaultValue boolean 
---@return boolean 
function GameSettingsActuator:GetBool(key, defaultValue) end
---通知设置组加载更新
---@public
---@param groupName string 组名称
---@return void 
function GameSettingsActuator:RequireSettingsLoad(groupName) end
---通知设置组更新
---@public
---@param groupName string 组名称
---@return void 
function GameSettingsActuator:NotifySettingsUpdate(groupName) end
---注册设置组更新回调
---@public
---@param groupName string 组名称
---@param callback GameSettingsCallback 
---@return void 
function GameSettingsActuator:RegisterSettingsUpdateCallback(groupName, callback) end
---设置执行器
Ballance2.Config.GameSettingsActuator = GameSettingsActuator