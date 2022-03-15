---@class P_Trafo_Base : ModulBase 
---@field _Tigger TiggerTester
---@field _TargetBallType string
---@field _Color Color
P_Trafo_Base = ModulBase:extend()

function P_Trafo_Base:new()
  P_Trafo_Base.super.new(self)
  self._Tigger = nil ---@type TiggerTester
  self._TargetBallType = ''
  self._Color = nil
  self.TranfoActived = false
end

function P_Trafo_Base:Start()
  if not self.IsPreviewMode then
    ---@param other GameObject
    self._Tigger.onTriggerEnter = function (_, other)
      --球，并且球类型于目标类型不一致
      if not self._TranfoActived and other.tag == 'Ball' and other.name ~= self._TargetBallType then
        --触发变球
        self._TranfoActived = true
        GamePlay.GamePlayManager:ActiveTranfo(self, self._TargetBallType, self._Color)
      end
    end
  end
end
function P_Trafo_Base:Active()
  self.gameObject:SetActive(true)
end
function P_Trafo_Base:Deactive()
  self.gameObject:SetActive(false)
end
function P_Trafo_Base:Reset()
  self._TranfoActived = false
end
function P_Trafo_Base:Backup()

end

function P_Trafo_Base:ActiveForPreview()
  self.gameObject:SetActive(true)
end
function P_Trafo_Base:DeactiveForPreview()
  self.gameObject:SetActive(false)
end

function CreateClass:P_Trafo_Base()
  return P_Trafo_Base()
end