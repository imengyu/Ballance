---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationPlayableOutput : ValueType
---@field public Null AnimationPlayableOutput 
local AnimationPlayableOutput={ }
---
---@public
---@param graph PlayableGraph 
---@param name string 
---@param target Animator 
---@return AnimationPlayableOutput 
function AnimationPlayableOutput.Create(graph, name, target) end
---
---@public
---@return PlayableOutputHandle 
function AnimationPlayableOutput:GetHandle() end
---
---@public
---@param output AnimationPlayableOutput 
---@return PlayableOutput 
function AnimationPlayableOutput.op_Implicit(output) end
---
---@public
---@param output PlayableOutput 
---@return AnimationPlayableOutput 
function AnimationPlayableOutput.op_Explicit(output) end
---
---@public
---@return Animator 
function AnimationPlayableOutput:GetTarget() end
---
---@public
---@param value Animator 
---@return void 
function AnimationPlayableOutput:SetTarget(value) end
---
UnityEngine.Animations.AnimationPlayableOutput = AnimationPlayableOutput