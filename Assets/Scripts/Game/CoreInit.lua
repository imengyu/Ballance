local GameManager = Ballance2.Sys.GameManager.Instance
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
  Game.Mediator = GameManager:GetSystemService('GameMediator')
  Game.PackageManager = GameManager:GetSystemService('GamePackageManager')
  Game.UIManager = GameManager:GetSystemService('GameUIManager')
  Game.SoundManager = GameManager:GetSystemService('GameSoundManager')

  SystemPackage:RequireLuaFile('GamePlayInit.lua');
  SystemPackage:RequireLuaFile('GamePlayDebug.lua');
  SystemPackage:RequireLuaFile('GameCoreLibInit.lua');

  --调试入口
  GameManager.GameMediator:RegisterEventHandler("CoreDebugGamePlayEntry", 'Core', function ()
    CoreDebugGamePlay()
  end)
end
function CoreUnload()
  
end