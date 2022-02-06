local SkyBoxUtils = Ballance2.Game.Utils.SkyBoxUtils
local CloneUtils = Ballance2.Utils.CloneUtils
local KeyListener = Ballance2.Services.InputManager.KeyListener
local GameSettingsManager = Ballance2.Services.GameSettingsManager
local GameSoundType = Ballance2.Services.GameSoundType
local DebugUtils = Ballance2.Utils.DebugUtils
local KeyCode = UnityEngine.KeyCode
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local Vector3 = UnityEngine.Vector3
local AudioRolloffMode = UnityEngine.AudioRolloffMode
local Log = Ballance2.Log

---游戏管理器
---@class GamePlayManager : GameLuaObjectHostClass
---@field GamePhysicsWorld PhysicsEnvironment
GamePlayManager = ClassicObject:extend()

function GamePlayManager:new()

  self.GameLightGameObject = nil
  self.GameLightA = nil
  self.GameLightB = nil

  self.StartLife = 3
  self.StartPoint = 1000
  self.LevelScore = 100 ---当前关卡的基础分数
  self.StartBall = 'BallWood'
  self.NextLevelName = ''

  self.CurrentLevelName = ''
  self.CurrentPoint = 0 ---当前时间点数
  self.CurrentLife = 0 ---当前生命数
  self.CurrentSector = 0 ---当前小节
  self.CurrentLevelPass = false ---获取是否过关
  
  self.CurrentDisableStart = false
  self.CurrentEndWithUFO = false

  self._IsGamePlaying = false
  self._IsCountDownPoint = false
  self._CommandIds = {}

  GamePlay.GamePlayManager = self
end
function GamePlayManager:Awake()
  self:_InitSounds()
  self:_InitKeyEvents()
  self:_InitSettings()

  local Mediator = Game.Mediator;
  local GameDebugCommandServer = Game.Manager.GameDebugCommandServer;

  --注册全局事件

  Mediator:RegisterGlobalEvent('GAME_START')
  Mediator:RegisterGlobalEvent('GAME_RESTART')
  Mediator:RegisterGlobalEvent('GAME_QUIT')
  Mediator:RegisterGlobalEvent('GAME_RESUME')
  Mediator:RegisterGlobalEvent('GAME_PAUSE')
  Mediator:RegisterGlobalEvent('GAME_FALL')
  Mediator:RegisterGlobalEvent('GAME_FAIL')
  Mediator:RegisterGlobalEvent('GAME_PASS')
  Mediator:SubscribeSingleEvent(Game.CorePackage, "CoreGamePlayManagerInitAndStart", 'GamePlayManager', function (evtName, params)
    self:_InitAndStart()
    return false
  end)

  --注册控制台指令

  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('win', function () self:Pass() return true end, 0, 'win > 直接过关'))
  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('fall', function () self:Fall() return true end, 0, 'fall > 触发球掉落死亡'))
  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('restart', function () self:Fall() return true end, 0, 'restart > 重新开始关卡'))
  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('pause', function () self:PauseLevel() return true end, 0, 'pause > 暂停'))
  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('resume', function () self:ResumeLevel() return true end, 0, 'resume > 恢复'))
  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('unload', function () self:QuitLevel() return true end, 0, 'unload > 卸载关卡'))
  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('nextlev', function () self:Fall() return true end, 0, 'nextlev > 加载下一关'))
  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('gos', function (keyword, fullCmd, argsCount, args) 
    local ox, nx = DebugUtils.CheckIntDebugParam(0, args, Slua.out, true, 0)
    if not ox then return false end
      GamePlay.BallManager:SetControllingStatus(BallControlStatus.NoControl)
      GamePlay.SectorManager:SetCurrentSector(nx)
      self:_SetCamPos()
      self:_Start(true)
    return true
  end, 1, 'gos <count:number> > 跳转到指定的小节'))
  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('rebirth', function () 
    GamePlay.BallManager:SetControllingStatus(BallControlStatus.NoControl)
    self:_SetCamPos()
    self:_Start(true) 
  return true end, 0, 'rebirth > 重新出生'))
  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('addlife', function () self:AddLife() return true end, 0, 'addlife > 添加一个生命球'))
  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('addtime', function (keyword, fullCmd, argsCount, args) 
    local ox, nx = DebugUtils.CheckIntDebugParam(0, args, Slua.out, true, 0)
    if not ox then return false end
    self:AddPoint(tonumber(nx))  
    return true
  end, 1, 'addtime <count:number> > 添加时间点 count：要添加数量'))

