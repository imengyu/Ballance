---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IPlayerEditorConnectionNative
local IPlayerEditorConnectionNative={ }
---
---@public
---@return void 
function IPlayerEditorConnectionNative:Initialize() end
---
---@public
---@return void 
function IPlayerEditorConnectionNative:DisconnectAll() end
---
---@public
---@param messageId Guid 
---@param data Byte[] 
---@param playerId number 
---@return void 
function IPlayerEditorConnectionNative:SendMessage(messageId, data, playerId) end
---
---@public
---@param messageId Guid 
---@param data Byte[] 
---@param playerId number 
---@return boolean 
function IPlayerEditorConnectionNative:TrySendMessage(messageId, data, playerId) end
---
---@public
---@return void 
function IPlayerEditorConnectionNative:Poll() end
---
---@public
---@param messageId Guid 
---@return void 
function IPlayerEditorConnectionNative:RegisterInternal(messageId) end
---
---@public
---@param messageId Guid 
---@return void 
function IPlayerEditorConnectionNative:UnregisterInternal(messageId) end
---
---@public
---@return boolean 
function IPlayerEditorConnectionNative:IsConnected() end
---
UnityEngine.IPlayerEditorConnectionNative = IPlayerEditorConnectionNative