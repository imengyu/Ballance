---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class DebugUtils
local DebugUtils={ }
---获取当前调用堆栈
---@public
---@param skipFrame number 要跳过的帧，为0不跳过
---@return string 
function DebugUtils.GetStackTrace(skipFrame) end
---打印出格式化过的 byte 数组，以十六进制显示
---@public
---@param vs Byte[] byte 数组
---@return string 
function DebugUtils.PrintBytes(vs) end
---打印出带行号的代码
---@public
---@param code string 代码字符串
---@return string 
function DebugUtils.PrintCodeWithLine(code) end
---调试工具类
Ballance2.Utils.DebugUtils = DebugUtils