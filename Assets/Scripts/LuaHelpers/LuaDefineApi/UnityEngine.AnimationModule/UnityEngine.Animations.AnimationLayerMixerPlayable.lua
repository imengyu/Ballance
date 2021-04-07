---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationLayerMixerPlayable : ValueType
---@field public Null AnimationLayerMixerPlayable 
local AnimationLayerMixerPlayable={ }
---
---@public
---@param graph PlayableGraph 
---@param inputCount number 
---@return AnimationLayerMixerPlayable 
function AnimationLayerMixerPlayable.Create(graph, inputCount) end
---
---@public
---@return PlayableHandle 
function AnimationLayerMixerPlayable:GetHandle() end
---
---@public
---@param playable AnimationLayerMixerPlayable 
---@return Playable 
function AnimationLayerMixerPlayable.op_Implicit(playable) end
---
---@public
---@param playable Playable 
---@return AnimationLayerMixerPlayable 
function AnimationLayerMixerPlayable.op_Explicit(playable) end
---
---@public
---@param other AnimationLayerMixerPlayable 
---@return boolean 
function AnimationLayerMixerPlayable:Equals(other) end
---
---@public
---@param layerIndex number 
---@return boolean 
function AnimationLayerMixerPlayable:IsLayerAdditive(layerIndex) end
---
---@public
---@param layerIndex number 
---@param value boolean 
---@return void 
function AnimationLayerMixerPlayable:SetLayerAdditive(layerIndex, value) end
---
---@public
---@param layerIndex number 
---@param mask AvatarMask 
---@return void 
function AnimationLayerMixerPlayable:SetLayerMaskFromAvatarMask(layerIndex, mask) end
---
UnityEngine.Animations.AnimationLayerMixerPlayable = AnimationLayerMixerPlayable