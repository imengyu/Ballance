---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Vector2 : ValueType
---@field public x number 
---@field public y number 
---@field public kEpsilon number 
---@field public kEpsilonNormalSqrt number 
---@field public Item number 
---@field public normalized Vector2 
---@field public magnitude number 
---@field public sqrMagnitude number 
---@field public zero Vector2 
---@field public one Vector2 
---@field public up Vector2 
---@field public down Vector2 
---@field public left Vector2 
---@field public right Vector2 
---@field public positiveInfinity Vector2 
---@field public negativeInfinity Vector2 
local Vector2={ }
---
---@public
---@param newX number 
---@param newY number 
---@return void 
function Vector2:Set(newX, newY) end
---
---@public
---@param a Vector2 
---@param b Vector2 
---@param t number 
---@return Vector2 
function Vector2.Lerp(a, b, t) end
---
---@public
---@param a Vector2 
---@param b Vector2 
---@param t number 
---@return Vector2 
function Vector2.LerpUnclamped(a, b, t) end
---
---@public
---@param current Vector2 
---@param target Vector2 
---@param maxDistanceDelta number 
---@return Vector2 
function Vector2.MoveTowards(current, target, maxDistanceDelta) end
---
---@public
---@param a Vector2 
---@param b Vector2 
---@return Vector2 
function Vector2.Scale(a, b) end
---
---@public
---@param scale Vector2 
---@return void 
function Vector2:Scale(scale) end
---
---@public
---@return void 
function Vector2:Normalize() end
---
---@public
---@return string 
function Vector2:ToString() end
---
---@public
---@param format string 
---@return string 
function Vector2:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Vector2:ToString(format, formatProvider) end
---
---@public
---@return number 
function Vector2:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function Vector2:Equals(other) end
---
---@public
---@param other Vector2 
---@return boolean 
function Vector2:Equals(other) end
---
---@public
---@param inDirection Vector2 
---@param inNormal Vector2 
---@return Vector2 
function Vector2.Reflect(inDirection, inNormal) end
---
---@public
---@param inDirection Vector2 
---@return Vector2 
function Vector2.Perpendicular(inDirection) end
---
---@public
---@param lhs Vector2 
---@param rhs Vector2 
---@return number 
function Vector2.Dot(lhs, rhs) end
---
---@public
---@param from Vector2 
---@param to Vector2 
---@return number 
function Vector2.Angle(from, to) end
---
---@public
---@param from Vector2 
---@param to Vector2 
---@return number 
function Vector2.SignedAngle(from, to) end
---
---@public
---@param a Vector2 
---@param b Vector2 
---@return number 
function Vector2.Distance(a, b) end
---
---@public
---@param vector Vector2 
---@param maxLength number 
---@return Vector2 
function Vector2.ClampMagnitude(vector, maxLength) end
---
---@public
---@param a Vector2 
---@return number 
function Vector2.SqrMagnitude(a) end
---
---@public
---@return number 
function Vector2:SqrMagnitude() end
---
---@public
---@param lhs Vector2 
---@param rhs Vector2 
---@return Vector2 
function Vector2.Min(lhs, rhs) end
---
---@public
---@param lhs Vector2 
---@param rhs Vector2 
---@return Vector2 
function Vector2.Max(lhs, rhs) end
---
---@public
---@param current Vector2 
---@param target Vector2 
---@param currentVelocity Vector2& 
---@param smoothTime number 
---@param maxSpeed number 
---@return Vector2 
function Vector2.SmoothDamp(current, target, currentVelocity, smoothTime, maxSpeed) end
---
---@public
---@param current Vector2 
---@param target Vector2 
---@param currentVelocity Vector2& 
---@param smoothTime number 
---@return Vector2 
function Vector2.SmoothDamp(current, target, currentVelocity, smoothTime) end
---
---@public
---@param current Vector2 
---@param target Vector2 
---@param currentVelocity Vector2& 
---@param smoothTime number 
---@param maxSpeed number 
---@param deltaTime number 
---@return Vector2 
function Vector2.SmoothDamp(current, target, currentVelocity, smoothTime, maxSpeed, deltaTime) end
---
---@public
---@param a Vector2 
---@param b Vector2 
---@return Vector2 
function Vector2.op_Addition(a, b) end
---
---@public
---@param a Vector2 
---@param b Vector2 
---@return Vector2 
function Vector2.op_Subtraction(a, b) end
---
---@public
---@param a Vector2 
---@param b Vector2 
---@return Vector2 
function Vector2.op_Multiply(a, b) end
---
---@public
---@param a Vector2 
---@param b Vector2 
---@return Vector2 
function Vector2.op_Division(a, b) end
---
---@public
---@param a Vector2 
---@return Vector2 
function Vector2.op_UnaryNegation(a) end
---
---@public
---@param a Vector2 
---@param d number 
---@return Vector2 
function Vector2.op_Multiply(a, d) end
---
---@public
---@param d number 
---@param a Vector2 
---@return Vector2 
function Vector2.op_Multiply(d, a) end
---
---@public
---@param a Vector2 
---@param d number 
---@return Vector2 
function Vector2.op_Division(a, d) end
---
---@public
---@param lhs Vector2 
---@param rhs Vector2 
---@return boolean 
function Vector2.op_Equality(lhs, rhs) end
---
---@public
---@param lhs Vector2 
---@param rhs Vector2 
---@return boolean 
function Vector2.op_Inequality(lhs, rhs) end
---
---@public
---@param v Vector3 
---@return Vector2 
function Vector2.op_Implicit(v) end
---
---@public
---@param v Vector2 
---@return Vector3 
function Vector2.op_Implicit(v) end
---
UnityEngine.Vector2 = Vector2