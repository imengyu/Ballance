---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Rect : ValueType
---@field public zero Rect 
---@field public x number 
---@field public y number 
---@field public position Vector2 
---@field public center Vector2 
---@field public min Vector2 
---@field public max Vector2 
---@field public width number 
---@field public height number 
---@field public size Vector2 
---@field public xMin number 
---@field public yMin number 
---@field public xMax number 
---@field public yMax number 
---@field public left number 
---@field public right number 
---@field public top number 
---@field public bottom number 
local Rect={ }
---
---@public
---@param xmin number 
---@param ymin number 
---@param xmax number 
---@param ymax number 
---@return Rect 
function Rect.MinMaxRect(xmin, ymin, xmax, ymax) end
---
---@public
---@param x number 
---@param y number 
---@param width number 
---@param height number 
---@return void 
function Rect:Set(x, y, width, height) end
---
---@public
---@param point Vector2 
---@return boolean 
function Rect:Contains(point) end
---
---@public
---@param point Vector3 
---@return boolean 
function Rect:Contains(point) end
---
---@public
---@param point Vector3 
---@param allowInverse boolean 
---@return boolean 
function Rect:Contains(point, allowInverse) end
---
---@public
---@param other Rect 
---@return boolean 
function Rect:Overlaps(other) end
---
---@public
---@param other Rect 
---@param allowInverse boolean 
---@return boolean 
function Rect:Overlaps(other, allowInverse) end
---
---@public
---@param rectangle Rect 
---@param normalizedRectCoordinates Vector2 
---@return Vector2 
function Rect.NormalizedToPoint(rectangle, normalizedRectCoordinates) end
---
---@public
---@param rectangle Rect 
---@param point Vector2 
---@return Vector2 
function Rect.PointToNormalized(rectangle, point) end
---
---@public
---@param lhs Rect 
---@param rhs Rect 
---@return boolean 
function Rect.op_Inequality(lhs, rhs) end
---
---@public
---@param lhs Rect 
---@param rhs Rect 
---@return boolean 
function Rect.op_Equality(lhs, rhs) end
---
---@public
---@return number 
function Rect:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function Rect:Equals(other) end
---
---@public
---@param other Rect 
---@return boolean 
function Rect:Equals(other) end
---
---@public
---@return string 
function Rect:ToString() end
---
---@public
---@param format string 
---@return string 
function Rect:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Rect:ToString(format, formatProvider) end
---
UnityEngine.Rect = Rect