---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationPosePlayable : ValueType
---@field public Null AnimationPosePlayable 
local AnimationPosePlayable={ }
---
---@public
---@param graph PlayableGraph 
---@return AnimationPosePlayable 
function AnimationPosePlayable.Create(graph) end
---
---@public
---@return PlayableHandle 
function AnimationPosePlayable:GetHandle() end
---
---@public
---@param playable AnimationPosePlayable 
---@return Playable 
function AnimationPosePlayable.op_Implicit(playable) end
---
---@public
---@param playable Playable 
---@return AnimationPosePlayable 
function AnimationPosePlayable.op_Explicit(playable) end
---
---@public
---@param other AnimationPosePlayable 
---@return boolean 
function AnimationPosePlayable:Equals(other) end
---
---@public
---@return boolean 
function AnimationPosePlayable:GetMustReadPreviousPose() end
---
---@public
---@param value boolean 
---@return void 
function AnimationPosePlayable:SetMustReadPreviousPose(value) end
---
---@public
---@return boolean 
function AnimationPosePlayable:GetReadDefaultPose() end
---
---@public
---@param value boolean 
---@return void 
function AnimationPosePlayable:SetReadDefaultPose(value) end
---
---@public
---@return boolean 
function AnimationPosePlayable:GetApplyFootIK() end
---
---@public
---@param value boolean 
---@return void 
function AnimationPosePlayable:SetApplyFootIK(value) end
---
UnityEngine.Animations.AnimationPosePlayable = AnimationPosePlayable