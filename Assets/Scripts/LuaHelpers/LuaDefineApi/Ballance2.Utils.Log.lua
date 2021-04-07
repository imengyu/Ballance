---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Log
local Log={ }
---
---@public
---@param tag string 
---@param message string 
---@return void 
function Log.V(tag, message) end
---
---@public
---@param tag string 
---@param format string 
---@param param Object[] 
---@return void 
function Log.V(tag, format, param) end
---
---@public
---@param tag string 
---@param message string 
---@return void 
function Log.D(tag, message) end
---
---@public
---@param tag string 
---@param format string 
---@param param Object[] 
---@return void 
function Log.D(tag, format, param) end
---
---@public
---@param tag string 
---@param message string 
---@return void 
function Log.I(tag, message) end
---
---@public
---@param tag string 
---@param format string 
---@param param Object[] 
---@return void 
function Log.I(tag, format, param) end
---
---@public
---@param tag string 
---@param message string 
---@return void 
function Log.W(tag, message) end
---
---@public
---@param tag string 
---@param format string 
---@param param Object[] 
---@return void 
function Log.W(tag, format, param) end
---
---@public
---@param tag string 
---@param message string 
---@return void 
function Log.E(tag, message) end
---
---@public
---@param tag string 
---@param format string 
---@param param Object[] 
---@return void 
function Log.E(tag, format, param) end
---手动写入日志
---@public
---@param level number 日志等级
---@param tag string 标签
---@param message string 信息
---@param stackTrace string 堆栈信息
---@return void 
function Log.LogWrite(level, tag, message, stackTrace) end
---注册日志观察者
---@public
---@param observer LogObserver 
---@param acceptLevel number 
---@return number 返回大于0的数字表示观察者ID，返回-1表示错误
function Log.RegisterLogObserver(observer, acceptLevel) end
---取消注册日志观察者
---@public
---@param id number 观察者ID（由 RegisterLogObserver 返回）
---@return void 
function Log.UnRegisterLogObserver(id) end
---获取日志观察者
---@public
---@param id number 观察者ID（由 RegisterLogObserver 返回）
---@return LogObserver 如果找到则返回观察者，如果找不到则返回null
function Log.GetLogObserver(id) end
---
---@public
---@param logLevel number 
---@return string 
function Log.LogLevelToString(logLevel) end
---日志器
Ballance2.Utils.Log = Log