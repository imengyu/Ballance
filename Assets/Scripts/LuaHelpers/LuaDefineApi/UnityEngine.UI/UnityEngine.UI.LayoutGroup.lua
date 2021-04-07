---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LayoutGroup : UIBehaviour
---@field public padding RectOffset 
---@field public childAlignment number 
---@field public minWidth number 
---@field public preferredWidth number 
---@field public flexibleWidth number 
---@field public minHeight number 
---@field public preferredHeight number 
---@field public flexibleHeight number 
---@field public layoutPriority number 
local LayoutGroup={ }
---
---@public
---@return void 
function LayoutGroup:CalculateLayoutInputHorizontal() end
---
---@public
---@return void 
function LayoutGroup:CalculateLayoutInputVertical() end
---
---@public
---@return void 
function LayoutGroup:SetLayoutHorizontal() end
---
---@public
---@return void 
function LayoutGroup:SetLayoutVertical() end
---
UnityEngine.UI.LayoutGroup = LayoutGroup