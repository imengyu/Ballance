---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Scrollbar : Selectable
---@field public handleRect RectTransform 
---@field public direction number 
---@field public value number 
---@field public size number 
---@field public numberOfSteps number 
---@field public onValueChanged ScrollEvent 
local Scrollbar={ }
---
---@public
---@param input number 
---@return void 
function Scrollbar:SetValueWithoutNotify(input) end
---
---@public
---@param executing number 
---@return void 
function Scrollbar:Rebuild(executing) end
---
---@public
---@return void 
function Scrollbar:LayoutComplete() end
---
---@public
---@return void 
function Scrollbar:GraphicUpdateComplete() end
---
---@public
---@param eventData PointerEventData 
---@return void 
function Scrollbar:OnBeginDrag(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function Scrollbar:OnDrag(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function Scrollbar:OnPointerDown(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function Scrollbar:OnPointerUp(eventData) end
---
---@public
---@param eventData AxisEventData 
---@return void 
function Scrollbar:OnMove(eventData) end
---
---@public
---@return Selectable 
function Scrollbar:FindSelectableOnLeft() end
---
---@public
---@return Selectable 
function Scrollbar:FindSelectableOnRight() end
---
---@public
---@return Selectable 
function Scrollbar:FindSelectableOnUp() end
---
---@public
---@return Selectable 
function Scrollbar:FindSelectableOnDown() end
---
---@public
---@param eventData PointerEventData 
---@return void 
function Scrollbar:OnInitializePotentialDrag(eventData) end
---
---@public
---@param direction number 
---@param includeRectLayouts boolean 
---@return void 
function Scrollbar:SetDirection(direction, includeRectLayouts) end
---
UnityEngine.UI.Scrollbar = Scrollbar