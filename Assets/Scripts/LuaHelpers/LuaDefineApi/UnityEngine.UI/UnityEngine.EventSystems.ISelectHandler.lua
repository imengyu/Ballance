---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ISelectHandler
local ISelectHandler={ }
---
---@public
---@param eventData BaseEventData 
---@return void 
function ISelectHandler:OnSelect(eventData) end
---
UnityEngine.EventSystems.ISelectHandler = ISelectHandler