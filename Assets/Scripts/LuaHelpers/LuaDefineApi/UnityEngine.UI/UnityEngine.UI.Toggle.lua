---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Toggle : Selectable
---@field public toggleTransition number 
---@field public graphic Graphic 
---@field public onValueChanged ToggleEvent 
---@field public group ToggleGroup 
---@field public isOn boolean 
local Toggle={ }
---
---@public
---@param executing number 
---@return void 
function Toggle:Rebuild(executing) end
---
---@public
---@return void 
function Toggle:LayoutComplete() end
---
---@public
---@return void 
function Toggle:GraphicUpdateComplete() end
---
---@public
---@param value boolean 
---@return void 
function Toggle:SetIsOnWithoutNotify(value) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function Toggle:OnPointerClick(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function Toggle:OnSubmit(eventData) end
---
UnityEngine.UI.Toggle = Toggle