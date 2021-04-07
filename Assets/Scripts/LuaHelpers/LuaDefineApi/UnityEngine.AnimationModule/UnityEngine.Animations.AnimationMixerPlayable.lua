---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationMixerPlayable : ValueType
---@field public Null AnimationMixerPlayable 
local AnimationMixerPlayable={ }
---
---@public
---@param graph PlayableGraph 
---@param inputCount number 
---@param normalizeWeights boolean 
---@return AnimationMixerPlayable 
function AnimationMixerPlayable.Create(graph, inputCount, normalizeWeights) end
---
---@public
---@return PlayableHandle 
function AnimationMixerPlayable:GetHandle() end
---
---@public
---@param playable AnimationMixerPlayable 
---@return Playable 
function AnimationMixerPlayable.op_Implicit(playable) end
---
---@public
---@param playable Playable 
---@return AnimationMixerPlayable 
function AnimationMixerPlayable.op_Explicit(playable) end
---
---@public
---@param other AnimationMixerPlayable 
---@return boolean 
function AnimationMixerPlayable:Equals(other) end
---
UnityEngine.Animations.AnimationMixerPlayable = AnimationMixerPlayable