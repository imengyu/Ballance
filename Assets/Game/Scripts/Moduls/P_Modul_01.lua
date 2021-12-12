local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

P_Modul_01 = ModulBase:extend()

---P_Modul_01
---栅栏机关
---@class P_Modul_01 : ModulBase
---@field P_Modul_01_Rinne PhysicsObject
---@field P_Modul_01_Filter PhysicsObject
---@field P_Modul_01_Pusher PhysicsObject
function P_Modul_01:new()
  ModulBase.super.new(self)
  self.EnableBallRangeChecker = true
  self.BallCheckeRange = 60
end
function P_Modul_01:Start()
  self.P_Modul_01_Pusher.CustomLayer = GamePlay.BallSoundManager:GetCustomSoundLayerByName('WoodOnlyHit')
end
function P_Modul_01:Active()
  self.gameObject:SetActive(true)
  self.P_Modul_01_Rinne:Physicalize()
  self.P_Modul_01_Filter:Physicalize()
  self.P_Modul_01_Pusher:Physicalize()
end
function P_Modul_01:Deactive()
  self.P_Modul_01_Rinne:UnPhysicalize(true)
  self.P_Modul_01_Filter:UnPhysicalize(true)
  self.P_Modul_01_Pusher:UnPhysicalize(true)
  self.gameObject:SetActive(false)
end
function P_Modul_01:Reset()
  ObjectStateBackupUtils.RestoreObject(self.gameObject)
end
function P_Modul_01:Backup()
  ObjectStateBackupUtils.BackUpObject(self.gameObject)
end
function P_Modul_01:BallEnterRange()
  if self.IsActive then
    self.P_Modul_01_Pusher:WakeUp()
  end
end

function CreateClass:P_Modul_01()
  return P_Modul_01()
end