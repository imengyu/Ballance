---节管理器
---@class SectorManager : GameLuaObjectHostClass
SectorManager = ClassicObject:extend()

---@class SectorDataStorage
---@field moduls ModulBase[]
SectorDataStorage = {}

---@class RestPointsDataStorage
---@field point GameObject
---@field flame ModulBase
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



end
function SectorManager:DoUnInitAllModuls() 
  --Do nothing.
end
function SectorManager:ClearAll() 
  self.CurrentLevelSectorCount = 0
  self.CurrentLevelSectors = {}
  self.CurrentLevelRestPoints = {}
  self.CurrentLevelEndBalloon = nil
end

---进入下一小节
function SectorManager:NextSector() 



end

function CreateClass_SectorManager() 
  return SectorManager()
end