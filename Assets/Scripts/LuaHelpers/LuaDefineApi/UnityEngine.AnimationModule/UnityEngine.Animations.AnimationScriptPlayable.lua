---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationScriptPlayable : ValueType
---@field public Null AnimationScriptPlayable 
local AnimationScriptPlayable={ }
---
---@public
---@return PlayableHandle 
function AnimationScriptPlayable:GetHandle() end
---
---@public
---@param playable AnimationScriptPlayable 
---@return Playable 
function AnimationScriptPlayable.op_Implicit(playable) end
---
---@public
---@param playable Playable 
---@return AnimationScriptPlayable 
function AnimationScriptPlayable.op_Explicit(playable) end
---
---@public
---@param other AnimationScriptPlayable 
---@return boolean 
function AnimationScriptPlayable:Equals(other) end
---
---@public
---@param value boolean 
---@return void 
function AnimationScriptPlayable:SetProcessInputs(value) end
---
---@public
---@return boolean 
function AnimationScriptPlayable:GetProcessInputs() end
---
UnityEngine.Animations.AnimationScriptPlayable = AnimationScriptPlayable