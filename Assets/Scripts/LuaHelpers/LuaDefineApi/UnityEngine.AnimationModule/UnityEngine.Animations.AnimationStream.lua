---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationStream : ValueType
---@field public isValid boolean 
---@field public deltaTime number 
---@field public velocity Vector3 
---@field public angularVelocity Vector3 
---@field public rootMotionPosition Vector3 
---@field public rootMotionRotation Quaternion 
---@field public isHumanStream boolean 
---@field public inputStreamCount number 
local AnimationStream={ }
---
---@public
---@return AnimationHumanStream 
function AnimationStream:AsHuman() end
---
---@public
---@param index number 
---@return AnimationStream 
function AnimationStream:GetInputStream(index) end
---
---@public
---@param index number 
---@return number 
function AnimationStream:GetInputWeight(index) end
---
---@public
---@param animationStream AnimationStream 
---@return void 
function AnimationStream:CopyAnimationStreamMotion(animationStream) end
---
UnityEngine.Animations.AnimationStream = AnimationStream