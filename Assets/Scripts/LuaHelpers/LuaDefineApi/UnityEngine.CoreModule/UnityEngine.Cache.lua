---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Cache : ValueType
---@field public valid boolean 
---@field public ready boolean 
---@field public readOnly boolean 
---@field public path string 
---@field public index number 
---@field public spaceFree number 
---@field public maximumAvailableStorageSpace number 
---@field public spaceOccupied number 
---@field public expirationDelay number 
local Cache={ }
---
---@public
---@param lhs Cache 
---@param rhs Cache 
---@return boolean 
function Cache.op_Equality(lhs, rhs) end
---
---@public
---@param lhs Cache 
---@param rhs Cache 
---@return boolean 
function Cache.op_Inequality(lhs, rhs) end
---
---@public
---@return number 
function Cache:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function Cache:Equals(other) end
---
---@public
---@param other Cache 
---@return boolean 
function Cache:Equals(other) end
---
---@public
---@return boolean 
function Cache:ClearCache() end
---
---@public
---@param expiration number 
---@return boolean 
function Cache:ClearCache(expiration) end
---
UnityEngine.Cache = Cache