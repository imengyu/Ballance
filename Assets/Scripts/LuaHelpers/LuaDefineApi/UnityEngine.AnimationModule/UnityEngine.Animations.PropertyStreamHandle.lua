---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PropertyStreamHandle : ValueType
local PropertyStreamHandle={ }
---
---@public
---@param stream AnimationStream 
---@return boolean 
function PropertyStreamHandle:IsValid(stream) end
---
---@public
---@param stream AnimationStream 
---@return void 
function PropertyStreamHandle:Resolve(stream) end
---
---@public
---@param stream AnimationStream 
---@return boolean 
function PropertyStreamHandle:IsResolved(stream) end
---
---@public
---@param stream AnimationStream 
---@return number 
function PropertyStreamHandle:GetFloat(stream) end
---
---@public
---@param stream AnimationStream 
---@param value number 
---@return void 
function PropertyStreamHandle:SetFloat(stream, value) end
---
---@public
---@param stream AnimationStream 
---@return number 
function PropertyStreamHandle:GetInt(stream) end
---
---@public
---@param stream AnimationStream 
---@param value number 
---@return void 
function PropertyStreamHandle:SetInt(stream, value) end
---
---@public
---@param stream AnimationStream 
---@return boolean 
function PropertyStreamHandle:GetBool(stream) end
---
---@public
---@param stream AnimationStream 
---@param value boolean 
---@return void 
function PropertyStreamHandle:SetBool(stream, value) end
---
---@public
---@param stream AnimationStream 
---@return boolean 
function PropertyStreamHandle:GetReadMask(stream) end
---
UnityEngine.Animations.PropertyStreamHandle = PropertyStreamHandle