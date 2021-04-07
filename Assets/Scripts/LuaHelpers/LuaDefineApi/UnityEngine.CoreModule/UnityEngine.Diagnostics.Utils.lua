---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Utils
local Utils={ }
---
---@public
---@param crashCategory number 
---@return void 
function Utils.ForceCrash(crashCategory) end
---
---@public
---@param message string 
---@return void 
function Utils.NativeAssert(message) end
---
---@public
---@param message string 
---@return void 
function Utils.NativeError(message) end
---
---@public
---@param message string 
---@return void 
function Utils.NativeWarning(message) end
---
UnityEngine.Diagnostics.Utils = Utils