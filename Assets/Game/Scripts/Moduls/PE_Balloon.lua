local ObjectStateBackupUtils = Ballance2.Sys.Utils.ObjectStateBackupUtils

---@class PE_Balloon : ModulBase 
PE_Balloon = ModulBase:extend()

function PE_Balloon:new()
end

function PE_Balloon:Active()
  self.gameObject:SetActive(true)
end
function PE_Balloon:Deactive()
  self.gameObject:SetActive(false)
end
function PE_Balloon:Reset()
end
function PE_Balloon:Backup()
end

function CreateClass_PE_Balloon()
  return PE_Balloon()
end