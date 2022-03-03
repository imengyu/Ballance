local KeyCode = UnityEngine.KeyCode
local Text = UnityEngine.UI.Text
local Image = UnityEngine.UI.Image
local Button = UnityEngine.UI.Button
local Vector3 = UnityEngine.Vector3
local Axis = UnityEngine.Animations.Axis
local GameObject = UnityEngine.GameObject
local ConstraintSource = UnityEngine.Animations.ConstraintSource

local I18N = Ballance2.Services.I18N.I18N
local GamePackage = Ballance2.Package.GamePackage
local GameSoundType = Ballance2.Services.GameSoundType
local CorePackage = GamePackage.GetCorePackage()

---第一关教程管理器
---@class Tutorial : GameLuaObjectHostClass
---@field Pfeil_HochHost ParentConstraint
---@field Pfeil_Rund01 GameObject
---@field Pfeil_Rund02 GameObject
---@field Pfeil_Hoch GameObject
---@field Tut_Richt_Pfeil ParentConstraint
---@field Tut_Richt_Pfeil01 GameObject
---@field Tut_Richt_Pfeil02 GameObject
---@field Tut_Richt_Pfeil03 GameObject
---@field Tut_Richt_Pfeil04 GameObject
---@field Pfeil_Runter_Trigger TiggerTester
---@field Pfeil_Runter LookAtConstraint
---@field Tut_ExtraLife TiggerTester
---@field Tut_StoneTranfo TiggerTester
---@field Tut_Rampe TiggerTester
---@field Tut_WoodTranfo TiggerTester
---@field Tut_KeyEnd TiggerTester
---@field Tut_ExtraPoint TiggerTester
---@field Tut_Checkpoint TiggerTester
---@field Tut_End TiggerTester
---@field Tut_ExtraLifeEnd GameObject
---@field Tut_StoneTranfoEnd GameObject
---@field Tut_WoodTranfoEnd GameObject
---@field Tut_ExtraPointEnd GameObject
---@field Tut_CheckpointEnd GameObject
Tutorial = ClassicObject:extend()

function Tutorial:new()
  self._Tutorial = false
  self._TutorialStep = 1
  self._TutorialShouldDisablePointDown = false
  self._TutorialBallFinded = false
  self._TutorialCamFinded = false
  self._TutorialCurrWaitkey = nil
  self._TutorialUI = nil ---@type RectTransform
  self._TutorialUIText = nil ---@type Text
  self._TutorialUIBg = nil ---@type Image
  self._TutorialUIButtonContinue = nil ---@type Button
  self._TutorialUIButtonQuit = nil ---@type Button
end
function Tutorial:Start()
  Game.Mediator:RegisterEventHandler(CorePackage, 'CoreTutorialLevelEventHandler', 'TutorialHandler', function (evtName, params)
    if params[1] == 'beforeStart' then
      self.startFun = function ()
        GamePlay.GamePlayManager._ShouldStartByCustom = true
        GamePlay.GamePlayManager.CanEscPause = false
        
        self._TutorialStep = 1
        self._TutorialShouldDisablePointDown = true

        --先隐藏箭头
        self.Tut_Richt_Pfeil01:SetActive(false)
        self.Tut_Richt_Pfeil02:SetActive(false)
        self.Tut_Richt_Pfeil03:SetActive(false)
        self.Tut_Richt_Pfeil04:SetActive(false)
        self.Pfeil_Rund02:SetActive(false)
        self.Pfeil_Rund01:SetActive(false)
        self.Pfeil_Hoch:SetActive(false)
        --初始化教程UI
        self._TutorialUI = Game.UIManager:InitViewToCanvas(CorePackage:GetPrefabAsset('GameTutorialUI.prefab'), "GameTutorialUI", false)
        self._TutorialUIBg = self._TutorialUI.transform:GetChild(0):GetComponent(Image)
        self._TutorialUIText = self._TutorialUI.transform:GetChild(0):GetChild(0):GetComponent(Text)
        self._TutorialUIButtonContinue = self._TutorialUI.transform:GetChild(0):GetChild(1):GetComponent(Button)
        self._TutorialUIButtonQuit = self._TutorialUI.transform:GetChild(0):GetChild(2):GetComponent(Button)
        self._TutorialUI.gameObject:SetActive(false)
        self:StartSeq()

        --移动到球出生位置
        self.Pfeil_HochHost.transform.position = GamePlay.SectorManager.CurrentLevelRestPoints[1].point.transform.position
        --设置箭头跟随摄像机旋转角度
        local constraintSource = ConstraintSource()
        constraintSource.sourceTransform = GamePlay.CamManager._CameraHostTransform
        constraintSource.weight = 1


        if self._TutorialCamFinded then
          self.Pfeil_Runter:SetSource(0, constraintSource)
        else
          self.Pfeil_Runter:AddSource(constraintSource)
        end
        self.Pfeil_Runter.rotationOffset = Vector3(-90, 0, 0)
        if self._TutorialCamFinded then
          self.Pfeil_HochHost:SetSource(0, constraintSource)
        else
          self.Pfeil_HochHost:AddSource(constraintSource)
        end
        self.Pfeil_HochHost:SetRotationOffset(0, Vector3(0, -90, 0)) 
        self._TutorialCamFinded = true

        self._Tutorial = true
      end
      self.fallFun = function ()
        self._Tutorial = true

        self.Tut_Richt_Pfeil01:SetActive(false)
        self.Tut_Richt_Pfeil02:SetActive(false)
        self.Tut_Richt_Pfeil03:SetActive(false)
        self.Tut_Richt_Pfeil04:SetActive(false)

      end
      self.quitFun = function ()
        self._Tutorial = false
        if self._TutorialCurrWaitkey then
          Game.UIManager:DeleteKeyListen(self._TutorialCurrWaitkey)
        end
        --删除UI
        if not Slua.IsNull(self._TutorialUI) then
          UnityEngine.Object.Destroy(self._TutorialUI.gameObject)
          self._TutorialUI = nil
        end
        UnityEngine.Object.Destroy(self.gameObject)

        GamePlay.GamePlayManager.Events:removeListener('Start', self.startFun):removeListener('Fall', self.fallFun):removeListener('Quit', self.quitFun)
      end
      GamePlay.GamePlayManager.Events:addListener('Start', self.startFun):addListener('Fall', self.fallFun):addListener('Quit', self.quitFun)
    end
    return false
  end)
