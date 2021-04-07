---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class EventInterests : ValueType
---@field public wantsMouseMove boolean 
---@field public wantsMouseEnterLeaveWindow boolean 
---@field public wantsLessLayoutEvents boolean 
local EventInterests={ }
---
---@public
---@param type number 
---@return boolean 
function EventInterests:WantsEvent(type) end
---
---@public
---@param type number 
---@return boolean 
function EventInterests:WantsLayoutPass(type) end
---
UnityEngine.EventInterests = EventInterests