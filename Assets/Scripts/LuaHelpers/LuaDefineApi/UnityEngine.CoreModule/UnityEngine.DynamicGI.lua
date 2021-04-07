---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class DynamicGI
---@field public indirectScale number 
---@field public updateThreshold number 
---@field public materialUpdateTimeSlice number 
---@field public synchronousMode boolean 
---@field public isConverged boolean 
local DynamicGI={ }
---
---@public
---@param renderer Renderer 
---@param color Color 
---@return void 
function DynamicGI.SetEmissive(renderer, color) end
---
---@public
---@param input Single[] 
---@return void 
function DynamicGI.SetEnvironmentData(input) end
---
---@public
---@return void 
function DynamicGI.UpdateEnvironment() end
---
---@public
---@param renderer Renderer 
---@return void 
function DynamicGI.UpdateMaterials(renderer) end
---
---@public
---@param renderer Object 
---@return void 
function DynamicGI.UpdateMaterials(renderer) end
---
---@public
---@param renderer Object 
---@param x number 
---@param y number 
---@param width number 
---@param height number 
---@return void 
function DynamicGI.UpdateMaterials(renderer, x, y, width, height) end
---
UnityEngine.DynamicGI = DynamicGI