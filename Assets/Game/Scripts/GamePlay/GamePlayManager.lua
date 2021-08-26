local SkyBoxUtils = Ballance2.Game.Utils.SkyBoxUtils
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local KeyListener = Ballance2.Sys.Utils.KeyListener
local GameSettingsManager = Ballance2.Config.GameSettingsManager
local GameSoundType = Ballance2.Sys.Services.GameSoundType
local KeyCode = UnityEngine.KeyCode
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local WaitUntil = UnityEngine.WaitUntil

---游戏管理器
---@class GamePlayManager : GameLuaObjectHostClass
---@field GamePhysicsWorld PhysicsWorld
GamePlayManager = ClassicObject:extend()

function GamePlayManager:new()

  self.GameLightGameObject = nil
  self.GameLightA = nil
  self.GameLightB = nil

  self.StartLife = 3
  self.StartPoint = 1000
  self.LevelScore = 100 ---当前关卡的基础分数

  self.CurrentLevelName = ''
  self.CurrentPoint = 0 ---当前时间点数
  self.CurrentLife = 0 ---当前生命数
  self.CurrentSector = 0 ---当前小节
  self.CurrentLevelPass = false
  
  self.CurrentDisableStart = false
  self.CurrentEndWithUFO = false

  self._IsGamePlaying = false
  self._IsCountDownPoint = false

  GamePlay.GamePlayManager = self
end
function GamePlayManager:Start()
  self:_InitSounds()
  self:_InitKeyEvents()
  self:_InitSetings()
end
function GamePlayManager:OnDestroy()
  if (not Slua.IsNull(self.GameLightGameObject)) then UnityEngine.Object.Destroy(self.GameLightGameObject) end 
  self.GameLightGameObject = nil
end
function GamePlayManager:FixedUpdate()
  --分数每半秒减一
  if self._IsCountDownPoint and self.CurrentPoint > 0 then
    self.CurrentPoint = self.CurrentPoint - 1
    GameUI.GamePlayUI:SetPointText(self.CurrentPoint)
  end
end

function GamePlayManager:_InitSounds() 
  self._SoundBallFall = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds:Misc_Fall.wav'), false, true, 'Misc_Lightning')
  self._SoundAddLife = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds:Misc_extraball.wav'), false, true, 'Misc_Lightning')
  self._SoundLastSector = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds.music:Music_EndCheckpoint.wav'), false, true, 'Misc_Lightning')
  self._SoundFinnal = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds.music:Music_Final.wav'), false, true, 'Misc_Lightning')
  self._SoundLastFinnal = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds.music:Music_LastFinal.wav'), false, true, 'Misc_Lightning')
  self._SoundLastSector.loop = true
end
function GamePlayManager:_InitKeyEvents() 
  self.keyListener = KeyListener.Get(self.gameObject)
  --ESC键
  self.keyListener:AddKeyListen(KeyCode.Escape, function (key, down)
    if down then
      if GameUI.WinScoreUIControl and GameUI.WinScoreUIControl:IsInSeq() then
        GameUI.WinScoreUIControl:Skip()
      elseif self._IsGamePlaying then
        self:PauseLevel(true)
      else
        if self.CurrentLevelPass then
          --跳过最后的分数UI
          --#TODO: 分数UI
        elseif Game.UIManager:GetCurrentPage().PageName == 'PageGamePause' then
          self:ResumeLevel()
        end
      end
    end
  end)
  --ENTER键
  self.keyListener:AddKeyListen(KeyCode.KeypadEnter, function (key, down)
    if down then
      if GameUI.WinScoreUIControl and GameUI.WinScoreUIControl:IsInSeq() then
        GameUI.WinScoreUIControl:Skip()
      end
    end
  end)
end
function GamePlayManager:_InitSetings() 
  local GameSettings = GameSettingsManager.GetSettings("core")
  GameSettings:RegisterSettingsUpdateCallback('video', function (groupName, action)
    if Game.LevelBuilder._CurrentLevelSkyLayer ~= nil then 
      if GameSettings:GetBool('video.cloud', true) then Game.LevelBuilder._CurrentLevelSkyLayer:SetActive(true)
      else GameSettings:GetBool('video.cloud', true) Game.LevelBuilder._CurrentLevelSkyLayer:SetActive(false) end
    end
  end)
