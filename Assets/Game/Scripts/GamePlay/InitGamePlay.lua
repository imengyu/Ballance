local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local GameLuaObjectHost = Ballance2.Sys.Bridge.LuaWapper.GameLuaObjectHost
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

local BallsManagerGameObject = nil
local GamePlayManagerGameObjec = nil
local GamePlayUIGameObject = nil
local GameSectorManagerGameObject = nil
local GameMusicManagerGameObject = nil
local GameTranfoManagerGameObject = nil
local GameLevelBrizGameObject = nil

---模块全局索引
---@class GamePlay
GamePlay = {
  BallManager = nil, ---@type BallManager
  BallPiecesControll = nil, ---@type BallPiecesControll
  CamManager = nil, ---@type CamManager
  GamePlayManager = nil, ---@type GamePlayManager
  SectorManager = nil, ---@type SectorManager
  MusicManager = nil, ---@type MusicManager
  TranfoManager = nil, ---@type TranfoAminControl
}

---游戏玩模块初始化
---@param callback function
function GamePlayInit(callback)
  coroutine.resume(coroutine.create(function()
    --GamePlayUI
    GamePlayUIGameObject = Game.UIManager:InitViewToCanvas(Game.PackageManager:GetPrefabAsset('__core.ui__/GamePlayUI.prefab'), 'GamePlayUI', false).gameObject
    Yield(WaitForSeconds(0.05))
    GamePlayUIGameObject:SetActive(false)

    --初始化基础对象
    GamePlayManagerGameObjec = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('GamePlayManager.prefab'), 'GamePlayManager')
    Yield(WaitForSeconds(0.05))
    Game.Manager:SetGameBaseCameraVisible(false)
    BallsManagerGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('BallManager.prefab'), 'GameBallsManager')
    Yield(WaitForSeconds(0.05))
    GameSectorManagerGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('SectorManager.prefab'), 'GameSectorManager')
    Yield(WaitForSeconds(0.05))
    GameMusicManagerGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('MusicManager.prefab'), 'GameMusicManager')
    Yield(WaitForSeconds(0.05))
    GameTranfoManagerGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('AminTranfo.prefab'), 'GameTranfoManager')
    Yield(WaitForSeconds(0.05))
    GameLevelBrizGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('LevelBriz.prefab'), 'GameLevelBriz')

    Game.GamePlay = GamePlay

    --回调
    callback()
  end))
end
---游戏玩模块卸载
function GamePlayUnload()
  GamePlay.BallManager = nil
  GamePlay.BallPiecesControll = nil
  GamePlay.CamManager = nil
  GamePlay.GamePlayManager = nil
  GamePlay.SectorManager = nil
  GamePlay.MusicManager = nil
  GamePlay.TranfoManager = nil
  
  if (not Slua.IsNull(BallsManagerGameObject)) then UnityEngine.Object.Destroy(BallsManagerGameObject) end 
  if (not Slua.IsNull(GamePlayManagerGameObjec)) then UnityEngine.Object.Destroy(GamePlayManagerGameObjec) end 
  if (not Slua.IsNull(GamePlayUIGameObject)) then UnityEngine.Object.Destroy(GamePlayUIGameObject) end 
  if (not Slua.IsNull(GameSectorManagerGameObject)) then UnityEngine.Object.Destroy(GameSectorManagerGameObject) end 
  if (not Slua.IsNull(GameMusicManagerGameObject)) then UnityEngine.Object.Destroy(GameMusicManagerGameObject) end 
  if (not Slua.IsNull(GameTranfoManagerGameObject)) then UnityEngine.Object.Destroy(GameTranfoManagerGameObject) end 
  if (not Slua.IsNull(GameLevelBrizGameObject)) then UnityEngine.Object.Destroy(GameLevelBrizGameObject) end 

  GameLevelBrizGameObject = nil
  GameTranfoManagerGameObject = nil
  GameMusicManagerGameObject = nil
  GameSectorManagerGameObject = nil
  GamePlayUIGameObject = nil
  GamePlayManagerGameObjec = nil
  BallsManagerGameObject = nil

  Game.GamePlay = nil
end