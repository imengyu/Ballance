---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Physics
---@field public IgnoreRaycastLayer number 
---@field public DefaultRaycastLayers number 
---@field public AllLayers number 
---@field public kIgnoreRaycastLayer number 
---@field public kDefaultRaycastLayers number 
---@field public kAllLayers number 
---@field public minPenetrationForPenalty number 
---@field public gravity Vector3 
---@field public defaultContactOffset number 
---@field public sleepThreshold number 
---@field public queriesHitTriggers boolean 
---@field public queriesHitBackfaces boolean 
---@field public bounceThreshold number 
---@field public defaultMaxDepenetrationVelocity number 
---@field public defaultSolverIterations number 
---@field public defaultSolverVelocityIterations number 
---@field public bounceTreshold number 
---@field public sleepVelocity number 
---@field public sleepAngularVelocity number 
---@field public maxAngularVelocity number 
---@field public solverIterationCount number 
---@field public solverVelocityIterationCount number 
---@field public penetrationPenaltyForce number 
---@field public defaultMaxAngularSpeed number 
---@field public defaultPhysicsScene PhysicsScene 
---@field public autoSimulation boolean 
---@field public autoSyncTransforms boolean 
---@field public reuseCollisionCallbacks boolean 
---@field public interCollisionDistance number 
---@field public interCollisionStiffness number 
---@field public interCollisionSettingsToggle boolean 
---@field public clothGravity Vector3 
local Physics={ }
---
---@public
---@param collider1 Collider 
---@param collider2 Collider 
---@param ignore boolean 
---@return void 
function Physics.IgnoreCollision(collider1, collider2, ignore) end
---
---@public
---@param collider1 Collider 
---@param collider2 Collider 
---@return void 
function Physics.IgnoreCollision(collider1, collider2) end
---
---@public
---@param layer1 number 
---@param layer2 number 
---@param ignore boolean 
---@return void 
function Physics.IgnoreLayerCollision(layer1, layer2, ignore) end
---
---@public
---@param layer1 number 
---@param layer2 number 
---@return void 
function Physics.IgnoreLayerCollision(layer1, layer2) end
---
---@public
---@param layer1 number 
---@param layer2 number 
---@return boolean 
function Physics.GetIgnoreLayerCollision(layer1, layer2) end
---
---@public
---@param collider1 Collider 
---@param collider2 Collider 
---@return boolean 
function Physics.GetIgnoreCollision(collider1, collider2) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.Raycast(origin, direction, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param maxDistance number 
---@param layerMask number 
---@return boolean 
function Physics.Raycast(origin, direction, maxDistance, layerMask) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param maxDistance number 
---@return boolean 
function Physics.Raycast(origin, direction, maxDistance) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@return boolean 
function Physics.Raycast(origin, direction) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.Raycast(origin, direction, hitInfo, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@param layerMask number 
---@return boolean 
function Physics.Raycast(origin, direction, hitInfo, maxDistance, layerMask) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@return boolean 
function Physics.Raycast(origin, direction, hitInfo, maxDistance) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@return boolean 
function Physics.Raycast(origin, direction, hitInfo) end
---
---@public
---@param ray Ray 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.Raycast(ray, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param ray Ray 
---@param maxDistance number 
---@param layerMask number 
---@return boolean 
function Physics.Raycast(ray, maxDistance, layerMask) end
---
---@public
---@param ray Ray 
---@param maxDistance number 
---@return boolean 
function Physics.Raycast(ray, maxDistance) end
---
---@public
---@param ray Ray 
---@return boolean 
function Physics.Raycast(ray) end
---
---@public
---@param ray Ray 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.Raycast(ray, hitInfo, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param ray Ray 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@param layerMask number 
---@return boolean 
function Physics.Raycast(ray, hitInfo, maxDistance, layerMask) end
---
---@public
---@param ray Ray 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@return boolean 
function Physics.Raycast(ray, hitInfo, maxDistance) end
---
---@public
---@param ray Ray 
---@param hitInfo RaycastHit& 
---@return boolean 
function Physics.Raycast(ray, hitInfo) end
---
---@public
---@param start Vector3 
---@param _end Vector3 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.Linecast(start, _end, layerMask, queryTriggerInteraction) end
---
---@public
---@param start Vector3 
---@param _end Vector3 
---@param layerMask number 
---@return boolean 
function Physics.Linecast(start, _end, layerMask) end
---
---@public
---@param start Vector3 
---@param _end Vector3 
---@return boolean 
function Physics.Linecast(start, _end) end
---
---@public
---@param start Vector3 
---@param _end Vector3 
---@param hitInfo RaycastHit& 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.Linecast(start, _end, hitInfo, layerMask, queryTriggerInteraction) end
---
---@public
---@param start Vector3 
---@param _end Vector3 
---@param hitInfo RaycastHit& 
---@param layerMask number 
---@return boolean 
function Physics.Linecast(start, _end, hitInfo, layerMask) end
---
---@public
---@param start Vector3 
---@param _end Vector3 
---@param hitInfo RaycastHit& 
---@return boolean 
function Physics.Linecast(start, _end, hitInfo) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.CapsuleCast(point1, point2, radius, direction, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param maxDistance number 
---@param layerMask number 
---@return boolean 
function Physics.CapsuleCast(point1, point2, radius, direction, maxDistance, layerMask) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param maxDistance number 
---@return boolean 
function Physics.CapsuleCast(point1, point2, radius, direction, maxDistance) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@return boolean 
function Physics.CapsuleCast(point1, point2, radius, direction) end
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
function Physics.CapsuleCast(point1, point2, radius, direction, hitInfo, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@param layerMask number 
---@return boolean 
function Physics.CapsuleCast(point1, point2, radius, direction, hitInfo, maxDistance, layerMask) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@return boolean 
function Physics.CapsuleCast(point1, point2, radius, direction, hitInfo, maxDistance) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@return boolean 
function Physics.CapsuleCast(point1, point2, radius, direction, hitInfo) end
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
function Physics.SphereCast(origin, radius, direction, hitInfo, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@param layerMask number 
---@return boolean 
function Physics.SphereCast(origin, radius, direction, hitInfo, maxDistance, layerMask) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@return boolean 
function Physics.SphereCast(origin, radius, direction, hitInfo, maxDistance) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@return boolean 
function Physics.SphereCast(origin, radius, direction, hitInfo) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.SphereCast(ray, radius, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param maxDistance number 
---@param layerMask number 
---@return boolean 
function Physics.SphereCast(ray, radius, maxDistance, layerMask) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param maxDistance number 
---@return boolean 
function Physics.SphereCast(ray, radius, maxDistance) end
---
---@public
---@param ray Ray 
---@param radius number 
---@return boolean 
function Physics.SphereCast(ray, radius) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.SphereCast(ray, radius, hitInfo, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@param layerMask number 
---@return boolean 
function Physics.SphereCast(ray, radius, hitInfo, maxDistance, layerMask) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param hitInfo RaycastHit& 
---@param maxDistance number 
---@return boolean 
function Physics.SphereCast(ray, radius, hitInfo, maxDistance) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param hitInfo RaycastHit& 
---@return boolean 
function Physics.SphereCast(ray, radius, hitInfo) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param orientation Quaternion 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.BoxCast(center, halfExtents, direction, orientation, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param orientation Quaternion 
---@param maxDistance number 
---@param layerMask number 
---@return boolean 
function Physics.BoxCast(center, halfExtents, direction, orientation, maxDistance, layerMask) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param orientation Quaternion 
---@param maxDistance number 
---@return boolean 
function Physics.BoxCast(center, halfExtents, direction, orientation, maxDistance) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param orientation Quaternion 
---@return boolean 
function Physics.BoxCast(center, halfExtents, direction, orientation) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@return boolean 
function Physics.BoxCast(center, halfExtents, direction) end
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
function Physics.BoxCast(center, halfExtents, direction, hitInfo, orientation, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param orientation Quaternion 
---@param maxDistance number 
---@param layerMask number 
---@return boolean 
function Physics.BoxCast(center, halfExtents, direction, hitInfo, orientation, maxDistance, layerMask) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param orientation Quaternion 
---@param maxDistance number 
---@return boolean 
function Physics.BoxCast(center, halfExtents, direction, hitInfo, orientation, maxDistance) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@param orientation Quaternion 
---@return boolean 
function Physics.BoxCast(center, halfExtents, direction, hitInfo, orientation) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param hitInfo RaycastHit& 
---@return boolean 
function Physics.BoxCast(center, halfExtents, direction, hitInfo) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return RaycastHit[] 
function Physics.RaycastAll(origin, direction, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param maxDistance number 
---@param layerMask number 
---@return RaycastHit[] 
function Physics.RaycastAll(origin, direction, maxDistance, layerMask) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param maxDistance number 
---@return RaycastHit[] 
function Physics.RaycastAll(origin, direction, maxDistance) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@return RaycastHit[] 
function Physics.RaycastAll(origin, direction) end
---
---@public
---@param ray Ray 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return RaycastHit[] 
function Physics.RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param ray Ray 
---@param maxDistance number 
---@param layerMask number 
---@return RaycastHit[] 
function Physics.RaycastAll(ray, maxDistance, layerMask) end
---
---@public
---@param ray Ray 
---@param maxDistance number 
---@return RaycastHit[] 
function Physics.RaycastAll(ray, maxDistance) end
---
---@public
---@param ray Ray 
---@return RaycastHit[] 
function Physics.RaycastAll(ray) end
---
---@public
---@param ray Ray 
---@param results RaycastHit[] 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function Physics.RaycastNonAlloc(ray, results, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param ray Ray 
---@param results RaycastHit[] 
---@param maxDistance number 
---@param layerMask number 
---@return number 
function Physics.RaycastNonAlloc(ray, results, maxDistance, layerMask) end
---
---@public
---@param ray Ray 
---@param results RaycastHit[] 
---@param maxDistance number 
---@return number 
function Physics.RaycastNonAlloc(ray, results, maxDistance) end
---
---@public
---@param ray Ray 
---@param results RaycastHit[] 
---@return number 
function Physics.RaycastNonAlloc(ray, results) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function Physics.RaycastNonAlloc(origin, direction, results, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param maxDistance number 
---@param layerMask number 
---@return number 
function Physics.RaycastNonAlloc(origin, direction, results, maxDistance, layerMask) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param maxDistance number 
---@return number 
function Physics.RaycastNonAlloc(origin, direction, results, maxDistance) end
---
---@public
---@param origin Vector3 
---@param direction Vector3 
---@param results RaycastHit[] 
---@return number 
function Physics.RaycastNonAlloc(origin, direction, results) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return RaycastHit[] 
function Physics.CapsuleCastAll(point1, point2, radius, direction, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param maxDistance number 
---@param layerMask number 
---@return RaycastHit[] 
function Physics.CapsuleCastAll(point1, point2, radius, direction, maxDistance, layerMask) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param maxDistance number 
---@return RaycastHit[] 
function Physics.CapsuleCastAll(point1, point2, radius, direction, maxDistance) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@return RaycastHit[] 
function Physics.CapsuleCastAll(point1, point2, radius, direction) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return RaycastHit[] 
function Physics.SphereCastAll(origin, radius, direction, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@param maxDistance number 
---@param layerMask number 
---@return RaycastHit[] 
function Physics.SphereCastAll(origin, radius, direction, maxDistance, layerMask) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@param maxDistance number 
---@return RaycastHit[] 
function Physics.SphereCastAll(origin, radius, direction, maxDistance) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@return RaycastHit[] 
function Physics.SphereCastAll(origin, radius, direction) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return RaycastHit[] 
function Physics.SphereCastAll(ray, radius, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param maxDistance number 
---@param layerMask number 
---@return RaycastHit[] 
function Physics.SphereCastAll(ray, radius, maxDistance, layerMask) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param maxDistance number 
---@return RaycastHit[] 
function Physics.SphereCastAll(ray, radius, maxDistance) end
---
---@public
---@param ray Ray 
---@param radius number 
---@return RaycastHit[] 
function Physics.SphereCastAll(ray, radius) end
---
---@public
---@param point0 Vector3 
---@param point1 Vector3 
---@param radius number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return Collider[] 
function Physics.OverlapCapsule(point0, point1, radius, layerMask, queryTriggerInteraction) end
---
---@public
---@param point0 Vector3 
---@param point1 Vector3 
---@param radius number 
---@param layerMask number 
---@return Collider[] 
function Physics.OverlapCapsule(point0, point1, radius, layerMask) end
---
---@public
---@param point0 Vector3 
---@param point1 Vector3 
---@param radius number 
---@return Collider[] 
function Physics.OverlapCapsule(point0, point1, radius) end
---
---@public
---@param position Vector3 
---@param radius number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return Collider[] 
function Physics.OverlapSphere(position, radius, layerMask, queryTriggerInteraction) end
---
---@public
---@param position Vector3 
---@param radius number 
---@param layerMask number 
---@return Collider[] 
function Physics.OverlapSphere(position, radius, layerMask) end
---
---@public
---@param position Vector3 
---@param radius number 
---@return Collider[] 
function Physics.OverlapSphere(position, radius) end
---
---@public
---@param step number 
---@return void 
function Physics.Simulate(step) end
---
---@public
---@return void 
function Physics.SyncTransforms() end
---
---@public
---@param colliderA Collider 
---@param positionA Vector3 
---@param rotationA Quaternion 
---@param colliderB Collider 
---@param positionB Vector3 
---@param rotationB Quaternion 
---@param direction Vector3& 
---@param distance Single& 
---@return boolean 
function Physics.ComputePenetration(colliderA, positionA, rotationA, colliderB, positionB, rotationB, direction, distance) end
---
---@public
---@param point Vector3 
---@param collider Collider 
---@param position Vector3 
---@param rotation Quaternion 
---@return Vector3 
function Physics.ClosestPoint(point, collider, position, rotation) end
---
---@public
---@param position Vector3 
---@param radius number 
---@param results Collider[] 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function Physics.OverlapSphereNonAlloc(position, radius, results, layerMask, queryTriggerInteraction) end
---
---@public
---@param position Vector3 
---@param radius number 
---@param results Collider[] 
---@param layerMask number 
---@return number 
function Physics.OverlapSphereNonAlloc(position, radius, results, layerMask) end
---
---@public
---@param position Vector3 
---@param radius number 
---@param results Collider[] 
---@return number 
function Physics.OverlapSphereNonAlloc(position, radius, results) end
---
---@public
---@param position Vector3 
---@param radius number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.CheckSphere(position, radius, layerMask, queryTriggerInteraction) end
---
---@public
---@param position Vector3 
---@param radius number 
---@param layerMask number 
---@return boolean 
function Physics.CheckSphere(position, radius, layerMask) end
---
---@public
---@param position Vector3 
---@param radius number 
---@return boolean 
function Physics.CheckSphere(position, radius) end
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
function Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param maxDistance number 
---@param layerMask number 
---@return number 
function Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results, maxDistance, layerMask) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param maxDistance number 
---@return number 
function Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results, maxDistance) end
---
---@public
---@param point1 Vector3 
---@param point2 Vector3 
---@param radius number 
---@param direction Vector3 
---@param results RaycastHit[] 
---@return number 
function Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results) end
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
function Physics.SphereCastNonAlloc(origin, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param maxDistance number 
---@param layerMask number 
---@return number 
function Physics.SphereCastNonAlloc(origin, radius, direction, results, maxDistance, layerMask) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param maxDistance number 
---@return number 
function Physics.SphereCastNonAlloc(origin, radius, direction, results, maxDistance) end
---
---@public
---@param origin Vector3 
---@param radius number 
---@param direction Vector3 
---@param results RaycastHit[] 
---@return number 
function Physics.SphereCastNonAlloc(origin, radius, direction, results) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param results RaycastHit[] 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function Physics.SphereCastNonAlloc(ray, radius, results, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param results RaycastHit[] 
---@param maxDistance number 
---@param layerMask number 
---@return number 
function Physics.SphereCastNonAlloc(ray, radius, results, maxDistance, layerMask) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param results RaycastHit[] 
---@param maxDistance number 
---@return number 
function Physics.SphereCastNonAlloc(ray, radius, results, maxDistance) end
---
---@public
---@param ray Ray 
---@param radius number 
---@param results RaycastHit[] 
---@return number 
function Physics.SphereCastNonAlloc(ray, radius, results) end
---
---@public
---@param start Vector3 
---@param _end Vector3 
---@param radius number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.CheckCapsule(start, _end, radius, layerMask, queryTriggerInteraction) end
---
---@public
---@param start Vector3 
---@param _end Vector3 
---@param radius number 
---@param layerMask number 
---@return boolean 
function Physics.CheckCapsule(start, _end, radius, layerMask) end
---
---@public
---@param start Vector3 
---@param _end Vector3 
---@param radius number 
---@return boolean 
function Physics.CheckCapsule(start, _end, radius) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param orientation Quaternion 
---@param layermask number 
---@param queryTriggerInteraction number 
---@return boolean 
function Physics.CheckBox(center, halfExtents, orientation, layermask, queryTriggerInteraction) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param orientation Quaternion 
---@param layerMask number 
---@return boolean 
function Physics.CheckBox(center, halfExtents, orientation, layerMask) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param orientation Quaternion 
---@return boolean 
function Physics.CheckBox(center, halfExtents, orientation) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@return boolean 
function Physics.CheckBox(center, halfExtents) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param orientation Quaternion 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return Collider[] 
function Physics.OverlapBox(center, halfExtents, orientation, layerMask, queryTriggerInteraction) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param orientation Quaternion 
---@param layerMask number 
---@return Collider[] 
function Physics.OverlapBox(center, halfExtents, orientation, layerMask) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param orientation Quaternion 
---@return Collider[] 
function Physics.OverlapBox(center, halfExtents, orientation) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@return Collider[] 
function Physics.OverlapBox(center, halfExtents) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param results Collider[] 
---@param orientation Quaternion 
---@param mask number 
---@param queryTriggerInteraction number 
---@return number 
function Physics.OverlapBoxNonAlloc(center, halfExtents, results, orientation, mask, queryTriggerInteraction) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param results Collider[] 
---@param orientation Quaternion 
---@param mask number 
---@return number 
function Physics.OverlapBoxNonAlloc(center, halfExtents, results, orientation, mask) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param results Collider[] 
---@param orientation Quaternion 
---@return number 
function Physics.OverlapBoxNonAlloc(center, halfExtents, results, orientation) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param results Collider[] 
---@return number 
function Physics.OverlapBoxNonAlloc(center, halfExtents, results) end
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
function Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param orientation Quaternion 
---@return number 
function Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param orientation Quaternion 
---@param maxDistance number 
---@return number 
function Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, maxDistance) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param results RaycastHit[] 
---@param orientation Quaternion 
---@param maxDistance number 
---@param layerMask number 
---@return number 
function Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, maxDistance, layerMask) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param results RaycastHit[] 
---@return number 
function Physics.BoxCastNonAlloc(center, halfExtents, direction, results) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param orientation Quaternion 
---@param maxDistance number 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return RaycastHit[] 
function Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, layerMask, queryTriggerInteraction) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param orientation Quaternion 
---@param maxDistance number 
---@param layerMask number 
---@return RaycastHit[] 
function Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, layerMask) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param orientation Quaternion 
---@param maxDistance number 
---@return RaycastHit[] 
function Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@param orientation Quaternion 
---@return RaycastHit[] 
function Physics.BoxCastAll(center, halfExtents, direction, orientation) end
---
---@public
---@param center Vector3 
---@param halfExtents Vector3 
---@param direction Vector3 
---@return RaycastHit[] 
function Physics.BoxCastAll(center, halfExtents, direction) end
---
---@public
---@param point0 Vector3 
---@param point1 Vector3 
---@param radius number 
---@param results Collider[] 
---@param layerMask number 
---@param queryTriggerInteraction number 
---@return number 
function Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results, layerMask, queryTriggerInteraction) end
---
---@public
---@param point0 Vector3 
---@param point1 Vector3 
---@param radius number 
---@param results Collider[] 
---@param layerMask number 
---@return number 
function Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results, layerMask) end
---
---@public
---@param point0 Vector3 
---@param point1 Vector3 
---@param radius number 
---@param results Collider[] 
---@return number 
function Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results) end
---
---@public
---@param worldBounds Bounds 
---@param subdivisions number 
---@return void 
function Physics.RebuildBroadphaseRegions(worldBounds, subdivisions) end
---
---@public
---@param meshID number 
---@param convex boolean 
---@return void 
function Physics.BakeMesh(meshID, convex) end
---
UnityEngine.Physics = Physics