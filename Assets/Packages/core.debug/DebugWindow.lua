Vector2 = UnityEngine.Vector2
InputField = UnityEngine.UI.InputField
Text = UnityEngine.UI.Text
LayoutRebuilder = UnityEngine.UI.LayoutRebuilder
RectTransform = UnityEngine.RectTransform
Log = Ballance2.Utils.Log
LogLevel = Ballance2.Utils.LogLevel
CloneUtils = Ballance2.Sys.Utils.CloneUtils
UIAnchorPosUtils = Ballance2.Sys.UI.Utils.UIAnchorPosUtils
UIAnchor = Ballance2.Sys.UI.Utils.UIAnchor
UIPivot = Ballance2.Sys.UI.Utils.UIPivot
EventTriggerListener = Ballance2.Sys.UI.Utils.EventTriggerListener
GameStaticResourcesPool = Ballance2.Sys.Res.GameStaticResourcesPool
GameEventNames = Ballance2.Sys.Bridge.GameEventNames
GamePackage = Ballance2.Sys.Package.GamePackage

---@type GameLuaObjectHostClass
DebugWindow = {
  --Initvars
  CommandInputField = nil,---@type InputField
  FilterLogInputField = nil,---@type InputField
  LogContentView = nil,---@type RectTransform
  LogScrollView = nil,---@type ScrollRect
  LogStacktraceScrollView = nil,---@type ScrollRect
  LogStacktraceView = nil,---@type RectTransform
  LogStacktraceText = nil,---@type Text
  CheckBoxWarningAndErr = nil,---@type Toggle
  CheckBoxMessages = nil,---@type Toggle
  ToggleAutoScroll = nil,---@type ToggleEx

  logObserver = nil,
  logAutoScroll = true,
  logShowErrAndWarn = true,
  logShowMessage = true,
  logItems = {},
  logForceDisable = true,
  logFont = nil,
  logMaxCount = 256,
  logFilter = '',

  cmdServer = nil, 
}

