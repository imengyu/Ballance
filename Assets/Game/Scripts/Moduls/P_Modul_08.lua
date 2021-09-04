local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

---@class P_Modul_08 : ModulBase 
---@field P_Modul_08_Schaukel_Force PhysicsForce
---@field P_Modul_08_Schaukel PhysicsBody
---@field P_Modul_08_ForcePovit_2 Transform
---@field P_Modul_08_ForcePovit_1 Transform
P_Modul_08 = ModulBase:extend()

function P_Modul_08:new()
  self._EnableForce = false
  self._ForceIsLeft = false
  self._ForceTick = 0
  self._ForceAppilyTick = 10 --(6/0.1s)
end

function P_Modul_08:Start()
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
        self.P_Modul_08_Schaukel_Force.ForceRef = self.P_Modul_08_ForcePovit_1
      else
        self.P_Modul_08_Schaukel_Force.ForceRef = self.P_Modul_08_ForcePovit_2
      end
    end
  end
end
function P_Modul_08:Active()
  self.gameObject:SetActive(true)
  self.P_Modul_08_Schaukel:ForcePhysics()
  self.P_Modul_08_Schaukel_Force.Enable = true
  self._EnableForce = true
end
function P_Modul_08:Deactive()
  self.P_Modul_08_Schaukel_Force.Enable = false
  self._EnableForce = false
  self._ForceTick = 0
  self.P_Modul_08_Schaukel:ForceDePhysics()
  self.gameObject:SetActive(false)
end
function P_Modul_08:Reset()
  ObjectStateBackupUtils.RestoreObject(self.P_Modul_08_Schaukel.gameObject)
  self._ForceIsLeft = false
  self._ForceTick = 0
end
function P_Modul_08:Backup()
  ObjectStateBackupUtils.BackUpObject(self.P_Modul_08_Schaukel.gameObject)
end

function CreateClass_P_Modul_08()
  return P_Modul_08()
end