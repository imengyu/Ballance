---@class P_Trafo_Base : ModulBase 
P_Trafo_Base = ModulBase:extend()

function P_Trafo_Base:new()
  self._Tigger = nil ---@type PhysicsPhantom
  self._TargetBallType = ''
end

function P_Trafo_Base:Active()
  self.gameObject:SetActive(true)
  self._Tigger.onOverlappingCollidableAdd = { "+=", self._OnOverlappingCollidableAdd }
end
function P_Trafo_Base:Deactive()
  self.gameObject:SetActive(false)
  self._Tigger.onOverlappingCollidableAdd = { "-=", self._OnOverlappingCollidableAdd }
end
function P_Trafo_Base:Reset()

end
function P_Trafo_Base:Backup()

end
function P_Trafo_Base:_OnOverlappingCollidableAdd()
  --TODO: Trafo
end

function CreateClass_P_Trafo_Base()
  return P_Trafo_Base()
end