local SkyBoxUtils = Ballance2.Game.Utils.SkyBoxUtils
local GameObject = UnityEngine.GameObject
local GameSettingsManager = Ballance2.Services.GameSettingsManager
local GameSoundType = Ballance2.Services.GameSoundType
local DebugUtils = Ballance2.Utils.DebugUtils
local KeyCode = UnityEngine.KeyCode
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local Vector3 = UnityEngine.Vector3
local AudioRolloffMode = UnityEngine.AudioRolloffMode
local Log = Ballance2.Log
local TAG = 'GamePlayManager'

---游戏管理器
---@class GamePlayManager : GameLuaObjectHostClass
---@field GamePhysicsWorld PhysicsEnvironment
GamePlayManager = ClassicObject:extend()

function GamePlayManager:new()

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
  self.CanEscPause = true

  self._IsGamePlaying = false
  self._IsCountDownPoint = false
  self._CommandIds = {}
  self._HideBalloonEndTimerID = nil
  --Used by Tutorial
  self._ShouldStartByCustom = false

  GamePlay.GamePlayManager = self
end
function GamePlayManager:Awake()
  self:_InitSounds()
  self:_InitKeyEvents()
  self:_InitSettings()
  self:_InitEvents()

  local Mediator = Game.Mediator
  local GameDebugCommandServer = Game.Manager.GameDebugCommandServer

  --注册全局事件
  Mediator:SubscribeSingleEvent(Game.CorePackage, "CoreGamePlayManagerInitAndStart", 'GamePlayManager', function (evtName, params)
    self:_InitAndStart()
    return false
  end)

  --注册控制台指令

  table.insert(self._CommandIds, GameDebugCommandServer:RegisterCommand('pass', function () self:Pass() return true end, 0, 'win > 直接过关'))
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
  
  local Mediator = Game.Mediator
  local GameDebugCommandServer = Game.Manager.GameDebugCommandServer

  Game.UIManager:DeleteKeyListen(self.escKeyId)

  --取消注册全局事件

  Mediator:UnRegisterSingleEvent("CoreGamePlayManagerInitAndStart")
  self:_DeleteEvents()

  --取消注册控制台指令
  for _, value in pairs(self._CommandIds) do
    GameDebugCommandServer:UnRegisterCommand(value)
  end
end
function GamePlayManager:FixedUpdate()
  --分数每半秒减一
  if self._IsCountDownPoint and self.CurrentPoint > 0 then
    self.CurrentPoint = self.CurrentPoint - 1
    GameUI.GamePlayUI:SetPointText(self.CurrentPoint)
  end
end

function GamePlayManager:_DeleteEvents() 
  Game.Mediator:UnRegisterEventEmitter('GamePlay')
end
function GamePlayManager:_InitEvents() 
  local events = Game.Mediator:RegisterEventEmitter('GamePlay')
  self.EventStart = events:RegisterEvent('Start') --关卡开始事件
  self.EventQuit = events:RegisterEvent('Quit') --关卡退出事件
  self.EventFall = events:RegisterEvent('Fall') --玩家球掉落事件
  self.EventDeath = events:RegisterEvent('Death') --关卡球掉落并且没有生命游戏结束事件（不会发出Fall）
  self.EventResume = events:RegisterEvent('Resume') --继续事件
  self.EventPause = events:RegisterEvent('Pause') --暂停事件
  self.EventRestart = events:RegisterEvent('Restart') --重新开始关卡事件
  self.EventUfoAnimFinish = events:RegisterEvent('UfoAnimFinish') --UFO动画完成事件
  self.EventPass = events:RegisterEvent('Pass') --过关事件
  self.EventHideBalloonEnd = events:RegisterEvent('HideBalloonEnd') --过关后飞船隐藏事件
  self.EventAddLife = events:RegisterEvent('AddLife') --生命增加事件
  self.EventAddPoint = events:RegisterEvent('AddPoint') --分数增加事件
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
  self._SoundLastSector.maxDistance = 160
