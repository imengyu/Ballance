local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

---@class ModulSingalPhysics : ModulBase 
ModulSingalPhysics = ModulBase:extend()

function ModulSingalPhysics:new()
  self._PhysicsBody = nil
  self._PhysicsBodyCustomLayerName = nil
end
function ModulSingalPhysics:Start()
  if self._PhysicsBody == nil then 
    self._PhysicsBody = self.gameObject:GetComponent(PhysicsRT.PhysicsBody) ---@type PhysicsBody
  end
  if not IsNilOrEmpty(self._PhysicsBodyCustomLayerName) then
    self._PhysicsBody.CustomLayer = GamePlay.BallSoundManager:GetCustomSoundLayerByName(self._PhysicsBodyCustomLayerName)
  end
end

function ModulSingalPhysics:GamePause() 
  self._PhysicsBody:ForceDeactive()
end
function ModulSingalPhysics:GameResume()
  self._PhysicsBody:ForceActive()
end
function ModulSingalPhysics:Active()
  self.gameObject:SetActive(true)
  self._PhysicsBody:ForcePhysics()
end
function ModulSingalPhysics:Deactive()
  self.gameObject:SetActive(false)
  self._PhysicsBody:ForceDePhysics()
end
function ModulSingalPhysics:Reset()
  ObjectStateBackupUtils.RestoreObject(self._PhysicsBody.gameObject)
end
function ModulSingalPhysics:Backup()
  ObjectStateBackupUtils.BackUpObject(self._PhysicsBody.gameObject)
end

function CreateClass_ModulSingalPhysics()
  return ModulSingalPhysics()
end