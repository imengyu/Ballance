local GameManager = Ballance2.Sys.GameManager
local GamePackage = Ballance2.Sys.Package.GamePackage
local SystemPackage = GamePackage.GetSystemPackage()

ClassicObject = SystemPackage:RequireLuaFile("classic") ---@type ClassicObject

---游戏功能索引
Game = {
  --获取系统管理器 [R]
  Manager = GameManager.Instance, 
  ---获取获取系统中介者 [R]
  Mediator = nil, ---@type GameMediator 
  --获取系统包管理器 [R]
  PackageManager = nil, ---@type GamePackageManager
  --获取UI管理器 [R]
  UIManager = nil, ---@type GameUIManager
  --获取声音管理器 [R]
  SoundManager = nil, ---@type GameSoundManager 
  SystemPackage = SystemPackage, --获取系统包 [R]
  GamePlay = nil, --获取游戏玩模块 [R]
  --获取关卡建造器模块 [R]
  LevelBuilder = nil, ---@type LevelBuilder
}

function CoreInit()
  local GameManagerInstance = GameManager.Instance

  Game.PackageManager = GameManagerInstance:GetSystemService('GamePackageManager')
  Game.UIManager = GameManagerInstance:GetSystemService('GameUIManager')
  Game.SoundManager = GameManagerInstance:GetSystemService('GameSoundManager')

  SystemPackage:RequireLuaFile('GameCoreLibInit.lua')
  SystemPackage:RequireLuaFile('LevelBuilder.lua') 
  SystemPackage:RequireLuaFile('InitGamePlay.lua')
  SystemPackage:RequireLuaFile('InitLevelBuilder.lua')
  SystemPackage:RequireLuaFile('InitBulitInModuls.lua')
  SystemPackage:RequireLuaFile('GamePlayDebug.lua')
  SystemPackage:RequireLuaFile('LevelBuilderDebug.lua')
  SystemPackage:RequireLuaClass('ModulBase')
  SystemPackage:RequireLuaClass('ModulSingalPhysics')
  SystemPackage:RequireLuaClass('ModulComplexPhysics')
  SystemPackage:RequireLuaClass('Ball')
  
  --调试入口
  GameManager.GameMediator:RegisterEventHandler(SystemPackage, "CoreDebugGamePlayEntry", 'Core', function ()
    CoreDebugGameGamePlay()
    return false
  end)
  --调试入口
  GameManager.GameMediator:RegisterEventHandler(SystemPackage, "CoreDebugLevelBuliderEntry", 'Core', function ()
    CoreDebugLevelBuliderEntry()
    return false
  end)

  --加载关卡入口
  GameManager.GameMediator:RegisterEventHandler(SystemPackage, "CoreStartLoadLevel", 'Core', function ()
    GameManager.Instance.RequestEnterLogicScense('Level')

    return false
  end)
end
function CoreUnload()
  GamePlayUnload()
  LevelBuilderDestroy()
end
function CoreVersion()
  return 1
end

function CoreStartLoadLevel() 

end