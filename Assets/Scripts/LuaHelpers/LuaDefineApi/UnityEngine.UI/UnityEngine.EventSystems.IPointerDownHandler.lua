---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IPointerDownHandler
local IPointerDownHandler={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function IPointerDownHandler:OnPointerDown(eventData) end
---
UnityEngine.EventSystems.IPointerDownHandler = IPointerDownHandler