---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimatorTransitionInfo : ValueType
---@field public fullPathHash number 
---@field public nameHash number 
---@field public userNameHash number 
---@field public durationUnit number 
---@field public duration number 
---@field public normalizedTime number 
---@field public anyState boolean 
local AnimatorTransitionInfo={ }
---
---@public
---@param name string 
---@return boolean 
function AnimatorTransitionInfo:IsName(name) end
---
---@public
---@param name string 
---@return boolean 
function AnimatorTransitionInfo:IsUserName(name) end
---
UnityEngine.AnimatorTransitionInfo = AnimatorTransitionInfo