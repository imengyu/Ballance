local Vector3 = UnityEngine.Vector3
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local ufoPositions = require('UFOPositions') ---@type UFOPositionItem[]

---游戏结束时的UFO动画控制器。
---@class UFOAnimController : GameLuaObjectHostClass
---@field PE_UFO_Arm_Inner_01 Animator
---@field PE_UFO_Arm_Inner_02 Animator
---@field PE_UFO_Arm_Inner_03 Animator
---@field PE_UFO_Arm_Inner_04 Animator
---@field PE_UFO_Flash GameObject
---@field PE_UFO_Sound AudioSource
---@field PE_UFO_CatchSound AudioSource
---@field PE_UFO_Fly SmoothFly
---@field PE_UFO_Body GameObject
UFOAnimController = ClassicObject:extend()

function UFOAnimController:new()
end
function UFOAnimController:Start()
  self.PE_UFO_Body:SetActive(false) 
  GamePlay.UFOAnimController = self
end
function UFOAnimController:StartSeq()
  self._CurrentPosIndex = 0
  self.PE_UFO_Body:SetActive(true) 
  self.PE_UFO_Sound:Play()
  self.PE_UFO_CatchSound:Stop()
  self.PE_UFO_Flash:SetActive(false)
  self.PE_UFO_Arm_Inner_01:Play('DefaultState')
  self.PE_UFO_Arm_Inner_02:Play('DefaultState')
  self.PE_UFO_Arm_Inner_03:Play('DefaultState')
  self.PE_UFO_Arm_Inner_04:Play('DefaultState')

  local PE_Balloon_BallRefPos = GamePlay.SectorManager.CurrentLevelEndBalloon.PE_Balloon_BallRefPos
  local fly = self.PE_UFO_Fly
  coroutine.resume(coroutine.create(function()
    for index, value in ipairs(ufoPositions) do
      fly.Time = value.flyTime
      fly.TargetPos = PE_Balloon_BallRefPos.transform:TransformPoint(value.pos)
      fly.Fly = true

      Yield(WaitForSeconds(value.waitTime))

      if value.startBall then
        self.PE_UFO_Arm_Inner_01:Play('PE_UFO_Arm_Animation')
        self.PE_UFO_Arm_Inner_02:Play('PE_UFO_Arm_Animation')
        self.PE_UFO_Arm_Inner_03:Play('PE_UFO_Arm_Animation')
        self.PE_UFO_Arm_Inner_04:Play('PE_UFO_Arm_Animation')
        self.PE_UFO_CatchSound:Play()
      end
      if value.catchBall then
        GamePlay.BallManager:SetControllingStatus(BallControlStatus.LockLookMode) --- 去除物理化，禁用控制
        local currentBall = GamePlay.BallManager.CurrentBall
        if currentBall then
          local transform = currentBall.gameObject.transform
          self._LastBallParent = transform.parent
          self._LastSetBall = currentBall
          transform:SetParent(self.PE_UFO_Fly.transform)
          transform.localPosition = Vector3.zero
        end
      end
      if index == #ufoPositions - 1 then
        --最后一个位置就隐藏UFO和球了
        self.PE_UFO_Body:SetActive(false)
        if self._LastSetBall ~= nil then  
          self._LastSetBall.gameObject:SetActive(false)
        end
      elseif index == #ufoPositions then
        self.PE_UFO_Flash:SetActive(true) --闪光
      end
    end

    self.PE_UFO_Fly.Fly = false

    --恢复球的父级
    if self._LastBallParent ~= nil then
      self._LastSetBall.transform:SetParent(self._LastBallParent)
      self._LastBallParent = nil
    end

    --UFO动画完成,现在控制权交回GamePlayManager
    GamePlay.GamePlayManager:UfoAnimFinish()
end))

end

function CreateClass:UFOAnimController()
  return UFOAnimController()
end