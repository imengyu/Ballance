---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Rigidbody2D : Component
---@field public position Vector2 
---@field public rotation number 
---@field public velocity Vector2 
---@field public angularVelocity number 
---@field public useAutoMass boolean 
---@field public mass number 
---@field public sharedMaterial PhysicsMaterial2D 
---@field public centerOfMass Vector2 
---@field public worldCenterOfMass Vector2 
---@field public inertia number 
---@field public drag number 
---@field public angularDrag number 
---@field public gravityScale number 
---@field public bodyType number 
---@field public useFullKinematicContacts boolean 
---@field public isKinematic boolean 
---@field public fixedAngle boolean 
---@field public freezeRotation boolean 
---@field public constraints number 
---@field public simulated boolean 
---@field public interpolation number 
---@field public sleepMode number 
---@field public collisionDetectionMode number 
---@field public attachedColliderCount number 
local Rigidbody2D={ }
---
---@public
---@param angle number 
---@return void 
function Rigidbody2D:SetRotation(angle) end
---
---@public
---@param rotation Quaternion 
---@return void 
function Rigidbody2D:SetRotation(rotation) end
---
---@public
---@param position Vector2 
---@return void 
function Rigidbody2D:MovePosition(position) end
---
---@public
---@param angle number 
---@return void 
function Rigidbody2D:MoveRotation(angle) end
---
---@public
---@param rotation Quaternion 
---@return void 
function Rigidbody2D:MoveRotation(rotation) end
---
---@public
---@return boolean 
function Rigidbody2D:IsSleeping() end
---
---@public
---@return boolean 
function Rigidbody2D:IsAwake() end
---
---@public
---@return void 
function Rigidbody2D:Sleep() end
---
---@public
---@return void 
function Rigidbody2D:WakeUp() end
---
---@public
---@param collider Collider2D 
---@return boolean 
function Rigidbody2D:IsTouching(collider) end
---
---@public
---@param collider Collider2D 
---@param contactFilter ContactFilter2D 
---@return boolean 
function Rigidbody2D:IsTouching(collider, contactFilter) end
---
---@public
---@param contactFilter ContactFilter2D 
---@return boolean 
function Rigidbody2D:IsTouching(contactFilter) end
---
---@public
---@return boolean 
function Rigidbody2D:IsTouchingLayers() end
---
---@public
---@param layerMask number 
---@return boolean 
function Rigidbody2D:IsTouchingLayers(layerMask) end
---
---@public
---@param point Vector2 
---@return boolean 
function Rigidbody2D:OverlapPoint(point) end
---
---@public
---@param collider Collider2D 
---@return ColliderDistance2D 
function Rigidbody2D:Distance(collider) end
---
---@public
---@param position Vector2 
---@return Vector2 
function Rigidbody2D:ClosestPoint(position) end
---
---@public
---@param force Vector2 
---@return void 
function Rigidbody2D:AddForce(force) end
---
---@public
---@param force Vector2 
---@param mode number 
---@return void 
function Rigidbody2D:AddForce(force, mode) end
---
---@public
---@param relativeForce Vector2 
---@return void 
function Rigidbody2D:AddRelativeForce(relativeForce) end
---
---@public
---@param relativeForce Vector2 
---@param mode number 
---@return void 
function Rigidbody2D:AddRelativeForce(relativeForce, mode) end
---
---@public
---@param force Vector2 
---@param position Vector2 
---@return void 
function Rigidbody2D:AddForceAtPosition(force, position) end
---
---@public
---@param force Vector2 
---@param position Vector2 
---@param mode number 
---@return void 
function Rigidbody2D:AddForceAtPosition(force, position, mode) end
---
---@public
---@param torque number 
---@return void 
function Rigidbody2D:AddTorque(torque) end
---
---@public
---@param torque number 
---@param mode number 
---@return void 
function Rigidbody2D:AddTorque(torque, mode) end
---
---@public
---@param point Vector2 
---@return Vector2 
function Rigidbody2D:GetPoint(point) end
---
---@public
---@param relativePoint Vector2 
---@return Vector2 
function Rigidbody2D:GetRelativePoint(relativePoint) end
---
---@public
---@param vector Vector2 
---@return Vector2 
function Rigidbody2D:GetVector(vector) end
---
---@public
---@param relativeVector Vector2 
---@return Vector2 
function Rigidbody2D:GetRelativeVector(relativeVector) end
---
---@public
---@param point Vector2 
---@return Vector2 
function Rigidbody2D:GetPointVelocity(point) end
---
---@public
---@param relativePoint Vector2 
---@return Vector2 
function Rigidbody2D:GetRelativePointVelocity(relativePoint) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function Rigidbody2D:OverlapCollider(contactFilter, results) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function Rigidbody2D:OverlapCollider(contactFilter, results) end
---
---@public
---@param contacts ContactPoint2D[] 
---@return number 
function Rigidbody2D:GetContacts(contacts) end
---
---@public
---@param contacts List`1 
---@return number 
function Rigidbody2D:GetContacts(contacts) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param contacts ContactPoint2D[] 
---@return number 
function Rigidbody2D:GetContacts(contactFilter, contacts) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param contacts List`1 
---@return number 
function Rigidbody2D:GetContacts(contactFilter, contacts) end
---
---@public
---@param colliders Collider2D[] 
---@return number 
function Rigidbody2D:GetContacts(colliders) end
---
---@public
---@param colliders List`1 
---@return number 
function Rigidbody2D:GetContacts(colliders) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param colliders Collider2D[] 
---@return number 
function Rigidbody2D:GetContacts(contactFilter, colliders) end
---
---@public
---@param contactFilter ContactFilter2D 
---@param colliders List`1 
---@return number 
function Rigidbody2D:GetContacts(contactFilter, colliders) end
---
---@public
---@param results Collider2D[] 
---@return number 
function Rigidbody2D:GetAttachedColliders(results) end
---
---@public
---@param results List`1 
---@return number 
function Rigidbody2D:GetAttachedColliders(results) end
---
---@public
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@return number 
function Rigidbody2D:Cast(direction, results) end
---
---@public
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Rigidbody2D:Cast(direction, results, distance) end
---
---@public
---@param direction Vector2 
---@param results List`1 
---@param distance number 
---@return number 
function Rigidbody2D:Cast(direction, results, distance) end
---
---@public
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function Rigidbody2D:Cast(direction, contactFilter, results) end
---
---@public
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Rigidbody2D:Cast(direction, contactFilter, results, distance) end
---
---@public
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@param distance number 
---@return number 
function Rigidbody2D:Cast(direction, contactFilter, results, distance) end
---
UnityEngine.Rigidbody2D = Rigidbody2D