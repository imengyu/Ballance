---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimatorControllerParameter
---@field public name string 
---@field public nameHash number 
---@field public type number 
---@field public defaultFloat number 
---@field public defaultInt number 
---@field public defaultBool boolean 
local AnimatorControllerParameter={ }
---
---@public
---@param o Object 
---@return boolean 
function AnimatorControllerParameter:Equals(o) end
---
---@public
---@return number 
function AnimatorControllerParameter:GetHashCode() end
---
UnityEngine.AnimatorControllerParameter = AnimatorControllerParameter