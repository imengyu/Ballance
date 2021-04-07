---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CapsulecastCommand : ValueType
---@field public point1 Vector3 
---@field public point2 Vector3 
---@field public radius number 
---@field public direction Vector3 
---@field public distance number 
---@field public layerMask number 
local CapsulecastCommand={ }
---
---@public
---@param commands NativeArray`1 
---@param results NativeArray`1 
---@param minCommandsPerJob number 
---@param dependsOn JobHandle 
---@return JobHandle 
function CapsulecastCommand.ScheduleBatch(commands, results, minCommandsPerJob, dependsOn) end
---
UnityEngine.CapsulecastCommand = CapsulecastCommand