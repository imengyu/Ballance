---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Event
---@field public rawType number 
---@field public mousePosition Vector2 
---@field public delta Vector2 
---@field public pointerType number 
---@field public button number 
---@field public modifiers number 
---@field public pressure number 
---@field public clickCount number 
---@field public character Char 
---@field public keyCode number 
---@field public displayIndex number 
---@field public type number 
---@field public commandName string 
---@field public mouseRay Ray 
---@field public shift boolean 
---@field public control boolean 
---@field public alt boolean 
---@field public command boolean 
---@field public capsLock boolean 
---@field public numeric boolean 
---@field public functionKey boolean 
---@field public current Event 
---@field public isKey boolean 
---@field public isMouse boolean 
---@field public isScrollWheel boolean 
local Event={ }
---
---@public
---@param controlID number 
---@return number 
function Event:GetTypeForControl(controlID) end
---
---@public
---@param outEvent Event 
---@return boolean 
function Event.PopEvent(outEvent) end
---
---@public
---@return number 
function Event.GetEventCount() end
---
---@public
---@param key string 
---@return Event 
function Event.KeyboardEvent(key) end
---
---@public
---@return number 
function Event:GetHashCode() end
---
---@public
---@param obj Object 
---@return boolean 
function Event:Equals(obj) end
---
---@public
---@return string 
function Event:ToString() end
---
---@public
---@return void 
function Event:Use() end
---
UnityEngine.Event = Event