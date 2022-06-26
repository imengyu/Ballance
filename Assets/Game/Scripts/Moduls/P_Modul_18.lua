local Vector3 = UnityEngine.Vector3

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
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 80
end

function P_Modul_18:Start()
  ModulBase.Start(self)
  if not self.IsPreviewMode then
    ---球进入时给一个方向向上的恒力
    ---@param other GameObject
    self.P_Modul_18_Kollisionsquader.onTriggerEnter = function (_, other)
      if self._CurrentInRangeBall == nil and other.tag == 'Ball' then
        self._CurrentInRangeBall = GamePlay.BallManager.CurrentBall._Rigidbody
        self.CurrentBallForce = self._CurrentInRangeBall:AddConstantForceLocalCenter(self.P_Modul_18_Force, self.transform:TransformVector(Vector3.up))
      end
    end
    ---球离开时去除恒力
    ---@param other GameObject
    self.P_Modul_18_Kollisionsquader.onTriggerExit = function (_, other)
      if self._CurrentInRangeBall ~= nil and other.tag == 'Ball' then
        if self.CurrentBallForce then
          self.CurrentBallForce:Delete()
          self.CurrentBallForceID = nil
        end
        self._CurrentInRangeBall = nil
      end
    end
  end
end
function P_Modul_18:Active()
  ModulBase.Active(self)
  self.P_Modul_18_Sound:Play()
  self.P_Modul_18_Rotor:Play('P_Modul_18_Rotor_Start_Animation')
  self.P_Modul_18_Kollisionsquader.gameObject:SetActive(true)
  if self.BallInRange then
    self.P_Modul_18_Particle:SetActive(true)
    self.P_Modul_18_Particle_Small:SetActive(true)
  end
end
function P_Modul_18:Deactive()
  ModulBase.Deactive(self)
  self._CurrentInRangeBall = nil
  self.P_Modul_18_Sound:Stop()
  self.P_Modul_18_Rotor:Play('P_Modul_18_Rotor_Stop_Animation')
  self.P_Modul_18_Kollisionsquader.gameObject:SetActive(false)
  self.P_Modul_18_Particle:SetActive(false)
  self.P_Modul_18_Particle_Small:SetActive(false) 
end

function P_Modul_18:ActiveForPreview()
  self.gameObject:SetActive(true)
  self.P_Modul_18_Particle:SetActive(true)
  self.P_Modul_18_Particle_Small:SetActive(true)
  self.P_Modul_18_Sound:Play()
  self.P_Modul_18_Rotor:Play('P_Modul_18_Rotor_Start_Animation')
end
function P_Modul_18:DeactiveForPreview()
  self.P_Modul_18_Particle:SetActive(false)
  self.P_Modul_18_Particle_Small:SetActive(false)
  self.P_Modul_18_Sound:Stop()
  self.gameObject:SetActive(false)
end

function P_Modul_18:BallEnterRange()
  if self.IsActive then
    self.P_Modul_18_Particle:SetActive(true)
    self.P_Modul_18_Particle_Small:SetActive(true) 
  end
end
function P_Modul_18:BallLeaveRange()
  if self.IsActive then
    self.P_Modul_18_Particle:SetActive(false)
    self.P_Modul_18_Particle_Small:SetActive(false)
  end
end
function P_Modul_18:Reset()
  self._CurrentInRangeBall = nil
end

function CreateClass:P_Modul_18()
  return P_Modul_18()
end