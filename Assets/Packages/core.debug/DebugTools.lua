Log = Ballance2.Utils.Log
LogLevel = Ballance2.Utils.LogLevel

---@type GameLuaObjectHostClass
DebugTools = {
  logObserver = nil,
  logFastItems = {},
  ---@type GameObject
  DebugLogText = nil,
  ---@type Text
  textDebugLogText = nil,
}

function class_DebugTools()
  function DebugTools:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param self table
  ---@param thisGameObject GameObject
  function DebugTools:Start(thisGameObject)
    self.textDebugLogText = self.DebugLogText:GetComponent('Text')
    -- 注册日志观察者
    self.logObserver =
      Log.RegisterLogObserver(
      function(level, tag, message, stackTrace)
        if (#self.logFastItems > 1) then
          table.remove(self.logFastItems, 1)
        end

        local logColor = GetLogColor(level)
        local t = {'<color=#', logColor, '>', Log.LogLevelToString(level), '/', tag, ' ', message, '</color>'}
        self.logFastItems[8] = table.concat(t)
        self:FlushLogFast()
      end,
      LogLevel.All
    )
    self.textDebugLogText.text = ''
  end
  function DebugTools:OnDestroy()
    Log.UnRegisterLogObserver(self.logObserver)
    self.logFastItems = {}
  end

  ---刷新日志预览
  function DebugTools:FlushLogFast()
    local str = table.concat(self.logFastItems, '\n')
    self.textDebugLogText.text = str
  end


  return DebugTools:new(nil)
end
