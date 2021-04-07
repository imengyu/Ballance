---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IDropHandler
local IDropHandler={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function IDropHandler:OnDrop(eventData) end
---
UnityEngine.EventSystems.IDropHandler = IDropHandler