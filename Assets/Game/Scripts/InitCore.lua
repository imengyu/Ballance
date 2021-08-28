local GameManager = Ballance2.Sys.GameManager
local GamePackage = Ballance2.Sys.Package.GamePackage
local GameEventNames = Ballance2.Sys.Bridge.GameEventNames
local GameErrorChecker = Ballance2.Sys.Debug.GameErrorChecker
local GameError = Ballance2.Sys.Debug.GameError
local Log = Ballance2.Utils.Log
local SystemPackage = GamePackage.GetSystemPackage()

ClassicObject = SystemPackage:RequireLuaFile("classic") ---@type ClassicObject

local TAG = 'Core'

---游戏功能索引
Game = {
  --获取系统管理器（GameManager.Instance） [R]
  Manager = GameManager.Instance, 
  ---获取获取系统中介者 [R]
  Mediator = GameManager.GameMediator, ---@type GameMediator 
  --获取系统包管理器 [R]
  PackageManager = nil, ---@type GamePackageManager
  --获取UI管理器 [R]
  UIManager = nil, ---@type GameUIManager
  --获取声音管理器 [R]
  SoundManager = nil, ---@type GameSoundManager 
  --获取系统包 [R]
  SystemPackage = SystemPackage, 
  --获取游戏玩模块（也可直接使用全局变量GamePlay获取） [R]
  GamePlay = nil, 
  --获取关卡建造器模块 [R]
  LevelBuilder = nil, ---@type LevelBuilder
  --获取调试命令 [R]
  CommandServer = nil, ---@type GameDebugCommandServer
}

function CoreInit()
  local GameManagerInstance = GameManager.Instance
  local GameMediator = GameManager.GameMediator

  Game.CommandServer = GameManagerInstance.GameDebugCommandServer
  Game.PackageManager = GameManagerInstance:GetSystemService('GamePackageManager')
  Game.UIManager = GameManagerInstance:GetSystemService('GameUIManager')
  Game.SoundManager = GameManagerInstance:GetSystemService('GameSoundManager')

  SystemPackage:RequireLuaFile('ConstLinks')
  SystemPackage:RequireLuaFile('GameLayers')
  SystemPackage:RequireLuaFile('GamePhysBall')
  SystemPackage:RequireLuaFile('GamePhysFloor')
  SystemPackage:RequireLuaFile('GameCoreLibInit')
  SystemPackage:RequireLuaFile('LevelBuilder') 
  SystemPackage:RequireLuaFile('InitGamePlay')
  SystemPackage:RequireLuaFile('InitLevelBuilder')
  SystemPackage:RequireLuaFile('InitBulitInModuls')
  SystemPackage:RequireLuaFile('GamePlayDebug')
  SystemPackage:RequireLuaFile('LevelBuilderDebug')
  SystemPackage:RequireLuaFile('DefaultHighscoreData')
  SystemPackage:RequireLuaFile('HighscoreManager')
  SystemPackage:RequireLuaClass('ModulBase')
  SystemPackage:RequireLuaClass('ModulSingalPhysics')
  SystemPackage:RequireLuaClass('ModulComplexPhysics')
  SystemPackage:RequireLuaClass('Ball')
  
  LevelBuilderInit()
  --加载分数数据
  HighscoreManagerLoad()

  --调试入口
  GameMediator:RegisterEventHandler(SystemPackage, "CoreDebugGamePlayEntry", TAG, function ()
    CoreDebugGameGamePlay()
    return false
  end)
  --调试入口
  GameMediator:RegisterEventHandler(SystemPackage, "CoreDebugLevelBuliderEntry", TAG, function ()
    CoreDebugLevelBuliderEntry()
    return false
  end)

  local nextLoadLevel = ''
  GameMediator:RegisterEventHandler(SystemPackage, GameEventNames.EVENT_LOGIC_SECNSE_ENTER, TAG, function (evtName, params)
    local scense = params[1]
    if(scense == 'Level') then 
      LuaTimer.Add(300, function ()
        GamePlayInit(function ()
          if nextLoadLevel ~= '' then
            Game.LevelBuilder:LoadLevel(nextLoadLevel)
            nextLoadLevel = ''
          end
        end)
      end)
    end
    return false
  end)    
  GameManager.GameMediator:RegisterEventHandler(SystemPackage, GameEventNames.EVENT_LOGIC_SECNSE_QUIT, TAG, function (evtName, params)
    local scense = params[1]
    if(scense == 'Level') then 
      GamePlayUnload()
    end
    return false
  end)
  --加载关卡入口
  GameMediator:SubscribeSingleEvent(SystemPackage, "CoreStartLoadLevel", TAG, function (evtName, params)
    if type(params[1]) ~= 'string' then
      local type = type(params[1]) 
      GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, TAG, 'Param 1 expect string, but got '..type)
      return false
    else
      nextLoadLevel = params[1]
      Log.D(TAG, 'Start load level '..nextLoadLevel..' ')
    end
    GameManagerInstance:RequestEnterLogicScense('Level')
    return false
  end)
  --退出
  GameMediator:RegisterEventHandler(SystemPackage, GameEventNames.EVENT_BEFORE_GAME_QUIT, TAG, function ()
    ---保存分数数据
    HighscoreManagerSave()
    return false
  end)

  Log.D(TAG, 'CoreInit')
end
function CoreUnload()
  Log.D(TAG, 'CoreUnload')
  GamePlayUnload()
  LevelBuilderDestroy()
end
function CoreVersion()
  return 1
end