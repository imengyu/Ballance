---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SpherecastCommand : ValueType
---@field public origin Vector3 
---@field public radius number 
---@field public direction Vector3 
---@field public distance number 
---@field public layerMask number 
local SpherecastCommand={ }
---
---@public
---@param commands NativeArray`1 
---@param results NativeArray`1 
---@param minCommandsPerJob number 
---@param dependsOn JobHandle 
---@return JobHandle 
function SpherecastCommand.ScheduleBatch(commands, results, minCommandsPerJob, dependsOn) end
---
UnityEngine.SpherecastCommand = SpherecastCommand