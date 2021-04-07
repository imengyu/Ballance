---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IAnimationJob
local IAnimationJob={ }
---
---@public
---@param stream AnimationStream 
---@return void 
function IAnimationJob:ProcessAnimation(stream) end
---
---@public
---@param stream AnimationStream 
---@return void 
function IAnimationJob:ProcessRootMotion(stream) end
---
UnityEngine.Animations.IAnimationJob = IAnimationJob