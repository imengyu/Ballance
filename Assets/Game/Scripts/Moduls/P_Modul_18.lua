local Vector3 = UnityEngine.Vector3

---@class P_Modul_18 : ModulBase 
---@field P_Modul_18_Kollisionsquader PhysicsPhantom
---@field P_Modul_18_Rotor Animator
---@field P_Modul_18_Particle GameObject
---@field P_Modul_18_Particle_Small GameObject
---@field P_Modul_18_Sound AudioSource
---@field P_Modul_18_Force number
P_Modul_18 = ModulBase:extend()

function P_Modul_18:new()
  self._CurrentInRangeBall = nil ---@type PhysicsBody
end

function P_Modul_18:Start()
  self.P_Modul_18_Kollisionsquader:ForceReCreate()
  ---@param phantom PhysicsPhantom
  ---@param otherBody PhysicsBody
  self.P_Modul_18_Kollisionsquader.onOverlappingCollidableAdd = function (phantom, otherBody)
    if self._CurrentInRangeBall == nil and otherBody.gameObject.tag == 'Ball' and otherBody.gameObject.name == 'BallPaper' then
      self._CurrentInRangeBall = otherBody
    end
  end
  ---@param phantom PhysicsPhantom
  ---@param otherBody PhysicsBody
  self.P_Modul_18_Kollisionsquader.onOverlappingCollidableRemove = function (phantom, otherBody)
    if self._CurrentInRangeBall ~= nil and otherBody.gameObject.tag == 'Ball' and otherBody.gameObject.name == 'BallPaper' then
      self._CurrentInRangeBall = nil
    end
  end
end
function P_Modul_18:FixedUpdate()
 if self._CurrentInRangeBall ~= nil then
  self._CurrentInRangeBall:ApplyForce(Vector3(0, self.P_Modul_18_Force, 0)) -- 为球添加向上的力
 end
end
function P_Modul_18:Active()
  self.P_Modul_18_Particle:SetActive(true)
  self.P_Modul_18_Particle_Small:SetActive(true)
  self.P_Modul_18_Sound:Play();
  self.P_Modul_18_Kollisionsquader.gameObject:SetActive(true)
  self.P_Modul_18_Rotor:Play('P_Modul_18_Rotor_Start_Animation')
end
function P_Modul_18:Deactive()
  self.P_Modul_18_Particle:SetActive(false)
  self.P_Modul_18_Particle_Small:SetActive(false)
  self.P_Modul_18_Sound:Stop()
  self.P_Modul_18_Kollisionsquader.gameObject:SetActive(false)
  self.P_Modul_18_Rotor:Play('P_Modul_18_Rotor_Stop_Animation')
  self._CurrentInRangeBall = nil
end
function P_Modul_18:Reset()
  self._CurrentInRangeBall = nil
end

function CreateClass_P_Modul_18()
  return P_Modul_18()
end