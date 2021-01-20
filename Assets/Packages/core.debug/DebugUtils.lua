

function getLogColor(logLevel)
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