end
function GamePlayManager:OnDestroy()

  Log.D('GamePlayManager', 'GamePlayManager:OnDestroy')
  
  local Mediator = Game.Mediator;
  local GameDebugCommandServer = Game.Manager.GameDebugCommandServer;

  --取消注册全局事件

  Mediator:UnRegisterSingleEvent("CoreGamePlayManagerInitAndStart")
  Mediator:UnRegisterGlobalEvent('GAME_START')
  Mediator:UnRegisterGlobalEvent('GAME_RESTART')
  Mediator:UnRegisterGlobalEvent('GAME_QUIT')
  Mediator:UnRegisterGlobalEvent('GAME_RESUME')
  Mediator:UnRegisterGlobalEvent('GAME_PAUSE')
  Mediator:UnRegisterGlobalEvent('GAME_FALL')
  Mediator:UnRegisterGlobalEvent('GAME_FAIL')
  Mediator:UnRegisterGlobalEvent('GAME_PASS')

  --取消注册控制台指令
  for _, value in pairs(self._CommandIds) do
    GameDebugCommandServer:UnRegisterCommand(value)
  end

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
  self._SoundBallFall = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds:Misc_Fall.wav'), false, true, 'Misc_Fall')
  self._SoundAddLife = Game.SoundManager:RegisterSoundPlayer(GameSoundType.UI, Game.SoundManager:LoadAudioResource('core.sounds:Misc_extraball.wav'), false, true, 'Misc_extraball')
  self._SoundLastSector = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Background, Game.SoundManager:LoadAudioResource('core.sounds.music:Music_EndCheckpoint.wav'), false, true, 'Music_EndCheckpoint')
  self._SoundFinnal = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds.music:Music_Final.wav'), false, true, 'Music_Final')
  self._SoundLastFinnal = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Normal, Game.SoundManager:LoadAudioResource('core.sounds.music:Music_LastFinal.wav'), false, true, 'Music_LastFinal')
  self._SoundLastSector.loop = true
  self._SoundLastSector.dopplerLevel = 0
  self._SoundLastSector.rolloffMode = AudioRolloffMode.Linear
  self._SoundLastSector.minDistance = 95
  self._SoundLastSector.maxDistance = 130
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
        else
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
function GamePlayManager:_InitSettings() 
  local GameSettings = GameSettingsManager.GetSettings("core")
  GameSettings:RegisterSettingsUpdateCallback('video', function (groupName, action)
    if Game.LevelBuilder._CurrentLevelSkyLayer ~= nil then 
      if GameSettings:GetBool('video.cloud', true) then Game.LevelBuilder._CurrentLevelSkyLayer:SetActive(true)
      else GameSettings:GetBool('video.cloud', true) Game.LevelBuilder._CurrentLevelSkyLayer:SetActive(false) end
    end
    return false
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
function GamePlayManager:_Start(isStartBySector) 
  self._IsGamePlaying = true

  if self.CurrentDisableStart then return end

  --开始音乐
  GamePlay.MusicManager:EnableBackgroundMusic()

  if isStartBySector then
    local startPos = Vector3.zero
    if self.CurrentSector > 0 then
      --初始位置
      local startRestPoint = GamePlay.SectorManager.CurrentLevelRestPoints[self.CurrentSector].point
      startPos = startRestPoint.transform.position
    end
    --等待闪电完成
    GamePlay.BallManager:PlayLighting(startPos, true, true, function ()
      --开始控制
      GamePlay.BallManager:SetNextRecoverPos(startPos)
      GamePlay.BallManager:SetControllingStatus(BallControlStatus.Control)

      self._IsCountDownPoint = true
    end)
  else
    self._IsCountDownPoint = true
    GamePlay.BallManager:SetControllingStatus(BallControlStatus.Control)
  end
end
function GamePlayManager:_SetCamPos()
  if self.CurrentSector > 0 then
    local startRestPoint = GamePlay.SectorManager.CurrentLevelRestPoints[self.CurrentSector].point
    GamePlay.CamManager:SetPosAndDirByRestPoint(startRestPoint):SetTarget(startRestPoint.transform):SetCamLook(true)
  end
end

---LevelBuilder 就绪，现在GamePlayManager进行初始化
function GamePlayManager:_InitAndStart() 

  self.CurrentLevelPass = false
  self.CurrentDisableStart = false
  self._IsGamePlaying = false
  self._IsCountDownPoint = false

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
    --设置初始球
    GamePlay.BallManager:SetCurrentBall(self.StartBall)
    self:_SetCamPos()
    Game.UIManager:MaskBlackFadeOut(1)
    --播放开始音乐
    Game.SoundManager:PlayFastVoice('core.sounds:Misc_StartLevel.wav', GameSoundType.Normal)
    --可控制摄像机了
    GamePlay.BallManager.CanControllCamera = true
    --发出事件
    Game.LevelBuilder:CallLevelCustomModEvent('beforeStart')
    Game.Mediator:DispatchGlobalEvent('GAME_START', '*', {})

    Yield(WaitForSeconds(1))

    --模拟
    self.GamePhysicsWorld.Simulate = true
    --开始
    self:_Start(true)
  end))
  
