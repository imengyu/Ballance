---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BaseInvokableCall
local BaseInvokableCall={ }
---
---@public
---@param args Object[] 
---@return void 
function BaseInvokableCall:Invoke(args) end
---
---@public
---@param targetObj Object 
---@param method MethodInfo 
---@return boolean 
function BaseInvokableCall:Find(targetObj, method) end
---
UnityEngine.Events.BaseInvokableCall = BaseInvokableCall