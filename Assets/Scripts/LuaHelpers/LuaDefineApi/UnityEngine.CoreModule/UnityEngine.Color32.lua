---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Color32 : ValueType
---@field public r number 
---@field public g number 
---@field public b number 
---@field public a number 
---@field public Item number 
local Color32={ }
---
---@public
---@param c Color 
---@return Color32 
function Color32.op_Implicit(c) end
---
---@public
---@param c Color32 
---@return Color 
function Color32.op_Implicit(c) end
---
---@public
---@param a Color32 
---@param b Color32 
---@param t number 
---@return Color32 
function Color32.Lerp(a, b, t) end
---
---@public
---@param a Color32 
---@param b Color32 
---@param t number 
---@return Color32 
function Color32.LerpUnclamped(a, b, t) end
---
---@public
---@return string 
function Color32:ToString() end
---
---@public
---@param format string 
---@return string 
function Color32:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Color32:ToString(format, formatProvider) end
---
UnityEngine.Color32 = Color32