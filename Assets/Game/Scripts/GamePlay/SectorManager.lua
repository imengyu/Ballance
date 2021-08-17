---节管理器
---@class SectorManager : GameLuaObjectHostClass
SectorManager = ClassicObject:extend()

function SectorManager:new() 

end
function SectorManager:Start() 


  GamePlay.SectorManager = self
end
