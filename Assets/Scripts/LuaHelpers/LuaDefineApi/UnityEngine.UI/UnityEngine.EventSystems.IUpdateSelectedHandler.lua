---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IUpdateSelectedHandler
local IUpdateSelectedHandler={ }
---
---@public
---@param eventData BaseEventData 
---@return void 
function IUpdateSelectedHandler:OnUpdateSelected(eventData) end
---
UnityEngine.EventSystems.IUpdateSelectedHandler = IUpdateSelectedHandler