local ObjectStateBackupUtils = Ballance2.Utils.ObjectStateBackupUtils

---@class P_Modul_34 : ModulBase 
---@field P_Modul_34_Kiste PhysicsObject
---@field P_Modul_34_Schiebestein PhysicsObject
P_Modul_34 = ModulBase:extend()

function P_Modul_34:new()
  P_Modul_34.super.new(self)
end

function P_Modul_34:Start()
  ModulBase.Start(self)
  if not self.IsPreviewMode then
    self.P_Modul_34_Kiste.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('WoodOnlyHit')
    self.P_Modul_34_Schiebestein.CollisionID = GamePlay.BallSoundManager:GetSoundCollIDByName('Stone')
  end
end
function P_Modul_34:Active()
  self.P_Modul_34_Kiste.gameObject:SetActive(true)
  self.P_Modul_34_Schiebestein.gameObject:SetActive(true)
  self.P_Modul_34_Kiste:Physicalize()
  self.P_Modul_34_Schiebestein:Physicalize()
  ModulBase.Active(self)
end
function P_Modul_34:Deactive()
  self.P_Modul_34_Kiste:UnPhysicalize(true)
  self.P_Modul_34_Schiebestein:UnPhysicalize(true)
  ModulBase.Deactive(self)
end

function P_Modul_34:ActiveForPreview()
  self.gameObject:SetActive(true)
end
function P_Modul_34:DeactiveForPreview()
  self.gameObject:SetActive(false)
end

function P_Modul_34:Reset()
  ObjectStateBackupUtils.RestoreObjectAndChilds(self.gameObject)
  self._ForceIsLeft = false
  self._ForceTick = 0
end
function P_Modul_34:Backup()
  ObjectStateBackupUtils.BackUpObjectAndChilds(self.gameObject)
end

function CreateClass:P_Modul_34()
  return P_Modul_34()
end