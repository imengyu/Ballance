local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

---@class P_Modul_17 : ModulBase 
---@field Modul17_Dreharme PhysicsObject
P_Modul_17 = ModulBase:extend()

function P_Modul_17:new()
  P_Modul_17.super.new(self)
end

function P_Modul_17:Active()
  ModulBase.Active(self)
  self.Modul17_Dreharme:Physicalize()
  self.Modul17_Dreharme.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('WoodOnlyHit')
  self._EnableForce = true
end
function P_Modul_17:Deactive()
  self.Modul17_Dreharme:UnPhysicalize(true)
  ModulBase.Deactive(self)
end

function P_Modul_17:ActiveForPreview()
  self.gameObject:SetActive(true)
end
function P_Modul_17:DeactiveForPreview()
  self.gameObject:SetActive(false)
end

function P_Modul_17:Reset()
  ObjectStateBackupUtils.RestoreObject(self.Modul17_Dreharme.gameObject)
  self._ForceIsLeft = false
  self._ForceTick = 0
end
function P_Modul_17:Backup()
  ObjectStateBackupUtils.BackUpObject(self.Modul17_Dreharme.gameObject)
end

function CreateClass:P_Modul_17()
  return P_Modul_17()
end