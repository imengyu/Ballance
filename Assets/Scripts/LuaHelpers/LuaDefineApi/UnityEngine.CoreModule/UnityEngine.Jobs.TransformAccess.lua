---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class TransformAccess : ValueType
---@field public position Vector3 
---@field public rotation Quaternion 
---@field public localPosition Vector3 
---@field public localRotation Quaternion 
---@field public localScale Vector3 
---@field public localToWorldMatrix Matrix4x4 
---@field public worldToLocalMatrix Matrix4x4 
---@field public isValid boolean 
local TransformAccess={ }
---
UnityEngine.Jobs.TransformAccess = TransformAccess