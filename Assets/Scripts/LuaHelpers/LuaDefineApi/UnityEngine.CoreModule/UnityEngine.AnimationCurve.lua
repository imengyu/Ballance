---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationCurve
---@field public keys Keyframe[] 
---@field public Item Keyframe 
---@field public length number 
---@field public preWrapMode number 
---@field public postWrapMode number 
local AnimationCurve={ }
---
---@public
---@param time number 
---@return number 
function AnimationCurve:Evaluate(time) end
---
---@public
---@param time number 
---@param value number 
---@return number 
function AnimationCurve:AddKey(time, value) end
---
---@public
---@param key Keyframe 
---@return number 
function AnimationCurve:AddKey(key) end
---
---@public
---@param index number 
---@param key Keyframe 
---@return number 
function AnimationCurve:MoveKey(index, key) end
---
---@public
---@param index number 
---@return void 
function AnimationCurve:RemoveKey(index) end
---
---@public
---@param index number 
---@param weight number 
---@return void 
function AnimationCurve:SmoothTangents(index, weight) end
---
---@public
---@param timeStart number 
---@param timeEnd number 
---@param value number 
---@return AnimationCurve 
function AnimationCurve.Constant(timeStart, timeEnd, value) end
---
---@public
---@param timeStart number 
---@param valueStart number 
---@param timeEnd number 
---@param valueEnd number 
---@return AnimationCurve 
function AnimationCurve.Linear(timeStart, valueStart, timeEnd, valueEnd) end
---
---@public
---@param timeStart number 
---@param valueStart number 
---@param timeEnd number 
---@param valueEnd number 
---@return AnimationCurve 
function AnimationCurve.EaseInOut(timeStart, valueStart, timeEnd, valueEnd) end
---
---@public
---@param o Object 
---@return boolean 
function AnimationCurve:Equals(o) end
---
---@public
---@param other AnimationCurve 
---@return boolean 
function AnimationCurve:Equals(other) end
---
---@public
---@return number 
function AnimationCurve:GetHashCode() end
---
UnityEngine.AnimationCurve = AnimationCurve