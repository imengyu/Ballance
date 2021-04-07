---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SliderJoint2D : AnchoredJoint2D
---@field public autoConfigureAngle boolean 
---@field public angle number 
---@field public useMotor boolean 
---@field public useLimits boolean 
---@field public motor JointMotor2D 
---@field public limits JointTranslationLimits2D 
---@field public limitState number 
---@field public referenceAngle number 
---@field public jointTranslation number 
---@field public jointSpeed number 
local SliderJoint2D={ }
---
---@public
---@param timeStep number 
---@return number 
function SliderJoint2D:GetMotorForce(timeStep) end
---
UnityEngine.SliderJoint2D = SliderJoint2D