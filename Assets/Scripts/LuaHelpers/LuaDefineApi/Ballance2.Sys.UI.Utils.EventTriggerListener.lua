---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class EventTriggerListener
---@field public onClick VoidDelegate 
---@field public onDown VoidDelegate 
---@field public onEnter VoidDelegate 
---@field public onExit VoidDelegate 
---@field public onUp VoidDelegate 
---@field public onSelect VoidDelegate 
---@field public onUpdateSelect VoidDelegate 
local EventTriggerListener={ }
---从 指定 GameObject 创建事件侦听器
---@public
---@param go GameObject 指定 GameObject
---@return EventTriggerListener 
function EventTriggerListener.Get(go) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTriggerListener:OnPointerClick(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTriggerListener:OnPointerDown(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTriggerListener:OnPointerEnter(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTriggerListener:OnPointerExit(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTriggerListener:OnPointerUp(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function EventTriggerListener:OnSelect(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function EventTriggerListener:OnUpdateSelected(eventData) end
---
Ballance2.Sys.UI.Utils.EventTriggerListener = EventTriggerListener