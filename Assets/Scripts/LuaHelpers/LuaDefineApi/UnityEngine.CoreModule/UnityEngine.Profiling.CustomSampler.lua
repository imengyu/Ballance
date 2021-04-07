---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CustomSampler : Sampler
local CustomSampler={ }
---
---@public
---@param name string 
---@param collectGpuData boolean 
---@return CustomSampler 
function CustomSampler.Create(name, collectGpuData) end
---
---@public
---@return void 
function CustomSampler:Begin() end
---
---@public
---@param targetObject Object 
---@return void 
function CustomSampler:Begin(targetObject) end
---
---@public
---@return void 
function CustomSampler:End() end
---
UnityEngine.Profiling.CustomSampler = CustomSampler