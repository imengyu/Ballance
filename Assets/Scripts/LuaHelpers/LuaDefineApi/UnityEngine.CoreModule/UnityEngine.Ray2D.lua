---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Ray2D : ValueType
---@field public origin Vector2 
---@field public direction Vector2 
local Ray2D={ }
---
---@public
---@param distance number 
---@return Vector2 
function Ray2D:GetPoint(distance) end
---
---@public
---@return string 
function Ray2D:ToString() end
---
---@public
---@param format string 
---@return string 
function Ray2D:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Ray2D:ToString(format, formatProvider) end
---
UnityEngine.Ray2D = Ray2D