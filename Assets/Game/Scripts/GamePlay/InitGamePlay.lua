local CloneUtils = Ballance2.Utils.CloneUtils
local Log = Ballance2.Log
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

local BallsManagerGameObject = nil
local GamePlayManagerGameObject = nil
local GamePlayUIGameObject = nil
local GamePlayPreviewUIGameObject = nil
local GameSectorManagerGameObject = nil
local GameMusicManagerGameObject = nil
local GameTranfoManagerGameObject = nil
local GameLevelBrizGameObject = nil
local GameUFOAnimControllerGameObject = nil

---游戏玩模块全局索引
---@class GamePlay
GamePlay = {
  BallManager = nil, ---@type BallManager
  BallPiecesControll = nil, ---@type BallPiecesControll
  CamManager = nil, ---@type CamManager
  GamePlayManager = nil, ---@type GamePlayManager
  GamePreviewManager = nil, ---@type GamePreviewManager
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

  Game.GamePlay = GamePlay
  
  coroutine.resume(coroutine.create(function()
    Game.UIManager:SetUIOverlayVisible(true)    
    Yield(WaitForSeconds(0.2))

    --初始化基础对象
    GamePlayManagerGameObject = CloneUtils.CloneNewObject(Game.CorePackage:GetPrefabAsset('GamePlayManager.prefab'), 'GamePlayManager')
    Game.Manager:SetGameBaseCameraVisible(false)
    BallsManagerGameObject = CloneUtils.CloneNewObject(Game.CorePackage:GetPrefabAsset('BallManager.prefab'), 'GameBallsManager')
    GameSectorManagerGameObject = CloneUtils.CloneNewObject(Game.CorePackage:GetPrefabAsset('SectorManager.prefab'), 'GameSectorManager')
    GameMusicManagerGameObject = CloneUtils.CloneNewObject(Game.CorePackage:GetPrefabAsset('MusicManager.prefab'), 'GameMusicManager')
    GameTranfoManagerGameObject = CloneUtils.CloneNewObject(Game.CorePackage:GetPrefabAsset('AminTranfo.prefab'), 'GameTranfoManager')
    GameLevelBrizGameObject = CloneUtils.CloneNewObject(Game.CorePackage:GetPrefabAsset('LevelBriz.prefab'), 'GameLevelBriz')
    GameUFOAnimControllerGameObject = CloneUtils.CloneNewObject(Game.CorePackage:GetPrefabAsset('PE_UFO.prefab'), 'GameUFOAnimController')
    --GamePlayUI
    GamePlayUIGameObject = Game.UIManager:InitViewToCanvas(Game.CorePackage:GetPrefabAsset('GamePlayUI.prefab'), 'GamePlayUI', false).gameObject
    Yield(WaitForSeconds(0.1))
    GamePlayUIGameObject:SetActive(false)
    --GamePlayUI
    GamePlayPreviewUIGameObject = Game.UIManager:InitViewToCanvas(Game.CorePackage:GetPrefabAsset('GamePreviewUI.prefab'), 'GamePlayPreviewUI', false).gameObject
    Yield(WaitForSeconds(0.2))
    GamePlayPreviewUIGameObject:SetActive(false)

    Yield(WaitForSeconds(0.2))
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
  if (not Slua.IsNull(GamePlayPreviewUIGameObject)) then UnityEngine.Object.Destroy(GamePlayPreviewUIGameObject) end 
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
  GamePlayPreviewUIGameObject = nil
  GamePlayManagerGameObject = nil
  BallsManagerGameObject = nil

  Game.GamePlay = nil
end