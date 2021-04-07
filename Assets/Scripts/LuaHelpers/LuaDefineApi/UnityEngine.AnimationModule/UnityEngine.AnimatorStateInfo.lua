---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimatorStateInfo : ValueType
---@field public fullPathHash number 
---@field public nameHash number 
---@field public shortNameHash number 
---@field public normalizedTime number 
---@field public length number 
---@field public speed number 
---@field public speedMultiplier number 
---@field public tagHash number 
---@field public loop boolean 
local AnimatorStateInfo={ }
---
---@public
---@param name string 
---@return boolean 
function AnimatorStateInfo:IsName(name) end
---
---@public
---@param tag string 
---@return boolean 
function AnimatorStateInfo:IsTag(tag) end
---
UnityEngine.AnimatorStateInfo = AnimatorStateInfo