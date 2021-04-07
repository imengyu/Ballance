---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IPointerUpHandler
local IPointerUpHandler={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function IPointerUpHandler:OnPointerUp(eventData) end
---
UnityEngine.EventSystems.IPointerUpHandler = IPointerUpHandler