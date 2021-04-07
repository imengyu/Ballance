---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class UILayoutUtils
local UILayoutUtils={ }
---
---@public
---@param gravity number 
---@param axis number 
---@return number 
function UILayoutUtils.GravityToAnchor(gravity, axis) end
---
---@public
---@param anchor number 
---@param axis number 
---@return number 
function UILayoutUtils.AnchorToPivot(anchor, axis) end
---
Ballance2.Sys.UI.UISystem.Layout.UILayoutUtils = UILayoutUtils