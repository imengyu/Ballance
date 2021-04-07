---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationHumanStream : ValueType
---@field public isValid boolean 
---@field public humanScale number 
---@field public leftFootHeight number 
---@field public rightFootHeight number 
---@field public bodyLocalPosition Vector3 
---@field public bodyLocalRotation Quaternion 
---@field public bodyPosition Vector3 
---@field public bodyRotation Quaternion 
---@field public leftFootVelocity Vector3 
---@field public rightFootVelocity Vector3 
local AnimationHumanStream={ }
---
---@public
---@param muscle MuscleHandle 
---@return number 
function AnimationHumanStream:GetMuscle(muscle) end
---
---@public
---@param muscle MuscleHandle 
---@param value number 
---@return void 
function AnimationHumanStream:SetMuscle(muscle, value) end
---
---@public
---@return void 
function AnimationHumanStream:ResetToStancePose() end
---
---@public
---@param index number 
---@return Vector3 
function AnimationHumanStream:GetGoalPositionFromPose(index) end
---
---@public
---@param index number 
---@return Quaternion 
function AnimationHumanStream:GetGoalRotationFromPose(index) end
---
---@public
---@param index number 
---@return Vector3 
function AnimationHumanStream:GetGoalLocalPosition(index) end
---
---@public
---@param index number 
---@param pos Vector3 
---@return void 
function AnimationHumanStream:SetGoalLocalPosition(index, pos) end
---
---@public
---@param index number 
---@return Quaternion 
function AnimationHumanStream:GetGoalLocalRotation(index) end
---
---@public
---@param index number 
---@param rot Quaternion 
---@return void 
function AnimationHumanStream:SetGoalLocalRotation(index, rot) end
---
---@public
---@param index number 
---@return Vector3 
function AnimationHumanStream:GetGoalPosition(index) end
---
---@public
---@param index number 
---@param pos Vector3 
---@return void 
function AnimationHumanStream:SetGoalPosition(index, pos) end
---
---@public
---@param index number 
---@return Quaternion 
function AnimationHumanStream:GetGoalRotation(index) end
---
---@public
---@param index number 
---@param rot Quaternion 
---@return void 
function AnimationHumanStream:SetGoalRotation(index, rot) end
---
---@public
---@param index number 
---@param value number 
---@return void 
function AnimationHumanStream:SetGoalWeightPosition(index, value) end
---
---@public
---@param index number 
---@param value number 
---@return void 
function AnimationHumanStream:SetGoalWeightRotation(index, value) end
---
---@public
---@param index number 
---@return number 
function AnimationHumanStream:GetGoalWeightPosition(index) end
---
---@public
---@param index number 
---@return number 
function AnimationHumanStream:GetGoalWeightRotation(index) end
---
---@public
---@param index number 
---@return Vector3 
function AnimationHumanStream:GetHintPosition(index) end
---
---@public
---@param index number 
---@param pos Vector3 
---@return void 
function AnimationHumanStream:SetHintPosition(index, pos) end
---
---@public
---@param index number 
---@param value number 
---@return void 
function AnimationHumanStream:SetHintWeightPosition(index, value) end
---
---@public
---@param index number 
---@return number 
function AnimationHumanStream:GetHintWeightPosition(index) end
---
---@public
---@param lookAtPosition Vector3 
---@return void 
function AnimationHumanStream:SetLookAtPosition(lookAtPosition) end
---
---@public
---@param weight number 
---@return void 
function AnimationHumanStream:SetLookAtClampWeight(weight) end
---
---@public
---@param weight number 
---@return void 
function AnimationHumanStream:SetLookAtBodyWeight(weight) end
---
---@public
---@param weight number 
---@return void 
function AnimationHumanStream:SetLookAtHeadWeight(weight) end
---
---@public
---@param weight number 
---@return void 
function AnimationHumanStream:SetLookAtEyesWeight(weight) end
---
---@public
---@return void 
function AnimationHumanStream:SolveIK() end
---
UnityEngine.Animations.AnimationHumanStream = AnimationHumanStream