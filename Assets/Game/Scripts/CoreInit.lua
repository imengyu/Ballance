local GameManager = Ballance2.Sys.GameManager
local GamePackage = Ballance2.Sys.Package.GamePackage
local SystemPackage = GamePackage.GetSystemPackage()

ClassicObject = SystemPackage:RequireLuaFile("classic") ---@type ClassicObject

---游戏功能索引
Game = {
  Manager = GameManager, --获取系统包 [R]
  Mediator = nil, ---@type GameMediator 获取获取系统中介者 [R]
  --获取系统包管理器 [R]
  PackageManager = nil, ---@type GamePackageManager
  --获取UI管理器 [R]
  UIManager = nil, ---@type GameUIManager
  --获取声音管理器 [R]
  SoundManager = nil, ---@type GameSoundManager 
  SystemPackage = SystemPackage, --获取系统包 [R]
  GamePlay = nil, --获取游戏玩模块 [R]
}

function CoreInit()
  local GameManagerInstance = Ballance2.Sys.GameManager.Instance

  Game.Mediator = GameManagerInstance:GetSystemService('GameMediator')
  Game.PackageManager = GameManagerInstance:GetSystemService('GamePackageManager')
  Game.UIManager = GameManagerInstance:GetSystemService('GameUIManager')
  Game.SoundManager = GameManagerInstance:GetSystemService('GameSoundManager')

  SystemPackage:RequireLuaFile('GamePlayInit.lua')
  SystemPackage:RequireLuaFile('GamePlayDebug.lua')
  SystemPackage:RequireLuaFile('GameCoreLibInit.lua')

  --调试入口
  GameManager.GameMediator:RegisterEventHandler(SystemPackage, "CoreDebugGamePlayEntry", 'Core', function ()
    CoreDebugGameGamePlay()
    return false
  end)
end
function CoreUnload()
  
end
function CoreVersion()
  return 1
end