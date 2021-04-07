---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class HingeJoint2D : AnchoredJoint2D
---@field public useMotor boolean 
---@field public useLimits boolean 
---@field public motor JointMotor2D 
---@field public limits JointAngleLimits2D 
---@field public limitState number 
---@field public referenceAngle number 
---@field public jointAngle number 
---@field public jointSpeed number 
local HingeJoint2D={ }
---
---@public
---@param timeStep number 
---@return number 
function HingeJoint2D:GetMotorTorque(timeStep) end
---
UnityEngine.HingeJoint2D = HingeJoint2D