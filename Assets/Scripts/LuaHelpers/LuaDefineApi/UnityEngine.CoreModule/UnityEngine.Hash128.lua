---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Hash128 : ValueType
---@field public isValid boolean 
local Hash128={ }
---
---@public
---@param rhs Hash128 
---@return number 
function Hash128:CompareTo(rhs) end
---
---@public
---@return string 
function Hash128:ToString() end
---
---@public
---@param hashString string 
---@return Hash128 
function Hash128.Parse(hashString) end
---
---@public
---@param data string 
---@return Hash128 
function Hash128.Compute(data) end
---
---@public
---@param val number 
---@return Hash128 
function Hash128.Compute(val) end
---
---@public
---@param val number 
---@return Hash128 
function Hash128.Compute(val) end
---
---@public
---@param data Void* 
---@param size number 
---@return Hash128 
function Hash128.Compute(data, size) end
---
---@public
---@param data string 
---@return void 
function Hash128:Append(data) end
---
---@public
---@param val number 
---@return void 
function Hash128:Append(val) end
---
---@public
---@param val number 
---@return void 
function Hash128:Append(val) end
---
---@public
---@param data Void* 
---@param size number 
---@return void 
function Hash128:Append(data, size) end
---
---@public
---@param obj Object 
---@return boolean 
function Hash128:Equals(obj) end
---
---@public
---@param obj Hash128 
---@return boolean 
function Hash128:Equals(obj) end
---
---@public
---@return number 
function Hash128:GetHashCode() end
---
---@public
---@param obj Object 
---@return number 
function Hash128:CompareTo(obj) end
---
---@public
---@param hash1 Hash128 
---@param hash2 Hash128 
---@return boolean 
function Hash128.op_Equality(hash1, hash2) end
---
---@public
---@param hash1 Hash128 
---@param hash2 Hash128 
---@return boolean 
function Hash128.op_Inequality(hash1, hash2) end
---
---@public
---@param x Hash128 
---@param y Hash128 
---@return boolean 
function Hash128.op_LessThan(x, y) end
---
---@public
---@param x Hash128 
---@param y Hash128 
---@return boolean 
function Hash128.op_GreaterThan(x, y) end
---
UnityEngine.Hash128 = Hash128