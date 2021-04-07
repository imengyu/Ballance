---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Animation : Behaviour
---@field public clip AnimationClip 
---@field public playAutomatically boolean 
---@field public wrapMode number 
---@field public isPlaying boolean 
---@field public Item AnimationState 
---@field public animatePhysics boolean 
---@field public animateOnlyIfVisible boolean 
---@field public cullingType number 
---@field public localBounds Bounds 
local Animation={ }
---
---@public
---@return void 
function Animation:Stop() end
---
---@public
---@param name string 
---@return void 
function Animation:Stop(name) end
---
---@public
---@return void 
function Animation:Rewind() end
---
---@public
---@param name string 
---@return void 
function Animation:Rewind(name) end
---
---@public
---@return void 
function Animation:Sample() end
---
---@public
---@param name string 
---@return boolean 
function Animation:IsPlaying(name) end
---
---@public
---@return boolean 
function Animation:Play() end
---
---@public
---@param mode number 
---@return boolean 
function Animation:Play(mode) end
---
---@public
---@param animation string 
---@return boolean 
function Animation:Play(animation) end
---
---@public
---@param animation string 
---@param mode number 
---@return boolean 
function Animation:Play(animation, mode) end
---
---@public
---@param animation string 
---@return void 
function Animation:CrossFade(animation) end
---
---@public
---@param animation string 
---@param fadeLength number 
---@return void 
function Animation:CrossFade(animation, fadeLength) end
---
---@public
---@param animation string 
---@param fadeLength number 
---@param mode number 
---@return void 
function Animation:CrossFade(animation, fadeLength, mode) end
---
---@public
---@param animation string 
---@return void 
function Animation:Blend(animation) end
---
---@public
---@param animation string 
---@param targetWeight number 
---@return void 
function Animation:Blend(animation, targetWeight) end
---
---@public
---@param animation string 
---@param targetWeight number 
---@param fadeLength number 
---@return void 
function Animation:Blend(animation, targetWeight, fadeLength) end
---
---@public
---@param animation string 
---@return AnimationState 
function Animation:CrossFadeQueued(animation) end
---
---@public
---@param animation string 
---@param fadeLength number 
---@return AnimationState 
function Animation:CrossFadeQueued(animation, fadeLength) end
---
---@public
---@param animation string 
---@param fadeLength number 
---@param queue number 
---@return AnimationState 
function Animation:CrossFadeQueued(animation, fadeLength, queue) end
---
---@public
---@param animation string 
---@param fadeLength number 
---@param queue number 
---@param mode number 
---@return AnimationState 
function Animation:CrossFadeQueued(animation, fadeLength, queue, mode) end
---
---@public
---@param animation string 
---@return AnimationState 
function Animation:PlayQueued(animation) end
---
---@public
---@param animation string 
---@param queue number 
---@return AnimationState 
function Animation:PlayQueued(animation, queue) end
---
---@public
---@param animation string 
---@param queue number 
---@param mode number 
---@return AnimationState 
function Animation:PlayQueued(animation, queue, mode) end
---
---@public
---@param clip AnimationClip 
---@param newName string 
---@return void 
function Animation:AddClip(clip, newName) end
---
---@public
---@param clip AnimationClip 
---@param newName string 
---@param firstFrame number 
---@param lastFrame number 
---@return void 
function Animation:AddClip(clip, newName, firstFrame, lastFrame) end
---
---@public
---@param clip AnimationClip 
---@param newName string 
---@param firstFrame number 
---@param lastFrame number 
---@param addLoopFrame boolean 
---@return void 
function Animation:AddClip(clip, newName, firstFrame, lastFrame, addLoopFrame) end
---
---@public
---@param clip AnimationClip 
---@return void 
function Animation:RemoveClip(clip) end
---
---@public
---@param clipName string 
---@return void 
function Animation:RemoveClip(clipName) end
---
---@public
---@return number 
function Animation:GetClipCount() end
---
---@public
---@param mode number 
---@return boolean 
function Animation:Play(mode) end
---
---@public
---@param animation string 
---@param mode number 
---@return boolean 
function Animation:Play(animation, mode) end
---
---@public
---@param layer number 
---@return void 
function Animation:SyncLayer(layer) end
---
---@public
---@return IEnumerator 
function Animation:GetEnumerator() end
---
---@public
---@param name string 
---@return AnimationClip 
function Animation:GetClip(name) end
---
UnityEngine.Animation = Animation