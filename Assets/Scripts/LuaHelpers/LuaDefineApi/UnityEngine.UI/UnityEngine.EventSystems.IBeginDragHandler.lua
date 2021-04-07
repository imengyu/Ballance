---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IBeginDragHandler
local IBeginDragHandler={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function IBeginDragHandler:OnBeginDrag(eventData) end
---
UnityEngine.EventSystems.IBeginDragHandler = IBeginDragHandler