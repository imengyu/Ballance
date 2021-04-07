---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class FloatComparer
---@field public s_ComparerWithDefaultTolerance FloatComparer 
---@field public kEpsilon number 
local FloatComparer={ }
---
---@public
---@param a number 
---@param b number 
---@return boolean 
function FloatComparer:Equals(a, b) end
---
---@public
---@param obj number 
---@return number 
function FloatComparer:GetHashCode(obj) end
---
---@public
---@param expected number 
---@param actual number 
---@param error number 
---@return boolean 
function FloatComparer.AreEqual(expected, actual, error) end
---
---@public
---@param expected number 
---@param actual number 
---@param error number 
---@return boolean 
function FloatComparer.AreEqualRelative(expected, actual, error) end
---
UnityEngine.Assertions.Comparers.FloatComparer = FloatComparer