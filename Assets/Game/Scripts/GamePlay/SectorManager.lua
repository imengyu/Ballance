local GameSoundType = Ballance2.Sys.Services.GameSoundType

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

function SectorManager:new() 
  self.CurrentLevelSectorCount = 0
  self.CurrentLevelSectors = {} ---@type SectorDataStorage[]
  self.CurrentLevelRestPoints = {} ---@type RestPointsDataStorage[]
  self.CurrentLevelEndBalloon = nil ---@type PE_Balloon
end
function SectorManager:Start() 
  GamePlay.SectorManager = self
end

function SectorManager:DoInitAllModuls() 
  --初次加载后通知每个modul进行备份
  for _, value in ipairs(Game.LevelBuilder._CurrentLevelModuls) do
    value.modul:Backup()
    value.modul:Deactive()
  end
end
function SectorManager:DoUnInitAllModuls() 
  --通知每个modul卸载
  for _, value in ipairs(Game.LevelBuilder._CurrentLevelModuls) do
    value.modul:Deactive()
    value.modul:UnLoad()
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
  if GamePlayManager.CurrentSector < self.CurrentLevelSectorCount then
    self:SetCurrentSector(GamePlayManager.CurrentSector + 1)
  end
end
---设置当前节
---@param sector number
function SectorManager:SetCurrentSector(sector) 
  local oldSector = GamePlayManager.CurrentSector
  if oldSector ~= sector then
    --禁用之前一节的所有机关
    if oldSector > 0 then
      local s = self.CurrentLevelSectors[oldSector]
      for _, value in ipairs(s.moduls) do
        value:Deactive()
      end 
      
      --设置火焰状态
      local flame = self.CurrentLevelRestPoints[oldSector].flame
      flame.CheckPointActived = true
      flame:Deactive()
    end

    GamePlayManager.CurrentSector = sector 

    --激活当前节的机关
    local s = self.CurrentLevelSectors[sector]
    for _, value in ipairs(s.moduls) do
      value:Active()
    end 

    --设置火焰状态
    self.CurrentLevelRestPoints[sector].flame:Active()
    if sector < self.CurrentLevelSectorCount then
      --下一关的火焰
      local flameNext = self.CurrentLevelRestPoints[sector + 1].flame
      if flameNext ~= nil then
        flameNext:InternalActive()
      end
    end

    --播放音乐
    if sector > 1 then
      Game.SoundManager:PlayFastVoice('core.sounds:Misc_Checkpoint.wav', GameSoundType.Normal)
    end

    --如果是最后一个小节，则激活飞船
    if sector == self.CurrentLevelSectorCount then
      self.CurrentLevelEndBalloon:Active()
    else
      self.CurrentLevelEndBalloon:Deactive()
    end

  end
end
---重置当前节的机关
---@param active boolean 重置机关后是否激活
function SectorManager:ResetCurrentSector(active)  
  local sector = GamePlayManager.CurrentSector
  if sector > 0 then
    local s = self.CurrentLevelSectors[sector]
    for _, value in ipairs(s.moduls) do
      value:Deactive()
      value:Reset()
      if active then value:Active() end
    end 
  end
  if sector == self.CurrentLevelSectorCount then
    self.CurrentLevelEndBalloon:Reset()
  end
end
---重置所有机关
---@param active boolean 重置机关后是否激活
function SectorManager:ResetAllSector(active) 
  --通知每个modul卸载
  for _, value in ipairs(Game.LevelBuilder._CurrentLevelModuls) do
    value.modul:Deactive()
    value.modul:Reset()
    if active then value.modul:Active() end
  end
end

function CreateClass_SectorManager() 
  return SectorManager()
end