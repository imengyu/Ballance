---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PointerInputModule : BaseInputModule
---@field public kMouseLeftId number 
---@field public kMouseRightId number 
---@field public kMouseMiddleId number 
---@field public kFakeTouchesId number 
local PointerInputModule={ }
---
---@public
---@param pointerId number 
---@return boolean 
function PointerInputModule:IsPointerOverGameObject(pointerId) end
---
---@public
---@return string 
function PointerInputModule:ToString() end
---
UnityEngine.EventSystems.PointerInputModule = PointerInputModule