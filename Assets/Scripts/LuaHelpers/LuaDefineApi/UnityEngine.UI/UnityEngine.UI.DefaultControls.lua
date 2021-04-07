---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class DefaultControls
---@field public factory IFactoryControls 
local DefaultControls={ }
---
---@public
---@param resources Resources 
---@return GameObject 
function DefaultControls.CreatePanel(resources) end
---
---@public
---@param resources Resources 
---@return GameObject 
function DefaultControls.CreateButton(resources) end
---
---@public
---@param resources Resources 
---@return GameObject 
function DefaultControls.CreateText(resources) end
---
---@public
---@param resources Resources 
---@return GameObject 
function DefaultControls.CreateImage(resources) end
---
---@public
---@param resources Resources 
---@return GameObject 
function DefaultControls.CreateRawImage(resources) end
---
---@public
---@param resources Resources 
---@return GameObject 
function DefaultControls.CreateSlider(resources) end
---
---@public
---@param resources Resources 
---@return GameObject 
function DefaultControls.CreateScrollbar(resources) end
---
---@public
---@param resources Resources 
---@return GameObject 
function DefaultControls.CreateToggle(resources) end
---
---@public
---@param resources Resources 
---@return GameObject 
function DefaultControls.CreateInputField(resources) end
---
---@public
---@param resources Resources 
---@return GameObject 
function DefaultControls.CreateDropdown(resources) end
---
---@public
---@param resources Resources 
---@return GameObject 
function DefaultControls.CreateScrollView(resources) end
---
UnityEngine.UI.DefaultControls = DefaultControls