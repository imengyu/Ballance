---@gendoc

local SkyBoxUtils = Ballance2.Game.Utils.SkyBoxUtils
local KeyCode = UnityEngine.KeyCode
local GameSoundType = Ballance2.Services.GameSoundType

---关卡预览管理器，负责关卡预览模式时的一些控制行为。
---@class GamePreviewManager : GameLuaObjectHostClass
---@field GamePhysicsWorld PhysicsEnvironment
---@field GamePreviewCamera Camera
---@field GamePreviewMinimapCamera Camera
---@field GamePreviewFreeCamera FreeCamera
---@field GamePreviewCameraSkyBox Skybox
---@field GameDepthTestCubes GameObject[]
GamePreviewManager = ClassicObject:extend()

function GamePreviewManager:new()
  self._IsGamePlaying = true
  self._IsLoaded = false
  GamePlay.GamePreviewManager = self
end
function GamePreviewManager:Awake()
  local Mediator = Game.Mediator

  --注册全局事件
  Mediator:SubscribeSingleEvent(Game.CorePackage, "CoreGamePreviewManagerInitAndStart", 'GamePreviewManager', function (evtName, params)
    Game.SoundManager:PlayFastVoice('core.sounds:Misc_StartLevel.wav', GameSoundType.Normal) --播放开始音乐
    GameUI.GamePreviewUI.gameObject:SetActive(true)
    GameUI.GamePreviewUI:SetLevelInfo(params[1], params[2], params[3])
    GameUI.GamePreviewUI:SetFreeCamera(self.GamePreviewFreeCamera)
    GamePlay.CamManager.gameObject:SetActive(false)
    GamePlay.MusicManager:EnableBackgroundMusic()
    self.GamePreviewMinimapCamera.gameObject:SetActive(true)
    self.GamePreviewCamera.gameObject:SetActive(true)
    GamePlay.SectorManager:ActiveAllModulsForPreview() ---激活机关

    Game.UIManager:CloseAllPage()
    Game.UIManager:MaskBlackFadeOut(1)
    self._IsLoaded = true

    --ESC键
    self.escKeyId = Game.UIManager:ListenKey(KeyCode.Escape, function (key, down)
      if down then
        if self._IsGamePlaying then
          self:PauseLevel()
        else
          self:ResumeLevel()
        end
      end
    end)
    return false
  end)
end
function GamePreviewManager:OnDestroy()
  local Mediator = Game.Mediator

  if self.escKeyId then
    Game.UIManager:DeleteKeyListen(self.escKeyId)
    self.escKeyId = nil
  end

  --取消注册全局事件
  Mediator:UnRegisterSingleEvent("CoreGamePreviewManagerInitAndStart")
end

function GamePreviewManager:PauseLevel()
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)
  Game.UIManager:GoPage('PageGamePreviewPause') 
  self._IsGamePlaying = false
end
function GamePreviewManager:QuitLevel()
  if self._IsLoaded then
    self._IsLoaded = false
    Game.SoundManager:PlayFastVoice('core.sounds:Menu_load.wav', GameSoundType.Normal)
    Game.UIManager:MaskBlackFadeIn(1)
    LuaTimer.Add(1000, function ()
      GameUI.GamePreviewUI.gameObject:SetActive(false)
      GamePlay.MusicManager:DisableBackgroundMusic()
      GamePlay.CamManager.gameObject:SetActive(true)
      Game.LevelBuilder:UnLoadLevel(nil)
      self.GamePreviewMinimapCamera.gameObject:SetActive(false)
      self.GamePreviewCamera.gameObject:SetActive(false)
    end)
  end
end
function GamePreviewManager:ResumeLevel()
  self._IsGamePlaying = true
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)
  Game.UIManager:CloseAllPage()
end

---初始化灯光和天空盒
---@param skyBoxPre string A-K 或者空，为空则使用 customSkyMat 材质
---@param customSkyMat Material 自定义天空盒材质
---@param lightColor Color 灯光颜色
function GamePreviewManager:CreateSkyAndLight(skyBoxPre, customSkyMat, lightColor)
  self.GamePreviewCameraSkyBox.material = customSkyMat or SkyBoxUtils.MakeSkyBox(skyBoxPre) --Init sky
  Ballance2.Services.GameManager.GameLight.color = lightColor
end
--隐藏天空盒和关卡灯光
function GamePreviewManager:HideSkyAndLight()
  self.GamePreviewCameraSkyBox.material = nil
end

function GamePreviewManager:ToggleDethTethCubesVisible(visible)
  for _, value in ipairs(self.GameDepthTestCubes) do
    value:SetActive(visible)
  end
end
function GamePreviewManager:ToggleSkylayerVisible(visible)
  if Game.LevelBuilder._CurrentLevelSkyLayer then
    Game.LevelBuilder._CurrentLevelSkyLayer:SetActive(visible)
  end
end

function CreateClass:GamePreviewManager() return GamePreviewManager() end