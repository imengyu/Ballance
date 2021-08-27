---@class P_Trafo_Base : ModulBase 
---@field _Tigger PhysicsPhantom
---@field _TargetBallType string
---@field _Color Color
P_Trafo_Base = ModulBase:extend()

function P_Trafo_Base:new()
  self._Tigger = nil ---@type PhysicsPhantom
  self._TargetBallType = ''
  self._Color = nil
  self.TranfoActived = false
end

function P_Trafo_Base:Start()
  self._Tigger.onOverlappingCollidableAdd = function (phantom, otherBody)
    self:_OnOverlappingCollidableAdd(phantom, otherBody)
  end
end
function P_Trafo_Base:Active()
  self.gameObject:SetActive(true)
end
function P_Trafo_Base:Deactive()
  self.gameObject:SetActive(false)
end
function P_Trafo_Base:Reset()

end
function P_Trafo_Base:Backup()

end
---@param phantom PhysicsPhantom
---@param otherBody PhysicsBody
function P_Trafo_Base:_OnOverlappingCollidableAdd(phantom, otherBody)
  --球，并且球类型于目标类型不一致
  if not self._TranfoActived and otherBody.gameObject.tag == 'Ball' and otherBody.gameObject.name ~= self._TargetBallType then
    --触发变球
    self._TranfoActived = true;
    GamePlay.GamePlayManager:ActiveTranfo(self, self._TargetBallType, self._Color)
  end
end

function CreateClass_P_Trafo_Base()
  return P_Trafo_Base()
end