---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BaseRaycaster : UIBehaviour
---@field public eventCamera Camera 
---@field public priority number 
---@field public sortOrderPriority number 
---@field public renderOrderPriority number 
---@field public rootRaycaster BaseRaycaster 
local BaseRaycaster={ }
---
---@public
---@param eventData PointerEventData 
---@param resultAppendList List`1 
---@return void 
function BaseRaycaster:Raycast(eventData, resultAppendList) end
---
---@public
---@return string 
function BaseRaycaster:ToString() end
---
UnityEngine.EventSystems.BaseRaycaster = BaseRaycaster