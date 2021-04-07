---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BoneWeight1 : ValueType
---@field public weight number 
---@field public boneIndex number 
local BoneWeight1={ }
---
---@public
---@param other Object 
---@return boolean 
function BoneWeight1:Equals(other) end
---
---@public
---@param other BoneWeight1 
---@return boolean 
function BoneWeight1:Equals(other) end
---
---@public
---@return number 
function BoneWeight1:GetHashCode() end
---
---@public
---@param lhs BoneWeight1 
---@param rhs BoneWeight1 
---@return boolean 
function BoneWeight1.op_Equality(lhs, rhs) end
---
---@public
---@param lhs BoneWeight1 
---@param rhs BoneWeight1 
---@return boolean 
function BoneWeight1.op_Inequality(lhs, rhs) end
---
UnityEngine.BoneWeight1 = BoneWeight1