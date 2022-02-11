local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

---@class P_Modul_08 : ModulBase 
---@field P_Modul_08_Schaukel PhysicsObject
---@field P_Modul_08_Schaukel_Force PhysicsConstraintForce
P_Modul_08 = ModulBase:extend()

function P_Modul_08:new()
  P_Modul_08.super.new(self)
  self._EnableForce = false
  self._ForceTimer = nil
  self._ForceState = 0 --0/2 none 1 left 3 right
end
function P_Modul_08:Active()
  ModulBase.Active(self)
  self.P_Modul_08_Schaukel:Physicalize()
  self.P_Modul_08_Schaukel.EnableConstantForce = true
  self._EnableForce = true
  self._ForceTimer = LuaTimer.Add(500, 500, function ()
    self._ForceState = self._ForceState + 1
    if self._ForceState > 3 then
      self._ForceState = 0
    end

    --切换方向
    if self._ForceState == 0 or self._ForceState == 2  then
      self.P_Modul_08_Schaukel_Force.Force = 0
    elseif self._ForceState == 1 then
      self.P_Modul_08_Schaukel_Force.Force = 1.1 * 2
      self.P_Modul_08_Schaukel:WakeUp()
    elseif self._ForceState == 3 then
      self.P_Modul_08_Schaukel_Force.Force = -1.1 * 2
    end
  end)
end
function P_Modul_08:Deactive()
  self.P_Modul_08_Schaukel.EnableConstantForce = false
  self._EnableForce = false
  self._ForceTick = 0
  self.P_Modul_08_Schaukel:UnPhysicalize(true)
  if self._ForceTimer then
    LuaTimer.Delete(self._ForceTimer)
    self._ForceTimer = nil
  end
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