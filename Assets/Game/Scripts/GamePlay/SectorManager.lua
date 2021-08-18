---节管理器
---@class SectorManager : GameLuaObjectHostClass
SectorManager = ClassicObject:extend()

---@class SectorDataStorage
---@field moduls ModulBase[]
SectorDataStorage = {}

---@class RestPointsDataStorage
---@field points GameObject[]
---@field flames ModulBase[]
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

function CreateClass_SectorManager() 
  return SectorManager()
end