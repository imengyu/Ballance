
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

