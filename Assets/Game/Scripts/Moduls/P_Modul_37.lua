local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

---P_Modul_37
---T推板机关
---@class P_Modul_37 : ModulBase
---@field P_Modul_37_Bridge PhysicsObject
P_Modul_37 = ModulBase:extend()

function P_Modul_37:new()
  P_Modul_37.super.new(self)
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 60
end

function P_Modul_37:Active()
  ModulBase.Active(self)
  self.P_Modul_37_Bridge.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('Wood')
  self.P_Modul_37_Bridge:Physicalize()
end
function P_Modul_37:Deactive()
  self.P_Modul_37_Bridge:UnPhysicalize(true)
  ModulBase.Deactive(self)
end

function P_Modul_37:ActiveForPreview()
  self.gameObject:SetActive(true)
end
function P_Modul_37:DeactiveForPreview()
  self.gameObject:SetActive(false)
end

function P_Modul_37:Reset()
  ObjectStateBackupUtils.RestoreObjectAndChilds(self.gameObject)
end
function P_Modul_37:Backup()
  ObjectStateBackupUtils.BackUpObjectAndChilds(self.gameObject)
end
function P_Modul_37:BallEnterRange()
  if not self.IsPreviewMode and self.IsActive then
    self.P_Modul_37_Bridge:WakeUp()
  end
end

function CreateClass:P_Modul_37()
  return P_Modul_37()
end