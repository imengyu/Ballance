---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BoxcastCommand : ValueType
---@field public center Vector3 
---@field public halfExtents Vector3 
---@field public orientation Quaternion 
---@field public direction Vector3 
---@field public distance number 
---@field public layerMask number 
local BoxcastCommand={ }
---
---@public
---@param commands NativeArray`1 
---@param results NativeArray`1 
---@param minCommandsPerJob number 
---@param dependsOn JobHandle 
---@return JobHandle 
function BoxcastCommand.ScheduleBatch(commands, results, minCommandsPerJob, dependsOn) end
---
UnityEngine.BoxcastCommand = BoxcastCommand