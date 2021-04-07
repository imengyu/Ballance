---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IExposedPropertyTable
local IExposedPropertyTable={ }
---
---@public
---@param id PropertyName 
---@param value Object 
---@return void 
function IExposedPropertyTable:SetReferenceValue(id, value) end
---
---@public
---@param id PropertyName 
---@param idValid Boolean& 
---@return Object 
function IExposedPropertyTable:GetReferenceValue(id, idValid) end
---
---@public
---@param id PropertyName 
---@return void 
function IExposedPropertyTable:ClearReferenceValue(id) end
---
UnityEngine.IExposedPropertyTable = IExposedPropertyTable