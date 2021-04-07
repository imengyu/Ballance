---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationRemoveScalePlayable : ValueType
---@field public Null AnimationRemoveScalePlayable 
local AnimationRemoveScalePlayable={ }
---
---@public
---@param graph PlayableGraph 
---@param inputCount number 
---@return AnimationRemoveScalePlayable 
function AnimationRemoveScalePlayable.Create(graph, inputCount) end
---
---@public
---@return PlayableHandle 
function AnimationRemoveScalePlayable:GetHandle() end
---
---@public
---@param playable AnimationRemoveScalePlayable 
---@return Playable 
function AnimationRemoveScalePlayable.op_Implicit(playable) end
---
---@public
---@param playable Playable 
---@return AnimationRemoveScalePlayable 
function AnimationRemoveScalePlayable.op_Explicit(playable) end
---
---@public
---@param other AnimationRemoveScalePlayable 
---@return boolean 
function AnimationRemoveScalePlayable:Equals(other) end
---
UnityEngine.Animations.AnimationRemoveScalePlayable = AnimationRemoveScalePlayable