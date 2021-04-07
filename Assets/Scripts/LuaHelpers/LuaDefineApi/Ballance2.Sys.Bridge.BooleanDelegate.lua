---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BooleanDelegate
local BooleanDelegate={ }
---
---@public
---@return boolean 
function BooleanDelegate:Invoke() end
---
---@public
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function BooleanDelegate:BeginInvoke(callback, object) end
---
---@public
---@param result IAsyncResult 
---@return boolean 
function BooleanDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.BooleanDelegate = BooleanDelegate