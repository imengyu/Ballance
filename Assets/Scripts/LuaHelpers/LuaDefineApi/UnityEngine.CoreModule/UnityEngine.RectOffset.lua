---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RectOffset
---@field public left number 
---@field public right number 
---@field public top number 
---@field public bottom number 
---@field public horizontal number 
---@field public vertical number 
local RectOffset={ }
---
---@public
---@return string 
function RectOffset:ToString() end
---
---@public
---@param format string 
---@return string 
function RectOffset:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function RectOffset:ToString(format, formatProvider) end
---
---@public
---@param rect Rect 
---@return Rect 
function RectOffset:Add(rect) end
---
---@public
---@param rect Rect 
---@return Rect 
function RectOffset:Remove(rect) end
---
UnityEngine.RectOffset = RectOffset