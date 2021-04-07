---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IJobParallelForTransform
local IJobParallelForTransform={ }
---
---@public
---@param index number 
---@param transform TransformAccess 
---@return void 
function IJobParallelForTransform:Execute(index, transform) end
---
UnityEngine.Jobs.IJobParallelForTransform = IJobParallelForTransform