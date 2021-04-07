---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameSettingsCallback
local GameSettingsCallback={ }
---
---@public
---@param groupName string 
---@param action number 
---@return boolean 
function GameSettingsCallback:Invoke(groupName, action) end
---
---@public
---@param groupName string 
---@param action number 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function GameSettingsCallback:BeginInvoke(groupName, action, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return boolean 
function GameSettingsCallback:EndInvoke(result) end
---设置回调
Ballance2.Config.GameSettingsCallback = GameSettingsCallback