local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils
local GameSoundType = Ballance2.Services.GameSoundType

---@class P_Modul_29 : ModulBase 
---@field _P_Modul_29_Platte01 PhysicsObject
---@field _P_Modul_29_Platte02 PhysicsObject
---@field _P_Modul_29_Platte03 PhysicsObject
---@field _P_Modul_29_Platte04 PhysicsObject
---@field _P_Modul_29_Platte05 PhysicsObject
---@field _P_Modul_29_Platte06 PhysicsObject
---@field _P_Modul_29_Platte07 PhysicsObject
---@field _P_Modul_29_Platte08 PhysicsObject
---@field _P_Modul_29_Platte09 PhysicsObject
---@field _P_Modul_29_Platte05_HingeConstraint PhysicsHinge
---@field _P_Modul_29_Platte05_Tigger TiggerTester
P_Modul_29 = ModulBase:extend()

function P_Modul_29:new()
  P_Modul_29.super.new(self)
  self._BrigeBreaked = false
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 60
end

function P_Modul_29:Start()
  ModulBase.Start(self)
  ---@param other GameObject
  self._P_Modul_29_Platte05_Tigger.onTriggerEnter = function (_, other)
    --石球断开木桥
    if not self._BrigeBreaked and other.tag == 'Ball' and other.name == 'BallStone' then
      self._BrigeBreaked = true
      self._P_Modul_29_Platte05_HingeConstraint:Destroy()
      Game.SoundManager:PlayFastVoice('core.sounds:Misc_RopeTears.wav', GameSoundType.Normal)
    end
  end
end
function P_Modul_29:Active()
  ModulBase.Active(self)
  self._P_Modul_29_Platte01.gameObject:SetActive(true)
  self._P_Modul_29_Platte02.gameObject:SetActive(true)
  self._P_Modul_29_Platte03.gameObject:SetActive(true)
  self._P_Modul_29_Platte04.gameObject:SetActive(true)
  self._P_Modul_29_Platte05.gameObject:SetActive(true)
  self._P_Modul_29_Platte06.gameObject:SetActive(true)
  self._P_Modul_29_Platte07.gameObject:SetActive(true)
  self._P_Modul_29_Platte08.gameObject:SetActive(true)
  self._P_Modul_29_Platte09.gameObject:SetActive(true)
  self._P_Modul_29_Platte01:Physicalize()
  self._P_Modul_29_Platte02:Physicalize()
  self._P_Modul_29_Platte03:Physicalize()
  self._P_Modul_29_Platte04:Physicalize()
  self._P_Modul_29_Platte05:Physicalize()
  self._P_Modul_29_Platte06:Physicalize()
  self._P_Modul_29_Platte07:Physicalize()
  self._P_Modul_29_Platte08:Physicalize()
  self._P_Modul_29_Platte09:Physicalize()
end
function P_Modul_29:Deactive()
  self._P_Modul_29_Platte01:UnPhysicalize(true)
  self._P_Modul_29_Platte02:UnPhysicalize(true)
  self._P_Modul_29_Platte03:UnPhysicalize(true)
  self._P_Modul_29_Platte04:UnPhysicalize(true)
  self._P_Modul_29_Platte05:UnPhysicalize(true)
  self._P_Modul_29_Platte06:UnPhysicalize(true)
  self._P_Modul_29_Platte07:UnPhysicalize(true)
  self._P_Modul_29_Platte08:UnPhysicalize(true)
  self._P_Modul_29_Platte09:UnPhysicalize(true)
  ModulBase.Deactive(self)
end

function P_Modul_29:ActiveForPreview()
  self.gameObject:SetActive(true)
end
function P_Modul_29:DeactiveForPreview()
  self.gameObject:SetActive(false)
end

function P_Modul_29:Reset()
  ObjectStateBackupUtils.RestoreObjectAndChilds(self.gameObject)
  self._BrigeBreaked = false
end
function P_Modul_29:Backup()
  ObjectStateBackupUtils.BackUpObjectAndChilds(self.gameObject)
end
function P_Modul_29:BallEnterRange()
  if not self.IsPreviewMode and self.IsActive then
    self._P_Modul_29_Platte04:WakeUp()
  end
end

function CreateClass:P_Modul_29()
  return P_Modul_29()
end