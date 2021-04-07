---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Physics2DRaycaster : PhysicsRaycaster
local Physics2DRaycaster={ }
---
---@public
---@param eventData PointerEventData 
---@param resultAppendList List`1 
---@return void 
function Physics2DRaycaster:Raycast(eventData, resultAppendList) end
---
UnityEngine.EventSystems.Physics2DRaycaster = Physics2DRaycaster