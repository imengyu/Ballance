---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class UnityEventQueueSystem
local UnityEventQueueSystem={ }
---
---@public
---@param eventPayloadName string 
---@return string 
function UnityEventQueueSystem.GenerateEventIdForPayload(eventPayloadName) end
---
---@public
---@return IntPtr 
function UnityEventQueueSystem.GetGlobalEventQueue() end
---
UnityEngine.UnityEventQueueSystem = UnityEventQueueSystem