---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RectInt : ValueType
---@field public x number 
---@field public y number 
---@field public center Vector2 
---@field public min Vector2Int 
---@field public max Vector2Int 
---@field public width number 
---@field public height number 
---@field public xMin number 
---@field public yMin number 
---@field public xMax number 
---@field public yMax number 
---@field public position Vector2Int 
---@field public size Vector2Int 
---@field public allPositionsWithin PositionEnumerator 
local RectInt={ }
---
---@public
---@param minPosition Vector2Int 
---@param maxPosition Vector2Int 
---@return void 
function RectInt:SetMinMax(minPosition, maxPosition) end
---
---@public
---@param bounds RectInt 
---@return void 
function RectInt:ClampToBounds(bounds) end
---
---@public
---@param position Vector2Int 
---@return boolean 
function RectInt:Contains(position) end
---
---@public
---@param other RectInt 
---@return boolean 
function RectInt:Overlaps(other) end
---
---@public
---@return string 
function RectInt:ToString() end
---
---@public
---@param format string 
---@return string 
function RectInt:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function RectInt:ToString(format, formatProvider) end
---
---@public
---@param other RectInt 
---@return boolean 
function RectInt:Equals(other) end
---
UnityEngine.RectInt = RectInt