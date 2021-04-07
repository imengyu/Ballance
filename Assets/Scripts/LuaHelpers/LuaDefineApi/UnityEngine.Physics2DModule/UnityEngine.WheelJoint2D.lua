---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class WheelJoint2D : AnchoredJoint2D
---@field public suspension JointSuspension2D 
---@field public useMotor boolean 
---@field public motor JointMotor2D 
---@field public jointTranslation number 
---@field public jointLinearSpeed number 
---@field public jointSpeed number 
---@field public jointAngle number 
local WheelJoint2D={ }
---
---@public
---@param timeStep number 
---@return number 
function WheelJoint2D:GetMotorTorque(timeStep) end
---
UnityEngine.WheelJoint2D = WheelJoint2D