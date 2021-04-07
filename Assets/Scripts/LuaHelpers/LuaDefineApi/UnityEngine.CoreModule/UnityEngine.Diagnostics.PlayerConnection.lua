---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PlayerConnection
---@field public connected boolean 
local PlayerConnection={ }
---
---@public
---@param remoteFilePath string 
---@param data Byte[] 
---@return void 
function PlayerConnection.SendFile(remoteFilePath, data) end
---
UnityEngine.Diagnostics.PlayerConnection = PlayerConnection