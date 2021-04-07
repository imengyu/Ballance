---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PlayerLoopSystemInternal : ValueType
---@field public type Type 
---@field public updateDelegate UpdateFunction 
---@field public updateFunction IntPtr 
---@field public loopConditionFunction IntPtr 
---@field public numSubSystems number 
local PlayerLoopSystemInternal={ }
---
UnityEngine.LowLevel.PlayerLoopSystemInternal = PlayerLoopSystemInternal