---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimatorControllerPlayable : ValueType
---@field public Null AnimatorControllerPlayable 
local AnimatorControllerPlayable={ }
---
---@public
---@param graph PlayableGraph 
---@param controller RuntimeAnimatorController 
---@return AnimatorControllerPlayable 
function AnimatorControllerPlayable.Create(graph, controller) end
---
---@public
---@return PlayableHandle 
function AnimatorControllerPlayable:GetHandle() end
---
---@public
---@param handle PlayableHandle 
---@return void 
function AnimatorControllerPlayable:SetHandle(handle) end
---
---@public
---@param playable AnimatorControllerPlayable 
---@return Playable 
function AnimatorControllerPlayable.op_Implicit(playable) end
---
---@public
---@param playable Playable 
---@return AnimatorControllerPlayable 
function AnimatorControllerPlayable.op_Explicit(playable) end
---
---@public
---@param other AnimatorControllerPlayable 
---@return boolean 
function AnimatorControllerPlayable:Equals(other) end
---
---@public
---@param name string 
---@return number 
function AnimatorControllerPlayable:GetFloat(name) end
---
---@public
---@param id number 
---@return number 
function AnimatorControllerPlayable:GetFloat(id) end
---
---@public
---@param name string 
---@param value number 
---@return void 
function AnimatorControllerPlayable:SetFloat(name, value) end
---
---@public
---@param id number 
---@param value number 
---@return void 
function AnimatorControllerPlayable:SetFloat(id, value) end
---
---@public
---@param name string 
---@return boolean 
function AnimatorControllerPlayable:GetBool(name) end
---
---@public
---@param id number 
---@return boolean 
function AnimatorControllerPlayable:GetBool(id) end
---
---@public
---@param name string 
---@param value boolean 
---@return void 
function AnimatorControllerPlayable:SetBool(name, value) end
---
---@public
---@param id number 
---@param value boolean 
---@return void 
function AnimatorControllerPlayable:SetBool(id, value) end
---
---@public
---@param name string 
---@return number 
function AnimatorControllerPlayable:GetInteger(name) end
---
---@public
---@param id number 
---@return number 
function AnimatorControllerPlayable:GetInteger(id) end
---
---@public
---@param name string 
---@param value number 
---@return void 
function AnimatorControllerPlayable:SetInteger(name, value) end
---
---@public
---@param id number 
---@param value number 
---@return void 
function AnimatorControllerPlayable:SetInteger(id, value) end
---
---@public
---@param name string 
---@return void 
function AnimatorControllerPlayable:SetTrigger(name) end
---
---@public
---@param id number 
---@return void 
function AnimatorControllerPlayable:SetTrigger(id) end
---
---@public
---@param name string 
---@return void 
function AnimatorControllerPlayable:ResetTrigger(name) end
---
---@public
---@param id number 
---@return void 
function AnimatorControllerPlayable:ResetTrigger(id) end
---
---@public
---@param name string 
---@return boolean 
function AnimatorControllerPlayable:IsParameterControlledByCurve(name) end
---
---@public
---@param id number 
---@return boolean 
function AnimatorControllerPlayable:IsParameterControlledByCurve(id) end
---
---@public
---@return number 
function AnimatorControllerPlayable:GetLayerCount() end
---
---@public
---@param layerIndex number 
---@return string 
function AnimatorControllerPlayable:GetLayerName(layerIndex) end
---
---@public
---@param layerName string 
---@return number 
function AnimatorControllerPlayable:GetLayerIndex(layerName) end
---
---@public
---@param layerIndex number 
---@return number 
function AnimatorControllerPlayable:GetLayerWeight(layerIndex) end
---
---@public
---@param layerIndex number 
---@param weight number 
---@return void 
function AnimatorControllerPlayable:SetLayerWeight(layerIndex, weight) end
---
---@public
---@param layerIndex number 
---@return AnimatorStateInfo 
function AnimatorControllerPlayable:GetCurrentAnimatorStateInfo(layerIndex) end
---
---@public
---@param layerIndex number 
---@return AnimatorStateInfo 
function AnimatorControllerPlayable:GetNextAnimatorStateInfo(layerIndex) end
---
---@public
---@param layerIndex number 
---@return AnimatorTransitionInfo 
function AnimatorControllerPlayable:GetAnimatorTransitionInfo(layerIndex) end
---
---@public
---@param layerIndex number 
---@return AnimatorClipInfo[] 
function AnimatorControllerPlayable:GetCurrentAnimatorClipInfo(layerIndex) end
---
---@public
---@param layerIndex number 
---@param clips List`1 
---@return void 
function AnimatorControllerPlayable:GetCurrentAnimatorClipInfo(layerIndex, clips) end
---
---@public
---@param layerIndex number 
---@param clips List`1 
---@return void 
function AnimatorControllerPlayable:GetNextAnimatorClipInfo(layerIndex, clips) end
---
---@public
---@param layerIndex number 
---@return number 
function AnimatorControllerPlayable:GetCurrentAnimatorClipInfoCount(layerIndex) end
---
---@public
---@param layerIndex number 
---@return number 
function AnimatorControllerPlayable:GetNextAnimatorClipInfoCount(layerIndex) end
---
---@public
---@param layerIndex number 
---@return AnimatorClipInfo[] 
function AnimatorControllerPlayable:GetNextAnimatorClipInfo(layerIndex) end
---
---@public
---@param layerIndex number 
---@return boolean 
function AnimatorControllerPlayable:IsInTransition(layerIndex) end
---
---@public
---@return number 
function AnimatorControllerPlayable:GetParameterCount() end
---
---@public
---@param index number 
---@return AnimatorControllerParameter 
function AnimatorControllerPlayable:GetParameter(index) end
---
---@public
---@param stateName string 
---@param transitionDuration number 
---@return void 
function AnimatorControllerPlayable:CrossFadeInFixedTime(stateName, transitionDuration) end
---
---@public
---@param stateName string 
---@param transitionDuration number 
---@param layer number 
---@return void 
function AnimatorControllerPlayable:CrossFadeInFixedTime(stateName, transitionDuration, layer) end
---
---@public
---@param stateName string 
---@param transitionDuration number 
---@param layer number 
---@param fixedTime number 
---@return void 
function AnimatorControllerPlayable:CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime) end
---
---@public
---@param stateNameHash number 
---@param transitionDuration number 
---@return void 
function AnimatorControllerPlayable:CrossFadeInFixedTime(stateNameHash, transitionDuration) end
---
---@public
---@param stateNameHash number 
---@param transitionDuration number 
---@param layer number 
---@return void 
function AnimatorControllerPlayable:CrossFadeInFixedTime(stateNameHash, transitionDuration, layer) end
---
---@public
---@param stateNameHash number 
---@param transitionDuration number 
---@param layer number 
---@param fixedTime number 
---@return void 
function AnimatorControllerPlayable:CrossFadeInFixedTime(stateNameHash, transitionDuration, layer, fixedTime) end
---
---@public
---@param stateName string 
---@param transitionDuration number 
---@return void 
function AnimatorControllerPlayable:CrossFade(stateName, transitionDuration) end
---
---@public
---@param stateName string 
---@param transitionDuration number 
---@param layer number 
---@return void 
function AnimatorControllerPlayable:CrossFade(stateName, transitionDuration, layer) end
---
---@public
---@param stateName string 
---@param transitionDuration number 
---@param layer number 
---@param normalizedTime number 
---@return void 
function AnimatorControllerPlayable:CrossFade(stateName, transitionDuration, layer, normalizedTime) end
---
---@public
---@param stateNameHash number 
---@param transitionDuration number 
---@return void 
function AnimatorControllerPlayable:CrossFade(stateNameHash, transitionDuration) end
---
---@public
---@param stateNameHash number 
---@param transitionDuration number 
---@param layer number 
---@return void 
function AnimatorControllerPlayable:CrossFade(stateNameHash, transitionDuration, layer) end
---
---@public
---@param stateNameHash number 
---@param transitionDuration number 
---@param layer number 
---@param normalizedTime number 
---@return void 
function AnimatorControllerPlayable:CrossFade(stateNameHash, transitionDuration, layer, normalizedTime) end
---
---@public
---@param stateName string 
---@return void 
function AnimatorControllerPlayable:PlayInFixedTime(stateName) end
---
---@public
---@param stateName string 
---@param layer number 
---@return void 
function AnimatorControllerPlayable:PlayInFixedTime(stateName, layer) end
---
---@public
---@param stateName string 
---@param layer number 
---@param fixedTime number 
---@return void 
function AnimatorControllerPlayable:PlayInFixedTime(stateName, layer, fixedTime) end
---
---@public
---@param stateNameHash number 
---@return void 
function AnimatorControllerPlayable:PlayInFixedTime(stateNameHash) end
---
---@public
---@param stateNameHash number 
---@param layer number 
---@return void 
function AnimatorControllerPlayable:PlayInFixedTime(stateNameHash, layer) end
---
---@public
---@param stateNameHash number 
---@param layer number 
---@param fixedTime number 
---@return void 
function AnimatorControllerPlayable:PlayInFixedTime(stateNameHash, layer, fixedTime) end
---
---@public
---@param stateName string 
---@return void 
function AnimatorControllerPlayable:Play(stateName) end
---
---@public
---@param stateName string 
---@param layer number 
---@return void 
function AnimatorControllerPlayable:Play(stateName, layer) end
---
---@public
---@param stateName string 
---@param layer number 
---@param normalizedTime number 
---@return void 
function AnimatorControllerPlayable:Play(stateName, layer, normalizedTime) end
---
---@public
---@param stateNameHash number 
---@return void 
function AnimatorControllerPlayable:Play(stateNameHash) end
---
---@public
---@param stateNameHash number 
---@param layer number 
---@return void 
function AnimatorControllerPlayable:Play(stateNameHash, layer) end
---
---@public
---@param stateNameHash number 
---@param layer number 
---@param normalizedTime number 
---@return void 
function AnimatorControllerPlayable:Play(stateNameHash, layer, normalizedTime) end
---
---@public
---@param layerIndex number 
---@param stateID number 
---@return boolean 
function AnimatorControllerPlayable:HasState(layerIndex, stateID) end
---
UnityEngine.Animations.AnimatorControllerPlayable = AnimatorControllerPlayable