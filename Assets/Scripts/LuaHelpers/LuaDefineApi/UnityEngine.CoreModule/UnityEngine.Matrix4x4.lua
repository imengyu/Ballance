---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Matrix4x4 : ValueType
---@field public m00 number 
---@field public m10 number 
---@field public m20 number 
---@field public m30 number 
---@field public m01 number 
---@field public m11 number 
---@field public m21 number 
---@field public m31 number 
---@field public m02 number 
---@field public m12 number 
---@field public m22 number 
---@field public m32 number 
---@field public m03 number 
---@field public m13 number 
---@field public m23 number 
---@field public m33 number 
---@field public rotation Quaternion 
---@field public lossyScale Vector3 
---@field public isIdentity boolean 
---@field public determinant number 
---@field public decomposeProjection FrustumPlanes 
---@field public inverse Matrix4x4 
---@field public transpose Matrix4x4 
---@field public Item number 
---@field public Item number 
---@field public zero Matrix4x4 
---@field public identity Matrix4x4 
local Matrix4x4={ }
---
---@public
---@return boolean 
function Matrix4x4:ValidTRS() end
---
---@public
---@param m Matrix4x4 
---@return number 
function Matrix4x4.Determinant(m) end
---
---@public
---@param pos Vector3 
---@param q Quaternion 
---@param s Vector3 
---@return Matrix4x4 
function Matrix4x4.TRS(pos, q, s) end
---
---@public
---@param pos Vector3 
---@param q Quaternion 
---@param s Vector3 
---@return void 
function Matrix4x4:SetTRS(pos, q, s) end
---
---@public
---@param input Matrix4x4 
---@param result Matrix4x4& 
---@return boolean 
function Matrix4x4.Inverse3DAffine(input, result) end
---
---@public
---@param m Matrix4x4 
---@return Matrix4x4 
function Matrix4x4.Inverse(m) end
---
---@public
---@param m Matrix4x4 
---@return Matrix4x4 
function Matrix4x4.Transpose(m) end
---
---@public
---@param left number 
---@param right number 
---@param bottom number 
---@param top number 
---@param zNear number 
---@param zFar number 
---@return Matrix4x4 
function Matrix4x4.Ortho(left, right, bottom, top, zNear, zFar) end
---
---@public
---@param fov number 
---@param aspect number 
---@param zNear number 
---@param zFar number 
---@return Matrix4x4 
function Matrix4x4.Perspective(fov, aspect, zNear, zFar) end
---
---@public
---@param from Vector3 
---@param to Vector3 
---@param up Vector3 
---@return Matrix4x4 
function Matrix4x4.LookAt(from, to, up) end
---
---@public
---@param left number 
---@param right number 
---@param bottom number 
---@param top number 
---@param zNear number 
---@param zFar number 
---@return Matrix4x4 
function Matrix4x4.Frustum(left, right, bottom, top, zNear, zFar) end
---
---@public
---@param fp FrustumPlanes 
---@return Matrix4x4 
function Matrix4x4.Frustum(fp) end
---
---@public
---@return number 
function Matrix4x4:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function Matrix4x4:Equals(other) end
---
---@public
---@param other Matrix4x4 
---@return boolean 
function Matrix4x4:Equals(other) end
---
---@public
---@param lhs Matrix4x4 
---@param rhs Matrix4x4 
---@return Matrix4x4 
function Matrix4x4.op_Multiply(lhs, rhs) end
---
---@public
---@param lhs Matrix4x4 
---@param vector Vector4 
---@return Vector4 
function Matrix4x4.op_Multiply(lhs, vector) end
---
---@public
---@param lhs Matrix4x4 
---@param rhs Matrix4x4 
---@return boolean 
function Matrix4x4.op_Equality(lhs, rhs) end
---
---@public
---@param lhs Matrix4x4 
---@param rhs Matrix4x4 
---@return boolean 
function Matrix4x4.op_Inequality(lhs, rhs) end
---
---@public
---@param index number 
---@return Vector4 
function Matrix4x4:GetColumn(index) end
---
---@public
---@param index number 
---@return Vector4 
function Matrix4x4:GetRow(index) end
---
---@public
---@param index number 
---@param column Vector4 
---@return void 
function Matrix4x4:SetColumn(index, column) end
---
---@public
---@param index number 
---@param row Vector4 
---@return void 
function Matrix4x4:SetRow(index, row) end
---
---@public
---@param point Vector3 
---@return Vector3 
function Matrix4x4:MultiplyPoint(point) end
---
---@public
---@param point Vector3 
---@return Vector3 
function Matrix4x4:MultiplyPoint3x4(point) end
---
---@public
---@param vector Vector3 
---@return Vector3 
function Matrix4x4:MultiplyVector(vector) end
---
---@public
---@param plane Plane 
---@return Plane 
function Matrix4x4:TransformPlane(plane) end
---
---@public
---@param vector Vector3 
---@return Matrix4x4 
function Matrix4x4.Scale(vector) end
---
---@public
---@param vector Vector3 
---@return Matrix4x4 
function Matrix4x4.Translate(vector) end
---
---@public
---@param q Quaternion 
---@return Matrix4x4 
function Matrix4x4.Rotate(q) end
---
---@public
---@return string 
function Matrix4x4:ToString() end
---
---@public
---@param format string 
---@return string 
function Matrix4x4:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Matrix4x4:ToString(format, formatProvider) end
---
UnityEngine.Matrix4x4 = Matrix4x4