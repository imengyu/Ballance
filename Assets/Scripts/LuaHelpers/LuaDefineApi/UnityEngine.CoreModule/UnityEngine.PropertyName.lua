---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PropertyName : ValueType
local PropertyName={ }
---
---@public
---@param prop PropertyName 
---@return boolean 
function PropertyName.IsNullOrEmpty(prop) end
---
---@public
---@param lhs PropertyName 
---@param rhs PropertyName 
---@return boolean 
function PropertyName.op_Equality(lhs, rhs) end
---
---@public
---@param lhs PropertyName 
---@param rhs PropertyName 
---@return boolean 
function PropertyName.op_Inequality(lhs, rhs) end
---
---@public
---@return number 
function PropertyName:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function PropertyName:Equals(other) end
---
---@public
---@param other PropertyName 
---@return boolean 
function PropertyName:Equals(other) end
---
---@public
---@param name string 
---@return PropertyName 
function PropertyName.op_Implicit(name) end
---
---@public
---@param id number 
---@return PropertyName 
function PropertyName.op_Implicit(id) end
---
---@public
---@return string 
function PropertyName:ToString() end
---
UnityEngine.PropertyName = PropertyName