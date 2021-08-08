local GameManager = Ballance2.Sys.GameManager
local GamePackage = Ballance2.Sys.Package.GamePackage
local SystemPackage = GamePackage.GetSystemPackage()

---游戏功能索引
Game = {
  Manager = GameManager,
  Mediator = nil, ---@type GameMediator
  PackageManager = nil, ---@type GamePackageManager
  UIManager = nil, ---@type GameUIManager
  SoundManager = nil, ---@type GameSoundManager
  SystemPackage = SystemPackage,
  GamePlay = nil,
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