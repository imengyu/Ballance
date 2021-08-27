---@class PS_FourFlames : ModulBase 
PS_FourFlames = ModulBase:extend()

function PS_FourFlames:new()
  self.Flame_A = nil ---@type GameObject
  self.Flame_B = nil ---@type GameObject
  self.Flame_C = nil ---@type GameObject
  self.Flame_D = nil ---@type GameObject
end
function PS_FourFlames:Start()
  self:Deactive()
end
function PS_FourFlames:Active()
  self.Flame_A:SetActive(true)
  self.Flame_B:SetActive(true)
  self.Flame_C:SetActive(true)
  self.Flame_D:SetActive(true)
end
function PS_FourFlames:Deactive()
  self.Flame_A:SetActive(false)
  self.Flame_B:SetActive(false)
  self.Flame_C:SetActive(false)
  self.Flame_D:SetActive(false)
end
function PS_FourFlames:Reset()
end
function PS_FourFlames:Backup()
end

function CreateClass_PS_FourFlames()
  return PS_FourFlames()
end