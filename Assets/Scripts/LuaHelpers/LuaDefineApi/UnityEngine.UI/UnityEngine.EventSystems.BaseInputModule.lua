---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BaseInputModule : UIBehaviour
---@field public input BaseInput 
---@field public inputOverride BaseInput 
local BaseInputModule={ }
---
---@public
---@return void 
function BaseInputModule:Process() end
---
---@public
---@param pointerId number 
---@return boolean 
function BaseInputModule:IsPointerOverGameObject(pointerId) end
---
---@public
---@return boolean 
function BaseInputModule:ShouldActivateModule() end
---
---@public
---@return void 
function BaseInputModule:DeactivateModule() end
---
---@public
---@return void 
function BaseInputModule:ActivateModule() end
---
---@public
---@return void 
function BaseInputModule:UpdateModule() end
---
---@public
---@return boolean 
function BaseInputModule:IsModuleSupported() end
---
UnityEngine.EventSystems.BaseInputModule = BaseInputModule