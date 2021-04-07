---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Button : Selectable
---@field public onClick ButtonClickedEvent 
local Button={ }
---
---@public
---@param eventData PointerEventData 
---@return void 
function Button:OnPointerClick(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function Button:OnSubmit(eventData) end
---
UnityEngine.UI.Button = Button