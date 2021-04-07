---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IEndDragHandler
local IEndDragHandler={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function IEndDragHandler:OnEndDrag(eventData) end
---
UnityEngine.EventSystems.IEndDragHandler = IEndDragHandler