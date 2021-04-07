---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimatorOverrideController : RuntimeAnimatorController
---@field public runtimeAnimatorController RuntimeAnimatorController 
---@field public Item AnimationClip 
---@field public Item AnimationClip 
---@field public overridesCount number 
---@field public clips AnimationClipPair[] 
local AnimatorOverrideController={ }
---
---@public
---@param overrides List`1 
---@return void 
function AnimatorOverrideController:GetOverrides(overrides) end
---
---@public
---@param overrides IList`1 
---@return void 
function AnimatorOverrideController:ApplyOverrides(overrides) end
---
UnityEngine.AnimatorOverrideController = AnimatorOverrideController