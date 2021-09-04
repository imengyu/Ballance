local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

---@class P_Modul_26 : ModulBase 
---@field P_Modul_26_SackForce PhysicsForce
---@field P_Modul_26_Rope PhysicsBody
---@field P_Modul_26_Sack PhysicsBody
---@field P_Modul_26_ForcePovit_2 Transform
---@field P_Modul_26_ForcePovit_1 Transform
P_Modul_26 = ModulBase:extend()

function P_Modul_26:new()
  self._EnableForce = false
  self._ForceIsLeft = false
  self._ForceTick = 0
  self._ForceAppilyTick = 8 --(6/0.1s)
end

function P_Modul_26:Start()
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
        self.P_Modul_26_SackForce.ForceRef = self.P_Modul_26_ForcePovit_1
      else
        self.P_Modul_26_SackForce.ForceRef = self.P_Modul_26_ForcePovit_2
      end
    end
  end
end
function P_Modul_26:Active()
  self.gameObject:SetActive(true)
  self.P_Modul_26_Rope:ForcePhysics()
  self.P_Modul_26_Sack:ForcePhysics()
  self.P_Modul_26_SackForce.Enable = true
  self._EnableForce = true
end
function P_Modul_26:Deactive()
  self.P_Modul_26_SackForce.Enable = false
  self._EnableForce = false
  self._ForceTick = 0
  self.P_Modul_26_Rope:ForceDePhysics()
  self.P_Modul_26_Sack:ForceDePhysics()
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

function CreateClass_P_Modul_26()
  return P_Modul_26()
end