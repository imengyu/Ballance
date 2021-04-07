---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ICancelHandler
local ICancelHandler={ }
---
---@public
---@param eventData BaseEventData 
---@return void 
function ICancelHandler:OnCancel(eventData) end
---
UnityEngine.EventSystems.ICancelHandler = ICancelHandler