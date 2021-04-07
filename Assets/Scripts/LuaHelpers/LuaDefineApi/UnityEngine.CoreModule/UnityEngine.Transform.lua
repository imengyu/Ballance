---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Transform : Component
---@field public position Vector3 
---@field public localPosition Vector3 
---@field public eulerAngles Vector3 
---@field public localEulerAngles Vector3 
---@field public right Vector3 
---@field public up Vector3 
---@field public forward Vector3 
---@field public rotation Quaternion 
---@field public localRotation Quaternion 
---@field public localScale Vector3 
---@field public parent Transform 
---@field public worldToLocalMatrix Matrix4x4 
---@field public localToWorldMatrix Matrix4x4 
---@field public root Transform 
---@field public childCount number 
---@field public lossyScale Vector3 
---@field public hasChanged boolean 
---@field public hierarchyCapacity number 
---@field public hierarchyCount number 
local Transform={ }
---
---@public
---@param p Transform 
---@return void 
function Transform:SetParent(p) end
---
---@public
---@param parent Transform 
---@param worldPositionStays boolean 
---@return void 
function Transform:SetParent(parent, worldPositionStays) end
---
---@public
---@param position Vector3 
---@param rotation Quaternion 
---@return void 
function Transform:SetPositionAndRotation(position, rotation) end
---
---@public
---@param translation Vector3 
---@param relativeTo number 
---@return void 
function Transform:Translate(translation, relativeTo) end
---
---@public
---@param translation Vector3 
---@return void 
function Transform:Translate(translation) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@param relativeTo number 
---@return void 
function Transform:Translate(x, y, z, relativeTo) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return void 
function Transform:Translate(x, y, z) end
---
---@public
---@param translation Vector3 
---@param relativeTo Transform 
---@return void 
function Transform:Translate(translation, relativeTo) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@param relativeTo Transform 
---@return void 
function Transform:Translate(x, y, z, relativeTo) end
---
---@public
---@param eulers Vector3 
---@param relativeTo number 
---@return void 
function Transform:Rotate(eulers, relativeTo) end
---
---@public
---@param eulers Vector3 
---@return void 
function Transform:Rotate(eulers) end
---
---@public
---@param xAngle number 
---@param yAngle number 
---@param zAngle number 
---@param relativeTo number 
---@return void 
function Transform:Rotate(xAngle, yAngle, zAngle, relativeTo) end
---
---@public
---@param xAngle number 
---@param yAngle number 
---@param zAngle number 
---@return void 
function Transform:Rotate(xAngle, yAngle, zAngle) end
---
---@public
---@param axis Vector3 
---@param angle number 
---@param relativeTo number 
---@return void 
function Transform:Rotate(axis, angle, relativeTo) end
---
---@public
---@param axis Vector3 
---@param angle number 
---@return void 
function Transform:Rotate(axis, angle) end
---
---@public
---@param point Vector3 
---@param axis Vector3 
---@param angle number 
---@return void 
function Transform:RotateAround(point, axis, angle) end
---
---@public
---@param target Transform 
---@param worldUp Vector3 
---@return void 
function Transform:LookAt(target, worldUp) end
---
---@public
---@param target Transform 
---@return void 
function Transform:LookAt(target) end
---
---@public
---@param worldPosition Vector3 
---@param worldUp Vector3 
---@return void 
function Transform:LookAt(worldPosition, worldUp) end
---
---@public
---@param worldPosition Vector3 
---@return void 
function Transform:LookAt(worldPosition) end
---
---@public
---@param direction Vector3 
---@return Vector3 
function Transform:TransformDirection(direction) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return Vector3 
function Transform:TransformDirection(x, y, z) end
---
---@public
---@param direction Vector3 
---@return Vector3 
function Transform:InverseTransformDirection(direction) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return Vector3 
function Transform:InverseTransformDirection(x, y, z) end
---
---@public
---@param vector Vector3 
---@return Vector3 
function Transform:TransformVector(vector) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return Vector3 
function Transform:TransformVector(x, y, z) end
---
---@public
---@param vector Vector3 
---@return Vector3 
function Transform:InverseTransformVector(vector) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return Vector3 
function Transform:InverseTransformVector(x, y, z) end
---
---@public
---@param position Vector3 
---@return Vector3 
function Transform:TransformPoint(position) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return Vector3 
function Transform:TransformPoint(x, y, z) end
---
---@public
---@param position Vector3 
---@return Vector3 
function Transform:InverseTransformPoint(position) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return Vector3 
function Transform:InverseTransformPoint(x, y, z) end
---
---@public
---@return void 
function Transform:DetachChildren() end
---
---@public
---@return void 
function Transform:SetAsFirstSibling() end
---
---@public
---@return void 
function Transform:SetAsLastSibling() end
---
---@public
---@param index number 
---@return void 
function Transform:SetSiblingIndex(index) end
---
---@public
---@return number 
function Transform:GetSiblingIndex() end
---
---@public
---@param n string 
---@return Transform 
function Transform:Find(n) end
---
---@public
---@param parent Transform 
---@return boolean 
function Transform:IsChildOf(parent) end
---
---@public
---@param n string 
---@return Transform 
function Transform:FindChild(n) end
---
---@public
---@return IEnumerator 
function Transform:GetEnumerator() end
---
---@public
---@param axis Vector3 
---@param angle number 
---@return void 
function Transform:RotateAround(axis, angle) end
---
---@public
---@param axis Vector3 
---@param angle number 
---@return void 
function Transform:RotateAroundLocal(axis, angle) end
---
---@public
---@param index number 
---@return Transform 
function Transform:GetChild(index) end
---
---@public
---@return number 
function Transform:GetChildCount() end
---
UnityEngine.Transform = Transform