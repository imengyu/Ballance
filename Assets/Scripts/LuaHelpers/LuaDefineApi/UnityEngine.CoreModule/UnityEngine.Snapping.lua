---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Snapping
local Snapping={ }
---
---@public
---@param val number 
---@param snap number 
---@return number 
function Snapping.Snap(val, snap) end
---
---@public
---@param val Vector2 
---@param snap Vector2 
---@return Vector2 
function Snapping.Snap(val, snap) end
---
---@public
---@param val Vector3 
---@param snap Vector3 
---@param axis number 
---@return Vector3 
function Snapping.Snap(val, snap, axis) end
---
UnityEngine.Snapping = Snapping