end

---初始化灯光和天空盒
---@param skyBoxPre string A-K 或者空，为空则使用 customSkyMat 材质
---@param customSkyMat Material 自定义天空盒材质
---@param lightColor Color 灯光颜色
function GamePlayManager:CreateSkyAndLight(skyBoxPre, customSkyMat, lightColor)
  Game.GamePlay.CamManager:SetSkyBox(customSkyMat or SkyBoxUtils.MakeSkyBox(skyBoxPre)) --Init sky

  if self.GameLightGameObject == nil then
    self.GameLightGameObject = CloneUtils.CloneNewObject(Game.CorePackage:GetPrefabAsset('Assets/Game/Prefabs/Core/GameLight.prefab'), 'GameLight')
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
  if self.GameLightGameObject ~= nil then  
    self.GameLightGameObject:SetActive(false)
  end
end

function GamePlayManager:_QuitOrLoadNextLevel(loadNext) 
  local callBack = nil
  if loadNext then
    callBack = function ()
      Game.LevelBuilder:LoadLevel(self.NextLevelName)
    end
  end

  --停止模拟
  self.GamePhysicsWorld.Simulate = false
  
  Game.UIManager:CloseAllPage()
  Game.UIManager:MaskBlackFadeIn(0.7)
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_load.wav', GameSoundType.Normal)

  LuaTimer.Add(800, function () 
    GameUI.GamePlayUI.gameObject:SetActive(false)
    Game.Mediator:DispatchGlobalEvent('GAME_QUIT', '*', {})
    Game.LevelBuilder:UnLoadLevel(callBack)
  end)
end

---加载下一关
function GamePlayManager:NextLevel() 
  if self.NextLevelName == '' then return end
  self:_QuitOrLoadNextLevel(true)
end
---重新开始关卡
function GamePlayManager:RestartLevel() 
  --黑色进入
  Game.UIManager:MaskBlackFadeIn(1)

  self:_Stop(BallControlStatus.NoControl)

  Game.Mediator:DispatchGlobalEvent('GAME_RESTART', '*', {})

  coroutine.resume(coroutine.create(function()

    Yield(WaitForSeconds(0.8))
    --重置所有节
    GamePlay.SectorManager:ResetAllSector(false)
    self.CurrentSector = 0

    Yield(WaitForSeconds(0.5))

    --开始
    self:_InitAndStart()
  end))

end
---退出关卡
function GamePlayManager:QuitLevel() 
  self:_QuitOrLoadNextLevel(false)
end
---暂停关卡
---@param showPauseUI boolean 是否显示暂停界面
function GamePlayManager:PauseLevel(showPauseUI) 
  self:_Stop(BallControlStatus.FreeMode)

  --停止模拟
  self.GamePhysicsWorld.Simulate = false

  Game.Mediator:DispatchGlobalEvent('GAME_PAUSE', '*', {})

  --UI
  if showPauseUI then
    Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)
    Game.UIManager:GoPage('PageGamePause') 
  end
end
---继续关卡
function GamePlayManager:ResumeLevel() 

  --停止继续
  self.GamePhysicsWorld.Simulate = true

  Game.Mediator:DispatchGlobalEvent('GAME_RESUME', '*', {})

  --UI
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)
  Game.UIManager:CloseAllPage()
  self:_Start(false)
end

