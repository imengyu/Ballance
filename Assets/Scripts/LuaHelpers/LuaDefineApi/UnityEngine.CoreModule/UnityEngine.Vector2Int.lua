---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Vector2Int : ValueType
---@field public x number 
---@field public y number 
---@field public Item number 
---@field public magnitude number 
---@field public sqrMagnitude number 
---@field public zero Vector2Int 
---@field public one Vector2Int 
---@field public up Vector2Int 
---@field public down Vector2Int 
---@field public left Vector2Int 
---@field public right Vector2Int 
local Vector2Int={ }
---
---@public
---@param x number 
---@param y number 
---@return void 
function Vector2Int:Set(x, y) end
---
---@public
---@param a Vector2Int 
---@param b Vector2Int 
---@return number 
function Vector2Int.Distance(a, b) end
---
---@public
---@param lhs Vector2Int 
---@param rhs Vector2Int 
---@return Vector2Int 
function Vector2Int.Min(lhs, rhs) end
---
---@public
---@param lhs Vector2Int 
---@param rhs Vector2Int 
---@return Vector2Int 
function Vector2Int.Max(lhs, rhs) end
---
---@public
---@param a Vector2Int 
---@param b Vector2Int 
---@return Vector2Int 
function Vector2Int.Scale(a, b) end
---
---@public
---@param scale Vector2Int 
---@return void 
function Vector2Int:Scale(scale) end
---
---@public
---@param min Vector2Int 
---@param max Vector2Int 
---@return void 
function Vector2Int:Clamp(min, max) end
---
---@public
---@param v Vector2Int 
---@return Vector2 
function Vector2Int.op_Implicit(v) end
---
---@public
---@param v Vector2Int 
---@return Vector3Int 
function Vector2Int.op_Explicit(v) end
---
---@public
---@param v Vector2 
---@return Vector2Int 
function Vector2Int.FloorToInt(v) end
---
---@public
---@param v Vector2 
---@return Vector2Int 
function Vector2Int.CeilToInt(v) end
---
---@public
---@param v Vector2 
---@return Vector2Int 
function Vector2Int.RoundToInt(v) end
---
---@public
---@param v Vector2Int 
---@return Vector2Int 
function Vector2Int.op_UnaryNegation(v) end
---
---@public
---@param a Vector2Int 
---@param b Vector2Int 
---@return Vector2Int 
function Vector2Int.op_Addition(a, b) end
---
---@public
---@param a Vector2Int 
---@param b Vector2Int 
---@return Vector2Int 
function Vector2Int.op_Subtraction(a, b) end
---
---@public
---@param a Vector2Int 
---@param b Vector2Int 
---@return Vector2Int 
function Vector2Int.op_Multiply(a, b) end
---
---@public
---@param a number 
---@param b Vector2Int 
---@return Vector2Int 
function Vector2Int.op_Multiply(a, b) end
---
---@public
---@param a Vector2Int 
---@param b number 
---@return Vector2Int 
function Vector2Int.op_Multiply(a, b) end
---
---@public
---@param a Vector2Int 
---@param b number 
---@return Vector2Int 
function Vector2Int.op_Division(a, b) end
---
---@public
---@param lhs Vector2Int 
---@param rhs Vector2Int 
---@return boolean 
function Vector2Int.op_Equality(lhs, rhs) end
---
---@public
---@param lhs Vector2Int 
---@param rhs Vector2Int 
---@return boolean 
function Vector2Int.op_Inequality(lhs, rhs) end
---
---@public
---@param other Object 
---@return boolean 
function Vector2Int:Equals(other) end
---
---@public
---@param other Vector2Int 
---@return boolean 
function Vector2Int:Equals(other) end
---
---@public
---@return number 
function Vector2Int:GetHashCode() end
---
---@public
---@return string 
function Vector2Int:ToString() end
---
---@public
---@param format string 
---@return string 
function Vector2Int:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Vector2Int:ToString(format, formatProvider) end
---
UnityEngine.Vector2Int = Vector2Int