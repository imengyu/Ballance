---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ICanvasRaycastFilter
local ICanvasRaycastFilter={ }
---
---@public
---@param sp Vector2 
---@param eventCamera Camera 
---@return boolean 
function ICanvasRaycastFilter:IsRaycastLocationValid(sp, eventCamera) end
---
UnityEngine.ICanvasRaycastFilter = ICanvasRaycastFilter