local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

---@class P_Modul_26 : ModulBase 
---@field P_Modul_26_Rope PhysicsObject
---@field P_Modul_26_Sack PhysicsObject
---@field P_Modul_26_ForcePovit_2 Transform
---@field P_Modul_26_ForcePovit_1 Transform
P_Modul_26 = ModulBase:extend()

function P_Modul_26:new()
  ModulBase.super.new(self)
  self._EnableForce = false
  self._ForceIsLeft = false
  self._ForceTick = 0
  self._ForceAppilyTick = 8 --(6/0.1s)
end

function P_Modul_26:FixedUpdate()
  if self._EnableForce then
    if self._ForceTick > 0 then
      self._ForceTick = self._ForceTick - 1
    else
      --切换方向
      self._ForceIsLeft = not self._ForceIsLeft
      self._ForceTick = self._ForceAppilyTick
      if self._ForceIsLeft then
        self.P_Modul_26_Sack.ConstantForceDirectionRef = self.P_Modul_26_ForcePovit_1
      else
        self.P_Modul_26_Sack.ConstantForceDirectionRef = self.P_Modul_26_ForcePovit_2
      end
    end
  end
end
function P_Modul_26:Active()
  ModulBase.Active(self)
  self.P_Modul_26_Rope:Physicalize()
  self.P_Modul_26_Sack:Physicalize()
  self.P_Modul_26_Sack.EnableConstantForce = true
  self._EnableForce = true
end
function P_Modul_26:Deactive()
  self.P_Modul_26_Sack.EnableConstantForce = false
  self._EnableForce = false
  self._ForceTick = 0
  self.P_Modul_26_Rope:UnPhysicalize(true)
  self.P_Modul_26_Sack:UnPhysicalize(true)
  ModulBase.Deactive(self)
end
function P_Modul_26:Reset()
  ObjectStateBackupUtils.RestoreObject(self.P_Modul_26_Rope.gameObject)
  ObjectStateBackupUtils.RestoreObject(self.P_Modul_26_Sack.gameObject)
  self._ForceIsLeft = false
  self._ForceTick = 0
end
function P_Modul_26:Backup()
  ObjectStateBackupUtils.BackUpObject(self.P_Modul_26_Rope.gameObject)
  ObjectStateBackupUtils.BackUpObject(self.P_Modul_26_Sack.gameObject)
end

function CreateClass:P_Modul_26()
  return P_Modul_26()
end