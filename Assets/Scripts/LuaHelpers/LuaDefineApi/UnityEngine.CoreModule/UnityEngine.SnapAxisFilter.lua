---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SnapAxisFilter : ValueType
---@field public all SnapAxisFilter 
---@field public x number 
---@field public y number 
---@field public z number 
---@field public active number 
---@field public Item number 
local SnapAxisFilter={ }
---
---@public
---@return string 
function SnapAxisFilter:ToString() end
---
---@public
---@param mask SnapAxisFilter 
---@return Vector3 
function SnapAxisFilter.op_Implicit(mask) end
---
---@public
---@param v Vector3 
---@return SnapAxisFilter 
function SnapAxisFilter.op_Explicit(v) end
---
---@public
---@param mask SnapAxisFilter 
---@return number 
function SnapAxisFilter.op_Explicit(mask) end
---
---@public
---@param left SnapAxisFilter 
---@param right SnapAxisFilter 
---@return SnapAxisFilter 
function SnapAxisFilter.op_BitwiseOr(left, right) end
---
---@public
---@param left SnapAxisFilter 
---@param right SnapAxisFilter 
---@return SnapAxisFilter 
function SnapAxisFilter.op_BitwiseAnd(left, right) end
---
---@public
---@param left SnapAxisFilter 
---@param right SnapAxisFilter 
---@return SnapAxisFilter 
function SnapAxisFilter.op_ExclusiveOr(left, right) end
---
---@public
---@param left SnapAxisFilter 
---@return SnapAxisFilter 
function SnapAxisFilter.op_OnesComplement(left) end
---
---@public
---@param mask SnapAxisFilter 
---@param value number 
---@return Vector3 
function SnapAxisFilter.op_Multiply(mask, value) end
---
---@public
---@param mask SnapAxisFilter 
---@param right Vector3 
---@return Vector3 
function SnapAxisFilter.op_Multiply(mask, right) end
---
---@public
---@param rotation Quaternion 
---@param mask SnapAxisFilter 
---@return Vector3 
function SnapAxisFilter.op_Multiply(rotation, mask) end
---
---@public
---@param left SnapAxisFilter 
---@param right SnapAxisFilter 
---@return boolean 
function SnapAxisFilter.op_Equality(left, right) end
---
---@public
---@param left SnapAxisFilter 
---@param right SnapAxisFilter 
---@return boolean 
function SnapAxisFilter.op_Inequality(left, right) end
---
---@public
---@param other SnapAxisFilter 
---@return boolean 
function SnapAxisFilter:Equals(other) end
---
---@public
---@param obj Object 
---@return boolean 
function SnapAxisFilter:Equals(obj) end
---
---@public
---@return number 
function SnapAxisFilter:GetHashCode() end
---
UnityEngine.SnapAxisFilter = SnapAxisFilter