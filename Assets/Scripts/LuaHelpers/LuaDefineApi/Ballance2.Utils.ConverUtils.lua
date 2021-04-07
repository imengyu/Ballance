---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ConverUtils
local ConverUtils={ }
---字符串转为数字
---@public
---@param stringValue string 要转换的字符串
---@param defaultValue number 默认值
---@param paramName string 参数名称（用于错误日志）
---@return number 转换成功的数字，如果输入字符串无法转为数字，则返回默认值
function ConverUtils.StringToInt(stringValue, defaultValue, paramName) end
---字符串转为长整型数字
---@public
---@param stringValue string 要转换的字符串
---@param defaultValue number 默认值
---@param paramName string 参数名称（用于错误日志）
---@return number 转换成功的长整型数字，如果输入字符串无法转为长整型数字，则返回默认值
function ConverUtils.StringToLong(stringValue, defaultValue, paramName) end
---字符串转为布尔类型
---@public
---@param stringValue string 要转换的字符串
---@param defaultValue boolean 默认值
---@param paramName string 参数名称（用于错误日志）
---@return boolean 转换成功的布尔类型，如果输入字符串无法转为布尔类型，则返回默认值
function ConverUtils.StringToBoolean(stringValue, defaultValue, paramName) end
---字符串转为浮点数
---@public
---@param stringValue string 要转换的字符串
---@param defaultValue number 默认值
---@param paramName string 参数名称（用于错误日志）
---@return number 转换成功的浮点数，如果输入字符串无法转为浮点数，则返回默认值
function ConverUtils.StringToFloat(stringValue, defaultValue, paramName) end
---字符串转换类
Ballance2.Utils.ConverUtils = ConverUtils