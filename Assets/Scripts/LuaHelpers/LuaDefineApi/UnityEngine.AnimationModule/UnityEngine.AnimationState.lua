---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimationState : TrackedReference
---@field public enabled boolean 
---@field public weight number 
---@field public wrapMode number 
---@field public time number 
---@field public normalizedTime number 
---@field public speed number 
---@field public normalizedSpeed number 
---@field public length number 
---@field public layer number 
---@field public clip AnimationClip 
---@field public name string 
---@field public blendMode number 
local AnimationState={ }
---
---@public
---@param mix Transform 
---@return void 
function AnimationState:AddMixingTransform(mix) end
---
---@public
---@param mix Transform 
---@param recursive boolean 
---@return void 
function AnimationState:AddMixingTransform(mix, recursive) end
---
---@public
---@param mix Transform 
---@return void 
function AnimationState:RemoveMixingTransform(mix) end
---
UnityEngine.AnimationState = AnimationState