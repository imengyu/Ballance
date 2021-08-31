local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

---@class ModulComplexPhysics : ModulBase 
ModulComplexPhysics = ModulBase:extend()

function ModulComplexPhysics:new()
  self._PhysicsBody1 = nil ---@type PhysicsBody
  self._PhysicsBody2 = nil ---@type PhysicsBody
  self._PhysicsBody3 = nil ---@type PhysicsBody
  self._PhysicsBody4 = nil ---@type PhysicsBody
  self._PhysicsBody1CustomLayerName = nil
  self._PhysicsBody2CustomLayerName = nil
  self._PhysicsBody3CustomLayerName = nil
  self._PhysicsBody4CustomLayerName = nil
end
function ModulComplexPhysics:Start()
  if not IsNilOrEmpty(self._PhysicsBody1CustomLayerName) then
    self._PhysicsBody1.CustomLayer = GamePlay.BallSoundManager:GetCustomSoundLayerByName(self._PhysicsBody1CustomLayerName)
  end
  if not IsNilOrEmpty(self._PhysicsBody2CustomLayerName) then
    self._PhysicsBody2.CustomLayer = GamePlay.BallSoundManager:GetCustomSoundLayerByName(self._PhysicsBody2CustomLayerName)
  end
  if self._PhysicsBody3 ~= nil then 
    self._PhysicsBody3:ForcePhysics() 
    if not IsNilOrEmpty(self._PhysicsBody3CustomLayerName) then
      self._PhysicsBody3.CustomLayer = GamePlay.BallSoundManager:GetCustomSoundLayerByName(self._PhysicsBody3CustomLayerName)
    end
  end
  if self._PhysicsBody4 ~= nil then 
    self._PhysicsBody4:ForcePhysics() 
    if not IsNilOrEmpty(self._PhysicsBody4CustomLayerName) then
      self._PhysicsBody4.CustomLayer = GamePlay.BallSoundManager:GetCustomSoundLayerByName(self._PhysicsBody4CustomLayerName)
    end
  end
end
function ModulComplexPhysics:Active()
  self.gameObject:SetActive(true)
  self._PhysicsBody1:ForcePhysics()
  self._PhysicsBody2:ForcePhysics()
  if self._PhysicsBody3 ~= nil then 
    self._PhysicsBody3:ForcePhysics() 
  end
  if self._PhysicsBody4 ~= nil then 
    self._PhysicsBody4:ForcePhysics() 
  end
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