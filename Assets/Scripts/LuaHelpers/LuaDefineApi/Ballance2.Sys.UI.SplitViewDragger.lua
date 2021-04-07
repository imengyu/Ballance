---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SplitViewDragger
---@field public draggerImage RectTransform 
---@field public image Image 
---@field public onValueChanged SliderEvent 拖动事件（value表示拖动的百分比）
---@field public referenceTransform RectTransform 拖动参考RectTransform
---@field public isDrag boolean 是否正在拖动
---@field public direction number 拖动方向
local SplitViewDragger={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function SplitViewDragger:OnDrag(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function SplitViewDragger:OnPointerDown(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function SplitViewDragger:OnPointerUp(eventData) end
---拖动托块组件
Ballance2.Sys.UI.SplitViewDragger = SplitViewDragger