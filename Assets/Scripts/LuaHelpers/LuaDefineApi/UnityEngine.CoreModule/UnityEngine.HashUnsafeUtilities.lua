---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class HashUnsafeUtilities
local HashUnsafeUtilities={ }
---
---@public
---@param data Void* 
---@param dataSize number 
---@param hash1 UInt64* 
---@param hash2 UInt64* 
---@return void 
function HashUnsafeUtilities.ComputeHash128(data, dataSize, hash1, hash2) end
---
---@public
---@param data Void* 
---@param dataSize number 
---@param hash Hash128* 
---@return void 
function HashUnsafeUtilities.ComputeHash128(data, dataSize, hash) end
---
UnityEngine.HashUnsafeUtilities = HashUnsafeUtilities