---球坠落
function GamePlayManager:Fall() 

  if self.CurrentLevelPass then return end

  if self._DethLock then return end
  self._DethLock = true

  --下落音乐
  self._SoundBallFall.volume = 1
  self._SoundBallFall:Play()

  if self.CurrentLife > 0 then
    
    Game.Mediator:DispatchGlobalEvent('GAME_FALL', '*', {})
    --禁用控制
    self:_Stop(BallControlStatus.FreeMode)

    self.CurrentLife = self.CurrentLife - 1
    Game.UIManager:MaskWhiteFadeIn(1)

    coroutine.resume(coroutine.create(function()
      Yield(WaitForSeconds(1))

      --禁用机关
      GamePlay.SectorManager:DeactiveCurrentSector()
      --禁用控制
      self:_Stop(BallControlStatus.NoControl)

      Yield(WaitForSeconds(1))

      Game.UIManager.UIFadeManager:AddAudioFadeOut(self._SoundBallFall, 1)

      --重置机关和摄像机
      GamePlay.SectorManager:ActiveCurrentSector(false)
      self:_SetCamPos()
      self:_Start(true)
      Game.UIManager:MaskWhiteFadeOut(1)
      
      --UI
      Yield(WaitForSeconds(1))
      GameUI.GamePlayUI:RemoveLifeBall()

      self._DethLock = false

    end))
  else
    
    Game.Mediator:DispatchGlobalEvent('GAME_FAIL', '*', {})
    --禁用控制
    self:_Stop(BallControlStatus.FreeMode)
    
    coroutine.resume(coroutine.create(function()
      Yield(WaitForSeconds(1))
      self:_Stop(BallControlStatus.UnleashingMode)

      --延时显示失败菜单
      Yield(WaitForSeconds(1))
      Game.UIManager:GoPage('PageGameFail') 

      self._DethLock = false
    end))
  end
end
---过关
function GamePlayManager:Pass() 

  if self.CurrentLevelPass then return end

  self.CurrentLevelPass = true
  self._SoundLastSector:Stop() --停止最后一小节的音乐
  self:_Stop(BallControlStatus.UnleashingMode)

  GamePlay.BallManager.CanControllCamera = false
  Game.Mediator:DispatchGlobalEvent('GAME_PASS', '*', {})

  if self.CurrentEndWithUFO then --播放结尾的UFO动画
    self._SoundLastFinnal:Play() --播放音乐
    GamePlay.UFOAnimController:StartSeq()
  else
    self._SoundFinnal:Play() --播放音乐
    --30秒后隐藏飞船
    LuaTimer.Add(30000, function ()
      GamePlay.SectorManager.CurrentLevelEndBalloon:Deactive()
    end)
    LuaTimer.Add(5000, function ()
      GameUI.WinScoreUIControl:StartSeq()
    end)
  end

end

---UFO 动画完成回调
function GamePlayManager:UfoAnimFinish() 
  self._SoundFinnal:Play()
  GamePlay.MusicManager:DisableBackgroundMusic()
  GamePlay.BallManager:SetControllingStatus(BallControlStatus.NoControl)
  GameUI.WinScoreUIControl:StartSeq()
  --38秒后隐藏飞船
  LuaTimer.Add(30000, function ()
    GamePlay.SectorManager.CurrentLevelEndBalloon:Deactive()
  end)
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

  local targetPos = tranfo.gameObject.transform:TransformPoint(Vector3(0, 2, 0))
  local oldBallType =  GamePlay.BallManager.CurrentBallName

  --快速回收目标球碎片
  GamePlay.BallManager:ResetPeices(targetType)
  GamePlay.BallManager:SetNextRecoverPos(targetPos)
  --快速将球锁定并移动至目标位置
  GamePlay.BallManager:FastMoveTo(targetPos, 0.1, function ()
    --播放变球动画
    GamePlay.TranfoManager:PlayAnim(tranfo.gameObject.transform, color, tranfo.gameObject, function ()
      
      GamePlay.BallManager:SetNoCurrentBall()

      --切换球并且抛出碎片
      GamePlay.BallManager:ThrowPeices(oldBallType, targetPos)

      --激活新球
      GamePlay.BallManager:SetCurrentBall(targetType, BallControlStatus.Control)

      --重置状态
      tranfo:Reset()
      self._IsTranfoIn = false
    end)
  end)  
end

---添加生命
function GamePlayManager:AddLife() 
  self.CurrentLife = self.CurrentLife + 1
  LuaTimer.Add(317, function ()
    self._SoundAddLife:Play()
    GameUI.GamePlayUI:AddLifeBall()
  end)
end
---添加时间点数
---@param count number|nil 时间点数，默认为10
function GamePlayManager:AddPoint(count) 
  self.CurrentPoint = self.CurrentPoint + (count or 10)
  GameUI.GamePlayUI:SetPointText(self.CurrentPoint)
  GameUI.GamePlayUI:TwinklePoint()
end

function CreateClass:GamePlayManager() return GamePlayManager() end