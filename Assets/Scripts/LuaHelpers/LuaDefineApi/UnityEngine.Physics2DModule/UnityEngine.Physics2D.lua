---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Physics2D
---@field public IgnoreRaycastLayer number 
---@field public DefaultRaycastLayers number 
---@field public AllLayers number 
---@field public defaultPhysicsScene PhysicsScene2D 
---@field public velocityIterations number 
---@field public positionIterations number 
---@field public gravity Vector2 
---@field public queriesHitTriggers boolean 
---@field public queriesStartInColliders boolean 
---@field public callbacksOnDisable boolean 
---@field public reuseCollisionCallbacks boolean 
---@field public autoSyncTransforms boolean 
---@field public simulationMode number 
---@field public jobOptions PhysicsJobOptions2D 
---@field public velocityThreshold number 
---@field public maxLinearCorrection number 
---@field public maxAngularCorrection number 
---@field public maxTranslationSpeed number 
---@field public maxRotationSpeed number 
---@field public defaultContactOffset number 
---@field public baumgarteScale number 
---@field public baumgarteTOIScale number 
---@field public timeToSleep number 
---@field public linearSleepTolerance number 
---@field public angularSleepTolerance number 
---@field public alwaysShowColliders boolean 
---@field public showColliderSleep boolean 
---@field public showColliderContacts boolean 
---@field public showColliderAABB boolean 
---@field public contactArrowScale number 
---@field public colliderAwakeColor Color 
---@field public colliderAsleepColor Color 
---@field public colliderContactColor Color 
---@field public colliderAABBColor Color 
---@field public raycastsHitTriggers boolean 
---@field public raycastsStartInColliders boolean 
---@field public deleteStopsCallbacks boolean 
---@field public changeStopsCallbacks boolean 
---@field public minPenetrationForPenalty number 
---@field public autoSimulation boolean 
local Physics2D={ }
---
---@public
---@param step number 
---@return boolean 
function Physics2D.Simulate(step) end
---
---@public
---@return void 
function Physics2D.SyncTransforms() end
---
---@public
---@param collider1 Collider2D 
---@param collider2 Collider2D 
---@return void 
function Physics2D.IgnoreCollision(collider1, collider2) end
---
---@public
---@param collider1 Collider2D 
---@param collider2 Collider2D 
---@param ignore boolean 
---@return void 
function Physics2D.IgnoreCollision(collider1, collider2, ignore) end
---
---@public
---@param collider1 Collider2D 
---@param collider2 Collider2D 
---@return boolean 
function Physics2D.GetIgnoreCollision(collider1, collider2) end
---
---@public
---@param layer1 number 
---@param layer2 number 
---@return void 
function Physics2D.IgnoreLayerCollision(layer1, layer2) end
---
---@public
---@param layer1 number 
---@param layer2 number 
---@param ignore boolean 
---@return void 
function Physics2D.IgnoreLayerCollision(layer1, layer2, ignore) end
---
---@public
---@param layer1 number 
---@param layer2 number 
---@return boolean 
function Physics2D.GetIgnoreLayerCollision(layer1, layer2) end
---
---@public
---@param layer number 
---@param layerMask number 
---@return void 
function Physics2D.SetLayerCollisionMask(layer, layerMask) end
---
---@public
---@param layer number 
---@return number 
function Physics2D.GetLayerCollisionMask(layer) end
---
---@public
---@param collider1 Collider2D 
---@param collider2 Collider2D 
---@return boolean 
function Physics2D.IsTouching(collider1, collider2) end
---
---@public
---@param collider1 Collider2D 
---@param collider2 Collider2D 
---@param contactFilter ContactFilter2D 
---@return boolean 
function Physics2D.IsTouching(collider1, collider2, contactFilter) end
---
---@public
---@param collider Collider2D 
---@param contactFilter ContactFilter2D 
---@return boolean 
function Physics2D.IsTouching(collider, contactFilter) end
---
---@public
---@param collider Collider2D 
---@return boolean 
function Physics2D.IsTouchingLayers(collider) end
---
---@public
---@param collider Collider2D 
---@param layerMask number 
---@return boolean 
function Physics2D.IsTouchingLayers(collider, layerMask) end
---
---@public
---@param colliderA Collider2D 
---@param colliderB Collider2D 
---@return ColliderDistance2D 
function Physics2D.Distance(colliderA, colliderB) end
---
---@public
---@param position Vector2 
---@param collider Collider2D 
---@return Vector2 
function Physics2D.ClosestPoint(position, collider) end
---
---@public
---@param position Vector2 
---@param rigidbody Rigidbody2D 
---@return Vector2 
function Physics2D.ClosestPoint(position, rigidbody) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@return RaycastHit2D 
function Physics2D.Linecast(start, _end) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param layerMask number 
---@return RaycastHit2D 
function Physics2D.Linecast(start, _end, layerMask) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param layerMask number 
---@param minDepth number 
---@return RaycastHit2D 
function Physics2D.Linecast(start, _end, layerMask, minDepth) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return RaycastHit2D 
function Physics2D.Linecast(start, _end, layerMask, minDepth, maxDepth) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function Physics2D.Linecast(start, _end, contactFilter, results) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function Physics2D.Linecast(start, _end, contactFilter, results) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@return RaycastHit2D[] 
function Physics2D.LinecastAll(start, _end) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param layerMask number 
---@return RaycastHit2D[] 
function Physics2D.LinecastAll(start, _end, layerMask) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param layerMask number 
---@param minDepth number 
---@return RaycastHit2D[] 
function Physics2D.LinecastAll(start, _end, layerMask, minDepth) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return RaycastHit2D[] 
function Physics2D.LinecastAll(start, _end, layerMask, minDepth, maxDepth) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param results RaycastHit2D[] 
---@return number 
function Physics2D.LinecastNonAlloc(start, _end, results) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param results RaycastHit2D[] 
---@param layerMask number 
---@return number 
function Physics2D.LinecastNonAlloc(start, _end, results, layerMask) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param results RaycastHit2D[] 
---@param layerMask number 
---@param minDepth number 
---@return number 
function Physics2D.LinecastNonAlloc(start, _end, results, layerMask, minDepth) end
---
---@public
---@param start Vector2 
---@param _end Vector2 
---@param results RaycastHit2D[] 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return number 
function Physics2D.LinecastNonAlloc(start, _end, results, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@return RaycastHit2D 
function Physics2D.Raycast(origin, direction) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@return RaycastHit2D 
function Physics2D.Raycast(origin, direction, distance) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D 
function Physics2D.Raycast(origin, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return RaycastHit2D 
function Physics2D.Raycast(origin, direction, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return RaycastHit2D 
function Physics2D.Raycast(origin, direction, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function Physics2D.Raycast(origin, direction, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Physics2D.Raycast(origin, direction, contactFilter, results, distance) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@param distance number 
---@return number 
function Physics2D.Raycast(origin, direction, contactFilter, results, distance) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@return number 
function Physics2D.RaycastNonAlloc(origin, direction, results) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Physics2D.RaycastNonAlloc(origin, direction, results, distance) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@return number 
function Physics2D.RaycastNonAlloc(origin, direction, results, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return number 
function Physics2D.RaycastNonAlloc(origin, direction, results, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return number 
function Physics2D.RaycastNonAlloc(origin, direction, results, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@return RaycastHit2D[] 
function Physics2D.RaycastAll(origin, direction) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@return RaycastHit2D[] 
function Physics2D.RaycastAll(origin, direction, distance) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D[] 
function Physics2D.RaycastAll(origin, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return RaycastHit2D[] 
function Physics2D.RaycastAll(origin, direction, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return RaycastHit2D[] 
function Physics2D.RaycastAll(origin, direction, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@return RaycastHit2D 
function Physics2D.CircleCast(origin, radius, direction) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@return RaycastHit2D 
function Physics2D.CircleCast(origin, radius, direction, distance) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D 
function Physics2D.CircleCast(origin, radius, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return RaycastHit2D 
function Physics2D.CircleCast(origin, radius, direction, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return RaycastHit2D 
function Physics2D.CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function Physics2D.CircleCast(origin, radius, direction, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Physics2D.CircleCast(origin, radius, direction, contactFilter, results, distance) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@param distance number 
---@return number 
function Physics2D.CircleCast(origin, radius, direction, contactFilter, results, distance) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@return RaycastHit2D[] 
function Physics2D.CircleCastAll(origin, radius, direction) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@return RaycastHit2D[] 
function Physics2D.CircleCastAll(origin, radius, direction, distance) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D[] 
function Physics2D.CircleCastAll(origin, radius, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return RaycastHit2D[] 
function Physics2D.CircleCastAll(origin, radius, direction, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return RaycastHit2D[] 
function Physics2D.CircleCastAll(origin, radius, direction, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@return number 
function Physics2D.CircleCastNonAlloc(origin, radius, direction, results) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Physics2D.CircleCastNonAlloc(origin, radius, direction, results, distance) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@return number 
function Physics2D.CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return number 
function Physics2D.CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param radius number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return number 
function Physics2D.CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@return RaycastHit2D 
function Physics2D.BoxCast(origin, size, angle, direction) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@return RaycastHit2D 
function Physics2D.BoxCast(origin, size, angle, direction, distance) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D 
function Physics2D.BoxCast(origin, size, angle, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return RaycastHit2D 
function Physics2D.BoxCast(origin, size, angle, direction, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return RaycastHit2D 
function Physics2D.BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function Physics2D.BoxCast(origin, size, angle, direction, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Physics2D.BoxCast(origin, size, angle, direction, contactFilter, results, distance) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@param distance number 
---@return number 
function Physics2D.BoxCast(origin, size, angle, direction, contactFilter, results, distance) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@return RaycastHit2D[] 
function Physics2D.BoxCastAll(origin, size, angle, direction) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@return RaycastHit2D[] 
function Physics2D.BoxCastAll(origin, size, angle, direction, distance) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D[] 
function Physics2D.BoxCastAll(origin, size, angle, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return RaycastHit2D[] 
function Physics2D.BoxCastAll(origin, size, angle, direction, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return RaycastHit2D[] 
function Physics2D.BoxCastAll(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@return number 
function Physics2D.BoxCastNonAlloc(origin, size, angle, direction, results) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Physics2D.BoxCastNonAlloc(origin, size, angle, direction, results, distance) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@return number 
function Physics2D.BoxCastNonAlloc(origin, size, angle, direction, results, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return number 
function Physics2D.BoxCastNonAlloc(origin, size, angle, direction, results, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param angle number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return number 
function Physics2D.BoxCastNonAlloc(origin, size, angle, direction, results, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@return RaycastHit2D 
function Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@return RaycastHit2D 
function Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance) end
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
function Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return RaycastHit2D 
function Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return RaycastHit2D 
function Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@return number 
function Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, contactFilter, results) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, contactFilter, results, distance) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@param distance number 
---@return number 
function Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, contactFilter, results, distance) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@return RaycastHit2D[] 
function Physics2D.CapsuleCastAll(origin, size, capsuleDirection, angle, direction) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@return RaycastHit2D[] 
function Physics2D.CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D[] 
function Physics2D.CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return RaycastHit2D[] 
function Physics2D.CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return RaycastHit2D[] 
function Physics2D.CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@return number 
function Physics2D.CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Physics2D.CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@return number 
function Physics2D.CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance, layerMask) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@return number 
function Physics2D.CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance, layerMask, minDepth) end
---
---@public
---@param origin Vector2 
---@param size Vector2 
---@param capsuleDirection number 
---@param angle number 
---@param direction Vector2 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return number 
function Physics2D.CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance, layerMask, minDepth, maxDepth) end
---
---@public
---@param ray Ray 
---@return RaycastHit2D 
function Physics2D.GetRayIntersection(ray) end
---
---@public
---@param ray Ray 
---@param distance number 
---@return RaycastHit2D 
function Physics2D.GetRayIntersection(ray, distance) end
---
---@public
---@param ray Ray 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D 
function Physics2D.GetRayIntersection(ray, distance, layerMask) end
---
---@public
---@param ray Ray 
---@return RaycastHit2D[] 
function Physics2D.GetRayIntersectionAll(ray) end
---
---@public
---@param ray Ray 
---@param distance number 
---@return RaycastHit2D[] 
function Physics2D.GetRayIntersectionAll(ray, distance) end
---
---@public
---@param ray Ray 
---@param distance number 
---@param layerMask number 
---@return RaycastHit2D[] 
function Physics2D.GetRayIntersectionAll(ray, distance, layerMask) end
---
---@public
---@param ray Ray 
---@param results RaycastHit2D[] 
---@return number 
function Physics2D.GetRayIntersectionNonAlloc(ray, results) end
---
---@public
---@param ray Ray 
---@param results RaycastHit2D[] 
---@param distance number 
---@return number 
function Physics2D.GetRayIntersectionNonAlloc(ray, results, distance) end
---
---@public
---@param ray Ray 
---@param results RaycastHit2D[] 
---@param distance number 
---@param layerMask number 
---@return number 
function Physics2D.GetRayIntersectionNonAlloc(ray, results, distance, layerMask) end
---
---@public
---@param point Vector2 
---@return Collider2D 
function Physics2D.OverlapPoint(point) end
---
---@public
---@param point Vector2 
---@param layerMask number 
---@return Collider2D 
function Physics2D.OverlapPoint(point, layerMask) end
---
---@public
---@param point Vector2 
---@param layerMask number 
---@param minDepth number 
---@return Collider2D 
function Physics2D.OverlapPoint(point, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return Collider2D 
function Physics2D.OverlapPoint(point, layerMask, minDepth, maxDepth) end
---
---@public
---@param point Vector2 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function Physics2D.OverlapPoint(point, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function Physics2D.OverlapPoint(point, contactFilter, results) end
---
---@public
---@param point Vector2 
---@return Collider2D[] 
function Physics2D.OverlapPointAll(point) end
---
---@public
---@param point Vector2 
---@param layerMask number 
---@return Collider2D[] 
function Physics2D.OverlapPointAll(point, layerMask) end
---
---@public
---@param point Vector2 
---@param layerMask number 
---@param minDepth number 
---@return Collider2D[] 
function Physics2D.OverlapPointAll(point, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return Collider2D[] 
function Physics2D.OverlapPointAll(point, layerMask, minDepth, maxDepth) end
---
---@public
---@param point Vector2 
---@param results Collider2D[] 
---@return number 
function Physics2D.OverlapPointNonAlloc(point, results) end
---
---@public
---@param point Vector2 
---@param results Collider2D[] 
---@param layerMask number 
---@return number 
function Physics2D.OverlapPointNonAlloc(point, results, layerMask) end
---
---@public
---@param point Vector2 
---@param results Collider2D[] 
---@param layerMask number 
---@param minDepth number 
---@return number 
function Physics2D.OverlapPointNonAlloc(point, results, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param results Collider2D[] 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return number 
function Physics2D.OverlapPointNonAlloc(point, results, layerMask, minDepth, maxDepth) end
---
---@public
---@param point Vector2 
---@param radius number 
---@return Collider2D 
function Physics2D.OverlapCircle(point, radius) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param layerMask number 
---@return Collider2D 
function Physics2D.OverlapCircle(point, radius, layerMask) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param layerMask number 
---@param minDepth number 
---@return Collider2D 
function Physics2D.OverlapCircle(point, radius, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return Collider2D 
function Physics2D.OverlapCircle(point, radius, layerMask, minDepth, maxDepth) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function Physics2D.OverlapCircle(point, radius, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function Physics2D.OverlapCircle(point, radius, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param radius number 
---@return Collider2D[] 
function Physics2D.OverlapCircleAll(point, radius) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param layerMask number 
---@return Collider2D[] 
function Physics2D.OverlapCircleAll(point, radius, layerMask) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param layerMask number 
---@param minDepth number 
---@return Collider2D[] 
function Physics2D.OverlapCircleAll(point, radius, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return Collider2D[] 
function Physics2D.OverlapCircleAll(point, radius, layerMask, minDepth, maxDepth) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param results Collider2D[] 
---@return number 
function Physics2D.OverlapCircleNonAlloc(point, radius, results) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param results Collider2D[] 
---@param layerMask number 
---@return number 
function Physics2D.OverlapCircleNonAlloc(point, radius, results, layerMask) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param results Collider2D[] 
---@param layerMask number 
---@param minDepth number 
---@return number 
function Physics2D.OverlapCircleNonAlloc(point, radius, results, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param radius number 
---@param results Collider2D[] 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return number 
function Physics2D.OverlapCircleNonAlloc(point, radius, results, layerMask, minDepth, maxDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@return Collider2D 
function Physics2D.OverlapBox(point, size, angle) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param layerMask number 
---@return Collider2D 
function Physics2D.OverlapBox(point, size, angle, layerMask) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param layerMask number 
---@param minDepth number 
---@return Collider2D 
function Physics2D.OverlapBox(point, size, angle, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return Collider2D 
function Physics2D.OverlapBox(point, size, angle, layerMask, minDepth, maxDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function Physics2D.OverlapBox(point, size, angle, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function Physics2D.OverlapBox(point, size, angle, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@return Collider2D[] 
function Physics2D.OverlapBoxAll(point, size, angle) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param layerMask number 
---@return Collider2D[] 
function Physics2D.OverlapBoxAll(point, size, angle, layerMask) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param layerMask number 
---@param minDepth number 
---@return Collider2D[] 
function Physics2D.OverlapBoxAll(point, size, angle, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return Collider2D[] 
function Physics2D.OverlapBoxAll(point, size, angle, layerMask, minDepth, maxDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param results Collider2D[] 
---@return number 
function Physics2D.OverlapBoxNonAlloc(point, size, angle, results) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param results Collider2D[] 
---@param layerMask number 
---@return number 
function Physics2D.OverlapBoxNonAlloc(point, size, angle, results, layerMask) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param results Collider2D[] 
---@param layerMask number 
---@param minDepth number 
---@return number 
function Physics2D.OverlapBoxNonAlloc(point, size, angle, results, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param angle number 
---@param results Collider2D[] 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return number 
function Physics2D.OverlapBoxNonAlloc(point, size, angle, results, layerMask, minDepth, maxDepth) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@return Collider2D 
function Physics2D.OverlapArea(pointA, pointB) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param layerMask number 
---@return Collider2D 
function Physics2D.OverlapArea(pointA, pointB, layerMask) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param layerMask number 
---@param minDepth number 
---@return Collider2D 
function Physics2D.OverlapArea(pointA, pointB, layerMask, minDepth) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return Collider2D 
function Physics2D.OverlapArea(pointA, pointB, layerMask, minDepth, maxDepth) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function Physics2D.OverlapArea(pointA, pointB, contactFilter, results) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function Physics2D.OverlapArea(pointA, pointB, contactFilter, results) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@return Collider2D[] 
function Physics2D.OverlapAreaAll(pointA, pointB) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param layerMask number 
---@return Collider2D[] 
function Physics2D.OverlapAreaAll(pointA, pointB, layerMask) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param layerMask number 
---@param minDepth number 
---@return Collider2D[] 
function Physics2D.OverlapAreaAll(pointA, pointB, layerMask, minDepth) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return Collider2D[] 
function Physics2D.OverlapAreaAll(pointA, pointB, layerMask, minDepth, maxDepth) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param results Collider2D[] 
---@return number 
function Physics2D.OverlapAreaNonAlloc(pointA, pointB, results) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param results Collider2D[] 
---@param layerMask number 
---@return number 
function Physics2D.OverlapAreaNonAlloc(pointA, pointB, results, layerMask) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param results Collider2D[] 
---@param layerMask number 
---@param minDepth number 
---@return number 
function Physics2D.OverlapAreaNonAlloc(pointA, pointB, results, layerMask, minDepth) end
---
---@public
---@param pointA Vector2 
---@param pointB Vector2 
---@param results Collider2D[] 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return number 
function Physics2D.OverlapAreaNonAlloc(pointA, pointB, results, layerMask, minDepth, maxDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@return Collider2D 
function Physics2D.OverlapCapsule(point, size, direction, angle) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param layerMask number 
---@return Collider2D 
function Physics2D.OverlapCapsule(point, size, direction, angle, layerMask) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param layerMask number 
---@param minDepth number 
---@return Collider2D 
function Physics2D.OverlapCapsule(point, size, direction, angle, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return Collider2D 
function Physics2D.OverlapCapsule(point, size, direction, angle, layerMask, minDepth, maxDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function Physics2D.OverlapCapsule(point, size, direction, angle, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function Physics2D.OverlapCapsule(point, size, direction, angle, contactFilter, results) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@return Collider2D[] 
function Physics2D.OverlapCapsuleAll(point, size, direction, angle) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param layerMask number 
---@return Collider2D[] 
function Physics2D.OverlapCapsuleAll(point, size, direction, angle, layerMask) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param layerMask number 
---@param minDepth number 
---@return Collider2D[] 
function Physics2D.OverlapCapsuleAll(point, size, direction, angle, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return Collider2D[] 
function Physics2D.OverlapCapsuleAll(point, size, direction, angle, layerMask, minDepth, maxDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param results Collider2D[] 
---@return number 
function Physics2D.OverlapCapsuleNonAlloc(point, size, direction, angle, results) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param results Collider2D[] 
---@param layerMask number 
---@return number 
function Physics2D.OverlapCapsuleNonAlloc(point, size, direction, angle, results, layerMask) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param results Collider2D[] 
---@param layerMask number 
---@param minDepth number 
---@return number 
function Physics2D.OverlapCapsuleNonAlloc(point, size, direction, angle, results, layerMask, minDepth) end
---
---@public
---@param point Vector2 
---@param size Vector2 
---@param direction number 
---@param angle number 
---@param results Collider2D[] 
---@param layerMask number 
---@param minDepth number 
---@param maxDepth number 
---@return number 
function Physics2D.OverlapCapsuleNonAlloc(point, size, direction, angle, results, layerMask, minDepth, maxDepth) end
---
---@public
---@param collider Collider2D 
---@param contactFilter ContactFilter2D 
---@param results Collider2D[] 
---@return number 
function Physics2D.OverlapCollider(collider, contactFilter, results) end
---
---@public
---@param collider Collider2D 
---@param contactFilter ContactFilter2D 
---@param results List`1 
---@return number 
function Physics2D.OverlapCollider(collider, contactFilter, results) end
---
---@public
---@param collider1 Collider2D 
---@param collider2 Collider2D 
---@param contactFilter ContactFilter2D 
---@param contacts ContactPoint2D[] 
---@return number 
function Physics2D.GetContacts(collider1, collider2, contactFilter, contacts) end
---
---@public
---@param collider Collider2D 
---@param contacts ContactPoint2D[] 
---@return number 
function Physics2D.GetContacts(collider, contacts) end
---
---@public
---@param collider Collider2D 
---@param contactFilter ContactFilter2D 
---@param contacts ContactPoint2D[] 
---@return number 
function Physics2D.GetContacts(collider, contactFilter, contacts) end
---
---@public
---@param collider Collider2D 
---@param colliders Collider2D[] 
---@return number 
function Physics2D.GetContacts(collider, colliders) end
---
---@public
---@param collider Collider2D 
---@param contactFilter ContactFilter2D 
---@param colliders Collider2D[] 
---@return number 
function Physics2D.GetContacts(collider, contactFilter, colliders) end
---
---@public
---@param rigidbody Rigidbody2D 
---@param contacts ContactPoint2D[] 
---@return number 
function Physics2D.GetContacts(rigidbody, contacts) end
---
---@public
---@param rigidbody Rigidbody2D 
---@param contactFilter ContactFilter2D 
---@param contacts ContactPoint2D[] 
---@return number 
function Physics2D.GetContacts(rigidbody, contactFilter, contacts) end
---
---@public
---@param rigidbody Rigidbody2D 
---@param colliders Collider2D[] 
---@return number 
function Physics2D.GetContacts(rigidbody, colliders) end
---
---@public
---@param rigidbody Rigidbody2D 
---@param contactFilter ContactFilter2D 
---@param colliders Collider2D[] 
---@return number 
function Physics2D.GetContacts(rigidbody, contactFilter, colliders) end
---
---@public
---@param collider1 Collider2D 
---@param collider2 Collider2D 
---@param contactFilter ContactFilter2D 
---@param contacts List`1 
---@return number 
function Physics2D.GetContacts(collider1, collider2, contactFilter, contacts) end
---
---@public
---@param collider Collider2D 
---@param contacts List`1 
---@return number 
function Physics2D.GetContacts(collider, contacts) end
---
---@public
---@param collider Collider2D 
---@param contactFilter ContactFilter2D 
---@param contacts List`1 
---@return number 
function Physics2D.GetContacts(collider, contactFilter, contacts) end
---
---@public
---@param collider Collider2D 
---@param colliders List`1 
---@return number 
function Physics2D.GetContacts(collider, colliders) end
---
---@public
---@param collider Collider2D 
---@param contactFilter ContactFilter2D 
---@param colliders List`1 
---@return number 
function Physics2D.GetContacts(collider, contactFilter, colliders) end
---
---@public
---@param rigidbody Rigidbody2D 
---@param contacts List`1 
---@return number 
function Physics2D.GetContacts(rigidbody, contacts) end
---
---@public
---@param rigidbody Rigidbody2D 
---@param contactFilter ContactFilter2D 
---@param contacts List`1 
---@return number 
function Physics2D.GetContacts(rigidbody, contactFilter, contacts) end
---
---@public
---@param rigidbody Rigidbody2D 
---@param colliders List`1 
---@return number 
function Physics2D.GetContacts(rigidbody, colliders) end
---
---@public
---@param rigidbody Rigidbody2D 
---@param contactFilter ContactFilter2D 
---@param colliders List`1 
---@return number 
function Physics2D.GetContacts(rigidbody, contactFilter, colliders) end
---
UnityEngine.Physics2D = Physics2D