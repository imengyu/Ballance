---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationClipPlayable : ValueType
local AnimationClipPlayable={ }
---
---@public
---@param graph PlayableGraph 
---@param clip AnimationClip 
---@return AnimationClipPlayable 
function AnimationClipPlayable.Create(graph, clip) end
---
---@public
---@return PlayableHandle 
function AnimationClipPlayable:GetHandle() end
---
---@public
---@param playable AnimationClipPlayable 
---@return Playable 
function AnimationClipPlayable.op_Implicit(playable) end
---
---@public
---@param playable Playable 
---@return AnimationClipPlayable 
function AnimationClipPlayable.op_Explicit(playable) end
---
---@public
---@param other AnimationClipPlayable 
---@return boolean 
function AnimationClipPlayable:Equals(other) end
---
---@public
---@return AnimationClip 
function AnimationClipPlayable:GetAnimationClip() end
---
---@public
---@return boolean 
function AnimationClipPlayable:GetApplyFootIK() end
---
---@public
---@param value boolean 
---@return void 
function AnimationClipPlayable:SetApplyFootIK(value) end
---
---@public
---@return boolean 
function AnimationClipPlayable:GetApplyPlayableIK() end
---
---@public
---@param value boolean 
---@return void 
function AnimationClipPlayable:SetApplyPlayableIK(value) end
---
UnityEngine.Animations.AnimationClipPlayable = AnimationClipPlayable