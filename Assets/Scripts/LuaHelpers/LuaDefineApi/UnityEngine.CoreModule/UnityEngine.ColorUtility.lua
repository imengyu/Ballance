---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ColorUtility
local ColorUtility={ }
---
---@public
---@param htmlString string 
---@param color Color& 
---@return boolean 
function ColorUtility.TryParseHtmlString(htmlString, color) end
---
---@public
---@param color Color 
---@return string 
function ColorUtility.ToHtmlStringRGB(color) end
---
---@public
---@param color Color 
---@return string 
function ColorUtility.ToHtmlStringRGBA(color) end
---
UnityEngine.ColorUtility = ColorUtility