end
function GamePlayManager:_InitKeyEvents() 
  --ESC键
  self.escKeyId = Game.UIManager:ListenKey(KeyCode.Escape, function (key, down)
    if down and self.CanEscPause and self._BallBirthed and not self.CurrentLevelPass then
      if self._IsGamePlaying then
        self:PauseLevel(true)
      else
        self:ResumeLevel()
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
---@param isStartBySector boolean
---@param customerFn function|nil
function GamePlayManager:_Start(isStartBySector, customerFn) 
  self._IsGamePlaying = true

  if self.CurrentDisableStart then return end

  --开始音乐
  GamePlay.MusicManager:EnableBackgroundMusic()

  if not self._BallBirthed then
    self._BallBirthed = true
  end

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

      if type(customerFn) == 'function' then
        customerFn()
      else
        GamePlay.BallManager:SetControllingStatus(BallControlStatus.Control)
        self._IsCountDownPoint = true
      end
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
    self._BallBirthed = false
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
    --显示云层 (用于过关之后重新开始，因为之前过关的时候隐藏了云层)
    if Game.Manager.GameSettings:GetBool('video.cloud', true) then
      if Game.LevelBuilder._CurrentLevelSkyLayer and not Game.LevelBuilder._CurrentLevelSkyLayer.activeSelf then
        Game.UIManager.UIFadeManager:AddFadeIn(Game.LevelBuilder._CurrentLevelSkyLayer, 1, nil)
      end
    end

    Log.D(TAG, 'Start')

    self.EventStart:Emit(nil)

    if not self._ShouldStartByCustom then
      Yield(WaitForSeconds(1))

      --模拟
      self.GamePhysicsWorld.Simulate = true
      --开始
      self:_Start(true)
    else
      Log.D(TAG, 'Should Start By Custom')
    end
  end))
  
end

---初始化灯光和天空盒
---@param skyBoxPre string A-K 或者空，为空则使用 customSkyMat 材质
---@param customSkyMat Material 自定义天空盒材质
---@param lightColor Color 灯光颜色
function GamePlayManager:CreateSkyAndLight(skyBoxPre, customSkyMat, lightColor)
  Game.GamePlay.CamManager:SetSkyBox(customSkyMat or SkyBoxUtils.MakeSkyBox(skyBoxPre)) --Init sky
  Ballance2.Services.GameManager.GameLight.color = lightColor
end
--隐藏天空盒和关卡灯光
function GamePlayManager:HideSkyAndLight()
  Game.GamePlay.CamManager:SetSkyBox(nil)
end

function GamePlayManager:_QuitOrLoadNextLevel(loadNext) 

  Log.D(TAG, 'Start Quit Level')

  local callBack = nil
  if loadNext then
    callBack = function ()
      Game.LevelBuilder:LoadLevel(self.NextLevelName)
    end
  end

  --停止隐藏飞船定时
  if self._HideBalloonEndTimerID then
    LuaTimer.Delete(self._HideBalloonEndTimerID)
    self._HideBalloonEndTimerID = nil
  end

  --发送事件
  self.EventQuit:Emit(nil)

  --停止背景音乐
  GamePlay.MusicManager:DisableBackgroundMusic()

  Game.UIManager:CloseAllPage()
  Game.UIManager:MaskBlackFadeIn(0.7)
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_load.wav', GameSoundType.Normal)

  LuaTimer.Add(800, function () 
    --停止模拟
    self.GamePhysicsWorld.Simulate = false
    --关闭球
    self:_Stop(BallControlStatus.NoControl)
    self.CurrentSector = 0
    --隐藏UI
    GameUI.GamePlayUI.gameObject:SetActive(false)
    --发出事件
    Game.LevelBuilder:CallLevelCustomModEvent('beforeQuit')
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

  Log.D(TAG, 'Restart Level')

  self:_Stop(BallControlStatus.NoControl)

  self.EventRestart:Emit(nil)

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

  Log.D(TAG, 'Pause')

  --停止模拟
  self.GamePhysicsWorld.Simulate = false

  --UI
  if showPauseUI then
    Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)
    Game.UIManager:GoPage('PageGamePause') 
  end

  self.EventPause:Emit(nil)
end
---继续关卡
---@param forceRestart boolean 是否强制重置
function GamePlayManager:ResumeLevel(forceRestart) 

  Log.D(TAG, 'Resume')

  --UI
  Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.UI)
  Game.UIManager:CloseAllPage()

  --停止继续
  self.GamePhysicsWorld.Simulate = true
  self:_Start(forceRestart or false)

  self.EventResume:Emit(nil)
