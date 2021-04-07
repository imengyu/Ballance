---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class UnityEventBase
local UnityEventBase={ }
---
---@public
---@return number 
function UnityEventBase:GetPersistentEventCount() end
---
---@public
---@param index number 
---@return Object 
function UnityEventBase:GetPersistentTarget(index) end
---
---@public
---@param index number 
---@return string 
function UnityEventBase:GetPersistentMethodName(index) end
---
---@public
---@param index number 
---@param state number 
---@return void 
function UnityEventBase:SetPersistentListenerState(index, state) end
---
---@public
---@return void 
function UnityEventBase:RemoveAllListeners() end
---
---@public
---@return string 
function UnityEventBase:ToString() end
---
---@public
---@param obj Object 
---@param functionName string 
---@param argumentTypes Type[] 
---@return MethodInfo 
function UnityEventBase.GetValidMethodInfo(obj, functionName, argumentTypes) end
---
---@public
---@param objectType Type 
---@param functionName string 
---@param argumentTypes Type[] 
---@return MethodInfo 
function UnityEventBase.GetValidMethodInfo(objectType, functionName, argumentTypes) end
---
UnityEngine.Events.UnityEventBase = UnityEventBase