---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Collider2D : Behaviour
---@field public density number 
---@field public isTrigger boolean 
---@field public usedByEffector boolean 
---@field public usedByComposite boolean 
---@field public composite CompositeCollider2D 
---@field public offset Vector2 
---@field public attachedRigidbody Rigidbody2D 
---@field public shapeCount number 
---@field public bounds Bounds 
---@field public sharedMaterial PhysicsMaterial2D 
---@field public friction number 
---@field public bounciness number 
local Collider2D={ }
---
---@public
---@param useBodyPosition boolean 
---@param useBodyRotation boolean 
---@return Mesh 
function Collider2D:CreateMesh(useBodyPosition, useBodyRotation) end
---
---@public
---@return number 
function Collider2D:GetShapeHash() end
---
---@public
---@param collider Collider2D 
---@return boolean 
function Collider2D:IsTouching(collider) end
---
---@public
---@param collider Collider2D 
---@param contactFilter ContactFilter2D 
---@return boolean 
function Collider2D:IsTouching(collider, contactFilter) end
---
---@public
---@param contactFilter ContactFilter2D 
---@return boolean 
function Collider2D:IsTouching(contactFilter) end
---
---@public
---@return boolean 
function Collider2D:IsTouchingLayers() end
---
---@public
---@param layerMask number 
---@return boolean 
function Collider2D:IsTouchingLayers(layerMask) end
---
---@public
---@param point Vector2 
---@return boolean 
function Collider2D:OverlapPoint(point) end
---
---@public
---@param collider Collider2D 
---@return ColliderDistance2D 
function Collider2D:Distance(collider) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function Collider2D:OverlapCollider(contactFilter, results) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function Collider2D:OverlapCollider(contactFilter, results) end
---
---@public
---@param contacts ContactPoint2D[] 
---@return number 
function Collider2D:GetContacts(contacts) end
---
---@public
---@param contacts List`1 
---@return number 
function Collider2D:GetContacts(contacts) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param contacts ContactPoint2D[] 
---@return number 
function Collider2D:GetContacts(contactFilter, contacts) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param contacts List`1 
---@return number 
function Collider2D:GetContacts(contactFilter, contacts) end
---
---@public
---@param colliders Collider2D[] 
---@return number 
function Collider2D:GetContacts(colliders) end
---
---@public
---@param colliders List`1 
---@return number 
function Collider2D:GetContacts(colliders) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param colliders Collider2D[] 
---@return number 
function Collider2D:GetContacts(contactFilter, colliders) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param colliders List`1 
---@return number 
function Collider2D:GetContacts(contactFilter, colliders) end
---
---@public
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@return number 
function Collider2D:Cast(direction, results) end
---
---@public
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Collider2D:Cast(direction, results, distance) end
---
---@public
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param ignoreSiblingColliders boolean 
---@return number 
function Collider2D:Cast(direction, results, distance, ignoreSiblingColliders) end
---
---@public
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function Collider2D:Cast(direction, contactFilter, results) end
---
---@public
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Collider2D:Cast(direction, contactFilter, results, distance) end
---
---@public
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@param distance number 
---@param ignoreSiblingColliders boolean 
---@return number 
function Collider2D:Cast(direction, contactFilter, results, distance, ignoreSiblingColliders) end
---
---@public
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@param distance number 
---@param ignoreSiblingColliders boolean 
---@return number 
function Collider2D:Cast(direction, contactFilter, results, distance, ignoreSiblingColliders) end
---
---@public
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@return number 
function Collider2D:Raycast(direction, results) end
---
---@public
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Collider2D:Raycast(direction, results, distance) end
---
---@public
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@return number 
function Collider2D:Raycast(direction, results, distance, layerMask) end
---
---@public
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return number 
function Collider2D:Raycast(direction, results, distance, layerMask, minDepth) end
---
---@public
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return number 
function Collider2D:Raycast(direction, results, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function Collider2D:Raycast(direction, contactFilter, results) end
---
---@public
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Collider2D:Raycast(direction, contactFilter, results, distance) end
---
---@public
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@param distance number 
---@return number 
function Collider2D:Raycast(direction, contactFilter, results, distance) end
---
---@public
---@param position Vector2 
---@return Vector2 
function Collider2D:ClosestPoint(position) end
---
UnityEngine.Collider2D = Collider2D