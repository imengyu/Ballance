---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PropertySceneHandle : ValueType
local PropertySceneHandle={ }
---
---@public
---@param stream AnimationStream 
---@return boolean 
function PropertySceneHandle:IsValid(stream) end
---
---@public
---@param stream AnimationStream 
---@return void 
function PropertySceneHandle:Resolve(stream) end
---
---@public
---@param stream AnimationStream 
---@return boolean 
function PropertySceneHandle:IsResolved(stream) end
---
---@public
---@param stream AnimationStream 
---@return number 
function PropertySceneHandle:GetFloat(stream) end
---
---@public
---@param stream AnimationStream 
---@param value number 
---@return void 
function PropertySceneHandle:SetFloat(stream, value) end
---
---@public
---@param stream AnimationStream 
---@return number 
function PropertySceneHandle:GetInt(stream) end
---
---@public
---@param stream AnimationStream 
---@param value number 
---@return void 
function PropertySceneHandle:SetInt(stream, value) end
---
---@public
---@param stream AnimationStream 
---@return boolean 
function PropertySceneHandle:GetBool(stream) end
---
---@public
---@param stream AnimationStream 
---@param value boolean 
---@return void 
function PropertySceneHandle:SetBool(stream, value) end
---
UnityEngine.Animations.PropertySceneHandle = PropertySceneHandle