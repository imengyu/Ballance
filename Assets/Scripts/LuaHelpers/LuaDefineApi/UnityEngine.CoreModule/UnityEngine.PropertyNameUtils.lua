---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PropertyNameUtils
local PropertyNameUtils={ }
---
---@public
---@param name string 
---@return PropertyName 
function PropertyNameUtils.PropertyNameFromString(name) end
---
---@public
---@param propertyName PropertyName 
---@return string 
function PropertyNameUtils.StringFromPropertyName(propertyName) end
---
---@public
---@param id number 
---@return number 
function PropertyNameUtils.ConflictCountForID(id) end
---
UnityEngine.PropertyNameUtils = PropertyNameUtils