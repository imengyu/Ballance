---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class DrivenPropertyManager
local DrivenPropertyManager={ }
---
---@public
---@param driver Object 
---@param target Object 
---@param propertyPath string 
---@return void 
function DrivenPropertyManager.RegisterProperty(driver, target, propertyPath) end
---
---@public
---@param driver Object 
---@param target Object 
---@param propertyPath string 
---@return void 
function DrivenPropertyManager.TryRegisterProperty(driver, target, propertyPath) end
---
---@public
---@param driver Object 
---@param target Object 
---@param propertyPath string 
---@return void 
function DrivenPropertyManager.UnregisterProperty(driver, target, propertyPath) end
---
---@public
---@param driver Object 
---@return void 
function DrivenPropertyManager.UnregisterProperties(driver) end
---
UnityEngine.DrivenPropertyManager = DrivenPropertyManager