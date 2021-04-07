---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ILogger
---@field public logHandler ILogHandler 
---@field public logEnabled boolean 
---@field public filterLogType number 
local ILogger={ }
---
---@public
---@param logType number 
---@return boolean 
function ILogger:IsLogTypeAllowed(logType) end
---
---@public
---@param logType number 
---@param message Object 
---@return void 
function ILogger:Log(logType, message) end
---
---@public
---@param logType number 
---@param message Object 
---@param context Object 
---@return void 
function ILogger:Log(logType, message, context) end
---
---@public
---@param logType number 
---@param tag string 
---@param message Object 
---@return void 
function ILogger:Log(logType, tag, message) end
---
---@public
---@param logType number 
---@param tag string 
---@param message Object 
---@param context Object 
---@return void 
function ILogger:Log(logType, tag, message, context) end
---
---@public
---@param message Object 
---@return void 
function ILogger:Log(message) end
---
---@public
---@param tag string 
---@param message Object 
---@return void 
function ILogger:Log(tag, message) end
---
---@public
---@param tag string 
---@param message Object 
---@param context Object 
---@return void 
function ILogger:Log(tag, message, context) end
---
---@public
---@param tag string 
---@param message Object 
---@return void 
function ILogger:LogWarning(tag, message) end
---
---@public
---@param tag string 
---@param message Object 
---@param context Object 
---@return void 
function ILogger:LogWarning(tag, message, context) end
---
---@public
---@param tag string 
---@param message Object 
---@return void 
function ILogger:LogError(tag, message) end
---
---@public
---@param tag string 
---@param message Object 
---@param context Object 
---@return void 
function ILogger:LogError(tag, message, context) end
---
---@public
---@param logType number 
---@param format string 
---@param args Object[] 
---@return void 
function ILogger:LogFormat(logType, format, args) end
---
---@public
---@param exception Exception 
---@return void 
function ILogger:LogException(exception) end
---
UnityEngine.ILogger = ILogger