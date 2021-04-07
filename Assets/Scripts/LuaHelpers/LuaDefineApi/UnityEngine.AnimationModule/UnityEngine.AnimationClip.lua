---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationClip : Motion
---@field public length number 
---@field public frameRate number 
---@field public wrapMode number 
---@field public localBounds Bounds 
---@field public legacy boolean 
---@field public humanMotion boolean 
---@field public empty boolean 
---@field public hasGenericRootTransform boolean 
---@field public hasMotionFloatCurves boolean 
---@field public hasMotionCurves boolean 
---@field public hasRootCurves boolean 
---@field public events AnimationEvent[] 
local AnimationClip={ }
---
---@public
---@param go GameObject 
---@param time number 
---@return void 
function AnimationClip:SampleAnimation(go, time) end
---
---@public
---@param relativePath string 
---@param type Type 
---@param propertyName string 
---@param curve AnimationCurve 
---@return void 
function AnimationClip:SetCurve(relativePath, type, propertyName, curve) end
---
---@public
---@return void 
function AnimationClip:EnsureQuaternionContinuity() end
---
---@public
---@return void 
function AnimationClip:ClearCurves() end
---
---@public
---@param evt AnimationEvent 
---@return void 
function AnimationClip:AddEvent(evt) end
---
UnityEngine.AnimationClip = AnimationClip