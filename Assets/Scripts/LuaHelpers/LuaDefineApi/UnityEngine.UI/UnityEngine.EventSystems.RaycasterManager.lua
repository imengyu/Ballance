---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RaycasterManager
local RaycasterManager={ }
---
---@public
---@param baseRaycaster BaseRaycaster 
---@return void 
function RaycasterManager.AddRaycaster(baseRaycaster) end
---
---@public
---@return List`1 
function RaycasterManager.GetRaycasters() end
---
---@public
---@param baseRaycaster BaseRaycaster 
---@return void 
function RaycasterManager.RemoveRaycasters(baseRaycaster) end
---
UnityEngine.EventSystems.RaycasterManager = RaycasterManager