---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ToggleGroup : UIBehaviour
---@field public allowSwitchOff boolean 
local ToggleGroup={ }
---
---@public
---@param toggle Toggle 
---@param sendCallback boolean 
---@return void 
function ToggleGroup:NotifyToggleOn(toggle, sendCallback) end
---
---@public
---@param toggle Toggle 
---@return void 
function ToggleGroup:UnregisterToggle(toggle) end
---
---@public
---@param toggle Toggle 
---@return void 
function ToggleGroup:RegisterToggle(toggle) end
---
---@public
---@return void 
function ToggleGroup:EnsureValidState() end
---
---@public
---@return boolean 
function ToggleGroup:AnyTogglesOn() end
---
---@public
---@return IEnumerable`1 
function ToggleGroup:ActiveToggles() end
---
---@public
---@return Toggle 
function ToggleGroup:GetFirstActiveToggle() end
---
---@public
---@param sendCallback boolean 
---@return void 
function ToggleGroup:SetAllTogglesOff(sendCallback) end
---
UnityEngine.UI.ToggleGroup = ToggleGroup