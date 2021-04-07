---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AbstractEventData
---@field public used boolean 
local AbstractEventData={ }
---
---@public
---@return void 
function AbstractEventData:Reset() end
---
---@public
---@return void 
function AbstractEventData:Use() end
---
UnityEngine.EventSystems.AbstractEventData = AbstractEventData