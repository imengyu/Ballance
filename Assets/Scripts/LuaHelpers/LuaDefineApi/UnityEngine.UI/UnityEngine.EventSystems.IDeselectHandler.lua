---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IDeselectHandler
local IDeselectHandler={ }
---
---@public
---@param eventData BaseEventData 
---@return void 
function IDeselectHandler:OnDeselect(eventData) end
---
UnityEngine.EventSystems.IDeselectHandler = IDeselectHandler