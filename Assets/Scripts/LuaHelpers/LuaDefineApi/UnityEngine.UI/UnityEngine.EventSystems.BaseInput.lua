---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BaseInput : UIBehaviour
---@field public compositionString string 
---@field public imeCompositionMode number 
---@field public compositionCursorPos Vector2 
---@field public mousePresent boolean 
---@field public mousePosition Vector2 
---@field public mouseScrollDelta Vector2 
---@field public touchSupported boolean 
---@field public touchCount number 
local BaseInput={ }
---
---@public
---@param button number 
---@return boolean 
function BaseInput:GetMouseButtonDown(button) end
---
---@public
---@param button number 
---@return boolean 
function BaseInput:GetMouseButtonUp(button) end
---
---@public
---@param button number 
---@return boolean 
function BaseInput:GetMouseButton(button) end
---
---@public
---@param index number 
---@return Touch 
function BaseInput:GetTouch(index) end
---
---@public
---@param axisName string 
---@return number 
function BaseInput:GetAxisRaw(axisName) end
---
---@public
---@param buttonName string 
---@return boolean 
function BaseInput:GetButtonDown(buttonName) end
---
UnityEngine.EventSystems.BaseInput = BaseInput