end
function Tutorial:StartSeq()
  local UIManager = Game.UIManager;
  local step1KeyReturn = 0
  local step1KeyQ = 0

  LuaTimer.Add(500, function ()
    self:ShowTutorialText()
  end)

  local funQuit = function ()
    Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.Normal)
    --按 q 退出
    UIManager:DeleteKeyListen(step1KeyReturn)
    self:HideTutorial()
    --继续游戏运行
    GamePlay.GamePlayManager.CanEscPause = true
    GamePlay.GamePlayManager:ResumeLevel(true)

    self.Tut_ExtraLife.onTriggerEnter = nil
    self.Tut_StoneTranfo.onTriggerEnter = nil
    self.Tut_Rampe.onTriggerEnter = nil
    self.Tut_ExtraPoint.onTriggerEnter = nil
    self.Tut_Checkpoint.onTriggerEnter = nil
    self.Tut_End.onTriggerEnter = nil
  end
  local funStep2 = function ()
    Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.Normal)

    --这个时候才开始控制
    GamePlay.BallManager:SetControllingStatus(BallControlStatus.Control)
    GamePlay.GamePlayManager._IsCountDownPoint = true
    GamePlay.GamePlayManager.CanEscPause = true
    self._TutorialShouldDisablePointDown = false

    --绑定当前球
    local ballWoodConstraintSource = ConstraintSource()
    ballWoodConstraintSource.sourceTransform = GamePlay.BallManager.CurrentBall.transform
    ballWoodConstraintSource.weight = 1
    if self._TutorialBallFinded then
      self.Tut_Richt_Pfeil:SetSource(0, ballWoodConstraintSource)
    else
      self.Tut_Richt_Pfeil:AddSource(ballWoodConstraintSource)
    end
    self.Tut_Richt_Pfeil.constraintActive = true
    self.Tut_Richt_Pfeil.rotationAxis = Axis.None
    self._TutorialBallFinded = true;

    --进行下一步，方向键导航
    UIManager.UIFadeManager:AddFadeOut(self.Pfeil_Hoch, 1, true, nil)
    UIManager.UIFadeManager:AddFadeIn(self.Tut_Richt_Pfeil01, 1, nil)
    UIManager.UIFadeManager:AddFadeIn(self.Tut_Richt_Pfeil02, 1, nil)
    UIManager.UIFadeManager:AddFadeIn(self.Tut_Richt_Pfeil03, 1, nil)
    UIManager.UIFadeManager:AddFadeIn(self.Tut_Richt_Pfeil04, 1, nil)

    local BallManager = GamePlay.BallManager;

    --按下按键以后几个箭头有拉长的特效        
    local FlushBallPushListener = function ()
      if(BallManager.KeyStateBack) then
        self.Tut_Richt_Pfeil03.transform.localPosition = Vector3(-4, 0.5, 0) 
      else
        self.Tut_Richt_Pfeil03.transform.localPosition = Vector3(-2, 0.5, 0) 
      end
      if(BallManager.KeyStateForward) then
        self.Tut_Richt_Pfeil04.transform.localPosition = Vector3(6, 1, 0) 
      else
        self.Tut_Richt_Pfeil04.transform.localPosition = Vector3(4, 1, 0) 
      end
      if(BallManager.KeyStateLeft) then
        self.Tut_Richt_Pfeil01.transform.localPosition = Vector3(1, 0, 5) 
      else
        self.Tut_Richt_Pfeil01.transform.localPosition = Vector3(1, 0, 3) 
      end
      if(BallManager.KeyStateRight) then
        self.Tut_Richt_Pfeil02.transform.localPosition = Vector3(1, 0, -5) 
      else
        self.Tut_Richt_Pfeil02.transform.localPosition = Vector3(1, 0, -3) 
      end
    end
    BallManager.Events:addListener('FlushBallPush', FlushBallPushListener)

    ---隐藏四个方向箭头
    local hideRichtPfeil = function ()
      UIManager.UIFadeManager:AddFadeOut(self.Tut_Richt_Pfeil01, 1, true, nil)
      UIManager.UIFadeManager:AddFadeOut(self.Tut_Richt_Pfeil02, 1, true, nil)
      UIManager.UIFadeManager:AddFadeOut(self.Tut_Richt_Pfeil03, 1, true, nil)
      UIManager.UIFadeManager:AddFadeOut(self.Tut_Richt_Pfeil04, 1, true, nil)
      LuaTimer.Add(1000, function ()
        self.Tut_Richt_Pfeil.constraintActive = false
        BallManager.Events:removeListener('FlushBallPush', FlushBallPushListener)  
      end)
    end

    LuaTimer.Add(1000, function ()
      self._TutorialStep = 4
      self:ShowTutorialText()
    end)

    --这个按键箭头最多15秒后隐藏
    LuaTimer.Add(15000, function ()
      if self._TutorialStep == 4 then
        self._TutorialStep = 0
        self:HideTutorial()
        hideRichtPfeil()
      end
    end)
    --或者到达木桥那里隐藏隐藏
    self.Tut_KeyEnd.onTriggerEnter = function ()
      if self._TutorialStep == 4 then
        self._TutorialStep = 0
        self:HideTutorial()
        hideRichtPfeil()
      end
    end
  end
  local funStep1 = function ()
    Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.Normal)

    --进行下一步，空格俯瞰
    UIManager.UIFadeManager:AddFadeOut(self.Pfeil_Rund01, 1, true, nil)
    UIManager.UIFadeManager:AddFadeOut(self.Pfeil_Rund02, 1, true, nil)

    LuaTimer.Add(1000, function ()
      UIManager.UIFadeManager:AddFadeIn(self.Pfeil_Hoch, 1, nil)
      self._TutorialStep = 3
      self:ShowTutorialText()
    end)
    
    self._TutorialCurrWaitkey = UIManager:WaitKey(KeyCode.Return, true, funStep2)
  end
  local commonTurHide = nil
  local funSeq = function ()
    Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.Normal)
    self._TutorialUIButtonQuit.gameObject:SetActive(false)
    UIManager:DeleteKeyListen(step1KeyQ)

    --进行下一步
    self:HideTutorial()

    --出生球序列
    GamePlay.GamePlayManager:_Start(true, function ()
      
      --这是出生球但是不能控制
      GamePlay.BallManager:SetControllingStatus(BallControlStatus.LockMode)

      --显示箭头
      UIManager.UIFadeManager:AddFadeIn(self.Pfeil_Rund01, 1, nil)
      UIManager.UIFadeManager:AddFadeIn(self.Pfeil_Rund02, 1, nil)
      
      self._TutorialStep = 2
      self:ShowTutorialText()

      self._TutorialCurrWaitkey = UIManager:WaitKey(KeyCode.Return, true, funStep1)
    end)

    ---移动箭头至指定位置
    ---@param obj GameObject
    local moveArrowToObject = function (obj)
      local arrowPosition = obj.transform.position
      arrowPosition.y = arrowPosition.y + 3
      self.Pfeil_Runter.transform.position = arrowPosition
      self.Pfeil_Runter.gameObject:SetActive(true)
      self.Pfeil_Runter_Trigger.transform.position = arrowPosition
      self.Pfeil_Runter_Trigger.onTriggerEnter = function ()
        UIManager.UIFadeManager:AddFadeOut(self.Pfeil_Runter.gameObject, 0.5, true, nil)
      end
    end
    commonTurHide = function ()
      Game.SoundManager:PlayFastVoice('core.sounds:Menu_click.wav', GameSoundType.Normal)
      GamePlay.GamePlayManager:ResumeLevel(false)
      self:HideTutorial()
    end
    local commonTurReturn = function ()
      GamePlay.GamePlayManager:PauseLevel(false)
      self._TutorialCurrWaitkey = UIManager:WaitKey(KeyCode.Return, true, commonTurHide)  
    end
    local commonTurTipSound = function ()
      Game.SoundManager:PlayFastVoice('core.sounds:Hit_Stone_Kuppel.wav', GameSoundType.Normal)
    end

    --下面设置几个触发器触发的教程
    self.Tut_ExtraLife.onTriggerEnter = function ()
      if self._TutorialStep < 5 then
        self._TutorialStep = 5
        self:ShowTutorialText()
        commonTurTipSound()
        commonTurReturn()
        moveArrowToObject(self.Tut_ExtraLifeEnd)
      end
    end
    self.Tut_StoneTranfo.onTriggerEnter = function ()
      if self._TutorialStep < 6 then
        self._TutorialStep = 6
        self:ShowTutorialText()
        commonTurTipSound()
        commonTurReturn()
        moveArrowToObject(self.Tut_StoneTranfoEnd)
      end
    end
    self.Tut_Rampe.onTriggerEnter = function (body, other)
      --石球上坡提示
      if other and other.gameObject.tag == "Ball" and other.gameObject.name == 'BallStone' then
        if self._TutorialStep < 7 then
          self._TutorialStep = 7
          self:ShowTutorialText()
          commonTurTipSound()
          commonTurReturn()
          moveArrowToObject(self.Tut_WoodTranfoEnd)
        end
      end
    end
    self.Tut_ExtraPoint.onTriggerEnter = function ()
      if self._TutorialStep < 8 then
        self._TutorialStep = 8
        self:ShowTutorialText()
        commonTurTipSound()
        commonTurReturn()
        moveArrowToObject(self.Tut_ExtraPointEnd)
      end
    end
    self.Tut_Checkpoint.onTriggerEnter = function ()
      if self._TutorialStep < 9 then
        self._TutorialStep = 9
        self:ShowTutorialText()
        commonTurTipSound()
        commonTurReturn()
        moveArrowToObject(self.Tut_CheckpointEnd)
      end
    end
    self.Tut_End.onTriggerEnter = function ()
      if self._TutorialStep < 10 then
        self._TutorialStep = 10
        self:ShowTutorialText()
        commonTurTipSound()
        commonTurReturn()
      end
    end
  end

  self._TutorialUIButtonContinue.onClick:RemoveAllListeners()
  self._TutorialUIButtonQuit.onClick:RemoveAllListeners()
  self._TutorialUIButtonContinue.onClick:AddListener(function ()
    if self._TutorialStep == 1  then
      funSeq()
    elseif self._TutorialStep == 2  then
      funStep1()
    elseif self._TutorialStep == 3  then
      funStep2()
    elseif self._TutorialStep >= 5 then
      commonTurHide()
    end
  end)
  self._TutorialUIButtonQuit.onClick:AddListener(funQuit)

  --步骤1，按 q 退出，按回车继续
  step1KeyQ = UIManager:WaitKey(KeyCode.Q, true, funQuit)
  step1KeyReturn = UIManager:WaitKey(KeyCode.Return, true, funSeq)
