---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ColorBlock : ValueType
---@field public defaultColorBlock ColorBlock 
---@field public normalColor Color 
---@field public highlightedColor Color 
---@field public pressedColor Color 
---@field public selectedColor Color 
---@field public disabledColor Color 
---@field public colorMultiplier number 
---@field public fadeDuration number 
local ColorBlock={ }
---
---@public
---@param obj Object 
---@return boolean 
function ColorBlock:Equals(obj) end
---
---@public
---@param other ColorBlock 
---@return boolean 
function ColorBlock:Equals(other) end
---
---@public
---@param point1 ColorBlock 
---@param point2 ColorBlock 
---@return boolean 
function ColorBlock.op_Equality(point1, point2) end
---
---@public
---@param point1 ColorBlock 
---@param point2 ColorBlock 
---@return boolean 
function ColorBlock.op_Inequality(point1, point2) end
---
---@public
---@return number 
function ColorBlock:GetHashCode() end
---
UnityEngine.UI.ColorBlock = ColorBlock