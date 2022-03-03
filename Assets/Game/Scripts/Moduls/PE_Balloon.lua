local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils
local DistanceChecker = Ballance2.Game.DistanceChecker
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local Log = Ballance2.Log

---@class PE_Balloon : ModulBase 
---@field PE_Balloon_BoxSlide PhysicsObject
---@field PE_Balloon_BoxSlideForce PhysicsConstraintForce
---@field PE_Balloon_PlatformForce PhysicsConstraintForce
---@field PE_Balloon_Platform PhysicsObject
---@field PE_Balloon_Ballon04 PhysicsObject
---@field PE_Balloon_Ballon03 PhysicsObject
---@field PE_Balloon_Ballon02 PhysicsObject
---@field PE_Balloon_Ballon01 PhysicsObject
---@field PE_Balloon_Ballon_Seil04 PhysicsObject
---@field PE_Balloon_Ballon_Seil03 PhysicsObject
---@field PE_Balloon_Ballon_Seil02 PhysicsObject
---@field PE_Balloon_Ballon_Seil01 PhysicsObject
---@field PE_Balloon_Platform_HingeJoint PhysicsHinge
---@field PE_Balloon_Platte00 PhysicsObject
---@field PE_Balloon_Platte01 PhysicsObject
---@field PE_Balloon_Platte02 PhysicsObject
---@field PE_Balloon_Platte03 PhysicsObject
---@field PE_Balloon_Platte04 PhysicsObject
---@field PE_Balloon_Platte05 PhysicsObject
---@field PE_Balloon_Platte06 PhysicsObject
---@field PE_Balloon_Platte07 PhysicsObject
---@field PE_Balloon_Platte08 PhysicsObject
---@field PE_Balloon_BallRefPos GameObject
---@field PE_Balloon_BallTigger TiggerTester
PE_Balloon = ModulBase:extend()

function PE_Balloon:new()
  PE_Balloon.super.new(self)
  self.BallTiggerActived = false
end
function PE_Balloon:Start()
  ModulBase.Start(self)

  --接近PE_Balloon时禁用背景音乐
  local musicDisabledByPE_Balloon = false
  local distanceChecker = self.PE_Balloon_Platform:GetComponent(DistanceChecker) ---@type DistanceChecker
  distanceChecker.Object2 = GamePlay.BallManager.PosFrame
  distanceChecker.OnEnterRange = function () 
    if self._MusicActived then
      musicDisabledByPE_Balloon = true
      GamePlay.MusicManager:DisableBackgroundMusic() 
    end
    if self.IsActive then
      self.PE_Balloon_Platform:WakeUp()
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
  self.PE_Balloon_BallTigger.onTriggerEnter = function (body, other)
    if other and other.gameObject.tag == "Ball" and not self.BallTiggerActived then

      Log.D('PE_Balloon', 'Break bridge!') 

      self.BallTiggerActived = true
      self._MusicActived = false

      self.PE_Balloon_Platform_HingeJoint:Destroy() --断开与桥的连接
      self.PE_Balloon_Platform:WakeUp()
      self.PE_Balloon_PlatformForce.enabled = false
      self.PE_Balloon_BoxSlideForce.enabled = false

      --速度放慢一些
      LuaTimer.Add(200, function ()
        self.PE_Balloon_BoxSlideForce.enabled = true
        self.PE_Balloon_BoxSlideForce.Force = 0.8

        --速度再放慢一些
        LuaTimer.Add(300, function ()
          self.PE_Balloon_BoxSlideForce.Force = 0.4

          LuaTimer.Add(600, function ()
            self.PE_Balloon_BoxSlideForce.Force = 0.3
          end)
          --速度再放慢一些
          LuaTimer.Add(40000, function ()
            self.PE_Balloon_BoxSlideForce.Force = 0.2
          end)
        end)
      end)

      if not BALLANCE_MODUL_DEBUG then
        GamePlay.GamePlayManager:Pass() --通知管理器关卡已结束
      end
    end
  end
  self._MusicActived = false

  local iWoodOnlyHit = GamePlay.BallSoundManager:GetSoundCollIDByName('WoodOnlyHit')
  self.PE_Balloon_Platte00.CollisionID = iWoodOnlyHit
  self.PE_Balloon_Platte01.CollisionID = iWoodOnlyHit
  self.PE_Balloon_Platte02.CollisionID = iWoodOnlyHit
  self.PE_Balloon_Platte03.CollisionID = iWoodOnlyHit
  self.PE_Balloon_Platte04.CollisionID = iWoodOnlyHit
  self.PE_Balloon_Platte05.CollisionID = iWoodOnlyHit
  self.PE_Balloon_Platte06.CollisionID = iWoodOnlyHit
  self.PE_Balloon_Platte07.CollisionID = iWoodOnlyHit
  self.PE_Balloon_Platte08.CollisionID = iWoodOnlyHit
  self.PE_Balloon_Platform.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('Wood')
