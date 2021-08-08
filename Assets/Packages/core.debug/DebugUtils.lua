--[[
Copyright(c) 2021  mengyu
* 模块名：     
DebugUtils.lua
* 用途：
日志信息
* 作者：
mengyu
* 

]]--

local LogLevel = Ballance2.Utils.LogLevel

---获取日志颜色
---@param level LogLevel
---@return string
function GetLogColor(level)
  if level == LogLevel.Info then
    return "67CCFF"
  elseif level == LogLevel.Verbose then
    return "FFFFFF"
  elseif level == LogLevel.Warning then
    return "FFCE00"
  elseif level == LogLevel.Error then
    return "FF1B00"
  else
    return "CFCFCF"
  end
end

