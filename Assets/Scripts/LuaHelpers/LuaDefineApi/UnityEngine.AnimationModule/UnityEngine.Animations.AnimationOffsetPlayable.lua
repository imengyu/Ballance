---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationOffsetPlayable : ValueType
---@field public Null AnimationOffsetPlayable 
local AnimationOffsetPlayable={ }
---
---@public
---@param graph PlayableGraph 
---@param position Vector3 
---@param rotation Quaternion 
---@param inputCount number 
---@return AnimationOffsetPlayable 
function AnimationOffsetPlayable.Create(graph, position, rotation, inputCount) end
---
---@public
---@return PlayableHandle 
function AnimationOffsetPlayable:GetHandle() end
---
---@public
---@param playable AnimationOffsetPlayable 
---@return Playable 
function AnimationOffsetPlayable.op_Implicit(playable) end
---
---@public
---@param playable Playable 
---@return AnimationOffsetPlayable 
function AnimationOffsetPlayable.op_Explicit(playable) end
---
---@public
---@param other AnimationOffsetPlayable 
---@return boolean 
function AnimationOffsetPlayable:Equals(other) end
---
---@public
---@return Vector3 
function AnimationOffsetPlayable:GetPosition() end
---
---@public
---@param value Vector3 
---@return void 
function AnimationOffsetPlayable:SetPosition(value) end
---
---@public
---@return Quaternion 
function AnimationOffsetPlayable:GetRotation() end
---
---@public
---@param value Quaternion 
---@return void 
function AnimationOffsetPlayable:SetRotation(value) end
---
UnityEngine.Animations.AnimationOffsetPlayable = AnimationOffsetPlayable