---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IInitializePotentialDragHandler
local IInitializePotentialDragHandler={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function IInitializePotentialDragHandler:OnInitializePotentialDrag(eventData) end
---
UnityEngine.EventSystems.IInitializePotentialDragHandler = IInitializePotentialDragHandler