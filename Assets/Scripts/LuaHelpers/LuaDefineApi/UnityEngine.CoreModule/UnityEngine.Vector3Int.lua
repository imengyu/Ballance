---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Vector3Int : ValueType
---@field public x number 
---@field public y number 
---@field public z number 
---@field public Item number 
---@field public magnitude number 
---@field public sqrMagnitude number 
---@field public zero Vector3Int 
---@field public one Vector3Int 
---@field public up Vector3Int 
---@field public down Vector3Int 
---@field public left Vector3Int 
---@field public right Vector3Int 
---@field public forward Vector3Int 
---@field public back Vector3Int 
local Vector3Int={ }
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return void 
function Vector3Int:Set(x, y, z) end
---
---@public
---@param a Vector3Int 
---@param b Vector3Int 
---@return number 
function Vector3Int.Distance(a, b) end
---
---@public
---@param lhs Vector3Int 
---@param rhs Vector3Int 
---@return Vector3Int 
function Vector3Int.Min(lhs, rhs) end
---
---@public
---@param lhs Vector3Int 
---@param rhs Vector3Int 
---@return Vector3Int 
function Vector3Int.Max(lhs, rhs) end
---
---@public
---@param a Vector3Int 
---@param b Vector3Int 
---@return Vector3Int 
function Vector3Int.Scale(a, b) end
---
---@public
---@param scale Vector3Int 
---@return void 
function Vector3Int:Scale(scale) end
---
---@public
---@param min Vector3Int 
---@param max Vector3Int 
---@return void 
function Vector3Int:Clamp(min, max) end
---
---@public
---@param v Vector3Int 
---@return Vector3 
function Vector3Int.op_Implicit(v) end
---
---@public
---@param v Vector3Int 
---@return Vector2Int 
function Vector3Int.op_Explicit(v) end
---
---@public
---@param v Vector3 
---@return Vector3Int 
function Vector3Int.FloorToInt(v) end
---
---@public
---@param v Vector3 
---@return Vector3Int 
function Vector3Int.CeilToInt(v) end
---
---@public
---@param v Vector3 
---@return Vector3Int 
function Vector3Int.RoundToInt(v) end
---
---@public
---@param a Vector3Int 
---@param b Vector3Int 
---@return Vector3Int 
function Vector3Int.op_Addition(a, b) end
---
---@public
---@param a Vector3Int 
---@param b Vector3Int 
---@return Vector3Int 
function Vector3Int.op_Subtraction(a, b) end
---
---@public
---@param a Vector3Int 
---@param b Vector3Int 
---@return Vector3Int 
function Vector3Int.op_Multiply(a, b) end
---
---@public
---@param a Vector3Int 
---@return Vector3Int 
function Vector3Int.op_UnaryNegation(a) end
---
---@public
---@param a Vector3Int 
---@param b number 
---@return Vector3Int 
function Vector3Int.op_Multiply(a, b) end
---
---@public
---@param a number 
---@param b Vector3Int 
---@return Vector3Int 
function Vector3Int.op_Multiply(a, b) end
---
---@public
---@param a Vector3Int 
---@param b number 
---@return Vector3Int 
function Vector3Int.op_Division(a, b) end
---
---@public
---@param lhs Vector3Int 
---@param rhs Vector3Int 
---@return boolean 
function Vector3Int.op_Equality(lhs, rhs) end
---
---@public
---@param lhs Vector3Int 
---@param rhs Vector3Int 
---@return boolean 
function Vector3Int.op_Inequality(lhs, rhs) end
---
---@public
---@param other Object 
---@return boolean 
function Vector3Int:Equals(other) end
---
---@public
---@param other Vector3Int 
---@return boolean 
function Vector3Int:Equals(other) end
---
---@public
---@return number 
function Vector3Int:GetHashCode() end
---
---@public
---@return string 
function Vector3Int:ToString() end
---
---@public
---@param format string 
---@return string 
function Vector3Int:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Vector3Int:ToString(format, formatProvider) end
---
UnityEngine.Vector3Int = Vector3Int