local GameSoundType = Ballance2.Services.GameSoundType
local DebugUtils = Ballance2.Utils.DebugUtils
local Log = Ballance2.Log

---节管理器
---@class SectorManager : GameLuaObjectHostClass
SectorManager = ClassicObject:extend()

---@class SectorDataStorage
---@field moduls ModulBase[]
SectorDataStorage = {}

---@class RestPointsDataStorage
---@field point GameObject
---@field flame PC_TwoFlames
RestPointsDataStorage = {}

local TAG = 'SectorManager'

function SectorManager:new() 
  self.CurrentLevelSectorCount = 0
  self.CurrentLevelSectors = {} ---@type SectorDataStorage[]
  self.CurrentLevelRestPoints = {} ---@type RestPointsDataStorage[]
  self.CurrentLevelEndBalloon = nil ---@type PE_Balloon
end
function SectorManager:Start() 
  GamePlay.SectorManager = self
  
  self._CommandId = Game.Manager.GameDebugCommandServer:RegisterCommand('sector', function (eyword, fullCmd, argsCount, args)
    local type = args[1]
    if type == 'next' then
      self:NextSector()
    elseif type == 'set' then
      local o, n = DebugUtils.CheckIntDebugParam(1, args, Slua.out, true, 1)
      if not o then return false end

      self:SetCurrentSector(n)
    elseif type == 'reset' then
      local o, n = DebugUtils.CheckIntDebugParam(1, args, Slua.out, true, 1)
      if not o then return false end

      self:ResetCurrentSector(n)
    elseif type == 'reset-all' then
      self:ResetAllSector()
    else
      Log.W(TAG, 'Unknow option '..type);
      return false
    end
    return true
  end, 1, "sector <next/set/reset/reset-all> 节管理器命令"..
          "  next                  ▶ 进入下一小节"..
          "  set <sector:number>   ▶ 设置当前激活的小节"..
          "  reset <sector:number> ▶ 重置指定的小节"..
          "  reset-all             ▶ 重置所有小节"
  )
end
function SectorManager:OnDestroy() 
  Game.Manager.GameDebugCommandServer:UnRegisterCommand(self._CommandId)
end

function SectorManager:DoInitAllModuls() 
  --初次加载后通知每个modul进行备份
  for _, value in pairs(Game.LevelBuilder._CurrentLevelModuls) do
    if value ~= nil then
      value.modul:Backup()
      value.modul:Deactive()
    end
  end
end
function SectorManager:DoUnInitAllModuls() 
  --通知每个modul卸载
  for _, value in pairs(Game.LevelBuilder._CurrentLevelModuls) do
    if value ~= nil then
      value.modul:Deactive()
      value.modul:UnLoad()
    end
  end
end
function SectorManager:ClearAll() 
  self.CurrentLevelSectorCount = 0
  self.CurrentLevelSectors = {}
  self.CurrentLevelRestPoints = {}
  self.CurrentLevelEndBalloon = nil
end

---进入下一小节
function SectorManager:NextSector() 
  if GamePlay.GamePlayManager.CurrentSector < self.CurrentLevelSectorCount then
    self:SetCurrentSector(GamePlay.GamePlayManager.CurrentSector + 1)
  end
end
---设置当前节
---@param sector number
function SectorManager:SetCurrentSector(sector) 
  local oldSector = GamePlay.GamePlayManager.CurrentSector
  if oldSector ~= sector then
    --禁用之前一节的所有机关
    if oldSector > 0 then
      local s = self.CurrentLevelSectors[oldSector]
      for _, value in pairs(s.moduls) do
        if value ~= nil then  
          value:Deactive()
        end
      end 
      
      --设置火焰状态
      local flame = self.CurrentLevelRestPoints[oldSector].flame
      if flame then
        flame.CheckPointActived = true
        flame:Deactive()
      else
        Log.D(TAG, "No flame found for sector "..oldSector)
      end
    end

    if sector > 0 then 
      GamePlay.GamePlayManager.CurrentSector = sector 
      self:ActiveCurrentSector(true)
    end
  end
end

---激活当前节的机关
---@param playCheckPointSound boolean 是否播放节点音乐
function SectorManager:ActiveCurrentSector(playCheckPointSound) 
  local sector = GamePlay.GamePlayManager.CurrentSector
  --激活当前节的机关
  local s = self.CurrentLevelSectors[sector]
  if s == nil and sector ~= 0 then
    Log.E(TAG, 'Sector '..sector..' not found')
    GamePlay.GamePlayManager.CurrentSector = 0 
    return
  end
  for _, value in pairs(s.moduls) do
    if value ~= nil then  
      value:Active()
    end
  end 

  local nowSector = self.CurrentLevelRestPoints[sector]

  --设置火焰状态

  if nowSector.flame ~= nil then
    nowSector.flame:Active()
  else
    Log.D(TAG, "No flame found for sector "..sector)
  end
  if sector < self.CurrentLevelSectorCount then
    nowSector = self.CurrentLevelRestPoints[sector + 1]
    --下一关的火焰
    local flameNext = nowSector.flame
    if flameNext ~= nil then
      flameNext:InternalActive()
    end
  end

  --播放音乐
  if playCheckPointSound and sector > 1 then
    Game.SoundManager:PlayFastVoice('core.sounds:Misc_Checkpoint.wav', GameSoundType.Normal)
  end

  --如果是最后一个小节，则激活飞船
  if self.CurrentLevelEndBalloon ~= nil then
    if sector == self.CurrentLevelSectorCount then
      self.CurrentLevelEndBalloon:Active()
    else
      self.CurrentLevelEndBalloon:Deactive()
    end
  else
    Log.W(TAG, "No found CurrentLevelEndBalloon !")
  end
end

---禁用当前节的机关
function SectorManager:DeactiveCurrentSector()  
  local sector = GamePlay.GamePlayManager.CurrentSector
  if sector > 0 then
    local s = self.CurrentLevelSectors[sector]
    for _, value in pairs(s.moduls) do
      if value ~= nil then
        value:Deactive()
        value:Reset('sectorRestart')
      end
    end 
  end
end
---重置当前节的机关
---@param active boolean 重置机关后是否激活
function SectorManager:ResetCurrentSector(active)  
  self:DeactiveCurrentSector()
  if active then
    self:ActiveCurrentSector(false)
  end
end
---重置所有机关
---@param active boolean 重置机关后是否激活
function SectorManager:ResetAllSector(active) 
  --通知每个modul卸载
  for _, value in pairs(Game.LevelBuilder._CurrentLevelModuls) do
    if value ~= nil then
      value.modul:Deactive()
      value.modul:Reset('levelRestart')
      if active then value.modul:Active() end
    end
  end
  self.CurrentLevelEndBalloon:Reset()
end

function CreateClass:SectorManager() 
  return SectorManager()
end