end 
function GamePlayManager:_Stop(controlStatus) 
  self._IsGamePlaying = false
  self._IsCountDownPoint = false
  --禁用控制
  GamePlay.BallManager:SetControllingStatus(controlStatus)
  --禁用音乐
  GamePlay.MusicManager:DisableBackgroundMusic()
end
function GamePlayManager:_Start() 
  self._IsGamePlaying = true
  self._IsCountDownPoint = true

  if self.CurrentDisableStart then return end

  coroutine.resume(coroutine.create(function()
    --初始位置
    local startRestPoint = GamePlay.SectorManager.CurrentLevelRestPoints[self.CurrentSector].point
    local startPos = startRestPoint.transform.position

    GamePlay.BallManager:PlayLighting(startPos, true, true)
    Yield(WaitUntil(function () return not GamePlay.BallManager:IsLighting() end)) --等待闪电完成

    --开始控制
    GamePlay.BallManager:SetNextRecoverPos(startPos)
    GamePlay.BallManager:SetControllingStatus(BallControlStatus.Control) 

    --开始音乐
    GamePlay.MusicManager:EnableBackgroundMusic()
  end))

end
function GamePlayManager:_SetCamPos()
  local startRestPoint = GamePlay.SectorManager.CurrentLevelRestPoints[self.CurrentSector].point
  GamePlay.CamManager:SetPosAndDirByRestPoint(startRestPoint)
end

---LevelBuilder 就绪，现在GamePlayManager进行初始化
function GamePlayManager:Init() 
  coroutine.resume(coroutine.create(function()
    --UI
    Game.UIManager:CloseAllPage()
    GameUI.GamePlayUI.gameObject:SetActive(true)
    --设置初始分数\生命球
    self.CurrentLife = self.StartLife
    self.CurrentPoint = self.StartPoint
    GameUI.GamePlayUI:SetLifeBallCount(self.CurrentLife)
    GameUI.GamePlayUI:SetPointText(self.CurrentPoint)
    ---进入第一小节
    GamePlay.SectorManager:SetCurrentSector(1)
    self._SetCamPos()
    Game.UIManager:MaskBlackFadeOut(1)
    --播放开始音乐
    Game.SoundManager:PlayFastVoice('core.sounds:Misc_StartLevel.wav', GameSoundType.Normal)
    Game.LevelBuilder:CallLevelCustomModEvent('beforeStart')

    Game.Mediator:DispatchGlobalEvent('GAME_START', '*')

    Yield(WaitForSeconds(1))

    --模拟
    self.GamePhysicsWorld.Simulating = true
    --开始
    self:_Start()
  end))
  
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

---加载下一关
function GamePlayManager:NextLevel() 
  --黑色进入
  Game.UIManager:MaskBlackFadeIn(1)
end
---重新开始关卡
function GamePlayManager:RestartLevel() 
  --黑色进入
  Game.UIManager:MaskBlackFadeIn(1)

  self:_Stop(BallControlStatus.NoControl)

  Game.Mediator:DispatchGlobalEvent('GAME_RESTART', '*')

  coroutine.resume(coroutine.create(function()

    Yield(WaitForSeconds(0.8))
    --重置所有节
    GamePlay.SectorManager:ResetAllSector(false)

    Yield(WaitForSeconds(0.5))

    --开始
    self:Init()
  end))

end
---退出关卡
function GamePlayManager:QuitLevel() 
  
  Game.Mediator:DispatchGlobalEvent('GAME_QUIT', '*')

  Game.UIManager:CloseAllPage()
  coroutine.resume(coroutine.create(function()
    --播放加载声音
    Game.SoundManager:PlayFastVoice('core.sounds:Menu_load.wav', GameSoundType.Normal)
    --黑色进入
    Game.UIManager:MaskBlackFadeIn(1)
    Yield(WaitForSeconds(1))
    Game.LevelBuilder:UnLoadLevel()
  end))
