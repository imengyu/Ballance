---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RaycastHit : ValueType
---@field public collider Collider 
---@field public point Vector3 
---@field public normal Vector3 
---@field public barycentricCoordinate Vector3 
---@field public distance number 
---@field public triangleIndex number 
---@field public textureCoord Vector2 
---@field public textureCoord2 Vector2 
---@field public textureCoord1 Vector2 
---@field public transform Transform 
---@field public rigidbody Rigidbody 
---@field public articulationBody ArticulationBody 
---@field public lightmapCoord Vector2 
local RaycastHit={ }
---
UnityEngine.RaycastHit = RaycastHit