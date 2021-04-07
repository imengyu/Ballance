---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Gradient
---@field public colorKeys GradientColorKey[] 
---@field public alphaKeys GradientAlphaKey[] 
---@field public mode number 
local Gradient={ }
---
---@public
---@param time number 
---@return Color 
function Gradient:Evaluate(time) end
---
---@public
---@param colorKeys GradientColorKey[] 
---@param alphaKeys GradientAlphaKey[] 
---@return void 
function Gradient:SetKeys(colorKeys, alphaKeys) end
---
---@public
---@param o Object 
---@return boolean 
function Gradient:Equals(o) end
---
---@public
---@param other Gradient 
---@return boolean 
function Gradient:Equals(other) end
---
---@public
---@return number 
function Gradient:GetHashCode() end
---
UnityEngine.Gradient = Gradient