---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class UIRayIgnore
local UIRayIgnore={ }
---
---@public
---@param screenPoint Vector2 
---@param eventCamera Camera 
---@return boolean 
function UIRayIgnore:IsRaycastLocationValid(screenPoint, eventCamera) end
---
Ballance2.Sys.UI.Utils.UIRayIgnore = UIRayIgnore