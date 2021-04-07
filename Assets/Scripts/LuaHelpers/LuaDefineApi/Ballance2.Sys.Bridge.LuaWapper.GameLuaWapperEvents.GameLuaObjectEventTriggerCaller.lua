---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameLuaObjectEventTriggerCaller
local GameLuaObjectEventTriggerCaller={ }
---
---@public
---@return number 
function GameLuaObjectEventTriggerCaller:GetEventType() end
---
---@public
---@return String[] 
function GameLuaObjectEventTriggerCaller:GetSupportEvents() end
---
---@public
---@param host GameLuaObjectHost 
---@return void 
function GameLuaObjectEventTriggerCaller:OnInitLua(host) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnBeginDrag(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnCancel(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnDeselect(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnDrag(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnDrop(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnEndDrag(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnInitializePotentialDrag(eventData) end
---
---@public
---@param eventData AxisEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnMove(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnPointerClick(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnPointerDown(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnPointerEnter(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnPointerExit(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnPointerUp(eventData) end
---
---@public
---@param eventData PointerEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnScroll(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnSelect(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnSubmit(eventData) end
---
---@public
---@param eventData BaseEventData 
---@return void 
function GameLuaObjectEventTriggerCaller:OnUpdateSelected(eventData) end
---
Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjectEventTriggerCaller = GameLuaObjectEventTriggerCaller