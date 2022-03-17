---@class PC_TwoFlames : ModulBase 
PC_TwoFlames = ModulBase:extend()

function PC_TwoFlames:new()
  PC_TwoFlames.super.new(self)
  self.FlameSmallLeft = nil ---@type GameObject
  self.FlameSmallRight = nil ---@type GameObject
  self.Flame = nil ---@type GameObject
  self.CheckPointTigger = nil ---@type TiggerTester
  self.CheckPointActived = false
end
function PC_TwoFlames:Start()
  self.FlameSmallLeft:SetActive(false)
  self.FlameSmallRight:SetActive(false)
  self.Flame:SetActive(false)
  
  if not self.IsPreviewMode then
    --Tigger 进入事件
    ---@param otherBody GameObject
    self.CheckPointTigger.onTriggerEnter = function (s, otherBody)
      if not self.CheckPointActived and otherBody.tag == 'Ball' then
        --触发下一关
        self.CheckPointActived = true
        self.Flame:SetActive(false)
        GamePlay.SectorManager:NextSector()
      end
    end
  end
end

function PC_TwoFlames:Active()
  self.CheckPointActived = true
  self.FlameSmallLeft:SetActive(true)
  self.FlameSmallRight:SetActive(true)
end
function PC_TwoFlames:Deactive()
  self.FlameSmallLeft:SetActive(false)
  self.FlameSmallRight:SetActive(false)
  self.Flame:SetActive(false)
end

function PC_TwoFlames:ActiveForPreview()
  self.FlameSmallLeft:SetActive(true)
  self.FlameSmallRight:SetActive(true)
  self.gameObject:SetActive(true)
end
function PC_TwoFlames:DeactiveForPreview()
  self.FlameSmallLeft:SetActive(false)
  self.FlameSmallRight:SetActive(false)
  self.gameObject:SetActive(false)
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

function CreateClass:PC_TwoFlames()
  return PC_TwoFlames()
end