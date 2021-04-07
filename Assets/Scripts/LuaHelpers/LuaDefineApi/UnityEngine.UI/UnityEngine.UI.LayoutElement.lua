---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LayoutElement : UIBehaviour
---@field public ignoreLayout boolean 
---@field public minWidth number 
---@field public minHeight number 
---@field public preferredWidth number 
---@field public preferredHeight number 
---@field public flexibleWidth number 
---@field public flexibleHeight number 
---@field public layoutPriority number 
local LayoutElement={ }
---
---@public
---@return void 
function LayoutElement:CalculateLayoutInputHorizontal() end
---
---@public
---@return void 
function LayoutElement:CalculateLayoutInputVertical() end
---
UnityEngine.UI.LayoutElement = LayoutElement