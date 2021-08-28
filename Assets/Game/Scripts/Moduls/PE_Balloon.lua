local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local AudioRolloffMode = UnityEngine.AudioRolloffMode

---@class PE_Balloon : ModulBase 
---@field PE_Balloon_Platform_ColTest PhysicsBody
---@field PE_Balloon_Platform_HingeJoint HingeConstraint
---@field PE_Balloon_Platform PhysicsBody
---@field PE_Balloon_Platte01 PhysicsBody
---@field PE_Balloon_Platte02 PhysicsBody
---@field PE_Balloon_Platte03 PhysicsBody
---@field PE_Balloon_Platte04 PhysicsBody
---@field PE_Balloon_Platte05 PhysicsBody
---@field PE_Balloon_Platte06 PhysicsBody
---@field PE_Balloon_Platte07 PhysicsBody
---@field PE_Balloon_Platte08 PhysicsBody
---@field PE_Balloon_BoxSide PhysicsBody
PE_Balloon = ModulBase:extend()

function PE_Balloon:new()
end
function PE_Balloon:Start()
  self.PE_Balloon_Platform_ColTest.onCollisionEnter = function (body, other, info)
    if other and other.gameObject.tag == "Ball" then
      self.PE_Balloon_Platform_HingeJoint:SetEnabled(false) --断开与桥的连接
      GamePlay.GamePlayManager:Pass() --通知管理器关卡已结束
    end
  end
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
  self.PE_Balloon_Platform_ColTest:ForcePhysics()
  self.PE_Balloon_Platform:ForcePhysics()
  self.PE_Balloon_Platform_HingeJoint:SetEnabled(true)

  --播放最后一小节的音乐
  coroutine.resume(coroutine.create(function()
    local _SoundLastSector = GamePlay.GamePlayManager._SoundLastSector
    Yield(WaitForSeconds(1))
    --设置为无衰减
    _SoundLastSector.rolloffMode = AudioRolloffMode.Logarithmic
    _SoundLastSector.maxDistance = 2000
    _SoundLastSector:Play()
    Yield(WaitForSeconds(5))
    --该音乐播放5秒后淡出
    Game.UIManager.UIFadeManager:AddAudioFadeOut(_SoundLastSector, 4)
    Yield(WaitForSeconds(6))
    _SoundLastSector.volume = 1
    --播放一次完成之后设置为100m范围衰减，位置为当前飞船位置
    _SoundLastSector.gameObject.transform.position = self.transform.position
    _SoundLastSector.rolloffMode = AudioRolloffMode.Linear
    _SoundLastSector.maxDistance = 100
  end))

end
function PE_Balloon:Deactive()

  self.PE_Balloon_Platform_ColTest:ForceDePhysics()
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
function PE_Balloon:Reset()
  GamePlay.GamePlayManager._SoundLastSector:Stop()
  ObjectStateBackupUtils.RestoreObjectAndChilds(self.gameObject)
end
function PE_Balloon:Backup()
  ObjectStateBackupUtils.BackUpObjectAndChilds(self.gameObject)
end

function CreateClass_PE_Balloon()
  return PE_Balloon()
end