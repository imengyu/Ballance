---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CullingGroupEvent : ValueType
---@field public index number 
---@field public isVisible boolean 
---@field public wasVisible boolean 
---@field public hasBecomeVisible boolean 
---@field public hasBecomeInvisible boolean 
---@field public currentDistance number 
---@field public previousDistance number 
local CullingGroupEvent={ }
---
UnityEngine.CullingGroupEvent = CullingGroupEvent