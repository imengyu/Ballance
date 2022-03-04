local GameManager = Ballance2.Services.GameManager
local GamePackage = Ballance2.Package.GamePackage
local GameEventNames = Ballance2.Base.GameEventNames
local GameErrorChecker = Ballance2.Services.Debug.GameErrorChecker
local GameError = Ballance2.Services.Debug.GameError
local Log = Ballance2.Log
local CorePackage = GamePackage.GetCorePackage()
local WaitForSeconds = UnityEngine.WaitForSeconds
local Yield = UnityEngine.Yield

---全局 CreateClass 引入
CreateClass = {}
---全局 ClassicObject 引入
ClassicObject = require('classic') ---@type ClassicObject 

require('ConstLinks')
require('GameLayers')
require('GamePhysBall')
require('GamePhysFloor')
require('LevelBuilder') 
require('InitGamePlay')
require('InitLevelBuilder')
require('InitBulitInModuls')
require('KeypadUIManager')



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
  CorePackage = CorePackage, 
  --获取游戏玩模块（也可直接使用全局变量GamePlay获取） [R]
  GamePlay = nil, 
  --获取关卡建造器模块 [R]
  LevelBuilder = nil, ---@type LevelBuilder
  --获取调试命令 [R]
  CommandServer = nil, ---@type GameDebugCommandServer
  --获取分数管理器 [R]
  HighScoreManager = nil, ---@type HighscoreManager
}

local Intro = nil
local MenuLevel = nil
local UIInit = nil

function CoreInit()
  Log.D(TAG, 'CoreInit')

  coroutine.resume(coroutine.create(function ()
    
    Yield(WaitForSeconds(0.06))

    Intro = require('IntroInit')

    Yield(WaitForSeconds(0.06))

    MenuLevel = require('MenuLevelInit')
    
    Yield(WaitForSeconds(0.06))

    UIInit = require('UIInit')
      
    Yield(WaitForSeconds(0.06))

    UIInit.Init()
    Intro.Init()
    MenuLevel.Init()

    local GameManagerInstance = GameManager.Instance
    local GameMediator = GameManager.GameMediator

    Game.CommandServer = GameManagerInstance.GameDebugCommandServer
    Game.PackageManager = GameManager.GetSystemService('GamePackageManager')
    Game.UIManager = GameManager.GetSystemService('GameUIManager')
    Game.SoundManager = GameManager.GetSystemService('GameSoundManager')
    Game.HighScoreManager = require('HighscoreManager')

    Yield(WaitForSeconds(0.06))

    CorePackage:RequireLuaClass('ModulBase')
    CorePackage:RequireLuaClass('ModulPhysics')
    CorePackage:RequireLuaClass('Ball')

    
    Yield(WaitForSeconds(0.06))
    
    LevelBuilderInit()
    --加载分数数据
    Game.HighScoreManager.Load()
    
    Yield(WaitForSeconds(0.06))

    --注册内置键盘
    KeypadUIManager.AddKeypad('BaseLeft', CorePackage:GetPrefabAsset('KeypadLeft.prefab'), CorePackage:GetSpriteAsset('keypad_l.png'))
    KeypadUIManager.AddKeypad('BaseRight', CorePackage:GetPrefabAsset('KeypadRight.prefab'), CorePackage:GetSpriteAsset('keypad_r.png'))
    KeypadUIManager.AddKeypad('BaseJoyLeft', CorePackage:GetPrefabAsset('KeypadJoyLeft.prefab'), CorePackage:GetSpriteAsset('keypad_joy_l.png'))
    KeypadUIManager.AddKeypad('BaseJoyRight', CorePackage:GetPrefabAsset('KeypadJoyRight.prefab'), CorePackage:GetSpriteAsset('keypad_joy_r.png'))

    --调试入口
    if GameManager.DebugMode then
      require('CoreLuaDebug')
      require('LevelBuilderDebug')
      require('ModulDebug')
      require('LevelDebug')

      GameMediator:RegisterEventHandler(CorePackage, "CoreDebugLevelBuliderEntry", TAG, function (evtName, params)
        CoreDebugLevelBuliderEntry()
        return false
      end)
      GameMediator:RegisterEventHandler(CorePackage, "CoreDebugLevelEnvironmentEntry", TAG, function (evtName, params)
        CoreDebugLevelEnvironmentEntry()
        return false
      end)
      GameMediator:RegisterEventHandler(CorePackage, "CoreDebugLuaEntry", TAG, function (evtName, params)
        CoreDebugLuaEntry()
        return false
      end)
      GameMediator:RegisterEventHandler(CorePackage, "CoreDebugEmptyEntry", TAG, function (evtName, params)
        CoreDebugEmptyEntry()
        return false
      end)
      GameMediator:RegisterEventHandler(CorePackage, "ModulCustomDebug", TAG, function (evtName, params)
        ModulCustomDebug()
        return false
      end)
      GameMediator:RegisterEventHandler(CorePackage, "LevelCustomDebug", TAG, function (evtName, params)
        LevelCustomDebug()
        return false
      end)
    end

    local nextLoadLevel = ''
    GameMediator:RegisterEventHandler(CorePackage, GameEventNames.EVENT_LOGIC_SECNSE_ENTER, TAG, function (evtName, params)
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
    GameMediator:RegisterEventHandler(CorePackage, GameEventNames.EVENT_LOGIC_SECNSE_QUIT, TAG, function (evtName, params)
      local scense = params[1]
      if(scense == 'Level') then 
        GamePlayUnload()
      end
      return false
    end)
    
    --加载关卡入口
    GameMediator:SubscribeSingleEvent(CorePackage, "CoreStartLoadLevel", TAG, function (evtName, params)
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
    GameMediator:RegisterEventHandler(CorePackage, GameEventNames.EVENT_BEFORE_GAME_QUIT, TAG, function ()
      ---保存分数数据
      Game.HighScoreManager.Save()
      return false
    end)
  end))
end
function CoreUnload()
  Log.D(TAG, 'CoreUnload')
  Intro.Unload()
  MenuLevel.Unload()
  UIInit.Unload()
  GamePlayUnload()
  LevelBuilderDestroy()
end