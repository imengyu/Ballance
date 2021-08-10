local GamePackage = Ballance2.Sys.Package.GamePackage
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

--系统包
local SystemPackage = GamePackage.GetSystemPackage()

local BallsManagerGameObject = nil
local BallCameraGameObject = nil

---模块全局索引
---@class GamePlay
GamePlay = {
  BallManager = nil, ---@type BallManager
  CamManager = nil, ---@type CamManager
}

---游戏玩模块初始化
---@param callback function
function GamePlayInit(callback)

  BallsManagerGameObject = CloneUtils.CloneNewObject(SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/BallManager.prefab'), 'GameBallsManager')
  BallCameraGameObject = BallsManagerGameObject.transform:Find('Ball_CameraHost/MainCamera').gameObject

  --延时
  coroutine.resume(coroutine.create(function()
    Yield(WaitForSeconds(0.5))
    
    GamePlay.BallManager = GameObjectToLuaClass(BallsManagerGameObject)
    GamePlay.CamManager = GameObjectToLuaClass(BallCameraGameObject)
  
    Game.GamePlay = GamePlay

    callback()
  end))
end
---游戏玩模块卸载
function GamePlayUnload()

  if (not Slua.IsNull(BallsManagerGameObject)) then UnityEngine.Object.Destroy(BallsManagerGameObject) end 

  BallsManagerGameObject = nil
  BallCameraGameObject = nil

  Game.GamePlay = nil
end