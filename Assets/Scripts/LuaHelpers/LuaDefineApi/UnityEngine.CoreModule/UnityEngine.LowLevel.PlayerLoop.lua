---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PlayerLoop
local PlayerLoop={ }
---
---@public
---@return PlayerLoopSystem 
function PlayerLoop.GetDefaultPlayerLoop() end
---
---@public
---@return PlayerLoopSystem 
function PlayerLoop.GetCurrentPlayerLoop() end
---
---@public
---@param loop PlayerLoopSystem 
---@return void 
function PlayerLoop.SetPlayerLoop(loop) end
---
UnityEngine.LowLevel.PlayerLoop = PlayerLoop