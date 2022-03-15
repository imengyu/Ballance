local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

---P_Modul_30
---跷跷板机关
---@class P_Modul_30 : ModulBase
---@field P_Modul_30_Wippe PhysicsObject
P_Modul_30 = ModulBase:extend()

function P_Modul_30:new()
  P_Modul_30.super.new(self)
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 60
end

function P_Modul_30:Active()
  ModulBase.Active(self)
  self.P_Modul_30_Wippe.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('Wood')
  self.P_Modul_30_Wippe:Physicalize()
end
function P_Modul_30:Deactive()
  self.P_Modul_30_Wippe:UnPhysicalize(true)
  ModulBase.Deactive(self)
end

function P_Modul_30:ActiveForPreview()
  self.gameObject:SetActive(true)
end
function P_Modul_30:DeactiveForPreview()
  self.gameObject:SetActive(false)
end

function P_Modul_30:Reset()
  ObjectStateBackupUtils.RestoreObject(self.gameObject)
end
function P_Modul_30:Backup()
  ObjectStateBackupUtils.BackUpObject(self.gameObject)
end
function P_Modul_30:BallEnterRange()
  if not self.IsPreviewMode and self.IsActive then
    self.P_Modul_30_Wippe:WakeUp()
  end
end

function CreateClass:P_Modul_30()
  return P_Modul_30()
end