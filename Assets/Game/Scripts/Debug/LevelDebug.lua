local GameManager = Ballance2.Services.GameManager
local GameDebugEntry = Ballance2.Entry.GameDebugEntry.Instance
local GameErrorChecker = Ballance2.Services.Debug.GameErrorChecker
local GameError = Ballance2.Services.Debug.GameError
local StringUtils = Ballance2.Utils.StringUtils

---自定义关卡调试环境
function LevelCustomDebug()
  --检查参数
  if StringUtils.isNullOrEmpty(GameDebugEntry.LevelName) then
    GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "LevelName 未设置！")
    return
  end
  
  GameManager.GameMediator:NotifySingleEvent('CoreStartLoadLevel', GameDebugEntry.LevelName)
end