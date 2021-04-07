---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationEvent
---@field public data string 
---@field public stringParameter string 
---@field public floatParameter number 
---@field public intParameter number 
---@field public objectReferenceParameter Object 
---@field public functionName string 
---@field public time number 
---@field public messageOptions number 
---@field public isFiredByLegacy boolean 
---@field public isFiredByAnimator boolean 
---@field public animationState AnimationState 
---@field public animatorStateInfo AnimatorStateInfo 
---@field public animatorClipInfo AnimatorClipInfo 
local AnimationEvent={ }
---
UnityEngine.AnimationEvent = AnimationEvent