end
function Tutorial:ShowTutorialText()
  if self._TutorialUI.gameObject.activeSelf then
    Game.UIManager.UIFadeManager:AddFadeOut(self._TutorialUIText, 0.3, false);
    LuaTimer.Add(350, function ()
      self._TutorialUIText.text = I18N.Tr('core.ui.TutorialText'..self._TutorialStep)
      self._TutorialUIText.gameObject:SetActive(true)
      Game.UIManager.UIFadeManager:AddFadeIn(self._TutorialUIText, 0.3);
    end)
  else
    self._TutorialUIText.text = I18N.Tr('core.ui.TutorialText'..self._TutorialStep)
    self._TutorialUI.gameObject:SetActive(true)
    self._TutorialUIText.gameObject:SetActive(true)
    Game.UIManager.UIFadeManager:AddFadeIn(self._TutorialUIText, 0.5);
    Game.UIManager.UIFadeManager:AddFadeIn(self._TutorialUIBg, 0.5);
  end
  GamePlay.GamePlayManager._IsCountDownPoint = false
end
function Tutorial:HideTutorial()
  Game.UIManager.UIFadeManager:AddFadeOut(self._TutorialUIText, 0.6, true);
  Game.UIManager.UIFadeManager:AddFadeOut(self._TutorialUIBg, 0.6, true);

  LuaTimer.Add(600, function ()
    self._TutorialUI.gameObject:SetActive(false)
  end)

  if not self._TutorialShouldDisablePointDown then
    GamePlay.GamePlayManager._IsCountDownPoint = true
  end
end

function CreateClass:Tutorial()
  return Tutorial()
end