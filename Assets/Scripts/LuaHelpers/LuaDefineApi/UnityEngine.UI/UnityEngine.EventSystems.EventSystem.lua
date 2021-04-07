---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class EventSystem : UIBehaviour
---@field public current EventSystem 
---@field public sendNavigationEvents boolean 
---@field public pixelDragThreshold number 
---@field public currentInputModule BaseInputModule 
---@field public firstSelectedGameObject GameObject 
---@field public currentSelectedGameObject GameObject 
---@field public lastSelectedGameObject GameObject 
---@field public isFocused boolean 
---@field public alreadySelecting boolean 
local EventSystem={ }
---
---@public
---@return void 
function EventSystem:UpdateModules() end
---
---@public
---@param selected GameObject 
---@param pointer BaseEventData 
---@return void 
function EventSystem:SetSelectedGameObject(selected, pointer) end
---
---@public
---@param selected GameObject 
---@return void 
function EventSystem:SetSelectedGameObject(selected) end
---
---@public
---@param eventData PointerEventData 
---@param raycastResults List`1 
---@return void 
function EventSystem:RaycastAll(eventData, raycastResults) end
---
---@public
---@return boolean 
function EventSystem:IsPointerOverGameObject() end
---
---@public
---@param pointerId number 
---@return boolean 
function EventSystem:IsPointerOverGameObject(pointerId) end
---
---@public
---@return string 
function EventSystem:ToString() end
---
UnityEngine.EventSystems.EventSystem = EventSystem