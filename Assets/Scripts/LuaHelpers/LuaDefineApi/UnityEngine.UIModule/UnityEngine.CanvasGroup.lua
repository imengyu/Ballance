---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CanvasGroup : Behaviour
---@field public alpha number 
---@field public interactable boolean 
---@field public blocksRaycasts boolean 
---@field public ignoreParentGroups boolean 
local CanvasGroup={ }
---
---@public
---@param sp Vector2 
---@param eventCamera Camera 
---@return boolean 
function CanvasGroup:IsRaycastLocationValid(sp, eventCamera) end
---
UnityEngine.CanvasGroup = CanvasGroup