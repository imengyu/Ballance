---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class StringUtils
local StringUtils={ }
---字符串是否为空
---@public
---@param text string 字符串
---@return boolean 
function StringUtils.isNullOrEmpty(text) end
---字符串是否为空或空白
---@public
---@param text string 字符串
---@return boolean 
function StringUtils.IsNullOrWhiteSpace(text) end
---字符串是否是URL
---@public
---@param text string 字符串
---@return boolean 
function StringUtils.IsUrl(text) end
---字符串是否是整数
---@public
---@param text string 字符串
---@return boolean 
function StringUtils.IsNumber(text) end
---字符串是否是浮点数
---@public
---@param text string 字符串
---@return boolean 
function StringUtils.IsFloatNumber(text) end
---字符串是否是包名
---@public
---@param text string 字符串
---@return boolean 
function StringUtils.IsPackageName(text) end
---比较两个版本先后，1 小于 2 返回 -1 ，大于返回 1，等于返回 0
---@public
---@param version1 string 版本1
---@param version2 string 版本2
---@return number 
function StringUtils.CompareTwoVersion(version1, version2) end
---替换字符串的 <br> 转为换行符
---@public
---@param str string 字符串
---@return string 
function StringUtils.ReplaceBrToLine(str) end
---颜色字符串转为 Color
---@public
---@param color string 颜色字符串
---@return Color 
function StringUtils.StringToColor(color) end
---尝试把字符串数组转为参数数组
---@public
---@param arr String[] 字符串数组
---@return Object[] 
function StringUtils.TryConvertStringArrayToValueArray(arr) end
---尝试把参数数组数组转为字符串
---@public
---@param arr Object[] 参数数组
---@return string 
function StringUtils.ValueArrayToString(arr) end
---比较Bytes
---@public
---@param inV Byte[] bytes数组1
---@param outV Byte[] bytes数组2
---@return boolean 返回两个Bytes是否相等
function StringUtils.TestBytesMatch(inV, outV) end
---
---@public
---@param buffer Byte[] 
---@return string 
function StringUtils.FixUtf8BOM(buffer) end
---
Ballance2.Utils.StringUtils = StringUtils