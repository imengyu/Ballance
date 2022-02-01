local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

---P_Modul_41
---浮块机关
---@class P_Modul_41 : ModulBase
---@field P_Modul_41_Box PhysicsObject
P_Modul_41 = ModulBase:extend()

function P_Modul_41:new()
  ModulBase.super.new(self)
end

function P_Modul_41:Active()
  ModulBase.Active(self)
  self.P_Modul_41_Box:Physicalize()
  self.P_Modul_41_Box.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('WoodOnlyHit')
end
function P_Modul_41:Deactive()
  self.P_Modul_41_Box:UnPhysicalize(true)
  ModulBase.Deactive(self)
end
function P_Modul_41:Reset()
  ObjectStateBackupUtils.RestoreObject(self.gameObject)
end
function P_Modul_41:Backup()
  ObjectStateBackupUtils.BackUpObject(self.gameObject)
end

function CreateClass:P_Modul_41()
  return P_Modul_41()
end