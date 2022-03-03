local Vector3 = UnityEngine.Vector3
local Time = UnityEngine.Time

---@class P_Modul_18 : ModulBase 
---@field P_Modul_18_Kollisionsquader TiggerTester
---@field P_Modul_18_Rotor Animator
---@field P_Modul_18_Particle GameObject
---@field P_Modul_18_Particle_Small GameObject
---@field P_Modul_18_Sound AudioSource
---@field P_Modul_18_Force number
P_Modul_18 = ModulBase:extend()

function P_Modul_18:new()
  P_Modul_18.super.new(self)
  self._CurrentInRangeBall = nil ---@type PhysicsObject
  self.AutoActiveBaseGameObject = false
  self.CurrentBallForce = nil
end

function P_Modul_18:Start()
  ModulBase.Start(self)
  ---@param other GameObject
  self.P_Modul_18_Kollisionsquader.onTriggerEnter = function (_, other)
    if self._CurrentInRangeBall == nil and other.tag == 'Ball' and other.name == 'BallPaper' then
      self._CurrentInRangeBall = GamePlay.BallManager.CurrentBall._Rigidbody
      self.CurrentBallForce = self._CurrentInRangeBall:AddConstantForceLocalCenter(self.P_Modul_18_Force, self.transform:TransformVector(Vector3.up))
    end
  end
  ---@param other GameObject
  self.P_Modul_18_Kollisionsquader.onTriggerExit = function (_, other)
    if self._CurrentInRangeBall ~= nil and other.tag == 'Ball' and other.name == 'BallPaper' then
      if self.CurrentBallForce then
        self.CurrentBallForce:Delete()
        self.CurrentBallForceID = nil
      end
      self._CurrentInRangeBall = nil
    end
  end
end
function P_Modul_18:Active()
  ModulBase.Active(self)
  self.P_Modul_18_Particle:SetActive(true)
  self.P_Modul_18_Particle_Small:SetActive(true)
  self.P_Modul_18_Sound:Play()
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
  ModulBase.Deactive(self)
end
function P_Modul_18:Reset()
  self._CurrentInRangeBall = nil
end

function CreateClass:P_Modul_18()
  return P_Modul_18()
end