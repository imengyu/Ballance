---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class MultipleDisplayUtilities
local MultipleDisplayUtilities={ }
---
---@public
---@param eventData PointerEventData 
---@param position Vector2& 
---@return boolean 
function MultipleDisplayUtilities.GetRelativeMousePositionForDrag(eventData, position) end
---
---@public
---@return Vector2 
function MultipleDisplayUtilities.GetMousePositionRelativeToMainDisplayResolution() end
---
UnityEngine.UI.MultipleDisplayUtilities = MultipleDisplayUtilities