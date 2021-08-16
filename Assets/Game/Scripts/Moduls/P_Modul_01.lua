local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

---@class P_Modul_01 : ModulBase 
P_Modul_01 = ModulBase:extend()

function P_Modul_01:new()
  self._P_Modul_01_Pusher = nil ---@type GameObject
  self._P_Modul_01_Pusher_PhysicsBody = nil ---@type PhysicsBody
  self._P_Modul_01_Filter_PhysicsBody = nil ---@type PhysicsBody
end

function P_Modul_01:Active()
  self.gameObject:SetActive(true)
  self._P_Modul_01_Filter_PhysicsBody:ForcePhysics()
  self._P_Modul_01_Pusher_PhysicsBody:ForcePhysics()
end
function P_Modul_01:Deactive()
  self.gameObject:SetActive(false)
  self._P_Modul_01_Pusher_PhysicsBody:ForceDeactive()
  self._P_Modul_01_Filter_PhysicsBody:ForceDeactive()
end
function P_Modul_01:Reset()
  ObjectStateBackupUtils.RestoreObject(self._P_Modul_01_Pusher)
end
function P_Modul_01:Backup()
  ObjectStateBackupUtils.BackUpObject(self._P_Modul_01_Pusher)
end

function CreateClass_P_Modul_01()
  return P_Modul_01()
end