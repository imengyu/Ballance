local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

---@class P_Modul_26 : ModulBase 
---@field P_Modul_26_Rope PhysicsObject
---@field P_Modul_26_Sack PhysicsObject
---@field P_Modul_26_Sack_Force PhysicsConstraintForce
P_Modul_26 = ModulBase:extend()

function P_Modul_26:new()
  P_Modul_26.super.new(self)
  self._ForceTimer = nil
  self._ForceState = false
end
function P_Modul_26:Active()
  ModulBase.Active(self)
  self.P_Modul_26_Rope:Physicalize()
  self.P_Modul_26_Sack:Physicalize()
  self.P_Modul_26_Sack.EnableConstantForce = true
  self._ForceTimer = LuaTimer.Add(500, 1500, function ()
    self._ForceState = not self._ForceState

    --切换方向
    if self._ForceState then
      self.P_Modul_26_Sack_Force.Force = 0.5
    else
      self.P_Modul_26_Sack_Force.Force = -0.5
    end
  end)
end
function P_Modul_26:Deactive()
  self.P_Modul_26_Sack.EnableConstantForce = false
  self.P_Modul_26_Rope:UnPhysicalize(true)
  self.P_Modul_26_Sack:UnPhysicalize(true)
  if self._ForceTimer then
    LuaTimer.Delete(self._ForceTimer)
    self._ForceTimer = nil
  end
  ModulBase.Deactive(self)
end

function P_Modul_26:ActiveForPreview()
  self.gameObject:SetActive(true)
end
function P_Modul_26:DeactiveForPreview()
  self.gameObject:SetActive(false)
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