function CreateClass_DebugWindow()
  function DebugWindow:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param thisGameObject GameObject
  function DebugWindow:Start(thisGameObject)
    self.logFont = GameStaticResourcesPool.FindStaticAssets('FontConsole')

    -- 注册日志观察者
    self.logObserver =
      Log.RegisterLogObserver(
      function(level, tag, message, stackTrace)
        if(self.logForceDisable) then
          return false
        end

        if(#self.logItems > self.logMaxCount) then
          local item = table.remove(self.logItems, 0)
          UnityEngine.Object.Destroy(item.go)
        end

        if #message >= 16384 then
          message = string.sub(message, 0, 16383)
        end
        if #message >= 32768 then
          message = string.sub(message, 0, 32767)
        end

        local logColor = GetLogColor(level)
        local t = table.concat({'<color=#', logColor, '>', Log.LogLevelToString(level), '/', tag, ' ', message, '</color>'})

        ---Add text
        local newEle = CloneUtils.CreateEmptyUIObjectWithParent(self.LogContentView)
        ---@type Text
        local text = newEle:AddComponent(UnityEngine.UI.Text)
        text.text = t
        text.font = self.logFont
        UIAnchorPosUtils.SetUIAnchor(newEle.transform, UIAnchor.Stretch, UIAnchor.Top)
        UIAnchorPosUtils.SetUIPivot(newEle.transform, UIPivot.TopLeft)

        ---日志点击事件
        ---@param go GameObject
        EventTriggerListener.Get(newEle).onClick = function (go)
          self:ShowLogStackTrace(go.transform:GetSiblingIndex() + 1)
        end

        table.insert(self.logItems, {
          go = newEle,
          message = message,
          level = level,
          stackTrace = stackTrace,
        })

        --重新布局
        self:RelayoutLogContent()

        --滚动到末尾
        if(self.logAutoScroll) then
          self.LogScrollView.normalizedPosition = Vector2(0, 0)
        end
      end,
      LogLevel.All)

    self.logForceDisable = false

    --命令服务的准备
    self.cmdServer = GameManager.Instance.GameDebugCommandServer

    --游戏退出事件
    GameManager.GameMediator:RegisterEventHandler(GamePackage.GetSystemPackage(), GameEventNames.EVENT_BEFORE_GAME_QUIT, 'DebugWindow', function ()
      self.logForceDisable = true
      Log.UnRegisterLogObserver(self.logObserver)
      return false
    end)

    --发送没有抓取到的日志
    Log.SendLogsInTemporary()
  end
  function DebugWindow:OnDestroy()
    self.logForceDisable = true
    Log.UnRegisterLogObserver(self.logObserver)
  end

  --[[
    工具函数
  ]]--

  ---布局日志界面
  ---@param self table
  function DebugWindow:RelayoutLogContent()
    local transform = self.LogContentView
    local c = transform.childCount
    local h = 0
    local logFilter = self.logFilter
    local logItems = self.logItems
    local logFilterEmpty = logFilter == ''
    local logShowErrAndWarn = self.logShowErrAndWarn
    local logShowMessage = self.logShowMessage
    for i = 0, c - 1, 1 do
      ---@type RectTransform
      local child = transform:GetChild(i)
      local log = logItems[i + 1]
      if(
        ((logShowErrAndWarn and (log.level == LogLevel.Warning or log.level == LogLevel.Error)) or
        (logShowMessage and (log.level == LogLevel.Info or log.level == LogLevel.Debug))) and  
        (logFilterEmpty or logFilter == string.find(log.message, logFilter))
      ) then
        child.gameObject:SetActive(true)
        child.anchoredPosition = Vector2(0, -h)
        UIAnchorPosUtils.SetUILeftBottom(child, 10, UIAnchorPosUtils.GetUIBottom(child))
        child.sizeDelta = Vector2(0, 15)
        h = h + 15
      else
        child.gameObject:SetActive(false)
      end
    end
    transform.sizeDelta = Vector2(transform.sizeDelta.x, h < 200 and 200 or h)
  end
  ---清空日志条目
  ---@param self table
  function DebugWindow:ClearLogContent()
    local transform = self.LogContentView
    local c = transform.childCount
    for i = c - 1, 0, -1 do
      UnityEngine.Object.Destroy(transform:GetChild(i).gameObject)
    end
    transform.sizeDelta = Vector2(transform.sizeDelta.x, 300)
  end
  ---显示日志StackTrace
  ---@param index number
  function DebugWindow:ShowLogStackTrace(index)
    local data = self.logItems[index]
    self.LogStacktraceText.text = table.concat({ data.message, '\n= StackTrace ====\n', data.stackTrace })
    LayoutRebuilder.ForceRebuildLayoutImmediate(self.LogStacktraceText.rectTransform);
    local size = self.LogStacktraceText.rectTransform.sizeDelta;
    self.LogStacktraceView.sizeDelta = Vector2(size.x + 40, size.y + 40)
    self.LogStacktraceScrollView.normalizedPosition = Vector2(0, 1)
  end
  function DebugWindow:SelectLastLog()
    self:ShowLogStackTrace(self.LogContentView.childCount)
  end
  ---AutoScroll更改
  function DebugWindow:OnAutoScrollChanged() 
    self.logAutoScroll = self.ToggleAutoScroll.isOn
  end
  ---显示错误Checkbox更改事件
  function DebugWindow:OnShowErrCheckChange()
    self.logShowErrAndWarn = self.CheckBoxWarningAndErr.isOn
    self:RelayoutLogContent()
  end
  ---显示信息Checkbox更改事件
  function DebugWindow:OnShowMessagesCheckChange()
    self.logShowMessage = self.CheckBoxMessages.isOn
    self:RelayoutLogContent()
  end
  ---筛选Input输入完成事件
  function DebugWindow:OnFilterInputEndInput()
    self.logFilter = self.FilterLogInputField.text
    self:RelayoutLogContent()
  end
  ---执行命令
  function DebugWindow:ExecCommand()
    if self.cmdServer:ExecuteCommand(self.CommandInputField.text) then
      self.CommandInputField.text = ''
      self:SelectLastLog()
    end
  end
  ---显示调试窗口
  function DebugWindow:ShowDebugOptWindow()
    GlobalDebugOptWindow:Show()
  end
  ---清空命令窗口
  function DebugWindow:ClearCommand()
    self.logWarnCount = 0
    self.logErrorCount = 0
    self.logInfoCount = 0
    --清空日志条目
    self:ClearLogContent()
    GlobalDebugToolbar.LuaSelf:Clear()
    --重新布局
    self:RelayoutLogContent()
  end

  return DebugWindow:new(nil)
end

