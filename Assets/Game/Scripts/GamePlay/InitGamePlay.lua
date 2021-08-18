local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

local BallsManagerGameObject = nil
local GamePlayManagerGameObjec = nil
local GamePlayUIGameObject = nil
local GameSectorManagerGameObject = nil

---模块全局索引
---@class GamePlay
GamePlay = {
  BallManager = nil, ---@type BallManager
  BallPiecesControll = nil, ---@type BallPiecesControll
  CamManager = nil, ---@type CamManager
  GamePlayManager = nil, ---@type GamePlayManager
  GamePlayUI = nil, ---@type GamePlayUIControl
  SectorManager = nil, ---@type SectorManager
}

---游戏玩模块初始化
---@param callback function
function GamePlayInit(callback)
  coroutine.resume(coroutine.create(function()
    --初始化关卡加载器
    local LevelBuilderGameObject = LevelBuilderInit()
    Yield(WaitForSeconds(0.1))
    Game.LevelBuilder = GameObjectToLuaClass(LevelBuilderGameObject) ---@type LevelBuilder
    InitBulitInModuls()

    --GamePlayUI
    local uiPackage = Game.PackageManager:FindPackage('core.ui')
    GamePlayUIGameObject = Game.UIManager:InitViewToCanvas(uiPackage:GetPrefabAsset('GamePlayUI.prefab'), 'GamePlayUI', false)
    Yield(WaitForSeconds(0.1))
    GamePlayUIGameObject.gameObject:SetActive(false)


    --初始化基础对象
    GamePlayManagerGameObjec = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/GamePlayManager.prefab'), 'GamePlayManager')
    Yield(WaitForSeconds(0.1))
    BallsManagerGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/BallManager.prefab'), 'GameBallsManager')
    Yield(WaitForSeconds(0.1))
    GameSectorManagerGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/SectorManager.prefab'), 'GameSectorManager')
    Yield(WaitForSeconds(0.1))

    Game.GamePlay = GamePlay

    --回调
    callback()
  end))
end
---游戏玩模块卸载
function GamePlayUnload()

  if (not Slua.IsNull(BallsManagerGameObject)) then UnityEngine.Object.Destroy(BallsManagerGameObject) end 
  if (not Slua.IsNull(GamePlayManagerGameObjec)) then UnityEngine.Object.Destroy(GamePlayManagerGameObjec) end 
  if (not Slua.IsNull(GamePlayUIGameObject)) then UnityEngine.Object.Destroy(GamePlayUIGameObject) end 
  if (not Slua.IsNull(GameSectorManagerGameObject)) then UnityEngine.Object.Destroy(GameSectorManagerGameObject) end 

  GameSectorManagerGameObject = nil
  GamePlayUIGameObject = nil
  GamePlayManagerGameObjec = nil
  BallsManagerGameObject = nil

  Game.GamePlay = nil
end