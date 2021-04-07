---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IPointerExitHandler
local IPointerExitHandler={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function IPointerExitHandler:OnPointerExit(eventData) end
---
UnityEngine.EventSystems.IPointerExitHandler = IPointerExitHandler