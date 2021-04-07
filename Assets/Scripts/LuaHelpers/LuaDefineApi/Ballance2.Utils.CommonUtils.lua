---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CommonUtils
local CommonUtils={ }
---生成随机ID
---@public
---@return number 
function CommonUtils.GenRandomID() end
---生成自增长ID
---@public
---@return number 
function CommonUtils.GenAutoIncrementID() end
---生成不重复ID
---@public
---@return number 
function CommonUtils.GenNonDuplicateID() end
---检查数组是否为空
---@public
---@param arr Object[] 
---@return boolean 
function CommonUtils.IsArrayNullOrEmpty(arr) end
---检查 Dictionary 是否为空
---@public
---@param arr IDictionary 
---@return boolean 
function CommonUtils.IsDictionaryNullOrEmpty(arr) end
---生成相同的字符串数组
---@public
---@param val string 字符串
---@param count number 长度
---@return String[] 
function CommonUtils.GenSameStringArray(val, count) end
---检查可变参数
---@public
---@param param Object[] 可变参数数组
---@param index number 要检查的参数索引
---@param typeName string 目标类型
---@return boolean 
function CommonUtils.CheckParam(param, index, typeName) end
---
---@public
---@param keyValuePairs Dictionary`2 
---@return String[] 
function CommonUtils.GetStringArrayFromDictionary(keyValuePairs) end
---通用帮助类
Ballance2.Utils.CommonUtils = CommonUtils