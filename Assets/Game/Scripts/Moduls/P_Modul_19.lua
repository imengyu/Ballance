local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

---P_Modul_19
---双向推板机关
---@class P_Modul_19 : ModulBase
---@field P_Modul_19_Flaps PhysicsObject
P_Modul_19 = ModulBase:extend()

function P_Modul_19:new()
  ModulBase.super.new(self)
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 60
end

function P_Modul_19:Active()
  ModulBase.Active(self)
  self.P_Modul_19_Flaps:Physicalize()
  self.P_Modul_19_Flaps.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('Wood')
end
function P_Modul_19:Deactive()
  self.P_Modul_19_Flaps:UnPhysicalize(true)
  ModulBase.Deactive(self)
end
function P_Modul_19:Reset()
  ObjectStateBackupUtils.RestoreObject(self.gameObject)
end
function P_Modul_19:Backup()
  ObjectStateBackupUtils.BackUpObject(self.gameObject)
end
function P_Modul_19:BallEnterRange()
  if self.IsActive then
    self.P_Modul_19_Flaps:WakeUp()
  end
end

function CreateClass:P_Modul_19()
  return P_Modul_19()
end