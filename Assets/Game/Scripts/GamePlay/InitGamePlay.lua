local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

local BallsManagerGameObject = nil
local GamePlayManagerGameObjec = nil

---模块全局索引
---@class GamePlay
GamePlay = {
  BallManager = nil, ---@type BallManager
  BallPiecesControll = nil, ---@type BallPiecesControll
  CamManager = nil, ---@type CamManager
  GamePlayManager = nil, ---@type GamePlayManager
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
    GamePlayManagerGameObjec = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/GamePlayManager.prefab'), 'GamePlayManager')
    Yield(WaitForSeconds(0.2))
    BallsManagerGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/BallManager.prefab'), 'GameBallsManager')
    Yield(WaitForSeconds(0.2))
    Game.GamePlay = GamePlay
    callback()
  end))
end
---游戏玩模块卸载
function GamePlayUnload()

  if (not Slua.IsNull(BallsManagerGameObject)) then UnityEngine.Object.Destroy(BallsManagerGameObject) end 
  if (not Slua.IsNull(GamePlayManagerGameObjec)) then UnityEngine.Object.Destroy(GamePlayManagerGameObjec) end 

  GamePlayManagerGameObjec = nil
  BallsManagerGameObject = nil

  Game.GamePlay = nil
end