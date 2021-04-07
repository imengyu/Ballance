---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Quaternion : ValueType
---@field public x number 
---@field public y number 
---@field public z number 
---@field public w number 
---@field public kEpsilon number 
---@field public Item number 
---@field public identity Quaternion 
---@field public eulerAngles Vector3 
---@field public normalized Quaternion 
local Quaternion={ }
---
---@public
---@param fromDirection Vector3 
---@param toDirection Vector3 
---@return Quaternion 
function Quaternion.FromToRotation(fromDirection, toDirection) end
---
---@public
---@param rotation Quaternion 
---@return Quaternion 
function Quaternion.Inverse(rotation) end
---
---@public
---@param a Quaternion 
---@param b Quaternion 
---@param t number 
---@return Quaternion 
function Quaternion.Slerp(a, b, t) end
---
---@public
---@param a Quaternion 
---@param b Quaternion 
---@param t number 
---@return Quaternion 
function Quaternion.SlerpUnclamped(a, b, t) end
---
---@public
---@param a Quaternion 
---@param b Quaternion 
---@param t number 
---@return Quaternion 
function Quaternion.Lerp(a, b, t) end
---
---@public
---@param a Quaternion 
---@param b Quaternion 
---@param t number 
---@return Quaternion 
function Quaternion.LerpUnclamped(a, b, t) end
---
---@public
---@param angle number 
---@param axis Vector3 
---@return Quaternion 
function Quaternion.AngleAxis(angle, axis) end
---
---@public
---@param forward Vector3 
---@param upwards Vector3 
---@return Quaternion 
function Quaternion.LookRotation(forward, upwards) end
---
---@public
---@param forward Vector3 
---@return Quaternion 
function Quaternion.LookRotation(forward) end
---
---@public
---@param newX number 
---@param newY number 
---@param newZ number 
---@param newW number 
---@return void 
function Quaternion:Set(newX, newY, newZ, newW) end
---
---@public
---@param lhs Quaternion 
---@param rhs Quaternion 
---@return Quaternion 
function Quaternion.op_Multiply(lhs, rhs) end
---
---@public
---@param rotation Quaternion 
---@param point Vector3 
---@return Vector3 
function Quaternion.op_Multiply(rotation, point) end
---
---@public
---@param lhs Quaternion 
---@param rhs Quaternion 
---@return boolean 
function Quaternion.op_Equality(lhs, rhs) end
---
---@public
---@param lhs Quaternion 
---@param rhs Quaternion 
---@return boolean 
function Quaternion.op_Inequality(lhs, rhs) end
---
---@public
---@param a Quaternion 
---@param b Quaternion 
---@return number 
function Quaternion.Dot(a, b) end
---
---@public
---@param view Vector3 
---@return void 
function Quaternion:SetLookRotation(view) end
---
---@public
---@param view Vector3 
---@param up Vector3 
---@return void 
function Quaternion:SetLookRotation(view, up) end
---
---@public
---@param a Quaternion 
---@param b Quaternion 
---@return number 
function Quaternion.Angle(a, b) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return Quaternion 
function Quaternion.Euler(x, y, z) end
---
---@public
---@param euler Vector3 
---@return Quaternion 
function Quaternion.Euler(euler) end
---
---@public
---@param angle Single& 
---@param axis Vector3& 
---@return void 
function Quaternion:ToAngleAxis(angle, axis) end
---
---@public
---@param fromDirection Vector3 
---@param toDirection Vector3 
---@return void 
function Quaternion:SetFromToRotation(fromDirection, toDirection) end
---
---@public
---@param from Quaternion 
---@param to Quaternion 
---@param maxDegreesDelta number 
---@return Quaternion 
function Quaternion.RotateTowards(from, to, maxDegreesDelta) end
---
---@public
---@param q Quaternion 
---@return Quaternion 
function Quaternion.Normalize(q) end
---
---@public
---@return void 
function Quaternion:Normalize() end
---
---@public
---@return number 
function Quaternion:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function Quaternion:Equals(other) end
---
---@public
---@param other Quaternion 
---@return boolean 
function Quaternion:Equals(other) end
---
---@public
---@return string 
function Quaternion:ToString() end
---
---@public
---@param format string 
---@return string 
function Quaternion:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Quaternion:ToString(format, formatProvider) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return Quaternion 
function Quaternion.EulerRotation(x, y, z) end
---
---@public
---@param euler Vector3 
---@return Quaternion 
function Quaternion.EulerRotation(euler) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return void 
function Quaternion:SetEulerRotation(x, y, z) end
---
---@public
---@param euler Vector3 
---@return void 
function Quaternion:SetEulerRotation(euler) end
---
---@public
---@return Vector3 
function Quaternion:ToEuler() end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return Quaternion 
function Quaternion.EulerAngles(x, y, z) end
---
---@public
---@param euler Vector3 
---@return Quaternion 
function Quaternion.EulerAngles(euler) end
---
---@public
---@param axis Vector3& 
---@param angle Single& 
---@return void 
function Quaternion:ToAxisAngle(axis, angle) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return void 
function Quaternion:SetEulerAngles(x, y, z) end
---
---@public
---@param euler Vector3 
---@return void 
function Quaternion:SetEulerAngles(euler) end
---
---@public
---@param rotation Quaternion 
---@return Vector3 
function Quaternion.ToEulerAngles(rotation) end
---
---@public
---@return Vector3 
function Quaternion:ToEulerAngles() end
---
---@public
---@param axis Vector3 
---@param angle number 
---@return void 
function Quaternion:SetAxisAngle(axis, angle) end
---
---@public
---@param axis Vector3 
---@param angle number 
---@return Quaternion 
function Quaternion.AxisAngle(axis, angle) end
---
UnityEngine.Quaternion = Quaternion