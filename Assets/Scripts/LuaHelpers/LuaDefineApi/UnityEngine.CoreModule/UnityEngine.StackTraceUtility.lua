---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class StackTraceUtility
local StackTraceUtility={ }
---
---@public
---@return string 
function StackTraceUtility.ExtractStackTrace() end
---
---@public
---@param exception Object 
---@return string 
function StackTraceUtility.ExtractStringFromException(exception) end
---
UnityEngine.StackTraceUtility = StackTraceUtility