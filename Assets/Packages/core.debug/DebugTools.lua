
Log = Ballance2.Utils.Log
LogLevel = Ballance2.Utils.LogLevel
KeyListener = Ballance2.Sys.Utils.KeyListener
KeyCode = UnityEngine.KeyCode

---@type GameLuaObjectHostClass
DebugTools = {
  
  DebugLogText = nil,---@type Text
  DebugErrText = nil,---@type Text
  DebugWarnText = nil,---@type Text

  logObserver = nil,
  logFastItems = {},
  logCount = 0,
  logWarnCount = 0,
  logErrorCount = 0,
}

function CreateClass_DebugTools()
  function DebugTools:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param self table
  ---@param thisGameObject GameObject
  function DebugTools:Start(thisGameObject)
    
    -- 注册日志观察者
    self.logObserver =
      Log.RegisterLogObserver(
      function(level, tag, message, stackTrace)
        if (self.logCount >= 8) then
          table.remove(self.logFastItems, 1)
          self.logCount = self.logCount - 1
        end

        local logColor = GetLogColor(level)
        local t = {'<color=#', logColor, '>', Log.LogLevelToString(level), '/', tag, ' ', self:SubstractMessage(message), '</color>'}
        
        self.logCount = self.logCount + 1
        self.logFastItems[self.logCount] = table.concat(t)
        self:FlushLogFast()

        if level == LogLevel.Warning then
          self.logWarnCount = self.logWarnCount + 1
          self:FlushLogCount()
        elseif level == LogLevel.Error then
          self.logErrorCount = self.logErrorCount + 1
          self:FlushLogCount()
        end
      end,
      LogLevel.All
    )
    self.DebugLogText.text = ''

    --游戏退出事件
    GameManager.GameMediator:RegisterEventHandler(GamePackage.GetSystemPackage(), GameEventNames.EVENT_BEFORE_GAME_QUIT, 'DebugWindow', function ()
      Log.UnRegisterLogObserver(self.logObserver)
      return false
    end)

    --注册控制台按键事件
    local k = KeyListener.Get(self.gameObject)
    k:AddKeyListen(KeyCode.F12, function (key, downed)
      if(not downed) then
        if(GlobalDebugWindow:GetVisible()) then GlobalDebugWindow:Hide() 
        else GlobalDebugWindow:Show() end
      end
    end)

  end
  function DebugTools:OnDestroy()
    Log.UnRegisterLogObserver(self.logObserver)
    self.logFastItems = {}
  end

  ---刷新日志预览
  function DebugTools:FlushLogFast()
    local str = table.concat(self.logFastItems, '\n')
    self.DebugLogText.text = str
  end
  ---刷新日志数量
  function DebugTools:FlushLogCount() 
    self.DebugWarnText.text = self.logWarnCount
    self.DebugErrText.text = self.logErrorCount
  end
  ---清空
  function DebugTools:Clear() 
    self.logWarnCount = 0
    self.logErrorCount = 0
    self.logFastItems = {}
    self.DebugLogText.text = ''
    self:FlushLogCount()
  end
  ---日志文字提取
  ---@param message string
  function DebugTools:SubstractMessage(message)
    local brPos = string.find(message, '\n')
    local messageSb = ''
    if(brPos and brPos > 0) then
      messageSb = string.sub(message, 0, brPos - 1)
    else
      messageSb = message
    end
    if(#messageSb > 256) then
      messageSb = string.sub(messageSb, 0, 255)
    end
    return messageSb
  end


  return DebugTools:new(nil)
end
