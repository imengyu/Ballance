---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Animator : Behaviour
---@field public isOptimizable boolean 
---@field public isHuman boolean 
---@field public hasRootMotion boolean 
---@field public humanScale number 
---@field public isInitialized boolean 
---@field public deltaPosition Vector3 
---@field public deltaRotation Quaternion 
---@field public velocity Vector3 
---@field public angularVelocity Vector3 
---@field public rootPosition Vector3 
---@field public rootRotation Quaternion 
---@field public applyRootMotion boolean 
---@field public linearVelocityBlending boolean 
---@field public animatePhysics boolean 
---@field public updateMode number 
---@field public hasTransformHierarchy boolean 
---@field public gravityWeight number 
---@field public bodyPosition Vector3 
---@field public bodyRotation Quaternion 
---@field public stabilizeFeet boolean 
---@field public layerCount number 
---@field public parameters AnimatorControllerParameter[] 
---@field public parameterCount number 
---@field public feetPivotActive number 
---@field public pivotWeight number 
---@field public pivotPosition Vector3 
---@field public isMatchingTarget boolean 
---@field public speed number 
---@field public targetPosition Vector3 
---@field public targetRotation Quaternion 
---@field public cullingMode number 
---@field public playbackTime number 
---@field public recorderStartTime number 
---@field public recorderStopTime number 
---@field public recorderMode number 
---@field public runtimeAnimatorController RuntimeAnimatorController 
---@field public hasBoundPlayables boolean 
---@field public avatar Avatar 
---@field public playableGraph PlayableGraph 
---@field public layersAffectMassCenter boolean 
---@field public leftFeetBottomHeight number 
---@field public rightFeetBottomHeight number 
---@field public logWarnings boolean 
---@field public fireEvents boolean 
---@field public keepAnimatorControllerStateOnDisable boolean 
local Animator={ }
---
---@public
---@param layerIndex number 
---@return AnimationInfo[] 
function Animator:GetCurrentAnimationClipState(layerIndex) end
---
---@public
---@param layerIndex number 
---@return AnimationInfo[] 
function Animator:GetNextAnimationClipState(layerIndex) end
---
---@public
---@return void 
function Animator:Stop() end
---
---@public
---@param name string 
---@return number 
function Animator:GetFloat(name) end
---
---@public
---@param id number 
---@return number 
function Animator:GetFloat(id) end
---
---@public
---@param name string 
---@param value number 
---@return void 
function Animator:SetFloat(name, value) end
---
---@public
---@param name string 
---@param value number 
---@param dampTime number 
---@param deltaTime number 
---@return void 
function Animator:SetFloat(name, value, dampTime, deltaTime) end
---
---@public
---@param id number 
---@param value number 
---@return void 
function Animator:SetFloat(id, value) end
---
---@public
---@param id number 
---@param value number 
---@param dampTime number 
---@param deltaTime number 
---@return void 
function Animator:SetFloat(id, value, dampTime, deltaTime) end
---
---@public
---@param name string 
---@return boolean 
function Animator:GetBool(name) end
---
---@public
---@param id number 
---@return boolean 
function Animator:GetBool(id) end
---
---@public
---@param name string 
---@param value boolean 
---@return void 
function Animator:SetBool(name, value) end
---
---@public
---@param id number 
---@param value boolean 
---@return void 
function Animator:SetBool(id, value) end
---
---@public
---@param name string 
---@return number 
function Animator:GetInteger(name) end
---
---@public
---@param id number 
---@return number 
function Animator:GetInteger(id) end
---
---@public
---@param name string 
---@param value number 
---@return void 
function Animator:SetInteger(name, value) end
---
---@public
---@param id number 
---@param value number 
---@return void 
function Animator:SetInteger(id, value) end
---
---@public
---@param name string 
---@return void 
function Animator:SetTrigger(name) end
---
---@public
---@param id number 
---@return void 
function Animator:SetTrigger(id) end
---
---@public
---@param name string 
---@return void 
function Animator:ResetTrigger(name) end
---
---@public
---@param id number 
---@return void 
function Animator:ResetTrigger(id) end
---
---@public
---@param name string 
---@return boolean 
function Animator:IsParameterControlledByCurve(name) end
---
---@public
---@param id number 
---@return boolean 
function Animator:IsParameterControlledByCurve(id) end
---
---@public
---@param goal number 
---@return Vector3 
function Animator:GetIKPosition(goal) end
---
---@public
---@param goal number 
---@param goalPosition Vector3 
---@return void 
function Animator:SetIKPosition(goal, goalPosition) end
---
---@public
---@param goal number 
---@return Quaternion 
function Animator:GetIKRotation(goal) end
---
---@public
---@param goal number 
---@param goalRotation Quaternion 
---@return void 
function Animator:SetIKRotation(goal, goalRotation) end
---
---@public
---@param goal number 
---@return number 
function Animator:GetIKPositionWeight(goal) end
---
---@public
---@param goal number 
---@param value number 
---@return void 
function Animator:SetIKPositionWeight(goal, value) end
---
---@public
---@param goal number 
---@return number 
function Animator:GetIKRotationWeight(goal) end
---
---@public
---@param goal number 
---@param value number 
---@return void 
function Animator:SetIKRotationWeight(goal, value) end
---
---@public
---@param hint number 
---@return Vector3 
function Animator:GetIKHintPosition(hint) end
---
---@public
---@param hint number 
---@param hintPosition Vector3 
---@return void 
function Animator:SetIKHintPosition(hint, hintPosition) end
---
---@public
---@param hint number 
---@return number 
function Animator:GetIKHintPositionWeight(hint) end
---
---@public
---@param hint number 
---@param value number 
---@return void 
function Animator:SetIKHintPositionWeight(hint, value) end
---
---@public
---@param lookAtPosition Vector3 
---@return void 
function Animator:SetLookAtPosition(lookAtPosition) end
---
---@public
---@param weight number 
---@return void 
function Animator:SetLookAtWeight(weight) end
---
---@public
---@param weight number 
---@param bodyWeight number 
---@return void 
function Animator:SetLookAtWeight(weight, bodyWeight) end
---
---@public
---@param weight number 
---@param bodyWeight number 
---@param headWeight number 
---@return void 
function Animator:SetLookAtWeight(weight, bodyWeight, headWeight) end
---
---@public
---@param weight number 
---@param bodyWeight number 
---@param headWeight number 
---@param eyesWeight number 
---@return void 
function Animator:SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight) end
---
---@public
---@param weight number 
---@param bodyWeight number 
---@param headWeight number 
---@param eyesWeight number 
---@param clampWeight number 
---@return void 
function Animator:SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight) end
---
---@public
---@param humanBoneId number 
---@param rotation Quaternion 
---@return void 
function Animator:SetBoneLocalRotation(humanBoneId, rotation) end
---
---@public
---@param fullPathHash number 
---@param layerIndex number 
---@return StateMachineBehaviour[] 
function Animator:GetBehaviours(fullPathHash, layerIndex) end
---
---@public
---@param layerIndex number 
---@return string 
function Animator:GetLayerName(layerIndex) end
---
---@public
---@param layerName string 
---@return number 
function Animator:GetLayerIndex(layerName) end
---
---@public
---@param layerIndex number 
---@return number 
function Animator:GetLayerWeight(layerIndex) end
---
---@public
---@param layerIndex number 
---@param weight number 
---@return void 
function Animator:SetLayerWeight(layerIndex, weight) end
---
---@public
---@param layerIndex number 
---@return AnimatorStateInfo 
function Animator:GetCurrentAnimatorStateInfo(layerIndex) end
---
---@public
---@param layerIndex number 
---@return AnimatorStateInfo 
function Animator:GetNextAnimatorStateInfo(layerIndex) end
---
---@public
---@param layerIndex number 
---@return AnimatorTransitionInfo 
function Animator:GetAnimatorTransitionInfo(layerIndex) end
---
---@public
---@param layerIndex number 
---@return number 
function Animator:GetCurrentAnimatorClipInfoCount(layerIndex) end
---
---@public
---@param layerIndex number 
---@return number 
function Animator:GetNextAnimatorClipInfoCount(layerIndex) end
---
---@public
---@param layerIndex number 
---@return AnimatorClipInfo[] 
function Animator:GetCurrentAnimatorClipInfo(layerIndex) end
---
---@public
---@param layerIndex number 
---@return AnimatorClipInfo[] 
function Animator:GetNextAnimatorClipInfo(layerIndex) end
---
---@public
---@param layerIndex number 
---@param clips List`1 
---@return void 
function Animator:GetCurrentAnimatorClipInfo(layerIndex, clips) end
---
---@public
---@param layerIndex number 
---@param clips List`1 
---@return void 
function Animator:GetNextAnimatorClipInfo(layerIndex, clips) end
---
---@public
---@param layerIndex number 
---@return boolean 
function Animator:IsInTransition(layerIndex) end
---
---@public
---@param index number 
---@return AnimatorControllerParameter 
function Animator:GetParameter(index) end
---
---@public
---@param matchPosition Vector3 
---@param matchRotation Quaternion 
---@param targetBodyPart number 
---@param weightMask MatchTargetWeightMask 
---@param startNormalizedTime number 
---@return void 
function Animator:MatchTarget(matchPosition, matchRotation, targetBodyPart, weightMask, startNormalizedTime) end
---
---@public
---@param matchPosition Vector3 
---@param matchRotation Quaternion 
---@param targetBodyPart number 
---@param weightMask MatchTargetWeightMask 
---@param startNormalizedTime number 
---@param targetNormalizedTime number 
---@return void 
function Animator:MatchTarget(matchPosition, matchRotation, targetBodyPart, weightMask, startNormalizedTime, targetNormalizedTime) end
---
---@public
---@param matchPosition Vector3 
---@param matchRotation Quaternion 
---@param targetBodyPart number 
---@param weightMask MatchTargetWeightMask 
---@param startNormalizedTime number 
---@param targetNormalizedTime number 
---@param completeMatch boolean 
---@return void 
function Animator:MatchTarget(matchPosition, matchRotation, targetBodyPart, weightMask, startNormalizedTime, targetNormalizedTime, completeMatch) end
---
---@public
---@return void 
function Animator:InterruptMatchTarget() end
---
---@public
---@param completeMatch boolean 
---@return void 
function Animator:InterruptMatchTarget(completeMatch) end
---
---@public
---@param normalizedTime number 
---@return void 
function Animator:ForceStateNormalizedTime(normalizedTime) end
---
---@public
---@param stateName string 
---@param fixedTransitionDuration number 
---@return void 
function Animator:CrossFadeInFixedTime(stateName, fixedTransitionDuration) end
---
---@public
---@param stateName string 
---@param fixedTransitionDuration number 
---@param layer number 
---@return void 
function Animator:CrossFadeInFixedTime(stateName, fixedTransitionDuration, layer) end
---
---@public
---@param stateName string 
---@param fixedTransitionDuration number 
---@param layer number 
---@param fixedTimeOffset number 
---@return void 
function Animator:CrossFadeInFixedTime(stateName, fixedTransitionDuration, layer, fixedTimeOffset) end
---
---@public
---@param stateName string 
---@param fixedTransitionDuration number 
---@param layer number 
---@param fixedTimeOffset number 
---@param normalizedTransitionTime number 
---@return void 
function Animator:CrossFadeInFixedTime(stateName, fixedTransitionDuration, layer, fixedTimeOffset, normalizedTransitionTime) end
---
---@public
---@param stateHashName number 
---@param fixedTransitionDuration number 
---@param layer number 
---@param fixedTimeOffset number 
---@return void 
function Animator:CrossFadeInFixedTime(stateHashName, fixedTransitionDuration, layer, fixedTimeOffset) end
---
---@public
---@param stateHashName number 
---@param fixedTransitionDuration number 
---@param layer number 
---@return void 
function Animator:CrossFadeInFixedTime(stateHashName, fixedTransitionDuration, layer) end
---
---@public
---@param stateHashName number 
---@param fixedTransitionDuration number 
---@return void 
function Animator:CrossFadeInFixedTime(stateHashName, fixedTransitionDuration) end
---
---@public
---@param stateHashName number 
---@param fixedTransitionDuration number 
---@param layer number 
---@param fixedTimeOffset number 
---@param normalizedTransitionTime number 
---@return void 
function Animator:CrossFadeInFixedTime(stateHashName, fixedTransitionDuration, layer, fixedTimeOffset, normalizedTransitionTime) end
---
---@public
---@return void 
function Animator:WriteDefaultValues() end
---
---@public
---@param stateName string 
---@param normalizedTransitionDuration number 
---@param layer number 
---@param normalizedTimeOffset number 
---@return void 
function Animator:CrossFade(stateName, normalizedTransitionDuration, layer, normalizedTimeOffset) end
---
---@public
---@param stateName string 
---@param normalizedTransitionDuration number 
---@param layer number 
---@return void 
function Animator:CrossFade(stateName, normalizedTransitionDuration, layer) end
---
---@public
---@param stateName string 
---@param normalizedTransitionDuration number 
---@return void 
function Animator:CrossFade(stateName, normalizedTransitionDuration) end
---
---@public
---@param stateName string 
---@param normalizedTransitionDuration number 
---@param layer number 
---@param normalizedTimeOffset number 
---@param normalizedTransitionTime number 
---@return void 
function Animator:CrossFade(stateName, normalizedTransitionDuration, layer, normalizedTimeOffset, normalizedTransitionTime) end
---
---@public
---@param stateHashName number 
---@param normalizedTransitionDuration number 
---@param layer number 
---@param normalizedTimeOffset number 
---@param normalizedTransitionTime number 
---@return void 
function Animator:CrossFade(stateHashName, normalizedTransitionDuration, layer, normalizedTimeOffset, normalizedTransitionTime) end
---
---@public
---@param stateHashName number 
---@param normalizedTransitionDuration number 
---@param layer number 
---@param normalizedTimeOffset number 
---@return void 
function Animator:CrossFade(stateHashName, normalizedTransitionDuration, layer, normalizedTimeOffset) end
---
---@public
---@param stateHashName number 
---@param normalizedTransitionDuration number 
---@param layer number 
---@return void 
function Animator:CrossFade(stateHashName, normalizedTransitionDuration, layer) end
---
---@public
---@param stateHashName number 
---@param normalizedTransitionDuration number 
---@return void 
function Animator:CrossFade(stateHashName, normalizedTransitionDuration) end
---
---@public
---@param stateName string 
---@param layer number 
---@return void 
function Animator:PlayInFixedTime(stateName, layer) end
---
---@public
---@param stateName string 
---@return void 
function Animator:PlayInFixedTime(stateName) end
---
---@public
---@param stateName string 
---@param layer number 
---@param fixedTime number 
---@return void 
function Animator:PlayInFixedTime(stateName, layer, fixedTime) end
---
---@public
---@param stateNameHash number 
---@param layer number 
---@param fixedTime number 
---@return void 
function Animator:PlayInFixedTime(stateNameHash, layer, fixedTime) end
---
---@public
---@param stateNameHash number 
---@param layer number 
---@return void 
function Animator:PlayInFixedTime(stateNameHash, layer) end
---
---@public
---@param stateNameHash number 
---@return void 
function Animator:PlayInFixedTime(stateNameHash) end
---
---@public
---@param stateName string 
---@param layer number 
---@return void 
function Animator:Play(stateName, layer) end
---
---@public
---@param stateName string 
---@return void 
function Animator:Play(stateName) end
---
---@public
---@param stateName string 
---@param layer number 
---@param normalizedTime number 
---@return void 
function Animator:Play(stateName, layer, normalizedTime) end
---
---@public
---@param stateNameHash number 
---@param layer number 
---@param normalizedTime number 
---@return void 
function Animator:Play(stateNameHash, layer, normalizedTime) end
---
---@public
---@param stateNameHash number 
---@param layer number 
---@return void 
function Animator:Play(stateNameHash, layer) end
---
---@public
---@param stateNameHash number 
---@return void 
function Animator:Play(stateNameHash) end
---
---@public
---@param targetIndex number 
---@param targetNormalizedTime number 
---@return void 
function Animator:SetTarget(targetIndex, targetNormalizedTime) end
---
---@public
---@param transform Transform 
---@return boolean 
function Animator:IsControlled(transform) end
---
---@public
---@param humanBoneId number 
---@return Transform 
function Animator:GetBoneTransform(humanBoneId) end
---
---@public
---@return void 
function Animator:StartPlayback() end
---
---@public
---@return void 
function Animator:StopPlayback() end
---
---@public
---@param frameCount number 
---@return void 
function Animator:StartRecording(frameCount) end
---
---@public
---@return void 
function Animator:StopRecording() end
---
---@public
---@param layerIndex number 
---@param stateID number 
---@return boolean 
function Animator:HasState(layerIndex, stateID) end
---
---@public
---@param name string 
---@return number 
function Animator.StringToHash(name) end
---
---@public
---@param deltaTime number 
---@return void 
function Animator:Update(deltaTime) end
---
---@public
---@return void 
function Animator:Rebind() end
---
---@public
---@return void 
function Animator:ApplyBuiltinRootMotion() end
---
---@public
---@param name string 
---@return Vector3 
function Animator:GetVector(name) end
---
---@public
---@param id number 
---@return Vector3 
function Animator:GetVector(id) end
---
---@public
---@param name string 
---@param value Vector3 
---@return void 
function Animator:SetVector(name, value) end
---
---@public
---@param id number 
---@param value Vector3 
---@return void 
function Animator:SetVector(id, value) end
---
---@public
---@param name string 
---@return Quaternion 
function Animator:GetQuaternion(name) end
---
---@public
---@param id number 
---@return Quaternion 
function Animator:GetQuaternion(id) end
---
---@public
---@param name string 
---@param value Quaternion 
---@return void 
function Animator:SetQuaternion(name, value) end
---
---@public
---@param id number 
---@param value Quaternion 
---@return void 
function Animator:SetQuaternion(id, value) end
---
UnityEngine.Animator = Animator