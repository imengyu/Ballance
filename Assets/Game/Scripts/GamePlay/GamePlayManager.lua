local SkyBoxUtils = Ballance2.Game.Utils.SkyBoxUtils
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local KeyListener = Ballance2.Sys.Utils.KeyListener
local GameManager = Ballance2.Sys.GameManager
local GameSoundType = Ballance2.Sys.Services.GameSoundType
local KeyCode = UnityEngine.KeyCode

---游戏管理器
---@class GamePlayManager : GameLuaObjectHostClass
GamePlayManager = ClassicObject:extend()

function GamePlayManager:new()

  self.GameLightGameObject = nil
  self.GameLightA = nil
  self.GameLightB = nil

  self.CurrentScore = 0 ---当前分数
  self.CurrentLife = 0 ---当前生命数
  self.CurrentSector = 0 ---当前小节

  
  self._IsGamePlaying = false
  self._IsCountDownScore = false

  GamePlay.GamePlayManager = self
end
function GamePlayManager:Start()
  self._SoundBallFall = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds:Misc_Fall.wav'), false, true, 'Misc_Lightning')
  self._SoundAddLife = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds:Misc_extraball.wav'), false, true, 'Misc_Lightning')
  self:_InitKeyEvents()
end
function GamePlayManager:OnDestroy()
  if (not Slua.IsNull(self.GameLightGameObject)) then UnityEngine.Object.Destroy(self.GameLightGameObject) end 
  self.GameLightGameObject = nil
end
function GamePlayManager:FixedUpdate()
  --分数每半秒减一
  if self._IsCountDownScore and self.CurrentScore > 0 then
    self.CurrentScore = self.CurrentScore - 1
    GamePlay.GamePlayUI:SetScoreText(self.CurrentScore)
  end
end

function GamePlayManager:_InitKeyEvents() 
  self.keyListener = KeyListener.Get(self.gameObject)
  --ESC键
  self.keyListener:AddKeyListen(KeyCode.Escape, function (key, down)
    if down then
      if self._IsGamePlaying then
        self:PauseLevel()
      else
        if Game.UIManager:GetCurrentPage().PageName == 'PageGamePause' then
          self:ResumeLevel()
        end
      end
    end
  end)
end
function GamePlayManager:_Stop() 
  self._IsGamePlaying = false
  self._IsCountDownScore = false
end
function GamePlayManager:_Start() 
  self._IsGamePlaying = true
  self._IsCountDownScore = true
end

---初始化灯光和天空盒
---@param skyBoxPre string A-K 或者空，为空则使用 customSkyMat 材质
---@param customSkyMat Material 自定义天空盒材质
---@param lightColor Color 灯光颜色
function GamePlayManager:CreateSkyAndLight(skyBoxPre, customSkyMat, lightColor)
  Game.GamePlay.CamManager:SetSkyBox(customSkyMat or SkyBoxUtils.MakeSkyBox(skyBoxPre)) --Init sky

  if self.GameLightGameObject == nil then
    self.GameLightGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/GameLight.prefab'), 'GameLight')
    self.GameLightA = self.GameLightGameObject.transform:Find('Light'):GetComponent(UnityEngine.Light) ---@type Light
    self.GameLightB = self.GameLightGameObject.transform:Find('LightSecond'):GetComponent(UnityEngine.Light) ---@type Light
  end

  self.GameLightA.color = lightColor
  self.GameLightB.color = lightColor
end

---重新开始关卡
function GamePlayManager:RestartLevel() 
  --黑色进入
  Game.UIManager:MaskBlackFadeIn(1)
end
---退出关卡
function GamePlayManager:QuitLevel() 

end
---开始关卡
function GamePlayManager:StartLevel() 
  --播放开始音乐
  Game.SoundManager:PlayFastVoice('core.sounds:Misc_StartLevel.wav', GameSoundType.Background)

  --UI
  Game.UIManager:CloseAllPage()
  GamePlay.GamePlayUI.gameObject:SetActive(true)
  --Hide black
  Game.UIManager:MaskBlackFadeOut(1)

  self:_Start()
end
---暂停关卡
function GamePlayManager:PauseLevel() 
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)

  self:_Stop()
  Game.UIManager:GoPage('PageGamePause')
end
---继续关卡
function GamePlayManager:ResumeLevel() 
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)

  Game.UIManager:CloseAllPage()
  self:_Start()
end

---球坠落
function GamePlayManager:Fall() 

  --禁用控制
  GamePlay.BallManager:SetControllingStatus(BallControlStatus.UnleashingMode)
  --下落音乐
  self._SoundBallFall:Play()

  if self.CurrentLife > 0 then
    self.CurrentLife = self.CurrentLife - 1
    GamePlay.GamePlayUI:RemoveLifeBall()
    Game.UIManager:MaskWhiteFadeIn(1)

  else
    
  end
end

---添加生命
function GamePlayManager:AddLife() 
  self._SoundAddLife:Play()
  self.CurrentLife = self.CurrentLife + 1
  GamePlay.GamePlayUI:AddLifeBall()
end
---添加分数
---@param count number
function GamePlayManager:AddScore(count) 
  self.CurrentScore = self.CurrentScore + count
  GamePlay.GamePlayUI:SetScoreText(self.CurrentScore)
  GamePlay.GamePlayUI:TwinkleScore()
end

function CreateClass_GamePlayManager() return GamePlayManager() end