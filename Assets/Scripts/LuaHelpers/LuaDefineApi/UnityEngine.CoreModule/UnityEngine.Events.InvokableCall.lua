---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class InvokableCall : BaseInvokableCall
local InvokableCall={ }
---
---@public
---@param args Object[] 
---@return void 
function InvokableCall:Invoke(args) end
---
---@public
---@return void 
function InvokableCall:Invoke() end
---
---@public
---@param targetObj Object 
---@param method MethodInfo 
---@return boolean 
function InvokableCall:Find(targetObj, method) end
---
UnityEngine.Events.InvokableCall = InvokableCall