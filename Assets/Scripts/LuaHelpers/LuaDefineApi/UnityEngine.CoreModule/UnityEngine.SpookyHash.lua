---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SpookyHash
local SpookyHash={ }
---
---@public
---@param message Void* 
---@param length number 
---@param hash1 UInt64* 
---@param hash2 UInt64* 
---@return void 
function SpookyHash.Hash(message, length, hash1, hash2) end
---
UnityEngine.SpookyHash = SpookyHash