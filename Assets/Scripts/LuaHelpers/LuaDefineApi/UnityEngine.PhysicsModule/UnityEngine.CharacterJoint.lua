---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CharacterJoint : Joint
---@field public targetRotation Quaternion 
---@field public targetAngularVelocity Vector3 
---@field public rotationDrive JointDrive 
---@field public swingAxis Vector3 
---@field public twistLimitSpring SoftJointLimitSpring 
---@field public swingLimitSpring SoftJointLimitSpring 
---@field public lowTwistLimit SoftJointLimit 
---@field public highTwistLimit SoftJointLimit 
---@field public swing1Limit SoftJointLimit 
---@field public swing2Limit SoftJointLimit 
---@field public enableProjection boolean 
---@field public projectionDistance number 
---@field public projectionAngle number 
local CharacterJoint={ }
---
UnityEngine.CharacterJoint = CharacterJoint