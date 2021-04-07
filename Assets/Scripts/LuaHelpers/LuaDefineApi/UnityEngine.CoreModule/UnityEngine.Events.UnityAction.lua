---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class UnityAction : MulticastDelegate
local UnityAction={ }
---
---@public
---@return void 
function UnityAction:Invoke() end
---
---@public
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function UnityAction:BeginInvoke(callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function UnityAction:EndInvoke(result) end
---
UnityEngine.Events.UnityAction = UnityAction