---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PersistentCall
---@field public target Object 
---@field public targetAssemblyTypeName string 
---@field public methodName string 
---@field public mode number 
---@field public arguments ArgumentCache 
---@field public callState number 
local PersistentCall={ }
---
---@public
---@return boolean 
function PersistentCall:IsValid() end
---
---@public
---@param theEvent UnityEventBase 
---@return BaseInvokableCall 
function PersistentCall:GetRuntimeCall(theEvent) end
---
---@public
---@param ttarget Object 
---@param targetType Type 
---@param mmethodName string 
---@return void 
function PersistentCall:RegisterPersistentListener(ttarget, targetType, mmethodName) end
---
---@public
---@return void 
function PersistentCall:UnregisterPersistentListener() end
---
---@public
---@return void 
function PersistentCall:OnBeforeSerialize() end
---
---@public
---@return void 
function PersistentCall:OnAfterDeserialize() end
---
UnityEngine.Events.PersistentCall = PersistentCall