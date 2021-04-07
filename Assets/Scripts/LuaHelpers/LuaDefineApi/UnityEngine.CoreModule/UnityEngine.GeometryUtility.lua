---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GeometryUtility
local GeometryUtility={ }
---
---@public
---@param camera Camera 
---@return Plane[] 
function GeometryUtility.CalculateFrustumPlanes(camera) end
---
---@public
---@param worldToProjectionMatrix Matrix4x4 
---@return Plane[] 
function GeometryUtility.CalculateFrustumPlanes(worldToProjectionMatrix) end
---
---@public
---@param camera Camera 
---@param planes Plane[] 
---@return void 
function GeometryUtility.CalculateFrustumPlanes(camera, planes) end
---
---@public
---@param worldToProjectionMatrix Matrix4x4 
---@param planes Plane[] 
---@return void 
function GeometryUtility.CalculateFrustumPlanes(worldToProjectionMatrix, planes) end
---
---@public
---@param positions Vector3[] 
---@param transform Matrix4x4 
---@return Bounds 
function GeometryUtility.CalculateBounds(positions, transform) end
---
---@public
---@param vertices Vector3[] 
---@param plane Plane& 
---@return boolean 
function GeometryUtility.TryCreatePlaneFromPolygon(vertices, plane) end
---
---@public
---@param planes Plane[] 
---@param bounds Bounds 
---@return boolean 
function GeometryUtility.TestPlanesAABB(planes, bounds) end
---
UnityEngine.GeometryUtility = GeometryUtility