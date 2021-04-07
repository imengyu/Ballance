---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PhysicsScene2D : ValueType
local PhysicsScene2D={ }
---
---@public
---@return string 
function PhysicsScene2D:ToString() end
---
---@public
---@param lhs PhysicsScene2D 
---@param rhs PhysicsScene2D 
---@return boolean 
function PhysicsScene2D.op_Equality(lhs, rhs) end
---
---@public
---@param lhs PhysicsScene2D 
---@param rhs PhysicsScene2D 
---@return boolean 
function PhysicsScene2D.op_Inequality(lhs, rhs) end
---
---@public
---@return number 
function PhysicsScene2D:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function PhysicsScene2D:Equals(other) end
---
---@public
---@param other PhysicsScene2D 
---@return boolean 
function PhysicsScene2D:Equals(other) end
---
---@public
---@return boolean 
function PhysicsScene2D:IsValid() end
---
---@public
---@return boolean 
function PhysicsScene2D:IsEmpty() end
---
---@public
---@param step number 
---@return boolean 
function PhysicsScene2D:Simulate(step) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param layerMask number 
---@return RaycastHit2D 
function PhysicsScene2D:Linecast(start, _end, layerMask) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param contactFilter ContactFilter2D 
---@return RaycastHit2D 
function PhysicsScene2D:Linecast(start, _end, contactFilter) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param results RaycastHit2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D:Linecast(start, _end, results, layerMask) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function PhysicsScene2D:Linecast(start, _end, contactFilter, results) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function PhysicsScene2D:Linecast(start, _end, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D 
function PhysicsScene2D:Raycast(origin, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@return RaycastHit2D 
function PhysicsScene2D:Raycast(origin, direction, distance, contactFilter) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@param results RaycastHit2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D:Raycast(origin, direction, distance, results, layerMask) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function PhysicsScene2D:Raycast(origin, direction, distance, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function PhysicsScene2D:Raycast(origin, direction, distance, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D 
function PhysicsScene2D:CircleCast(origin, radius, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@return RaycastHit2D 
function PhysicsScene2D:CircleCast(origin, radius, direction, distance, contactFilter) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@param results RaycastHit2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D:CircleCast(origin, radius, direction, distance, results, layerMask) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function PhysicsScene2D:CircleCast(origin, radius, direction, distance, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function PhysicsScene2D:CircleCast(origin, radius, direction, distance, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D 
function PhysicsScene2D:BoxCast(origin, size, angle, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@return RaycastHit2D 
function PhysicsScene2D:BoxCast(origin, size, angle, direction, distance, contactFilter) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param results RaycastHit2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D:BoxCast(origin, size, angle, direction, distance, results, layerMask) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function PhysicsScene2D:BoxCast(origin, size, angle, direction, distance, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function PhysicsScene2D:BoxCast(origin, size, angle, direction, distance, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D 
function PhysicsScene2D:CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@return RaycastHit2D 
function PhysicsScene2D:CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param results RaycastHit2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D:CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, results, layerMask) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function PhysicsScene2D:CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function PhysicsScene2D:CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, contactFilter, results) end
---
---@public
---@param ray Ray 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D 
function PhysicsScene2D:GetRayIntersection(ray, distance, layerMask) end
---
---@public
---@param ray Ray 
---@param distance number 
---@param results RaycastHit2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D:GetRayIntersection(ray, distance, results, layerMask) end
---
---@public
---@param point Vector2 
---@param layerMask number 
---@return Collider2D 
function PhysicsScene2D:OverlapPoint(point, layerMask) end
---
---@public
---@param point Vector2 
---@param contactFilter ContactFilter2D 
---@return Collider2D 
function PhysicsScene2D:OverlapPoint(point, contactFilter) end
---
---@public
---@param point Vector2 
---@param results Collider2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D:OverlapPoint(point, results, layerMask) end
---
---@public
---@param point Vector2 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function PhysicsScene2D:OverlapPoint(point, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function PhysicsScene2D:OverlapPoint(point, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param layerMask number 
---@return Collider2D 
function PhysicsScene2D:OverlapCircle(point, radius, layerMask) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param contactFilter ContactFilter2D 
---@return Collider2D 
function PhysicsScene2D:OverlapCircle(point, radius, contactFilter) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param results Collider2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D:OverlapCircle(point, radius, results, layerMask) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function PhysicsScene2D:OverlapCircle(point, radius, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function PhysicsScene2D:OverlapCircle(point, radius, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param layerMask number 
---@return Collider2D 
function PhysicsScene2D:OverlapBox(point, size, angle, layerMask) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param contactFilter ContactFilter2D 
---@return Collider2D 
function PhysicsScene2D:OverlapBox(point, size, angle, contactFilter) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param results Collider2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D:OverlapBox(point, size, angle, results, layerMask) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function PhysicsScene2D:OverlapBox(point, size, angle, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function PhysicsScene2D:OverlapBox(point, size, angle, contactFilter, results) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param layerMask number 
---@return Collider2D 
function PhysicsScene2D:OverlapArea(pointA, pointB, layerMask) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param contactFilter ContactFilter2D 
---@return Collider2D 
function PhysicsScene2D:OverlapArea(pointA, pointB, contactFilter) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param results Collider2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D:OverlapArea(pointA, pointB, results, layerMask) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function PhysicsScene2D:OverlapArea(pointA, pointB, contactFilter, results) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function PhysicsScene2D:OverlapArea(pointA, pointB, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param layerMask number 
---@return Collider2D 
function PhysicsScene2D:OverlapCapsule(point, size, direction, angle, layerMask) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param contactFilter ContactFilter2D 
---@return Collider2D 
function PhysicsScene2D:OverlapCapsule(point, size, direction, angle, contactFilter) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param results Collider2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D:OverlapCapsule(point, size, direction, angle, results, layerMask) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function PhysicsScene2D:OverlapCapsule(point, size, direction, angle, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function PhysicsScene2D:OverlapCapsule(point, size, direction, angle, contactFilter, results) end
---
---@public
---@param collider Collider2D 
---@param results Collider2D[] 
---@param layerMask number 
---@return number 
function PhysicsScene2D.OverlapCollider(collider, results, layerMask) end
---
---@public
---@param collider Collider2D 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function PhysicsScene2D.OverlapCollider(collider, contactFilter, results) end
---
---@public
---@param collider Collider2D 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function PhysicsScene2D.OverlapCollider(collider, contactFilter, results) end
---
UnityEngine.PhysicsScene2D = PhysicsScene2D