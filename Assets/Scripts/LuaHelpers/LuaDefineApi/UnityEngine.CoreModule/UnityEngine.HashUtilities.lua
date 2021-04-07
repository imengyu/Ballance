---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class HashUtilities
local HashUtilities={ }
---
---@public
---@param inHash Hash128& 
---@param outHash Hash128& 
---@return void 
function HashUtilities.AppendHash(inHash, outHash) end
---
---@public
---@param value Matrix4x4& 
---@param hash Hash128& 
---@return void 
function HashUtilities.QuantisedMatrixHash(value, hash) end
---
---@public
---@param value Vector3& 
---@param hash Hash128& 
---@return void 
function HashUtilities.QuantisedVectorHash(value, hash) end
---
---@public
---@param value Byte[] 
---@param hash Hash128& 
---@return void 
function HashUtilities.ComputeHash128(value, hash) end
---
UnityEngine.HashUtilities = HashUtilities