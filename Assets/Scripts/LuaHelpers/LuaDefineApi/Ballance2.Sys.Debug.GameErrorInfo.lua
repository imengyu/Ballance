---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameErrorInfo
local GameErrorInfo={ }
---获取错误代码的说明信息
---@public
---@param err number 错误代码
---@return string 
function GameErrorInfo.GetErrorMessage(err) end
---错误信息
Ballance2.Sys.Debug.GameErrorInfo = GameErrorInfo