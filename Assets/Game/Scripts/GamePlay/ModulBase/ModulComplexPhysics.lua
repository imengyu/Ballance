local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

---@class ModulComplexPhysics : ModulBase 
ModulComplexPhysics = ModulBase:extend()

function ModulComplexPhysics:new()
  self._PhysicsBody1 = nil ---@type PhysicsBody
  self._PhysicsBody2 = nil ---@type PhysicsBody
  self._PhysicsBody3 = nil ---@type PhysicsBody
  self._PhysicsBody4 = nil ---@type PhysicsBody
end

function ModulComplexPhysics:Active()
  self.gameObject:SetActive(true)
  self._PhysicsBody1:ForcePhysics()
  self._PhysicsBody2:ForcePhysics()
  if self._PhysicsBody3 ~= nil then self._PhysicsBody3:ForcePhysics() end
  if self._PhysicsBody4 ~= nil then self._PhysicsBody4:ForcePhysics() end
end
function ModulComplexPhysics:Deactive()
  self.gameObject:SetActive(false)
  self._PhysicsBody1:ForceDePhysics()
  self._PhysicsBody2:ForceDePhysics()
  if self._PhysicsBody3 ~= nil then self._PhysicsBody3:ForceDePhysics() end
  if self._PhysicsBody4 ~= nil then self._PhysicsBody4:ForceDePhysics() end
end
function ModulComplexPhysics:Reset()
  ObjectStateBackupUtils.RestoreObject(self._PhysicsBody1.gameObject)
  ObjectStateBackupUtils.RestoreObject(self._PhysicsBody2.gameObject)
  if self._PhysicsBody3 ~= nil then ObjectStateBackupUtils.RestoreObject(self._PhysicsBody3.gameObject) end
  if self._PhysicsBody4 ~= nil then ObjectStateBackupUtils.RestoreObject(self._PhysicsBody4.gameObject) end
end
function ModulComplexPhysics:Backup()
  ObjectStateBackupUtils.BackUpObject(self._PhysicsBody1.gameObject)
  ObjectStateBackupUtils.BackUpObject(self._PhysicsBody2.gameObject)
  if self._PhysicsBody3 ~= nil then ObjectStateBackupUtils.BackUpObject(self._PhysicsBody3.gameObject) end
  if self._PhysicsBody4 ~= nil then ObjectStateBackupUtils.BackUpObject(self._PhysicsBody4.gameObject) end
end

function CreateClass_ModulComplexPhysics()
  return ModulComplexPhysics()
end