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
  self.CurrentLevelPass = false
  
  self._IsGamePlaying = false
  self._IsCountDownScore = false

  GamePlay.GamePlayManager = self
end
function GamePlayManager:Start()
  self:_InitSounds()
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

function GamePlayManager:_InitSounds() 
  self._SoundBallFall = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds:Misc_Fall.wav'), false, true, 'Misc_Lightning')
  self._SoundAddLife = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds:Misc_extraball.wav'), false, true, 'Misc_Lightning')
  self._SoundLastSector = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds:Music_EndCheckpoint.wav'), false, true, 'Misc_Lightning')
  self._SoundFinnal = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds:Music_Final.wav'), false, true, 'Misc_Lightning')
  self._SoundLastFinnal = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds:Music_LastFinal.wav'), false, true, 'Misc_Lightning')
  self._SoundLastSector.loop = true
  self._SoundLastSector.maxDistance = 100
end
function GamePlayManager:_InitKeyEvents() 
  self.keyListener = KeyListener.Get(self.gameObject)
  --ESC键
  self.keyListener:AddKeyListen(KeyCode.Escape, function (key, down)
    if down then
      if self._IsGamePlaying then
        self:PauseLevel(true)
      else
        if self.CurrentLevelPass then
          --跳过最后的分数UI
          --TODO: 分数UI
        elseif Game.UIManager:GetCurrentPage().PageName == 'PageGamePause' then
          self:ResumeLevel()
        end
      end
    end
  end)
end
function GamePlayManager:_Stop(controlStatus) 
  self._IsGamePlaying = false
  self._IsCountDownScore = false
  --禁用控制
  GamePlay.BallManager:SetControllingStatus(controlStatus)
end
function GamePlayManager:_Start() 
  self._IsGamePlaying = true
  self._IsCountDownScore = true


end

---LevelBuilder 就绪，现在GamePlayManager进行初始化
function GamePlayManager:Init() 
  
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

  self.GameLightGameObject:SetActive(true)
  self.GameLightA.color = lightColor
  self.GameLightB.color = lightColor
end
--隐藏天空盒和关卡灯光
function GamePlayManager:HideSkyAndLight()
  Game.GamePlay.CamManager:SetSkyBox(nil)
  self.GameLightGameObject:SetActive(false)
end

---重新开始关卡
function GamePlayManager:RestartLevel() 
  --黑色进入
  Game.UIManager:MaskBlackFadeIn(1)

  --TODO: 重新开始关卡

end
---退出关卡
function GamePlayManager:QuitLevel() 
  --黑色进入
  Game.UIManager:MaskBlackFadeIn(1)

  --TODO: 退出关卡
end
---开始关卡
function GamePlayManager:StartLevel() 
  --播放开始音乐
  Game.SoundManager:PlayFastVoice('core.sounds:Misc_StartLevel.wav', GameSoundType.Normal)

  --UI
  Game.UIManager:CloseAllPage()
  GamePlay.GamePlayUI.gameObject:SetActive(true)
  --Hide black
  Game.UIManager:MaskBlackFadeOut(1)

  self:_Start()
end
---暂停关卡
---@param showPauseUI boolean 是否显示暂停界面
function GamePlayManager:PauseLevel(showPauseUI) 
  self:_Stop(BallControlStatus.LockMode)

  if showPauseUI then
    Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)
    Game.UIManager:GoPage('PageGamePause') 
  end
end
---继续关卡
function GamePlayManager:ResumeLevel() 
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)

  Game.UIManager:CloseAllPage()
  self:_Start()
end

---球坠落
function GamePlayManager:Fall() 

  if self.CurrentLevelPass then return end

  --禁用控制
  self:_Stop(BallControlStatus.UnleashingMode)
  --下落音乐
  self._SoundBallFall:Play()

  if self.CurrentLife > 0 then
    self.CurrentLife = self.CurrentLife - 1
    GamePlay.GamePlayUI:RemoveLifeBall()
    Game.UIManager:MaskWhiteFadeIn(1)

    --TODO: 重生
  else
    --TODO: 失败
  end
end
---过关
function GamePlayManager:Pass() 

  if self.CurrentLevelPass then
    return
  end

  self.CurrentLevelPass = true
  self._SoundLastSector:Stop() --停止最后一小节的音乐
  self:_Stop(BallControlStatus.UnleashingMode)

  if Game.LevelBuilder._CurrentLevelJson.level.endWithUFO then --播放结尾的UFO动画
    self._SoundLastFinnal:Play() --播放音乐

    --TODO: UFO动画
  else
    self._SoundFinnal:Play() --播放音乐

    --TODO: 过关
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