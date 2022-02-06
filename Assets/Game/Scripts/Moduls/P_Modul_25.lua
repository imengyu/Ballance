local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

P_Modul_25 = ModulBase:extend()

---P_Modul_25
---推板机关
---@class P_Modul_25 : ModulBase
---@field P_Modul_25_Bridge PhysicsObject
function P_Modul_25:new()
  ModulBase.super.new(self)
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 60
end

function P_Modul_25:Active()
  ModulBase.Active(self)
  self.P_Modul_25_Bridge:Physicalize()
  self.P_Modul_25_Bridge.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('WoodOnlyHit')
end
function P_Modul_25:Deactive()
  self.P_Modul_25_Bridge:UnPhysicalize(true)
  ModulBase.Deactive(self)
end
function P_Modul_25:Reset()
  ObjectStateBackupUtils.RestoreObject(self.gameObject)
end
function P_Modul_25:Backup()
  ObjectStateBackupUtils.BackUpObject(self.gameObject)
end
function P_Modul_25:BallEnterRange()
  if self.IsActive then
    self.P_Modul_25_Bridge:WakeUp()
  end
end

function CreateClass:P_Modul_25()
  return P_Modul_25()
end