---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IDragHandler
local IDragHandler={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function IDragHandler:OnDrag(eventData) end
---
UnityEngine.EventSystems.IDragHandler = IDragHandler