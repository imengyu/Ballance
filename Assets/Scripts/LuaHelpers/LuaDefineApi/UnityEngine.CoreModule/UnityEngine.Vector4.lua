---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Vector4 : ValueType
---@field public kEpsilon number 
---@field public x number 
---@field public y number 
---@field public z number 
---@field public w number 
---@field public Item number 
---@field public normalized Vector4 
---@field public magnitude number 
---@field public sqrMagnitude number 
---@field public zero Vector4 
---@field public one Vector4 
---@field public positiveInfinity Vector4 
---@field public negativeInfinity Vector4 
local Vector4={ }
---
---@public
---@param newX number 
---@param newY number 
---@param newZ number 
---@param newW number 
---@return void 
function Vector4:Set(newX, newY, newZ, newW) end
---
---@public
---@param a Vector4 
---@param b Vector4 
---@param t number 
---@return Vector4 
function Vector4.Lerp(a, b, t) end
---
---@public
---@param a Vector4 
---@param b Vector4 
---@param t number 
---@return Vector4 
function Vector4.LerpUnclamped(a, b, t) end
---
---@public
---@param current Vector4 
---@param target Vector4 
---@param maxDistanceDelta number 
---@return Vector4 
function Vector4.MoveTowards(current, target, maxDistanceDelta) end
---
---@public
---@param a Vector4 
---@param b Vector4 
---@return Vector4 
function Vector4.Scale(a, b) end
---
---@public
---@param scale Vector4 
---@return void 
function Vector4:Scale(scale) end
---
---@public
---@return number 
function Vector4:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function Vector4:Equals(other) end
---
---@public
---@param other Vector4 
---@return boolean 
function Vector4:Equals(other) end
---
---@public
---@param a Vector4 
---@return Vector4 
function Vector4.Normalize(a) end
---
---@public
---@return void 
function Vector4:Normalize() end
---
---@public
---@param a Vector4 
---@param b Vector4 
---@return number 
function Vector4.Dot(a, b) end
---
---@public
---@param a Vector4 
---@param b Vector4 
---@return Vector4 
function Vector4.Project(a, b) end
---
---@public
---@param a Vector4 
---@param b Vector4 
---@return number 
function Vector4.Distance(a, b) end
---
---@public
---@param a Vector4 
---@return number 
function Vector4.Magnitude(a) end
---
---@public
---@param lhs Vector4 
---@param rhs Vector4 
---@return Vector4 
function Vector4.Min(lhs, rhs) end
---
---@public
---@param lhs Vector4 
---@param rhs Vector4 
---@return Vector4 
function Vector4.Max(lhs, rhs) end
---
---@public
---@param a Vector4 
---@param b Vector4 
---@return Vector4 
function Vector4.op_Addition(a, b) end
---
---@public
---@param a Vector4 
---@param b Vector4 
---@return Vector4 
function Vector4.op_Subtraction(a, b) end
---
---@public
---@param a Vector4 
---@return Vector4 
function Vector4.op_UnaryNegation(a) end
---
---@public
---@param a Vector4 
---@param d number 
---@return Vector4 
function Vector4.op_Multiply(a, d) end
---
---@public
---@param d number 
---@param a Vector4 
---@return Vector4 
function Vector4.op_Multiply(d, a) end
---
---@public
---@param a Vector4 
---@param d number 
---@return Vector4 
function Vector4.op_Division(a, d) end
---
---@public
---@param lhs Vector4 
---@param rhs Vector4 
---@return boolean 
function Vector4.op_Equality(lhs, rhs) end
---
---@public
---@param lhs Vector4 
---@param rhs Vector4 
---@return boolean 
function Vector4.op_Inequality(lhs, rhs) end
---
---@public
---@param v Vector3 
---@return Vector4 
function Vector4.op_Implicit(v) end
---
---@public
---@param v Vector4 
---@return Vector3 
function Vector4.op_Implicit(v) end
---
---@public
---@param v Vector2 
---@return Vector4 
function Vector4.op_Implicit(v) end
---
---@public
---@param v Vector4 
---@return Vector2 
function Vector4.op_Implicit(v) end
---
---@public
---@return string 
function Vector4:ToString() end
---
---@public
---@param format string 
---@return string 
function Vector4:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Vector4:ToString(format, formatProvider) end
---
---@public
---@param a Vector4 
---@return number 
function Vector4.SqrMagnitude(a) end
---
---@public
---@return number 
function Vector4:SqrMagnitude() end
---
UnityEngine.Vector4 = Vector4