end
---暂停关卡
---@param showPauseUI boolean 是否显示暂停界面
function GamePlayManager:PauseLevel(showPauseUI) 
  self:_Stop(BallControlStatus.UnleashingMode)

  --停止模拟
  self.GamePhysicsWorld.Simulating = false

  Game.Mediator:DispatchGlobalEvent('GAME_PAUSE', '*')

  --UI
  if showPauseUI then
    Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)
    Game.UIManager:GoPage('PageGamePause') 
  end
end
---继续关卡
function GamePlayManager:ResumeLevel() 

  --停止继续
  self.GamePhysicsWorld.Simulating = true

  Game.Mediator:DispatchGlobalEvent('GAME_RESUME', '*')

  --UI
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)
  Game.UIManager:CloseAllPage()
  self:_Start()
end

---球坠落
function GamePlayManager:Fall() 

  if self.CurrentLevelPass then return end

  --下落音乐
  self._SoundBallFall:Play()

  if self.CurrentLife > 0 then
    
    Game.Mediator:DispatchGlobalEvent('GAME_FALL', '*')
    --禁用控制
    self:_Stop(BallControlStatus.Control)

    self.CurrentLife = self.CurrentLife - 1
    GameUI.GamePlayUI:RemoveLifeBall()
    Game.UIManager:MaskWhiteFadeIn(1)

    coroutine.resume(coroutine.create(function()
      Yield(WaitForSeconds(2))
 
      --禁用控制
      self:_Stop(BallControlStatus.NoControl)
      self._SoundBallFall:Stop()

      --重置
      self._SetCamPos()
      self._Start()
      Game.UIManager:MaskWhiteFadeOut(1)
    end))
  else
    
    Game.Mediator:DispatchGlobalEvent('GAME_FAIL', '*')
    --禁用控制
    self:_Stop(BallControlStatus.UnleashingMode)
    --延时显示失败菜单
    coroutine.resume(coroutine.create(function()
      Yield(WaitForSeconds(2))
      Game.UIManager:GoPage('PageGameFail') 
    end))
  end
end
---过关
function GamePlayManager:Pass() 

  if self.CurrentLevelPass then return end

  Game.Mediator:DispatchGlobalEvent('GAME_PASS', '*')

  self.CurrentLevelPass = true
  self._SoundLastSector:Stop() --停止最后一小节的音乐
  self:_Stop(BallControlStatus.UnleashingMode)

  if self.CurrentEndWithUFO then --播放结尾的UFO动画
    self._SoundLastFinnal:Play() --播放音乐

    --#TODO: UFO动画
  else
    self._SoundFinnal:Play() --播放音乐

    coroutine.resume(coroutine.create(function()
      Yield(WaitForSeconds(2))
      Game.UIManager:GoPage('PageGameWin')
      Yield(WaitForSeconds(2))
      GameUI.WinScoreUIControl:StartSeq()
    end))
  end

end

---激活变球序列
---@param tranfo P_Trafo_Base
---@param targetType string 要变成的目标球类型
---@param color Color 变球器颜色
function GamePlayManager:ActiveTranfo(tranfo, targetType, color) 
  if self._IsTranfoIn then
    return
  end
  self._IsTranfoIn = true

  coroutine.resume(coroutine.create(function()

    local targetPos = tranfo.gameObject.transform.position

    GamePlay.BallManager:SetNextRecoverPos(targetPos)
    --快速将球锁定并移动至目标位置
    GamePlay.BallManager:FastMoveTo(targetPos, 1)

    Yield(WaitForSeconds(1))

    --播放变球动画
    GamePlay.TranfoManager:PlayAnim(targetPos, color, tranfo, function ()
      --切换球
      GamePlay.BallManager:SetCurrentBall(targetType, BallControlStatus.Control)
    end)
  end))
  
end

---添加生命
function GamePlayManager:AddLife() 
  self._SoundAddLife:Play()
  self.CurrentLife = self.CurrentLife + 1
  GameUI.GamePlayUI:AddLifeBall()
end
---添加时间点数
---@param count number|nil 时间点数，默认为10
function GamePlayManager:AddPoint(count) 
  self.CurrentPoint = self.CurrentPoint + (count or 10)
  GameUI.GamePlayUI:SetPointText(self.CurrentPoint)
  GameUI.GamePlayUI:TwinklePoint()
end

function CreateClass_GamePlayManager() return GamePlayManager() end