---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PhysicsScene : ValueType
local PhysicsScene={ }
---
---@public
---@return string 
function PhysicsScene:ToString() end
---
---@public
---@param lhs PhysicsScene 
---@param rhs PhysicsScene 
---@return boolean 
function PhysicsScene.op_Equality(lhs, rhs) end
---
---@public
---@param lhs PhysicsScene 
---@param rhs PhysicsScene 
---@return boolean 
function PhysicsScene.op_Inequality(lhs, rhs) end
---
---@public
---@return number 
function PhysicsScene:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function PhysicsScene:Equals(other) end
---
---@public
---@param other PhysicsScene 
---@return boolean 
function PhysicsScene:Equals(other) end
---
---@public
---@return boolean 
function PhysicsScene:IsValid() end
---
---@public
---@return boolean 
function PhysicsScene:IsEmpty() end
---
---@public
---@param step number 
---@return void 
function PhysicsScene:Simulate(step) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function PhysicsScene:Raycast(origin, direction, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function PhysicsScene:Raycast(origin, direction, hitInfo, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param raycastHits RaycastHit[] 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function PhysicsScene:Raycast(origin, direction, raycastHits, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function PhysicsScene:CapsuleCast(point1, point2, radius, direction, hitInfo, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function PhysicsScene:CapsuleCast(point1, point2, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param point0 Vector3 
---@param point1 Vector3 
---@param radius number 
---@param results Collider[] 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function PhysicsScene:OverlapCapsule(point0, point1, radius, results, layerMask, queryTriggerInteraction) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function PhysicsScene:SphereCast(origin, radius, direction, hitInfo, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function PhysicsScene:SphereCast(origin, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param position Vector3 
---@param radius number 
---@param results Collider[] 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function PhysicsScene:OverlapSphere(position, radius, results, layerMask, queryTriggerInteraction) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param orientation Quaternion 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function PhysicsScene:BoxCast(center, halfExtents, direction, hitInfo, orientation, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@return boolean 
function PhysicsScene:BoxCast(center, halfExtents, direction, hitInfo) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param results Collider[] 
---@param orientation Quaternion 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function PhysicsScene:OverlapBox(center, halfExtents, results, orientation, layerMask, queryTriggerInteraction) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param results Collider[] 
---@return number 
function PhysicsScene:OverlapBox(center, halfExtents, results) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param orientation Quaternion 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function PhysicsScene:BoxCast(center, halfExtents, direction, results, orientation, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param results RaycastHit[] 
---@return number 
function PhysicsScene:BoxCast(center, halfExtents, direction, results) end
---
UnityEngine.PhysicsScene = PhysicsScene