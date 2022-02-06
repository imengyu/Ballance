local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

---带物理的机关基类，你可用于只有一个物体的简单物理机关
---@class ModulPhysics : ModulBase 
---@field _PhysicsObject PhysicsObject 物理物体，请在编辑器中设置此字段
---@field _PhysicsObjectCollIDName string 设置物理物体球碰撞声音组，可以在编辑器中设置此字段
ModulPhysics = ModulBase:extend()

function ModulPhysics:new()
  ModulBase.super.new(self)
  self._PhysicsObject = nil
  self._PhysicsObjectCollIDName = nil
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 50
end
function ModulPhysics:Start()
  if self._PhysicsObject == nil then 
    self._PhysicsObject = self.gameObject:GetComponent(BallancePhysics.Wapper.PhysicsObject) ---@type PhysicsObject
  end
  if not IsNilOrEmpty(self._PhysicsObjectCollIDName) then
    self._PhysicsObject.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName(self._PhysicsObjectCollIDName)
  end
end
function ModulPhysics:BallEnterRange()
  if self._PhysicsObject == nil and self._PhysicsObject.IsPhysicalized then 
    self._PhysicsObject:WakeUp()
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