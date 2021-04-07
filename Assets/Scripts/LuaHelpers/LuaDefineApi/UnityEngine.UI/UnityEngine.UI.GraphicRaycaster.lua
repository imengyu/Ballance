---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GraphicRaycaster : BaseRaycaster
---@field public sortOrderPriority number 
---@field public renderOrderPriority number 
---@field public ignoreReversedGraphics boolean 
---@field public blockingObjects number 
---@field public blockingMask LayerMask 
---@field public eventCamera Camera 
local GraphicRaycaster={ }
---
---@public
---@param eventData PointerEventData 
---@param resultAppendList List`1 
---@return void 
function GraphicRaycaster:Raycast(eventData, resultAppendList) end
---
UnityEngine.UI.GraphicRaycaster = GraphicRaycaster