---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RaycastHit2D : ValueType
---@field public centroid Vector2 
---@field public point Vector2 
---@field public normal Vector2 
---@field public distance number 
---@field public fraction number 
---@field public collider Collider2D 
---@field public rigidbody Rigidbody2D 
---@field public transform Transform 
local RaycastHit2D={ }
---
---@public
---@param hit RaycastHit2D 
---@return boolean 
function RaycastHit2D.op_Implicit(hit) end
---
---@public
---@param other RaycastHit2D 
---@return number 
function RaycastHit2D:CompareTo(other) end
---
UnityEngine.RaycastHit2D = RaycastHit2D