local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils
local GameSoundType = Ballance2.Sys.Services.GameSoundType

---@class P_Modul_29 : ModulBase 
---@field _P_Modul_29_Platte01 PhysicsBody
---@field _P_Modul_29_Platte02 PhysicsBody
---@field _P_Modul_29_Platte03 PhysicsBody
---@field _P_Modul_29_Platte04 PhysicsBody
---@field _P_Modul_29_Platte05 PhysicsBody
---@field _P_Modul_29_Platte05_HingeConstraint HingeConstraint
---@field _P_Modul_29_Platte06 PhysicsBody
---@field _P_Modul_29_Platte07 PhysicsBody
---@field _P_Modul_29_Platte08 PhysicsBody
---@field _P_Modul_29_Platte09 PhysicsBody
P_Modul_29 = ModulBase:extend()

function P_Modul_29:new()
  self._BrigeBreaked = false
end

function P_Modul_29:Start()
  ---@param body PhysicsBody
  ---@param otherBody PhysicsBody
  self._P_Modul_29_Platte05.onCollisionEnter = function (body, otherBody)
    --石球断开木桥
    if not self._BrigeBreaked and otherBody.gameObject.tag == 'Ball' and otherBody.gameObject.name ~= 'BallStone' then
      self._BrigeBreaked = true
      self._P_Modul_29_Platte05_HingeConstraint:SetEnabled(false)
      Game.SoundManager:PlayFastVoice('core.sounds:Misc_RopeTears.wav', GameSoundType.Normal)
    end
  end
end
function P_Modul_29:Active()
  self.gameObject:SetActive(true)
  self._P_Modul_29_Platte01:ForcePhysics()
  self._P_Modul_29_Platte02:ForcePhysics()
  self._P_Modul_29_Platte03:ForcePhysics()
  self._P_Modul_29_Platte04:ForcePhysics()
  self._P_Modul_29_Platte05:ForcePhysics()
  self._P_Modul_29_Platte06:ForcePhysics()
  self._P_Modul_29_Platte07:ForcePhysics()
  self._P_Modul_29_Platte08:ForcePhysics()
  self._P_Modul_29_Platte09:ForcePhysics()
end
function P_Modul_29:Deactive()
  self._P_Modul_29_Platte05_HingeConstraint:SetEnabled(true)
  self._P_Modul_29_Platte01:ForceDePhysics()
  self._P_Modul_29_Platte02:ForceDePhysics()
  self._P_Modul_29_Platte03:ForceDePhysics()
  self._P_Modul_29_Platte04:ForceDePhysics()
  self._P_Modul_29_Platte05:ForceDePhysics()
  self._P_Modul_29_Platte06:ForceDePhysics()
  self._P_Modul_29_Platte07:ForceDePhysics()
  self._P_Modul_29_Platte08:ForceDePhysics()
  self._P_Modul_29_Platte09:ForceDePhysics()
  self.gameObject:SetActive(false)
end
function P_Modul_29:Reset()
  self._P_Modul_29_Platte05_HingeConstraint:SetEnabled(true)
  ObjectStateBackupUtils.RestoreObjectAndChilds(self.gameObject)
  self._BrigeBreaked = false
end
function P_Modul_29:Backup()
  ObjectStateBackupUtils.BackUpObjectAndChilds(self.gameObject)
end

function CreateClass_P_Modul_29()
  return P_Modul_29()
end