---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Navigation : ValueType
---@field public mode number 
---@field public wrapAround boolean 
---@field public selectOnUp Selectable 
---@field public selectOnDown Selectable 
---@field public selectOnLeft Selectable 
---@field public selectOnRight Selectable 
---@field public defaultNavigation Navigation 
local Navigation={ }
---
---@public
---@param other Navigation 
---@return boolean 
function Navigation:Equals(other) end
---
UnityEngine.UI.Navigation = Navigation