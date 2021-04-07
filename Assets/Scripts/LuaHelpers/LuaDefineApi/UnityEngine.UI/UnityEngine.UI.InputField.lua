---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class InputField : Selectable
---@field public shouldHideMobileInput boolean 
---@field public shouldActivateOnSelect boolean 
---@field public text string 
---@field public isFocused boolean 
---@field public caretBlinkRate number 
---@field public caretWidth number 
---@field public textComponent Text 
---@field public placeholder Graphic 
---@field public caretColor Color 
---@field public customCaretColor boolean 
---@field public selectionColor Color 
---@field public onEndEdit SubmitEvent 
---@field public onValueChange OnChangeEvent 
---@field public onValueChanged OnChangeEvent 
---@field public onValidateInput OnValidateInput 
---@field public characterLimit number 
---@field public contentType number 
---@field public lineType number 
---@field public inputType number 
---@field public touchScreenKeyboard TouchScreenKeyboard 
---@field public keyboardType number 
---@field public characterValidation number 
---@field public readOnly boolean 
---@field public multiLine boolean 
---@field public asteriskChar Char 
---@field public wasCanceled boolean 
---@field public caretSelectPosition number 
---@field public caretPosition number 
---@field public selectionAnchorPosition number 
---@field public selectionFocusPosition number 
---@field public minWidth number 
---@field public preferredWidth number 
---@field public flexibleWidth number 
---@field public minHeight number 
---@field public preferredHeight number 
---@field public flexibleHeight number 
---@field public layoutPriority number 
local InputField={ }
---
---@public
---@param input string 
---@return void 
function InputField:SetTextWithoutNotify(input) end
---
---@public
---@param shift boolean 
---@return void 
function InputField:MoveTextEnd(shift) end
---
---@public
---@param shift boolean 
---@return void 
function InputField:MoveTextStart(shift) end
---
---@public
---@param screen Vector2 
---@return Vector2 
function InputField:ScreenToLocal(screen) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function InputField:OnBeginDrag(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function InputField:OnDrag(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function InputField:OnEndDrag(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function InputField:OnPointerDown(eventData) end
---
---@public
---@param e Event 
---@return void 
function InputField:ProcessEvent(e) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function InputField:OnUpdateSelected(eventData) end
---
---@public
---@return void 
function InputField:ForceLabelUpdate() end
---
---@public
---@param update number 
---@return void 
function InputField:Rebuild(update) end
---
---@public
---@return void 
function InputField:LayoutComplete() end
---
---@public
---@return void 
function InputField:GraphicUpdateComplete() end
---
---@public
---@return void 
function InputField:ActivateInputField() end
---
---@public
---@param eventData BaseEventData 
---@return void 
function InputField:OnSelect(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function InputField:OnPointerClick(eventData) end
---
---@public
---@return void 
function InputField:DeactivateInputField() end
---
---@public
---@param eventData BaseEventData 
---@return void 
function InputField:OnDeselect(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function InputField:OnSubmit(eventData) end
---
---@public
---@return void 
function InputField:CalculateLayoutInputHorizontal() end
---
---@public
---@return void 
function InputField:CalculateLayoutInputVertical() end
---
UnityEngine.UI.InputField = InputField