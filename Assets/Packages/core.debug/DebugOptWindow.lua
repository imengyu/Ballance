Log = Ballance2.Utils.Log
LogLevel = Ballance2.Utils.LogLevel
GameManager = Ballance2.Sys.GameManager

---@type GameLuaObjectHostClass
DebugOptWindow = { 
  CheckBoxShowSystemInfo = nil,---@type Toggle
  CheckBoxShowStats = nil,---@type Toggle

  DbgStatShowSystemInfo = nil,
  DbgStatShowStats = nil,

  DataObserverDbgStatShowSystemInfo = nil,
  DataObserverDbgStatShowStats = nil,

}

function CreateClass_DebugOptWindow()
  function DebugOptWindow:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  function DebugOptWindow:Start()

    self.DbgStatShowSystemInfo = GameManager.Instance.GameStore:GetParameter('DbgStatShowSystemInfo');
    self.DbgStatShowStats = GameManager.Instance.GameStore:GetParameter('DbgStatShowStats');

    self.DataObserverDbgStatShowSystemInfo = self.DbgStatShowSystemInfo:RegisterDataObserver(function (data, ov, nv)
      if nv ~= self.CheckBoxShowSystemInfo.isOn then
        self.CheckBoxShowSystemInfo.isOn = nv
      end
    end)
    self.DataObserverDbgStatShowStats = self.DbgStatShowStats:RegisterDataObserver(function (data, ov, nv)
      if nv ~= self.CheckBoxShowStats.isOn then
        self.CheckBoxShowStats.isOn = nv
      end
    end)
    
    self.DbgStatShowSystemInfo:ReNotificationAllDataObserver()
    self.DbgStatShowStats:ReNotificationAllDataObserver()
    
  end
  function DebugOptWindow:OnDestroy()
    self.DbgStatShowSystemInfo:UnRegisterDataObserver(self.DataObserverDbgStatShowSystemInfo)
    self.DbgStatShowStats:UnRegisterDataObserver(self.DataObserverDbgStatShowStats)
  end

  function DebugOptWindow:OnCheckBoxShowSystemInfoCheckChanged() 
    self.DbgStatShowSystemInfo:SetData(0, self.CheckBoxShowSystemInfo.isOn)
  end
  function DebugOptWindow:OnCheckBoxShowStatsCheckChanged() 
    self.DbgStatShowStats:SetData(0, self.CheckBoxShowStats.isOn)
  end

  return DebugOptWindow:new(nil)
end

