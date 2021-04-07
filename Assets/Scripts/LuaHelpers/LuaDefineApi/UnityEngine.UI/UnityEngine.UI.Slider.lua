---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Slider : Selectable
---@field public fillRect RectTransform 
---@field public handleRect RectTransform 
---@field public direction number 
---@field public minValue number 
---@field public maxValue number 
---@field public wholeNumbers boolean 
---@field public value number 
---@field public normalizedValue number 
---@field public onValueChanged SliderEvent 
local Slider={ }
---
---@public
---@param input number 
---@return void 
function Slider:SetValueWithoutNotify(input) end
---
---@public
---@param executing number 
---@return void 
function Slider:Rebuild(executing) end
---
---@public
---@return void 
function Slider:LayoutComplete() end
---
---@public
---@return void 
function Slider:GraphicUpdateComplete() end
---
---@public
---@param eventData PointerEventData 
---@return void 
function Slider:OnPointerDown(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function Slider:OnDrag(eventData) end
---
---@public
---@param eventData AxisEventData 
---@return void 
function Slider:OnMove(eventData) end
---
---@public
---@return Selectable 
function Slider:FindSelectableOnLeft() end
---
---@public
---@return Selectable 
function Slider:FindSelectableOnRight() end
---
---@public
---@return Selectable 
function Slider:FindSelectableOnUp() end
---
---@public
---@return Selectable 
function Slider:FindSelectableOnDown() end
---
---@public
---@param eventData PointerEventData 
---@return void 
function Slider:OnInitializePotentialDrag(eventData) end
---
---@public
---@param direction number 
---@param includeRectLayouts boolean 
---@return void 
function Slider:SetDirection(direction, includeRectLayouts) end
---
UnityEngine.UI.Slider = Slider