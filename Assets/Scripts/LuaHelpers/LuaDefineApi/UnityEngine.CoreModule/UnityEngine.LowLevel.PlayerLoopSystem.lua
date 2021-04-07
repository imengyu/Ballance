---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PlayerLoopSystem : ValueType
---@field public type Type 
---@field public subSystemList PlayerLoopSystem[] 
---@field public updateDelegate UpdateFunction 
---@field public updateFunction IntPtr 
---@field public loopConditionFunction IntPtr 
local PlayerLoopSystem={ }
---
---@public
---@return string 
function PlayerLoopSystem:ToString() end
---
UnityEngine.LowLevel.PlayerLoopSystem = PlayerLoopSystem