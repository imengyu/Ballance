local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

---@class PE_Balloon : ModulBase 
---@field PE_Balloon_Platform_ColTest PhysicsBody
---@field PE_Balloon_Platform_HingeJoint HingeConstraint
---@field PE_Balloon_Platform PhysicsBody
---@field PE_Balloon_Platte01 PhysicsBody
---@field PE_Balloon_Platte02 PhysicsBody
---@field PE_Balloon_Platte03 PhysicsBody
---@field PE_Balloon_Platte04 PhysicsBody
---@field PE_Balloon_Platte05 PhysicsBody
---@field PE_Balloon_Platte06 PhysicsBody
---@field PE_Balloon_Platte07 PhysicsBody
---@field PE_Balloon_Platte08 PhysicsBody
---@field PE_Balloon_BoxSide PhysicsBody
PE_Balloon = ModulBase:extend()

function PE_Balloon:new()
end
function PE_Balloon:Start()
  self.PE_Balloon_Platform_ColTest.onCollisionEnter = function (body, other, info)
    if other and other.gameObject.tag == "Ball" then
      self.PE_Balloon_Platform_HingeJoint.enable = false --断开与桥的连接
      GamePlay.GamePlayManager:Pass() --通知管理器关卡已结束
    end
  end
end

function PE_Balloon:Active()
  self.PE_Balloon_Platform_HingeJoint.enable = true
  self.gameObject:SetActive(true)

  self.PE_Balloon_Platform_ColTest:ForcePhysics()
  self.PE_Balloon_Platform:ForcePhysics()
  self.PE_Balloon_Platte01:ForcePhysics()
  self.PE_Balloon_Platte02:ForcePhysics()
  self.PE_Balloon_Platte03:ForcePhysics()
  self.PE_Balloon_Platte04:ForcePhysics()
  self.PE_Balloon_Platte05:ForcePhysics()
  self.PE_Balloon_Platte06:ForcePhysics()
  self.PE_Balloon_Platte07:ForcePhysics()
  self.PE_Balloon_Platte08:ForcePhysics()
  self.PE_Balloon_BoxSide:ForcePhysics()

end
function PE_Balloon:Deactive()

  self.PE_Balloon_Platform_ColTest:ForceDePhysics()
  self.PE_Balloon_Platform:ForceDePhysics()
  self.PE_Balloon_Platte01:ForceDePhysics()
  self.PE_Balloon_Platte02:ForceDePhysics()
  self.PE_Balloon_Platte03:ForceDePhysics()
  self.PE_Balloon_Platte04:ForceDePhysics()
  self.PE_Balloon_Platte05:ForceDePhysics()
  self.PE_Balloon_Platte06:ForceDePhysics()
  self.PE_Balloon_Platte07:ForceDePhysics()
  self.PE_Balloon_Platte08:ForceDePhysics()
  self.PE_Balloon_BoxSide:ForceDePhysics()

  self.gameObject:SetActive(false)
end
function PE_Balloon:Reset()
  ObjectStateBackupUtils.RestoreObjectAndChilds(self.gameObject)
end
function PE_Balloon:Backup()
  ObjectStateBackupUtils.BackUpObjectAndChilds(self.gameObject)
end

function CreateClass_PE_Balloon()
  return PE_Balloon()
end