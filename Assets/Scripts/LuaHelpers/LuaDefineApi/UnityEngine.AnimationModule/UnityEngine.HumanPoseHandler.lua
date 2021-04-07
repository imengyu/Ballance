---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class HumanPoseHandler
local HumanPoseHandler={ }
---
---@public
---@return void 
function HumanPoseHandler:Dispose() end
---
---@public
---@param humanPose HumanPose& 
---@return void 
function HumanPoseHandler:GetHumanPose(humanPose) end
---
---@public
---@param humanPose HumanPose& 
---@return void 
function HumanPoseHandler:SetHumanPose(humanPose) end
---
---@public
---@param humanPose HumanPose& 
---@return void 
function HumanPoseHandler:GetInternalHumanPose(humanPose) end
---
---@public
---@param humanPose HumanPose& 
---@return void 
function HumanPoseHandler:SetInternalHumanPose(humanPose) end
---
---@public
---@param avatarPose NativeArray`1 
---@return void 
function HumanPoseHandler:GetInternalAvatarPose(avatarPose) end
---
---@public
---@param avatarPose NativeArray`1 
---@return void 
function HumanPoseHandler:SetInternalAvatarPose(avatarPose) end
---
UnityEngine.HumanPoseHandler = HumanPoseHandler