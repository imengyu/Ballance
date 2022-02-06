local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

---@class P_Modul_08 : ModulBase 
---@field P_Modul_08_Schaukel PhysicsObject
---@field P_Modul_08_ForcePovit_2 Transform
---@field P_Modul_08_ForcePovit_1 Transform
P_Modul_08 = ModulBase:extend()

function P_Modul_08:new()
  ModulBase.super.new(self)
  self._EnableForce = false
  self._ForceIsLeft = false
  self._ForceTick = 0
  self._ForceAppilyTick = 10 --(6/0.1s)
end

function P_Modul_08:FixedUpdate()
  if self._EnableForce then
    if self._ForceTick > 0 then
      self._ForceTick = self._ForceTick - 1
    else
      --切换方向
      self._ForceIsLeft = not self._ForceIsLeft
      self._ForceTick = self._ForceAppilyTick
      if self._ForceIsLeft then
        self.P_Modul_08_Schaukel.ConstantForceDirectionRef = self.P_Modul_08_ForcePovit_1
      else
        self.P_Modul_08_Schaukel.ConstantForceDirectionRef = self.P_Modul_08_ForcePovit_2
      end
    end
  end
end
function P_Modul_08:Active()
  ModulBase.Active(self)
  self.P_Modul_08_Schaukel:Physicalize()
  self.P_Modul_08_Schaukel.EnableConstantForce = true
  self._EnableForce = true
end
function P_Modul_08:Deactive()
  self.P_Modul_08_Schaukel.EnableConstantForce = false
  self._EnableForce = false
  self._ForceTick = 0
  self.P_Modul_08_Schaukel:UnPhysicalize(true)
  ModulBase.Deactive(self)
end
function P_Modul_08:Reset()
  ObjectStateBackupUtils.RestoreObject(self.P_Modul_08_Schaukel.gameObject)
  self._ForceIsLeft = false
  self._ForceTick = 0
end
function P_Modul_08:Backup()
  ObjectStateBackupUtils.BackUpObject(self.P_Modul_08_Schaukel.gameObject)
end

function CreateClass:P_Modul_08()
  return P_Modul_08()
end