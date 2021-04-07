---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Logger
---@field public logHandler ILogHandler 
---@field public logEnabled boolean 
---@field public filterLogType number 
local Logger={ }
---
---@public
---@param logType number 
---@return boolean 
function Logger:IsLogTypeAllowed(logType) end
---
---@public
---@param logType number 
---@param message Object 
---@return void 
function Logger:Log(logType, message) end
---
---@public
---@param logType number 
---@param message Object 
---@param context Object 
---@return void 
function Logger:Log(logType, message, context) end
---
---@public
---@param logType number 
---@param tag string 
---@param message Object 
---@return void 
function Logger:Log(logType, tag, message) end
---
---@public
---@param logType number 
---@param tag string 
---@param message Object 
---@param context Object 
---@return void 
function Logger:Log(logType, tag, message, context) end
---
---@public
---@param message Object 
---@return void 
function Logger:Log(message) end
---
---@public
---@param tag string 
---@param message Object 
---@return void 
function Logger:Log(tag, message) end
---
---@public
---@param tag string 
---@param message Object 
---@param context Object 
---@return void 
function Logger:Log(tag, message, context) end
---
---@public
---@param tag string 
---@param message Object 
---@return void 
function Logger:LogWarning(tag, message) end
---
---@public
---@param tag string 
---@param message Object 
---@param context Object 
---@return void 
function Logger:LogWarning(tag, message, context) end
---
---@public
---@param tag string 
---@param message Object 
---@return void 
function Logger:LogError(tag, message) end
---
---@public
---@param tag string 
---@param message Object 
---@param context Object 
---@return void 
function Logger:LogError(tag, message, context) end
---
---@public
---@param logType number 
---@param format string 
---@param args Object[] 
---@return void 
function Logger:LogFormat(logType, format, args) end
---
---@public
---@param exception Exception 
---@return void 
function Logger:LogException(exception) end
---
---@public
---@param logType number 
---@param context Object 
---@param format string 
---@param args Object[] 
---@return void 
function Logger:LogFormat(logType, context, format, args) end
---
---@public
---@param exception Exception 
---@param context Object 
---@return void 
function Logger:LogException(exception, context) end
---
UnityEngine.Logger = Logger