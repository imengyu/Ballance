Log = Ballance2.Utils.Log
LogLevel = Ballance2.Utils.LogLevel

DebugTools = {
  logObserver = nil,
  logFastItems = {}
}

function class_DebugTools()

  function DebugTools:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  function DebugTools:Start(thisGameObject)
    -- 注册日志观察者
    self.logObserver = Log.RegisterLogObserver(self.LogObserver, LogLevel.All)
    self.DebugLogText.text = ""
  end
  function DebugTools:OnDestroy()
    Log.UnRegisterLogObserver(self.logObserver)
    self.logFastItems = {}
  end
  --日志观察者
  function DebugTools:LogObserver(level, tag, message, stackTrace)
    if (#self.logFastItems > 1) then
      table.remove(self.logFastItems, 1)
    end

    local logColor = getLogColor(level)
    local t = { "<color=#", logColor, ">", Log.LogLevelToString(level), "/", tag, " ", message, "</color>" }
    self.logFastItems[8] = {
      level = level,
      str = table.concat(t)
    }
    self:FlushLogFast()
  end
  --刷新日志预览
  function DebugTools:FlushLogFast()
    local str = table.concat(self.logFastItems, "\n")
    self.DebugLogText.text = str
  end

  return DebugTools:new(nil)
end
