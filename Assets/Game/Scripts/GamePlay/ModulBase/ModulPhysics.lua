local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

---@class ModulPhysics : ModulBase 
ModulPhysics = ModulBase:extend()

function ModulPhysics:new()
  ModulBase.super.new(self)
  self._PhysicsObject = nil
  self._PhysicsObjectCustomLayerName = nil
end
function ModulPhysics:Start()
  if self._PhysicsObject == nil then 
    self._PhysicsObject = self.gameObject:GetComponent(PhysicsRT.PhysicsObject) ---@type PhysicsObject
  end
  if not IsNilOrEmpty(self._PhysicsObjectCustomLayerName) then
    self._PhysicsObject.CustomLayer = GamePlay.BallSoundManager:GetCustomSoundLayerByName(self._PhysicsObjectCustomLayerName)
  end
end

function ModulPhysics:Active()
  self.gameObject:SetActive(true)
  self._PhysicsObject:Physicalize()
end
function ModulPhysics:Deactive()
  self.gameObject:SetActive(false)
  self._PhysicsObject:UnPhysicalize(true)
end
function ModulPhysics:Reset()
  ObjectStateBackupUtils.RestoreObject(self._PhysicsObject.gameObject)
end
function ModulPhysics:Backup()
  ObjectStateBackupUtils.BackUpObject(self._PhysicsObject.gameObject)
end

function CreateClass:ModulPhysics()
  return ModulPhysics()
end