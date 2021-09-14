local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils
local DistanceChecker = Ballance2.Game.DistanceChecker
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

---@class PE_Balloon : ModulBase 
---@field PE_Balloon_Platform_Tigger PhysicsBody
---@field PE_Balloon_Platform_HingeJoint HingeConstraint
---@field PE_Balloon_Platform PhysicsBody
---@field PE_Balloon_Platform_Force PhysicsForce
---@field PE_Balloon_Platte01 PhysicsBody
---@field PE_Balloon_Platte02 PhysicsBody
---@field PE_Balloon_Platte03 PhysicsBody
---@field PE_Balloon_Platte04 PhysicsBody
---@field PE_Balloon_Platte05 PhysicsBody
---@field PE_Balloon_Platte06 PhysicsBody
---@field PE_Balloon_Platte07 PhysicsBody
---@field PE_Balloon_Platte08 PhysicsBody
---@field PE_Balloon_BoxSide PhysicsBody
---@field PE_Balloon_BallRefPos GameObject
PE_Balloon = ModulBase:extend()

function PE_Balloon:new()
end
function PE_Balloon:Start()

  --接近PE_Balloon时禁用背景音乐
  local musicDisabledByPE_Balloon = false
  local distanceChecker = self.PE_Balloon_Platform:GetComponent(DistanceChecker) ---@type DistanceChecker
  distanceChecker.Object2 = GamePlay.BallManager.PosFrame
  distanceChecker.OnEnterRange = function () 
    if self._MusicActived then
      musicDisabledByPE_Balloon = true
      GamePlay.MusicManager:DisableBackgroundMusic() 
    end
  end
  distanceChecker.OnLeaveRange = function () 
    if self._MusicActived and musicDisabledByPE_Balloon and not GamePlay.GamePlayManager.CurrentLevelPass then
      musicDisabledByPE_Balloon = false
      GamePlay.MusicManager:EnableBackgroundMusic()
    end 
  end
  distanceChecker.CheckEnabled = true

  --PE_Balloon 过关触发器
  self.PE_Balloon_Platform_Tigger.onTiggerEnter = function (body, other)
    if other and other.gameObject.tag == "Ball" then
      self.PE_Balloon_Platform_Force.Force = 0
      self.PE_Balloon_Platform_HingeJoint:SetEnabled(false) --断开与桥的连接
      LuaTimer.Add(2000, function ()
        self.PE_Balloon_Platform_Force.Enable = true
        self.PE_Balloon_Platform_Force.Force = 100
      end)
      LuaTimer.Add(5000, function ()
        self.PE_Balloon_Platform_Force.Force = 50
      end)
      LuaTimer.Add(20000, function ()
        self.PE_Balloon_Platform_Force.Force = 40
      end)
      self._MusicActived = false
      GamePlay.GamePlayManager:Pass() --通知管理器关卡已结束
    end
  end
  self._MusicActived = false

  local iWoodOnlyHit = GamePlay.BallSoundManager:GetCustomSoundLayerByName('WoodOnlyHit')
  self.PE_Balloon_Platte01.CustomLayer = iWoodOnlyHit
  self.PE_Balloon_Platte02.CustomLayer = iWoodOnlyHit
  self.PE_Balloon_Platte03.CustomLayer = iWoodOnlyHit
  self.PE_Balloon_Platte04.CustomLayer = iWoodOnlyHit
  self.PE_Balloon_Platte05.CustomLayer = iWoodOnlyHit
  self.PE_Balloon_Platte06.CustomLayer = iWoodOnlyHit
  self.PE_Balloon_Platte07.CustomLayer = iWoodOnlyHit
  self.PE_Balloon_Platte08.CustomLayer = iWoodOnlyHit
  self.PE_Balloon_Platform.CustomLayer = GamePlay.BallSoundManager:GetCustomSoundLayerByName('Wood')
end

function PE_Balloon:Active()
  self.gameObject:SetActive(true)

  self.PE_Balloon_BoxSide:ForcePhysics()
  self.PE_Balloon_Platte08:ForcePhysics()
  self.PE_Balloon_Platte07:ForcePhysics()
  self.PE_Balloon_Platte06:ForcePhysics()
  self.PE_Balloon_Platte05:ForcePhysics()
  self.PE_Balloon_Platte04:ForcePhysics()
  self.PE_Balloon_Platte03:ForcePhysics()
  self.PE_Balloon_Platte02:ForcePhysics()
  self.PE_Balloon_Platte01:ForcePhysics()
  self.PE_Balloon_Platform:ForcePhysics()
  self.PE_Balloon_Platform_Tigger:ForcePhysics()
  self.PE_Balloon_Platform_Force.Enable = false

  if not self._MusicActived then
    self._MusicActived = true;
    --播放最后一小节的音乐
    coroutine.resume(coroutine.create(function()
      local _SoundLastSector = GamePlay.GamePlayManager._SoundLastSector

      --设置为2d
      _SoundLastSector.spatialBlend = 0
      _SoundLastSector:Play()

      GamePlay.MusicManager:DisableInSec(15)      
      Yield(WaitForSeconds(5))

      --该音乐播放5秒后淡出
      Game.UIManager.UIFadeManager:AddAudioFadeOut(_SoundLastSector, 5)
      Yield(WaitForSeconds(5))
      --播放一次完成之后设置为3d声音100m范围衰减，位置为当前飞船位置
      _SoundLastSector.gameObject.transform.position = self.transform.position
      _SoundLastSector.spatialBlend = 1
      _SoundLastSector.volume = 1
    end))
  end
  

end
function PE_Balloon:Deactive()

  self.PE_Balloon_Platform_Tigger:ForceDePhysics()
  self.PE_Balloon_Platform:ForceDePhysics()
  self.PE_Balloon_Platte01:ForceDePhysics()
  self.PE_Balloon_Platte02:ForceDePhysics()
  self.PE_Balloon_Platte03:ForceDePhysics()
  self.PE_Balloon_Platte04:ForceDePhysics()
  self.PE_Balloon_Platte05:ForceDePhysics()
  self.PE_Balloon_Platte06:ForceDePhysics()
  self.PE_Balloon_Platte07:ForceDePhysics()
  self.PE_Balloon_Platte08:ForceDePhysics()
  self.PE_Balloon_BoxSide:ForceDePhysics()

  self.gameObject:SetActive(false)

  GamePlay.GamePlayManager._SoundLastSector:Stop()
end
function PE_Balloon:Reset(type)
  GamePlay.GamePlayManager._SoundLastSector:Stop()
  ObjectStateBackupUtils.RestoreObjectAndChilds(self.gameObject)
  if type == 'levelRestart' then
    self._MusicActived = false
  end
end
function PE_Balloon:Backup()
  ObjectStateBackupUtils.BackUpObjectAndChilds(self.gameObject)
end

function CreateClass_PE_Balloon()
  return PE_Balloon()
end