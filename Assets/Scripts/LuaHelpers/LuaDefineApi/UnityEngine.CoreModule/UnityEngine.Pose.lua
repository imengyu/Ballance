---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Pose : ValueType
---@field public position Vector3 
---@field public rotation Quaternion 
---@field public forward Vector3 
---@field public right Vector3 
---@field public up Vector3 
---@field public identity Pose 
local Pose={ }
---
---@public
---@return string 
function Pose:ToString() end
---
---@public
---@param format string 
---@return string 
function Pose:ToString(format) end
---
---@public
---@param lhs Pose 
---@return Pose 
function Pose:GetTransformedBy(lhs) end
---
---@public
---@param lhs Transform 
---@return Pose 
function Pose:GetTransformedBy(lhs) end
---
---@public
---@param obj Object 
---@return boolean 
function Pose:Equals(obj) end
---
---@public
---@param other Pose 
---@return boolean 
function Pose:Equals(other) end
---
---@public
---@return number 
function Pose:GetHashCode() end
---
---@public
---@param a Pose 
---@param b Pose 
---@return boolean 
function Pose.op_Equality(a, b) end
---
---@public
---@param a Pose 
---@param b Pose 
---@return boolean 
function Pose.op_Inequality(a, b) end
---
UnityEngine.Pose = Pose