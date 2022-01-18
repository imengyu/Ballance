local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

P_Modul_30 = ModulBase:extend()

---P_Modul_30
---跷跷板机关
---@class P_Modul_30 : ModulBase
---@field P_Modul_30_Wippe PhysicsObject
function P_Modul_30:new()
  ModulBase.super.new(self)
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 60
end

function P_Modul_30:Active()
  ModulBase.Active(self)
  self.P_Modul_30_Wippe:Physicalize()
  self.P_Modul_30_Wippe.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('Wood')
end
function P_Modul_30:Deactive()
  self.P_Modul_30_Wippe:UnPhysicalize(true)
  ModulBase.Deactive(self)
end
function P_Modul_30:Reset()
  ObjectStateBackupUtils.RestoreObject(self.gameObject)
end
function P_Modul_30:Backup()
  ObjectStateBackupUtils.BackUpObject(self.gameObject)
end
function P_Modul_30:BallEnterRange()
  if self.IsActive then
    self.P_Modul_30_Wippe:WakeUp()
  end
end

function CreateClass:P_Modul_30()
  return P_Modul_30()
end