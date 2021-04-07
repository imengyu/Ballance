---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LogObserver
local LogObserver={ }
---
---@public
---@param level number 
---@param tag string 
---@param message string 
---@param stackTrace string 
---@return void 
function LogObserver:Invoke(level, tag, message, stackTrace) end
---
---@public
---@param level number 
---@param tag string 
---@param message string 
---@param stackTrace string 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LogObserver:BeginInvoke(level, tag, message, stackTrace, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LogObserver:EndInvoke(result) end
---
Ballance2.Utils.LogObserver = LogObserver