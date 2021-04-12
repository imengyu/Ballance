InputField = UnityEngine.UI.InputField;
Text = UnityEngine.UI.Text;
RectTransform = UnityEngine.RectTransform;
Log = Ballance2.Utils.Log
LogLevel = Ballance2.Utils.LogLevel
CloneUtils = Ballance2.Sys.Utils.CloneUtils

---@type GameLuaObjectHostClass
DebugWindow = {
  --Initvars
  CommandInputField = nil,---@type GameObject
  LogContentView = nil,---@type GameObject
  LogStacktraceView = nil,---@type GameObject
  DebugWarnText = nil,---@type GameObject
  DebugErrorText = nil,---@type GameObject
  DebugInfoText = nil,---@type GameObject
  --变量
  insCommandInputField = nil,---@type InputField
  insLogContentView = nil,---@type RectTransform
  insLogStacktraceView = nil,---@type RectTransform
  insDebugWarnText = nil,---@type Text
  insDebugErrorText = nil,---@type Text
  insDebugInfoText = nil,---@type Text

  logObserver = nil,
  logAutoScroll = true,
  logItems = {},
}

function class_DebugWindow()
  function DebugWindow:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param thisGameObject GameObject
  function DebugWindow:Start(thisGameObject)
    self.insCommandInputField = self.CommandInputField:GetComponent('UnityEngine.UI.InputField');
    self.insLogContentView = self.LogContentView:GetComponent('UnityEngine.RectTransform');
    self.insLogStacktraceView = self.insLogStacktraceView:GetComponent('UnityEngine.RectTransform');
    self.insDebugWarnText = self.DebugWarnText:GetComponent('UnityEngine.UI.Text');
    self.insDebugErrorText = self.DebugErrorText:GetComponent('UnityEngine.UI.Text');
    self.insDebugInfoText = self.DebugInfoText:GetComponent('UnityEngine.UI.Text');
    -- 注册日志观察者
    self.logObserver =
      Log.RegisterLogObserver(
      function(level, tag, message, stackTrace)
        local logColor = GetLogColor(level)
        local t = table.concat({'<color=#', logColor, '>', Log.LogLevelToString(level), '/', tag, ' ', message, '</color>'})
        table.insert(self.logItems, {
          base = t,
          stackTrace = stackTrace,
        });
        ---Add text
        local newEle = CloneUtils.CreateEmptyUIObjectWithParent(self.insLogContentView.transform)
        ---@type Text
        local text = newEle:AddComponent('UnityEngine.UI.Text')
        text.text = t
      end,
      LogLevel.All)
  end
  function DebugWindow:OnDestroy()
    Log.UnRegisterLogObserver(self.logObserver)
  end

  ---AutoScroll更改
  ---@param scroll boolean
  function DebugWindow:OnAutoScrollChanged(scroll)
    
  end
  ---执行命令
  function DebugWindow:ExecCommand()
    
  end
  ---清空命令窗口
  function DebugWindow:ClearCommand()
    
  end

  return DebugWindow:new(nil)
end

