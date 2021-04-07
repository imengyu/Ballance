---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SplitView
---@field public self RectTransform 
---@field public dragger RectTransform 
---@field public panelOne RectTransform 
---@field public panelTwo RectTransform 
---@field public splitViewDragger SplitViewDragger 
---@field public min number 最小的分割比例（相当于左边或是上部最小大小）
---@field public max number 最大的分割比例（相当于右边或是下部最小大小）
---@field public direction number 分割方向
---@field public value number 分割比例(0-1,相当于两个视图的大小比例)
local SplitView={ }
---
---@public
---@return void 
function SplitView:BindDragger() end
---
---@public
---@return void 
function SplitView:UpdateVal() end
---分割两个视图控件，用户可以手动拖动调整两个视图的大小
Ballance2.Sys.UI.SplitView = SplitView