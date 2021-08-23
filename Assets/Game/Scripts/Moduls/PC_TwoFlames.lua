---@class PC_TwoFlames : ModulBase 
PC_TwoFlames = ModulBase:extend()

function PC_TwoFlames:new()
  self.FlameSmallLeft = nil ---@type GameObject
  self.FlameSmallRight = nil ---@type GameObject
  self.Flame = nil ---@type GameObject
  self.CheckPointTigger = nil ---@type PhysicsPhantom
  self.CheckPointActived = false
end
function PC_TwoFlames:Start()
  self.FlameSmallLeft:SetActive(false)
  self.FlameSmallRight:SetActive(false)
  self.Flame:SetActive(false)
  self.CheckPointTigger.onOverlappingCollidableAdd = {"+=",
  ---@param phantom PhysicsPhantom
  ---@param otherBody PhysicsBody
  function (phantom, otherBody)
    if not self.CheckPointActived and otherBody.gameObject.tag == 'Ball' then
      --触发下一关
      self.CheckPointActived = true;
      GamePlay.SectorManager:NextSector()
    end
  end}
end

function PC_TwoFlames:Active()
  self.FlameSmallLeft:SetActive(true)
  self.FlameSmallRight:SetActive(true)
end
function PC_TwoFlames:Deactive()
  self.FlameSmallLeft:SetActive(false)
  self.FlameSmallRight:SetActive(false)
  self.Flame:SetActive(false)
end
function PC_TwoFlames:Reset()
end
function PC_TwoFlames:Backup()
end
---设置火焰激活状态
function PC_TwoFlames:InternalActive()
  self.FlameSmallLeft:SetActive(false)
  self.FlameSmallRight:SetActive(false)
  self.Flame:SetActive(true)
  self.CheckPointActived = false
end

function CreateClass_PC_TwoFlames()
  return PC_TwoFlames()
end