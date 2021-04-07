---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ILayoutElement
---@field public minWidth number 
---@field public preferredWidth number 
---@field public flexibleWidth number 
---@field public minHeight number 
---@field public preferredHeight number 
---@field public flexibleHeight number 
---@field public layoutPriority number 
local ILayoutElement={ }
---
---@public
---@return void 
function ILayoutElement:CalculateLayoutInputHorizontal() end
---
---@public
---@return void 
function ILayoutElement:CalculateLayoutInputVertical() end
---
UnityEngine.UI.ILayoutElement = ILayoutElement