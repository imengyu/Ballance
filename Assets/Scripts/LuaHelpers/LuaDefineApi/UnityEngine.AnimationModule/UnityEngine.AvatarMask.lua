---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AvatarMask : Object
---@field public humanoidBodyPartCount number 
---@field public transformCount number 
local AvatarMask={ }
---
---@public
---@param index number 
---@return boolean 
function AvatarMask:GetHumanoidBodyPartActive(index) end
---
---@public
---@param index number 
---@param value boolean 
---@return void 
function AvatarMask:SetHumanoidBodyPartActive(index, value) end
---
---@public
---@param transform Transform 
---@return void 
function AvatarMask:AddTransformPath(transform) end
---
---@public
---@param transform Transform 
---@param recursive boolean 
---@return void 
function AvatarMask:AddTransformPath(transform, recursive) end
---
---@public
---@param transform Transform 
---@return void 
function AvatarMask:RemoveTransformPath(transform) end
---
---@public
---@param transform Transform 
---@param recursive boolean 
---@return void 
function AvatarMask:RemoveTransformPath(transform, recursive) end
---
---@public
---@param index number 
---@return string 
function AvatarMask:GetTransformPath(index) end
---
---@public
---@param index number 
---@param path string 
---@return void 
function AvatarMask:SetTransformPath(index, path) end
---
---@public
---@param index number 
---@return boolean 
function AvatarMask:GetTransformActive(index) end
---
---@public
---@param index number 
---@param value boolean 
---@return void 
function AvatarMask:SetTransformActive(index, value) end
---
UnityEngine.AvatarMask = AvatarMask