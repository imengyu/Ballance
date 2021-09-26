local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

P_Modul_03 = ModulBase:extend()

---@class P_Modul_03 : ModulBase
---@field P_Modul_03_Floor PhysicsBody
---@field P_Modul_03_Gate PhysicsBody
---@field P_Modul_03_Wall01 PhysicsBody
---@field P_Modul_03_Wall02 PhysicsBody
---@field P_Modul_03_Wall03 PhysicsBody
---@field P_Modul_03_Wall04 PhysicsBody
---@field P_Modul_03_Wall05 PhysicsBody
---@field P_Modul_03_Wall06 PhysicsBody
---@field P_Modul_03_Wall07 PhysicsBody
function P_Modul_03:new()
end

function P_Modul_03:Active()
  self.gameObject:SetActive(true)
  self.P_Modul_03_Floor:ForcePhysics()
  self.P_Modul_03_Gate:ForcePhysics()
  self.P_Modul_03_Wall01:ForcePhysics()
  self.P_Modul_03_Wall02:ForcePhysics()
  self.P_Modul_03_Wall03:ForcePhysics()
  self.P_Modul_03_Wall04:ForcePhysics()
  self.P_Modul_03_Wall05:ForcePhysics()
  self.P_Modul_03_Wall06:ForcePhysics()
  self.P_Modul_03_Wall07:ForcePhysics()
end
function P_Modul_03:Deactive()
  self.P_Modul_03_Floor:ForceDePhysics()
  self.P_Modul_03_Gate:ForceDePhysics()
  self.P_Modul_03_Wall01:ForceDePhysics()
  self.P_Modul_03_Wall02:ForceDePhysics()
  self.P_Modul_03_Wall03:ForceDePhysics()
  self.P_Modul_03_Wall04:ForceDePhysics()
  self.P_Modul_03_Wall05:ForceDePhysics()
  self.P_Modul_03_Wall06:ForceDePhysics()
  self.P_Modul_03_Wall07:ForceDePhysics()
  self.gameObject:SetActive(false)
end
function P_Modul_03:Reset()
  ObjectStateBackupUtils.RestoreObject(self.gameObject)
end
function P_Modul_03:Backup()
  ObjectStateBackupUtils.BackUpObject(self.gameObject)
end

function CreateClass_P_Modul_03()
  return P_Modul_03()
end