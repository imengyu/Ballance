---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Vector3 : ValueType
---@field public kEpsilon number 
---@field public kEpsilonNormalSqrt number 
---@field public x number 
---@field public y number 
---@field public z number 
---@field public Item number 
---@field public normalized Vector3 
---@field public magnitude number 
---@field public sqrMagnitude number 
---@field public zero Vector3 
---@field public one Vector3 
---@field public forward Vector3 
---@field public back Vector3 
---@field public up Vector3 
---@field public down Vector3 
---@field public left Vector3 
---@field public right Vector3 
---@field public positiveInfinity Vector3 
---@field public negativeInfinity Vector3 
---@field public fwd Vector3 
local Vector3={ }
---
---@public
---@param a Vector3 
---@param b Vector3 
---@param t number 
---@return Vector3 
function Vector3.Slerp(a, b, t) end
---
---@public
---@param a Vector3 
---@param b Vector3 
---@param t number 
---@return Vector3 
function Vector3.SlerpUnclamped(a, b, t) end
---
---@public
---@param normal Vector3& 
---@param tangent Vector3& 
---@return void 
function Vector3.OrthoNormalize(normal, tangent) end
---
---@public
---@param normal Vector3& 
---@param tangent Vector3& 
---@param binormal Vector3& 
---@return void 
function Vector3.OrthoNormalize(normal, tangent, binormal) end
---
---@public
---@param current Vector3 
---@param target Vector3 
---@param maxRadiansDelta number 
---@param maxMagnitudeDelta number 
---@return Vector3 
function Vector3.RotateTowards(current, target, maxRadiansDelta, maxMagnitudeDelta) end
---
---@public
---@param a Vector3 
---@param b Vector3 
---@param t number 
---@return Vector3 
function Vector3.Lerp(a, b, t) end
---
---@public
---@param a Vector3 
---@param b Vector3 
---@param t number 
---@return Vector3 
function Vector3.LerpUnclamped(a, b, t) end
---
---@public
---@param current Vector3 
---@param target Vector3 
---@param maxDistanceDelta number 
---@return Vector3 
function Vector3.MoveTowards(current, target, maxDistanceDelta) end
---
---@public
---@param current Vector3 
---@param target Vector3 
---@param currentVelocity Vector3& 
---@param smoothTime number 
---@param maxSpeed number 
---@return Vector3 
function Vector3.SmoothDamp(current, target, currentVelocity, smoothTime, maxSpeed) end
---
---@public
---@param current Vector3 
---@param target Vector3 
---@param currentVelocity Vector3& 
---@param smoothTime number 
---@return Vector3 
function Vector3.SmoothDamp(current, target, currentVelocity, smoothTime) end
---
---@public
---@param current Vector3 
---@param target Vector3 
---@param currentVelocity Vector3& 
---@param smoothTime number 
---@param maxSpeed number 
---@param deltaTime number 
---@return Vector3 
function Vector3.SmoothDamp(current, target, currentVelocity, smoothTime, maxSpeed, deltaTime) end
---
---@public
---@param newX number 
---@param newY number 
---@param newZ number 
---@return void 
function Vector3:Set(newX, newY, newZ) end
---
---@public
---@param a Vector3 
---@param b Vector3 
---@return Vector3 
function Vector3.Scale(a, b) end
---
---@public
---@param scale Vector3 
---@return void 
function Vector3:Scale(scale) end
---
---@public
---@param lhs Vector3 
---@param rhs Vector3 
---@return Vector3 
function Vector3.Cross(lhs, rhs) end
---
---@public
---@return number 
function Vector3:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function Vector3:Equals(other) end
---
---@public
---@param other Vector3 
---@return boolean 
function Vector3:Equals(other) end
---
---@public
---@param inDirection Vector3 
---@param inNormal Vector3 
---@return Vector3 
function Vector3.Reflect(inDirection, inNormal) end
---
---@public
---@param value Vector3 
---@return Vector3 
function Vector3.Normalize(value) end
---
---@public
---@return void 
function Vector3:Normalize() end
---
---@public
---@param lhs Vector3 
---@param rhs Vector3 
---@return number 
function Vector3.Dot(lhs, rhs) end
---
---@public
---@param vector Vector3 
---@param onNormal Vector3 
---@return Vector3 
function Vector3.Project(vector, onNormal) end
---
---@public
---@param vector Vector3 
---@param planeNormal Vector3 
---@return Vector3 
function Vector3.ProjectOnPlane(vector, planeNormal) end
---
---@public
---@param from Vector3 
---@param to Vector3 
---@return number 
function Vector3.Angle(from, to) end
---
---@public
---@param from Vector3 
---@param to Vector3 
---@param axis Vector3 
---@return number 
function Vector3.SignedAngle(from, to, axis) end
---
---@public
---@param a Vector3 
---@param b Vector3 
---@return number 
function Vector3.Distance(a, b) end
---
---@public
---@param vector Vector3 
---@param maxLength number 
---@return Vector3 
function Vector3.ClampMagnitude(vector, maxLength) end
---
---@public
---@param vector Vector3 
---@return number 
function Vector3.Magnitude(vector) end
---
---@public
---@param vector Vector3 
---@return number 
function Vector3.SqrMagnitude(vector) end
---
---@public
---@param lhs Vector3 
---@param rhs Vector3 
---@return Vector3 
function Vector3.Min(lhs, rhs) end
---
---@public
---@param lhs Vector3 
---@param rhs Vector3 
---@return Vector3 
function Vector3.Max(lhs, rhs) end
---
---@public
---@param a Vector3 
---@param b Vector3 
---@return Vector3 
function Vector3.op_Addition(a, b) end
---
---@public
---@param a Vector3 
---@param b Vector3 
---@return Vector3 
function Vector3.op_Subtraction(a, b) end
---
---@public
---@param a Vector3 
---@return Vector3 
function Vector3.op_UnaryNegation(a) end
---
---@public
---@param a Vector3 
---@param d number 
---@return Vector3 
function Vector3.op_Multiply(a, d) end
---
---@public
---@param d number 
---@param a Vector3 
---@return Vector3 
function Vector3.op_Multiply(d, a) end
---
---@public
---@param a Vector3 
---@param d number 
---@return Vector3 
function Vector3.op_Division(a, d) end
---
---@public
---@param lhs Vector3 
---@param rhs Vector3 
---@return boolean 
function Vector3.op_Equality(lhs, rhs) end
---
---@public
---@param lhs Vector3 
---@param rhs Vector3 
---@return boolean 
function Vector3.op_Inequality(lhs, rhs) end
---
---@public
---@return string 
function Vector3:ToString() end
---
---@public
---@param format string 
---@return string 
function Vector3:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Vector3:ToString(format, formatProvider) end
---
---@public
---@param from Vector3 
---@param to Vector3 
---@return number 
function Vector3.AngleBetween(from, to) end
---
---@public
---@param excludeThis Vector3 
---@param fromThat Vector3 
---@return Vector3 
function Vector3.Exclude(excludeThis, fromThat) end
---
UnityEngine.Vector3 = Vector3