---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IPointerClickHandler
local IPointerClickHandler={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function IPointerClickHandler:OnPointerClick(eventData) end
---
UnityEngine.EventSystems.IPointerClickHandler = IPointerClickHandler