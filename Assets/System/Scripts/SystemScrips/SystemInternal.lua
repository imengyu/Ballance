local GameErrorChecker = Ballance2.Services.Debug.GameErrorChecker
local GameError = Ballance2.Services.Debug.GameError
local GameManager = Ballance2.Services.GameManager
local GamePackageManager = Slua.As(GameManager.GetSystemService('GamePackageManager'), Ballance2.Services.GamePackageManager) ---@type GamePackageManager

---初始化包信息
---@param packageName string
---@param entryCode string
function IntneralLoadLuaPackage(packageName, entryCode)
  local pack = GamePackageManager:FindRegisteredPackage(packageName)
  if(not pack) then
    GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, 'Intneral', 'Page '..packageName..' not found')
    return false
  end

  ---@type PackageEntryStruct
  local ret = pack:RequireLuaFile(entryCode) 
  if(not ret) then
    GameErrorChecker.SetLastErrorAndLog(GameError.ExecutionFailed, 'Intneral', 'EntryCode '..entryCode..' require failed')
    return false
  end

  if(type(ret.PackageBeforeUnLoad) == "function") then
    pack.PackageEntry.OnBeforeUnLoad = ret.PackageBeforeUnLoad
  end
  if(type(ret.PackageEntry) == "function") then
    pack.PackageEntry.OnLoad = ret.PackageEntry
  end
  if(type(ret.PackageVersion) == "number") then
    pack.PackageEntry.Version = ret.PackageVersion
  end

  return true
end