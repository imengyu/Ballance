---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class VoidDelegate
local VoidDelegate={ }
---
---@public
---@return void 
function VoidDelegate:Invoke() end
---
---@public
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function VoidDelegate:BeginInvoke(callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function VoidDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.VoidDelegate = VoidDelegate