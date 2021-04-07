---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IPointerEnterHandler
local IPointerEnterHandler={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function IPointerEnterHandler:OnPointerEnter(eventData) end
---
UnityEngine.EventSystems.IPointerEnterHandler = IPointerEnterHandler