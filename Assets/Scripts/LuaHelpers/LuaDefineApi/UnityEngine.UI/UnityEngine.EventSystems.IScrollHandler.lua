---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IScrollHandler
local IScrollHandler={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function IScrollHandler:OnScroll(eventData) end
---
UnityEngine.EventSystems.IScrollHandler = IScrollHandler