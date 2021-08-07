local GamePackage = Ballance2.Sys.Package.GamePackage
local CloneUtils = Ballance2.Sys.Utils.CloneUtils

--系统包
local SystemPackage = GamePackage.GetSystemPackage()

local BallsManagerGameObject = nil
local BallCameraGameObject = nil

---模块全局索引
---@class GamePlay
local GamePlay = {
  BallManager = nil, ---@type BallManager
  CamManager = nil, ---@type CamManager
}

---游戏玩模块初始化
function GamePlayInit()

  BallsManagerGameObject = CloneUtils.CloneNewObject(SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/BallManager.prefab'), 'GameBallsManager')
  BallCameraGameObject = BallsManagerGameObject.transform:FindChild('Ball_CameraHost/CameraOverlook/MainCamera').gameObject

  GamePlay.BallManager = GameObjectToLuaClass(BallsManagerGameObject)
  GamePlay.CamManager = GameObjectToLuaClass(BallCameraGameObject)

  Game.GamePlay = GamePlay
end
---游戏玩模块卸载
function GamePlayUnload()

  if (not Slua.IsNull(BallsManagerGameObject)) then UnityEngine.Object.Destroy(BallsManagerGameObject) end 

  BallsManagerGameObject = nil
  BallCameraGameObject = nil

  Game.GamePlay = nil
end