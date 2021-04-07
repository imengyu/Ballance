---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class MuscleHandle : ValueType
---@field public humanPartDof number 
---@field public dof number 
---@field public name string 
---@field public muscleHandleCount number 
local MuscleHandle={ }
---
---@public
---@param muscleHandles MuscleHandle[] 
---@return void 
function MuscleHandle.GetMuscleHandles(muscleHandles) end
---
UnityEngine.Animations.MuscleHandle = MuscleHandle