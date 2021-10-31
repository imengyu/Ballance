local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local Log = Ballance2.Utils.Log
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

local BallsManagerGameObject = nil
local GamePlayManagerGameObject = nil
local GamePlayUIGameObject = nil
local GameSectorManagerGameObject = nil
local GameMusicManagerGameObject = nil
local GameTranfoManagerGameObject = nil
local GameLevelBrizGameObject = nil
local GameUFOAnimControllerGameObject = nil

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
  UFOAnimController = nil, ---@type UFOAnimController
  BallSoundManager = nil, ---@type BallSoundManager
}

---游戏玩模块初始化
---@param callback function
function GamePlayInit(callback)
  Log.D('GamePlay', 'GamePlayInit')
  
  coroutine.resume(coroutine.create(function()
    --GamePlayUI
    GamePlayUIGameObject = Game.UIManager:InitViewToCanvas(Game.PackageManager:GetPrefabAsset('__core.ui__/GamePlayUI.prefab'), 'GamePlayUI', false).gameObject
    Yield(WaitForSeconds(0.05))
    GamePlayUIGameObject:SetActive(false)

    --初始化基础对象
    GamePlayManagerGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('GamePlayManager.prefab'), 'GamePlayManager')
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
    Yield(WaitForSeconds(0.05))
    GameUFOAnimControllerGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('PE_UFO.prefab'), 'GameUFOAnimController')

    Game.GamePlay = GamePlay

    --回调
    callback()
  end))
end
---游戏玩模块卸载
function GamePlayUnload()
  Log.D('GamePlay', 'GamePlayUnload')

  GamePlay.BallManager = nil
  GamePlay.BallPiecesControll = nil
  GamePlay.CamManager = nil
  GamePlay.GamePlayManager = nil
  GamePlay.SectorManager = nil
  GamePlay.MusicManager = nil
  GamePlay.TranfoManager = nil
  GamePlay.UFOAnimController = nil
  
  if (not Slua.IsNull(BallsManagerGameObject)) then UnityEngine.Object.Destroy(BallsManagerGameObject) end 
  if (not Slua.IsNull(GamePlayManagerGameObject)) then UnityEngine.Object.Destroy(GamePlayManagerGameObject) end 
  if (not Slua.IsNull(GamePlayUIGameObject)) then UnityEngine.Object.Destroy(GamePlayUIGameObject) end 
  if (not Slua.IsNull(GameSectorManagerGameObject)) then UnityEngine.Object.Destroy(GameSectorManagerGameObject) end 
  if (not Slua.IsNull(GameMusicManagerGameObject)) then UnityEngine.Object.Destroy(GameMusicManagerGameObject) end 
  if (not Slua.IsNull(GameTranfoManagerGameObject)) then UnityEngine.Object.Destroy(GameTranfoManagerGameObject) end 
  if (not Slua.IsNull(GameLevelBrizGameObject)) then UnityEngine.Object.Destroy(GameLevelBrizGameObject) end 
  if (not Slua.IsNull(GameUFOAnimControllerGameObject)) then UnityEngine.Object.Destroy(GameUFOAnimControllerGameObject) end 

  GameUFOAnimControllerGameObject = nil
  GameLevelBrizGameObject = nil
  GameTranfoManagerGameObject = nil
  GameMusicManagerGameObject = nil
  GameSectorManagerGameObject = nil
  GamePlayUIGameObject = nil
  GamePlayManagerGameObject = nil
  BallsManagerGameObject = nil

  Game.GamePlay = nil
end