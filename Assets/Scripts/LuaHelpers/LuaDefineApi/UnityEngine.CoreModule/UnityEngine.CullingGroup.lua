---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CullingGroup
---@field public onStateChanged StateChanged 
---@field public enabled boolean 
---@field public targetCamera Camera 
local CullingGroup={ }
---
---@public
---@return void 
function CullingGroup:Dispose() end
---
---@public
---@param array BoundingSphere[] 
---@return void 
function CullingGroup:SetBoundingSpheres(array) end
---
---@public
---@param count number 
---@return void 
function CullingGroup:SetBoundingSphereCount(count) end
---
---@public
---@param index number 
---@return void 
function CullingGroup:EraseSwapBack(index) end
---
---@public
---@param visible boolean 
---@param result Int32[] 
---@param firstIndex number 
---@return number 
function CullingGroup:QueryIndices(visible, result, firstIndex) end
---
---@public
---@param distanceIndex number 
---@param result Int32[] 
---@param firstIndex number 
---@return number 
function CullingGroup:QueryIndices(distanceIndex, result, firstIndex) end
---
---@public
---@param visible boolean 
---@param distanceIndex number 
---@param result Int32[] 
---@param firstIndex number 
---@return number 
function CullingGroup:QueryIndices(visible, distanceIndex, result, firstIndex) end
---
---@public
---@param index number 
---@return boolean 
function CullingGroup:IsVisible(index) end
---
---@public
---@param index number 
---@return number 
function CullingGroup:GetDistance(index) end
---
---@public
---@param distances Single[] 
---@return void 
function CullingGroup:SetBoundingDistances(distances) end
---
---@public
---@param point Vector3 
---@return void 
function CullingGroup:SetDistanceReferencePoint(point) end
---
---@public
---@param transform Transform 
---@return void 
function CullingGroup:SetDistanceReferencePoint(transform) end
---
UnityEngine.CullingGroup = CullingGroup