---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LayoutUtility
local LayoutUtility={ }
---
---@public
---@param rect RectTransform 
---@param axis number 
---@return number 
function LayoutUtility.GetMinSize(rect, axis) end
---
---@public
---@param rect RectTransform 
---@param axis number 
---@return number 
function LayoutUtility.GetPreferredSize(rect, axis) end
---
---@public
---@param rect RectTransform 
---@param axis number 
---@return number 
function LayoutUtility.GetFlexibleSize(rect, axis) end
---
---@public
---@param rect RectTransform 
---@return number 
function LayoutUtility.GetMinWidth(rect) end
---
---@public
---@param rect RectTransform 
---@return number 
function LayoutUtility.GetPreferredWidth(rect) end
---
---@public
---@param rect RectTransform 
---@return number 
function LayoutUtility.GetFlexibleWidth(rect) end
---
---@public
---@param rect RectTransform 
---@return number 
function LayoutUtility.GetMinHeight(rect) end
---
---@public
---@param rect RectTransform 
---@return number 
function LayoutUtility.GetPreferredHeight(rect) end
---
---@public
---@param rect RectTransform 
---@return number 
function LayoutUtility.GetFlexibleHeight(rect) end
---
---@public
---@param rect RectTransform 
---@param property Func`2 
---@param defaultValue number 
---@return number 
function LayoutUtility.GetLayoutProperty(rect, property, defaultValue) end
---
---@public
---@param rect RectTransform 
---@param property Func`2 
---@param defaultValue number 
---@param source ILayoutElement& 
---@return number 
function LayoutUtility.GetLayoutProperty(rect, property, defaultValue, source) end
---
UnityEngine.UI.LayoutUtility = LayoutUtility