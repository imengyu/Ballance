---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class StandaloneInputModule : PointerInputModule
---@field public inputMode number 
---@field public allowActivationOnMobileDevice boolean 
---@field public forceModuleActive boolean 
---@field public inputActionsPerSecond number 
---@field public repeatDelay number 
---@field public horizontalAxis string 
---@field public verticalAxis string 
---@field public submitButton string 
---@field public cancelButton string 
local StandaloneInputModule={ }
---
---@public
---@return void 
function StandaloneInputModule:UpdateModule() end
---
---@public
---@return boolean 
function StandaloneInputModule:IsModuleSupported() end
---
---@public
---@return boolean 
function StandaloneInputModule:ShouldActivateModule() end
---
---@public
---@return void 
function StandaloneInputModule:ActivateModule() end
---
---@public
---@return void 
function StandaloneInputModule:DeactivateModule() end
---
---@public
---@return void 
function StandaloneInputModule:Process() end
---
UnityEngine.EventSystems.StandaloneInputModule = StandaloneInputModule