end

---球坠落
function GamePlayManager:Fall() 

  if self.CurrentLevelPass then return end

  if self._DethLock then return end
  self._DethLock = true

  Log.D(TAG, 'Fall . CurrentLife: '..tostring(self.CurrentLife))

  --下落音乐
  self._SoundBallFall.volume = 1
  self._SoundBallFall:Play()

  if self.CurrentLife > 0 then
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

    self.EventFall:Emit(nil)
  else
    
    Log.D(TAG, 'Death')

    --禁用控制
    self:_Stop(BallControlStatus.FreeMode)
    
    coroutine.resume(coroutine.create(function()
      Yield(WaitForSeconds(1))
      self:_Stop(BallControlStatus.UnleashingMode)
      GamePlay.MusicManager:DisableBackgroundMusicWithoutAtmo()

      --延时显示失败菜单
      Yield(WaitForSeconds(1))
      Game.UIManager:GoPage('PageGameFail') 

      self._DethLock = false
    end))

    self.EventDeath:Emit(nil)
  end
end
---过关
function GamePlayManager:Pass() 

  if self.CurrentLevelPass then return end

  Log.D(TAG, 'Pass')

  self.CurrentLevelPass = true
  self._SoundLastSector:Stop() --停止最后一小节的音乐
  self:_Stop(BallControlStatus.UnleashingMode)

  GamePlay.BallManager.CanControllCamera = false

  --过关后马上渐变淡出云层，因为摄像机要看到边界的地方了
  if Game.LevelBuilder._CurrentLevelSkyLayer and Game.LevelBuilder._CurrentLevelSkyLayer.activeSelf then
    Game.UIManager.UIFadeManager:AddFadeOut(Game.LevelBuilder._CurrentLevelSkyLayer, 5, true, nil)
  end

  --停止背景音乐
  GamePlay.MusicManager:DisableBackgroundMusicWithoutAtmo()

  if self.CurrentEndWithUFO then --播放结尾的UFO动画
    self._SoundLastFinnal:Play() --播放音乐
    GamePlay.UFOAnimController:StartSeq()
  else
    self._SoundFinnal:Play() --播放音乐
    self:_HideBalloonEnd(false) --开始隐藏飞船
    LuaTimer.Add(6000, function ()
      GameUI.WinScoreUIControl:StartSeq()
    end)
  end

  self.EventPass:Emit(nil)

end

---过关后隐藏飞船
function GamePlayManager:_HideBalloonEnd(fromUfo) 
  if self._HideBalloonEndTimerID then
    LuaTimer.Delete(self._HideBalloonEndTimerID)
    self._HideBalloonEndTimerID = nil
  end
  --60秒后隐藏飞船
  self._HideBalloonEndTimerID = LuaTimer.Add(fromUfo and 40000 or 60000, function ()
    self._HideBalloonEndTimerID = nil
    GamePlay.BallManager:SetControllingStatus(BallControlStatus.NoControl)
    GamePlay.SectorManager.CurrentLevelEndBalloon:Deactive()
    self.EventHideBalloonEnd:Emit(nil)
  end)
end

---UFO 动画完成回调
function GamePlayManager:UfoAnimFinish() 
  self._SoundFinnal:Play()
  self:_HideBalloonEnd(true) --开始隐藏飞船
  GamePlay.BallManager:SetControllingStatus(BallControlStatus.NoControl)
  GameUI.WinScoreUIControl:StartSeq()
  self.EventUfoAnimFinish:Emit(nil)
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
      
      --播放烟雾
      GamePlay.BallManager:PlaySmoke(targetPos)

      --先设置无球
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

  self.EventAddLife:Emit(nil)
end
---添加时间点数
---@param count number|nil 时间点数，默认为10
function GamePlayManager:AddPoint(count) 
  self.CurrentPoint = self.CurrentPoint + (count or 10)
  GameUI.GamePlayUI:SetPointText(self.CurrentPoint)
  GameUI.GamePlayUI:TwinklePoint()
  self.EventAddPoint:Emit({ count })
end

function CreateClass:GamePlayManager() return GamePlayManager() end