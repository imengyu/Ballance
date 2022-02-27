local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

---@class P_Modul_03 : ModulBase
---@field P_Modul_03_Floor PhysicsObject
---@field P_Modul_03_Gate PhysicsObject
---@field P_Modul_03_Wall01 PhysicsObject
---@field P_Modul_03_Wall02 PhysicsObject
---@field P_Modul_03_Wall03 PhysicsObject
---@field P_Modul_03_Wall04 PhysicsObject
---@field P_Modul_03_Wall05 PhysicsObject
---@field P_Modul_03_Wall06 PhysicsObject
---@field P_Modul_03_Wall07 PhysicsObject
P_Modul_03 = ModulBase:extend()

function P_Modul_03:new()
  P_Modul_03.super.new(self)
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 30
end

function P_Modul_03:Active()
  ModulBase.Active(self)
  self.P_Modul_03_Floor:Physicalize()
  self.P_Modul_03_Gate:Physicalize()
  self.P_Modul_03_Wall01:Physicalize()
  self.P_Modul_03_Wall02:Physicalize()
  self.P_Modul_03_Wall03:Physicalize()
  self.P_Modul_03_Wall04:Physicalize()
  self.P_Modul_03_Wall05:Physicalize()
  self.P_Modul_03_Wall06:Physicalize()
  self.P_Modul_03_Wall07:Physicalize()
  self.P_Modul_03_Floor.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('Wood')
end
function P_Modul_03:Deactive()
  self.P_Modul_03_Floor:UnPhysicalize(true)
  self.P_Modul_03_Gate:UnPhysicalize(true)
  self.P_Modul_03_Wall01:UnPhysicalize(true)
  self.P_Modul_03_Wall02:UnPhysicalize(true)
  self.P_Modul_03_Wall03:UnPhysicalize(true)
  self.P_Modul_03_Wall04:UnPhysicalize(true)
  self.P_Modul_03_Wall05:UnPhysicalize(true)
  self.P_Modul_03_Wall06:UnPhysicalize(true)
  self.P_Modul_03_Wall07:UnPhysicalize(true)
  ModulBase.Deactive(self)
end
function P_Modul_03:Reset()
  ObjectStateBackupUtils.RestoreObjectAndChilds(self.gameObject)
end
function P_Modul_03:Backup()
  ObjectStateBackupUtils.BackUpObjectAndChilds(self.gameObject)
end
function P_Modul_03:BallEnterRange()
  if self.IsActive then
    self.P_Modul_03_Floor:WakeUp()
  end
end

function CreateClass:P_Modul_03()
  return P_Modul_03()
end