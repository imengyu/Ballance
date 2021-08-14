local GamePackage = Ballance2.Sys.Package.GamePackage
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

--系统包
local SystemPackage = GamePackage.GetSystemPackage()

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
  --延时
  coroutine.resume(coroutine.create(function()
    GamePlayManagerGameObjec = CloneUtils.CloneNewObject(SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/GamePlayManager.prefab'), 'GamePlayManager')
    Yield(WaitForSeconds(0.1))
    BallsManagerGameObject = CloneUtils.CloneNewObject(SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/BallManager.prefab'), 'GameBallsManager')
    Yield(WaitForSeconds(0.5))
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