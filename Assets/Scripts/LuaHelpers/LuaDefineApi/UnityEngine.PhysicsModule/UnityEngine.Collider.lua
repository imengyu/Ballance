---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Collider : Component
---@field public enabled boolean 
---@field public attachedRigidbody Rigidbody 
---@field public attachedArticulationBody ArticulationBody 
---@field public isTrigger boolean 
---@field public contactOffset number 
---@field public bounds Bounds 
---@field public sharedMaterial PhysicMaterial 
---@field public material PhysicMaterial 
local Collider={ }
---
---@public
---@param position Vector3 
---@return Vector3 
function Collider:ClosestPoint(position) end
---
---@public
---@param ray Ray 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@return boolean 
function Collider:Raycast(ray, hitInfo, maxDistance) end
---
---@public
---@param position Vector3 
---@return Vector3 
function Collider:ClosestPointOnBounds(position) end
---
UnityEngine.Collider = Collider