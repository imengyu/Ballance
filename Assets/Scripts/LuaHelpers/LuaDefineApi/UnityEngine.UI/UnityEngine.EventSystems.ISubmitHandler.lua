---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ISubmitHandler
local ISubmitHandler={ }
---
---@public
---@param eventData BaseEventData 
---@return void 
function ISubmitHandler:OnSubmit(eventData) end
---
UnityEngine.EventSystems.ISubmitHandler = ISubmitHandler