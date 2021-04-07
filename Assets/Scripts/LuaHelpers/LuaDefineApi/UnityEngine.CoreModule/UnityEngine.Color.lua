---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Color : ValueType
---@field public r number 
---@field public g number 
---@field public b number 
---@field public a number 
---@field public red Color 
---@field public green Color 
---@field public blue Color 
---@field public white Color 
---@field public black Color 
---@field public yellow Color 
---@field public cyan Color 
---@field public magenta Color 
---@field public gray Color 
---@field public grey Color 
---@field public clear Color 
---@field public grayscale number 
---@field public linear Color 
---@field public gamma Color 
---@field public maxColorComponent number 
---@field public Item number 
local Color={ }
---
---@public
---@return string 
function Color:ToString() end
---
---@public
---@param format string 
---@return string 
function Color:ToString(format) end
---
---@public
---@param format string 
---@param formatProvider IFormatProvider 
---@return string 
function Color:ToString(format, formatProvider) end
---
---@public
---@return number 
function Color:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function Color:Equals(other) end
---
---@public
---@param other Color 
---@return boolean 
function Color:Equals(other) end
---
---@public
---@param a Color 
---@param b Color 
---@return Color 
function Color.op_Addition(a, b) end
---
---@public
---@param a Color 
---@param b Color 
---@return Color 
function Color.op_Subtraction(a, b) end
---
---@public
---@param a Color 
---@param b Color 
---@return Color 
function Color.op_Multiply(a, b) end
---
---@public
---@param a Color 
---@param b number 
---@return Color 
function Color.op_Multiply(a, b) end
---
---@public
---@param b number 
---@param a Color 
---@return Color 
function Color.op_Multiply(b, a) end
---
---@public
---@param a Color 
---@param b number 
---@return Color 
function Color.op_Division(a, b) end
---
---@public
---@param lhs Color 
---@param rhs Color 
---@return boolean 
function Color.op_Equality(lhs, rhs) end
---
---@public
---@param lhs Color 
---@param rhs Color 
---@return boolean 
function Color.op_Inequality(lhs, rhs) end
---
---@public
---@param a Color 
---@param b Color 
---@param t number 
---@return Color 
function Color.Lerp(a, b, t) end
---
---@public
---@param a Color 
---@param b Color 
---@param t number 
---@return Color 
function Color.LerpUnclamped(a, b, t) end
---
---@public
---@param c Color 
---@return Vector4 
function Color.op_Implicit(c) end
---
---@public
---@param v Vector4 
---@return Color 
function Color.op_Implicit(v) end
---
---@public
---@param rgbColor Color 
---@param H Single& 
---@param S Single& 
---@param V Single& 
---@return void 
function Color.RGBToHSV(rgbColor, H, S, V) end
---
---@public
---@param H number 
---@param S number 
---@param V number 
---@return Color 
function Color.HSVToRGB(H, S, V) end
---
---@public
---@param H number 
---@param S number 
---@param V number 
---@param hdr boolean 
---@return Color 
function Color.HSVToRGB(H, S, V, hdr) end
---
UnityEngine.Color = Color