local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

---P_Modul_25
---推板机关
---@class P_Modul_25 : ModulBase
---@field P_Modul_25_Bridge PhysicsObject
---@field P_Modul_25_Bridge_Stopper_Left PhysicsObject
---@field P_Modul_25_Bridge_Stopper_Right PhysicsObject
---@field P_Modul_25_Hinge_Col_Left PhysicsObject
---@field P_Modul_25_Hinge_Col_Right PhysicsObject
P_Modul_25 = ModulBase:extend()

function P_Modul_25:new()
  P_Modul_25.super.new(self)
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 60
end

function P_Modul_25:Active()
  ModulBase.Active(self)
  self.P_Modul_25_Bridge.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('WoodOnlyHit')
  self.P_Modul_25_Bridge:Physicalize()
  self.P_Modul_25_Bridge_Stopper_Left:Physicalize()
  self.P_Modul_25_Bridge_Stopper_Right:Physicalize()
  self.P_Modul_25_Hinge_Col_Left:Physicalize()
  self.P_Modul_25_Hinge_Col_Right:Physicalize()
end
function P_Modul_25:Deactive()
  self.P_Modul_25_Bridge:UnPhysicalize(true)
  self.P_Modul_25_Hinge_Col_Left:UnPhysicalize(true)
  self.P_Modul_25_Hinge_Col_Right:UnPhysicalize(true)
  self.P_Modul_25_Bridge_Stopper_Left:UnPhysicalize(true)
  self.P_Modul_25_Bridge_Stopper_Right:UnPhysicalize(true)
  ModulBase.Deactive(self)
end

function P_Modul_25:ActiveForPreview()
  self.gameObject:SetActive(true)
end
function P_Modul_25:DeactiveForPreview()
  self.gameObject:SetActive(false)
end

function P_Modul_25:Reset()
  ObjectStateBackupUtils.RestoreObjectAndChilds(self.gameObject)
end
function P_Modul_25:Backup()
  ObjectStateBackupUtils.BackUpObjectAndChilds(self.gameObject)
end
function P_Modul_25:BallEnterRange()
  if self.IsActive then
    self.P_Modul_25_Bridge:WakeUp()
  end
end

function CreateClass:P_Modul_25()
  return P_Modul_25()
end