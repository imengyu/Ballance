---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameErrorChecker
---@field public LastError number 上一个操作的错误
local GameErrorChecker={ }
---抛出游戏异常，此操作会直接终止游戏
---@public
---@param code number 
---@param message string 
---@return void 
function GameErrorChecker.ThrowGameError(code, message) end
---上一个操作的错误说明文字
---@public
---@return string 
function GameErrorChecker.GetLastErrorMessage() end
---设置错误码并打印日志
---@public
---@param code number 错误码
---@param tag string TAG
---@param message string 错误信息
---@param param Object[] 日志信息
---@return void 
function GameErrorChecker.SetLastErrorAndLog(code, tag, message, param) end
---设置错误码并打印日志
---@public
---@param code number 错误码
---@param tag string TAG
---@param message string 错误信息
---@return void 
function GameErrorChecker.SetLastErrorAndLog(code, tag, message) end
---错误检查器
Ballance2.Sys.Debug.GameErrorChecker = GameErrorChecker