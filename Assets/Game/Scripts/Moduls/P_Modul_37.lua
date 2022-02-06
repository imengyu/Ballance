local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

P_Modul_37 = ModulBase:extend()

---P_Modul_37
---T推板机关
---@class P_Modul_37 : ModulBase
---@field P_Modul_37_Bridge PhysicsObject
function P_Modul_37:new()
  ModulBase.super.new(self)
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 60
end

function P_Modul_37:Active()
  ModulBase.Active(self)
  self.P_Modul_37_Bridge:Physicalize()
  self.P_Modul_37_Bridge.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('Wood')
end
function P_Modul_37:Deactive()
  self.P_Modul_37_Bridge:UnPhysicalize(true)
  ModulBase.Deactive(self)
end
function P_Modul_37:Reset()
  ObjectStateBackupUtils.RestoreObject(self.gameObject)
end
function P_Modul_37:Backup()
  ObjectStateBackupUtils.BackUpObject(self.gameObject)
end
function P_Modul_37:BallEnterRange()
  if self.IsActive then
    self.P_Modul_37_Bridge:WakeUp()
  end
end

function CreateClass:P_Modul_37()
  return P_Modul_37()
end