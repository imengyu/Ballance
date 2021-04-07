---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BoneWeight : ValueType
---@field public weight0 number 
---@field public weight1 number 
---@field public weight2 number 
---@field public weight3 number 
---@field public boneIndex0 number 
---@field public boneIndex1 number 
---@field public boneIndex2 number 
---@field public boneIndex3 number 
local BoneWeight={ }
---
---@public
---@return number 
function BoneWeight:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function BoneWeight:Equals(other) end
---
---@public
---@param other BoneWeight 
---@return boolean 
function BoneWeight:Equals(other) end
---
---@public
---@param lhs BoneWeight 
---@param rhs BoneWeight 
---@return boolean 
function BoneWeight.op_Equality(lhs, rhs) end
---
---@public
---@param lhs BoneWeight 
---@param rhs BoneWeight 
---@return boolean 
function BoneWeight.op_Inequality(lhs, rhs) end
---
UnityEngine.BoneWeight = BoneWeight