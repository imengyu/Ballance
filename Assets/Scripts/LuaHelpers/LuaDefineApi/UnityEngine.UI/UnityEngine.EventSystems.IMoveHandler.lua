---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IMoveHandler
local IMoveHandler={ }
---
---@public
---@param eventData AxisEventData 
---@return void 
function IMoveHandler:OnMove(eventData) end
---
UnityEngine.EventSystems.IMoveHandler = IMoveHandler