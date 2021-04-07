---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class StencilMaterial
local StencilMaterial={ }
---
---@public
---@param baseMat Material 
---@param stencilID number 
---@return Material 
function StencilMaterial.Add(baseMat, stencilID) end
---
---@public
---@param baseMat Material 
---@param stencilID number 
---@param operation number 
---@param compareFunction number 
---@param colorWriteMask number 
---@return Material 
function StencilMaterial.Add(baseMat, stencilID, operation, compareFunction, colorWriteMask) end
---
---@public
---@param baseMat Material 
---@param stencilID number 
---@param operation number 
---@param compareFunction number 
---@param colorWriteMask number 
---@param readMask number 
---@param writeMask number 
---@return Material 
function StencilMaterial.Add(baseMat, stencilID, operation, compareFunction, colorWriteMask, readMask, writeMask) end
---
---@public
---@param customMat Material 
---@return void 
function StencilMaterial.Remove(customMat) end
---
---@public
---@return void 
function StencilMaterial.ClearAll() end
---
UnityEngine.UI.StencilMaterial = StencilMaterial