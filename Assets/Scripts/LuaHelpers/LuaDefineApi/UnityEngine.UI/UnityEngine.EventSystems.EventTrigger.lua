---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class EventTrigger : MonoBehaviour
---@field public delegates List`1 
---@field public triggers List`1 
local EventTrigger={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTrigger:OnPointerEnter(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTrigger:OnPointerExit(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTrigger:OnDrag(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTrigger:OnDrop(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTrigger:OnPointerDown(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTrigger:OnPointerUp(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTrigger:OnPointerClick(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function EventTrigger:OnSelect(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function EventTrigger:OnDeselect(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTrigger:OnScroll(eventData) end
---
---@public
---@param eventData AxisEventData 
---@return void 
function EventTrigger:OnMove(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function EventTrigger:OnUpdateSelected(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTrigger:OnInitializePotentialDrag(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTrigger:OnBeginDrag(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function EventTrigger:OnEndDrag(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function EventTrigger:OnSubmit(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function EventTrigger:OnCancel(eventData) end
---
UnityEngine.EventSystems.EventTrigger = EventTrigger