end

function PE_Balloon:Active()
  ModulBase.Active(self)

  self.BallTiggerActived = false
  self.PE_Balloon_Platform:Physicalize()
  self.PE_Balloon_PlatformForce.enabled = true
  self.PE_Balloon_PlatformForce.Force = 0.75
  self.PE_Balloon_BoxSlide:Physicalize()
  self.PE_Balloon_BoxSlideForce.enabled = true
  self.PE_Balloon_BoxSlideForce.Force = 0.65
  self.PE_Balloon_Ballon04:Physicalize()
  self.PE_Balloon_Ballon03:Physicalize()
  self.PE_Balloon_Ballon02:Physicalize()
  self.PE_Balloon_Ballon01:Physicalize()
  self.PE_Balloon_Ballon_Seil04:Physicalize()
  self.PE_Balloon_Ballon_Seil03:Physicalize()
  self.PE_Balloon_Ballon_Seil02:Physicalize()
  self.PE_Balloon_Ballon_Seil01:Physicalize()
  self.PE_Balloon_Platte00:Physicalize()
  self.PE_Balloon_Platte01:Physicalize()
  self.PE_Balloon_Platte02:Physicalize()
  self.PE_Balloon_Platte03:Physicalize()
  self.PE_Balloon_Platte04:Physicalize()
  self.PE_Balloon_Platte05:Physicalize()
  self.PE_Balloon_Platte06:Physicalize()
  self.PE_Balloon_Platte07:Physicalize()
  self.PE_Balloon_Platte08:Physicalize()

  if BALLANCE_MODUL_DEBUG then
    Log.D('PE_Balloon', 'Active!')
    Log.D('PE_Balloon', 'State: '..tostring(self._MusicActived)..' CurrentSector: '..tostring(GamePlay.GamePlayManager.CurrentSector) )
  end

  if not self._MusicActived and GamePlay.GamePlayManager.CurrentSector ~= 1 then
    
    if BALLANCE_MODUL_DEBUG then
      Log.D('PE_Balloon', '_MusicActived !')
    end

    self._MusicActived = true
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

  self.PE_Balloon_BoxSlide:UnPhysicalize(true)
  self.PE_Balloon_Ballon04:UnPhysicalize(true)
  self.PE_Balloon_Ballon03:UnPhysicalize(true)
  self.PE_Balloon_Ballon02:UnPhysicalize(true)
  self.PE_Balloon_Ballon01:UnPhysicalize(true)
  self.PE_Balloon_Ballon_Seil04:UnPhysicalize(true)
  self.PE_Balloon_Ballon_Seil03:UnPhysicalize(true)
  self.PE_Balloon_Ballon_Seil02:UnPhysicalize(true)
  self.PE_Balloon_Ballon_Seil01:UnPhysicalize(true)
  self.PE_Balloon_Platte00:UnPhysicalize(true)
  self.PE_Balloon_Platte01:UnPhysicalize(true)
  self.PE_Balloon_Platte02:UnPhysicalize(true)
  self.PE_Balloon_Platte03:UnPhysicalize(true)
  self.PE_Balloon_Platte04:UnPhysicalize(true)
  self.PE_Balloon_Platte05:UnPhysicalize(true)
  self.PE_Balloon_Platte06:UnPhysicalize(true)
  self.PE_Balloon_Platte07:UnPhysicalize(true)
  self.PE_Balloon_Platte08:UnPhysicalize(true)
  self.PE_Balloon_Platform:UnPhysicalize(true)

  GamePlay.GamePlayManager._SoundLastSector:Stop()

  ModulBase.Deactive(self)
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
function PE_Balloon:Custom(index)
  if index == 1 then
    self.PE_Balloon_Platform_HingeJoint:Destroy() --断开与桥的连接
    self.PE_Balloon_Platform:WakeUp()
    Log.D('PE_Balloon', 'Break bridge!')
  elseif index == 2 then
    Log.D('PE_Balloon', 'Test Pass!')
    GamePlay.GamePlayManager:Pass() --通知管理器关卡已结束
  end
end

function CreateClass:PE_Balloon()
  return PE_Balloon()
end