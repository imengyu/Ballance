local GameManager = Ballance2.Services.GameManager
local GameUIManager = GameManager.GetSystemService('GameUIManager') ---@type GameUIManager
local GameSettingsManager = Ballance2.Services.GameSettingsManager
local Log = Ballance2.Log
local CloneUtils = Ballance2.Utils.CloneUtils
local Image = UnityEngine.UI.Image
local Color = UnityEngine.Color
local Vector2 = UnityEngine.Vector2
local InputField = UnityEngine.UI.InputField
local Yield = UnityEngine.Yield
local Mathf = UnityEngine.Mathf
local Time = UnityEngine.Time
local WaitForSeconds = UnityEngine.WaitForSeconds
local PlayerPrefs = UnityEngine.PlayerPrefs

---创建主游戏菜单UI
---@param package GamePackage
function CreateGamePlayUI(package)

  local PageGamePause = GameUIManager:RegisterPage('PageGamePause', 'PageCommon')
  local PageGameFail = GameUIManager:RegisterPage('PageGameFail', 'PageCommon')
  local PageGameQuitAsk = GameUIManager:RegisterPage('PageGameQuitAsk', 'PageCommon')
  local PageGameRestartAsk = GameUIManager:RegisterPage('PageGameRestartAsk', 'PageCommon')
  local PageGameWinRestartAsk = GameUIManager:RegisterPage('PageGameWinRestartAsk', 'PageCommon')
  local PageGameWin = GameUIManager:RegisterPage('PageGameWin', 'PageCommon')
  local PageEndScore = GameUIManager:RegisterPage('PageEndScore', 'PageTransparent')
  local PageHighscoreEntry = GameUIManager:RegisterPage('PageHighscoreEntry', 'PageCommon')
  local PageGamePreviewPause = GameUIManager:RegisterPage('PageGamePreviewPause', 'PageCommon')
  local PageGamePreviewQuitAsk = GameUIManager:RegisterPage('PageGamePreviewQuitAsk', 'PageCommon')
  
  coroutine.resume(coroutine.create(function ()
    
    PageGamePause:CreateContent(package)
    PageGameQuitAsk:CreateContent(package)
    PageGameRestartAsk:CreateContent(package)
    Yield(WaitForSeconds(0.1))
    PageGamePause:CreateContent(package)
    PageGamePause.CanEscBack = false
    PageGameWin:CreateContent(package)
    Yield(WaitForSeconds(0.1))
    PageGameWinRestartAsk:CreateContent(package)
    PageGameWin.CanEscBack = false
    PageEndScore:CreateContent(package)
    PageEndScore.CanEscBack = false
    Yield(WaitForSeconds(0.1))
    PageHighscoreEntry:CreateContent(package)
    PageHighscoreEntry.CanEscBack = false
    PageGameFail:CreateContent(package)
    PageGameFail.CanEscBack = false
    Yield(WaitForSeconds(0.1))
    PageGamePreviewPause:CreateContent(package)
    PageGamePreviewQuitAsk:CreateContent(package)

    MessageCenter:SubscribeEvent('BtnGameHomeClick', function () GamePlay.GamePlayManager:QuitLevel() end)
    MessageCenter:SubscribeEvent('BtnNextLevellick', function () GamePlay.GamePlayManager:NextLevel() end)
    MessageCenter:SubscribeEvent('BtnGameRestartClick', function () GameUIManager:GoPage('PageGameRestartAsk') end)
    MessageCenter:SubscribeEvent('BtnGameWinRestartClick', function () GameUIManager:GoPage('PageGameWinRestartAsk') end)
    MessageCenter:SubscribeEvent('BtnGameQuitClick', function () GameUIManager:GoPage('PageGameQuitAsk') end)
    MessageCenter:SubscribeEvent('BtnGamePreviewQuitClick', function () GameUIManager:GoPage('PageGamePreviewQuitAsk') end)
    MessageCenter:SubscribeEvent('BtnPauseSettingsClick', function () GameUIManager:GoPage('PageSettingsInGame') end)
    MessageCenter:SubscribeEvent('BtnGameFailRestartClick', function ()
      GameUIManager:HideCurrentPage()
      Game.GamePlay.GamePlayManager:RestartLevel()
    end)
    MessageCenter:SubscribeEvent('BtnGameQuitSureClick', function ()
      GameUIManager:HideCurrentPage()
      Game.GamePlay.GamePlayManager:QuitLevel()
    end)
    MessageCenter:SubscribeEvent('BtnGameFailQuitClick', function ()
      GameUIManager:HideCurrentPage()
      Game.GamePlay.GamePlayManager:QuitLevel()
    end)
    MessageCenter:SubscribeEvent('BtnResumeClick', function () Game.GamePlay.GamePlayManager:ResumeLevel() end)
    MessageCenter:SubscribeEvent('BtnPreviewResumeClick', function () Game.GamePlay.GamePreviewManager:ResumeLevel() end)
    MessageCenter:SubscribeEvent('BtnGamePreviewQuitSureClick', function ()
      GameUIManager:HideCurrentPage()
      Game.GamePlay.GamePreviewManager:QuitLevel()
    end)

    --高分默认数据
    local HighscoreEntryName = PageHighscoreEntry.Content:Find('InputField'):GetComponent(InputField) ---@type InputField
    HighscoreEntryName.text = PlayerPrefs.GetString('LastEnterHighscoreEntry', 'NAME')

    --过关之后的下一关按扭
    local ButtonNext = PageGameWin.Content:Find('ButtonNext').gameObject
    PageGameWin.OnShow = function ()
      ButtonNext:SetActive(GamePlay.GamePlayManager.NextLevelName ~= '')
    end

    MessageCenter:SubscribeEvent('BtnHighscrollEnterClick', function () 
      PlayerPrefs.SetString('LastEnterHighscoreEntry', HighscoreEntryName.text)
      GameUI.WinScoreUIControl:SaveHighscore(HighscoreEntryName.text)

      --当前有新的高分，跳转到高分页，否则直接进入下一关菜单
      GameUIManager:GoPage('PageGameWin') 
      if GameUI.WinScoreUIControl._ThisTimeHasNewHighscore then
        LuaTimer.Add(200, function ()
          GameUIManager:GoPage('PageHighscore')
          --高分页加载当前关卡数据
          GameUI.HighscoreUIControl:LoadLevelData(GamePlay.GamePlayManager.CurrentLevelName)
        end)
      end
    end)
  end))
end

---主游戏菜单控制器类
---@class GamePlayUIControl : GameLuaObjectHostClass
---@field _ScoreBoardActive Image
---@field _ScoreText Text
---@field _LifeBoardLeftBaffle Image
---@field _LifeBoardBallPrefab GameObject
---@field _LifeBoardBallInfPrefab GameObject
---@field _LifeBalls RectTransform
---@field _DebugStats GuiStats
---@field _DebugStatValues GuiStatsValue[]
GamePlayUIControl = ClassicObject:extend()

function GamePlayUIControl:new() 
  self._CurrentShowLifeBallCount = 0
  self._CurrentMoveBaffleTick = 0.3
  self._MoveBaffleSec = 0.3
  self._CurrentMoveBaffleStart = 0
  self._CurrentMoveBaffleTarget = 0
  self._CurrentMobileKeyPad = nil ---@type GameObject
  self._CurrentMobileKeyPadShow = false
  self._DebugStatValues = {}
end
function GamePlayUIControl:Start() 
  self._ScoreBoardActive.gameObject:SetActive(false)

  --创建调试信息
  if BALLANCE_DEBUG then
    self._DebugStatValues['CurrentBall'] = self._DebugStats:AddStat('CurrentBall')
    self._DebugStatValues['CurrentStatus'] = self._DebugStats:AddStat('CurrentStatus')
    self._DebugStatValues['Position'] = self._DebugStats:AddStat('Position')
    self._DebugStatValues['Rotation'] = self._DebugStats:AddStat('Rotation')
    self._DebugStatValues['CamDirection'] = self._DebugStats:AddStat('CamDirection')
    self._DebugStatValues['CamState'] = self._DebugStats:AddStat('CamState')
    self._DebugStatValues['Velocity'] = self._DebugStats:AddStat('Velocity')
    self._DebugStatValues['PushValue'] = self._DebugStats:AddStat('PushValue')
    self._DebugStatValues['Sector'] = self._DebugStats:AddStat('Sector')
    self._DebugStatValues['Moduls'] = self._DebugStats:AddStat('Moduls')
  end

  --手机端还需要创建键盘
  if UNITY_ANDROID or UNITY_IOS then
    self:ReBuildMobileKeyPad()
    self._CurrentMobileKeyPadShow = false
    GamePlay.GamePlayManager.EventStart:On(function ()
      self._CurrentMobileKeyPadShow = true
      if self._CurrentMobileKeyPad then
        self._CurrentMobileKeyPad.gameObject:SetActive(true)
      end
    end)
    GamePlay.GamePlayManager.EventQuit:On(function ()
      self._CurrentMobileKeyPadShow = false
      if self._CurrentMobileKeyPad then
        self._CurrentMobileKeyPad.gameObject:SetActive(false)
      end
    end)
  end

  GameUI.GamePlayUI = self
end
function GamePlayUIControl:Update()
  if self._CurrentMoveBaffleTick < self._MoveBaffleSec then
    self._CurrentMoveBaffleTick = self._CurrentMoveBaffleTick + Time.deltaTime
    self._LifeBoardLeftBaffle.rectTransform.anchoredPosition = Vector2(
      Mathf.Lerp(self._CurrentMoveBaffleStart, self._CurrentMoveBaffleTarget, self._CurrentMoveBaffleTick / self._MoveBaffleSec),
      0
    )
  end
end
function GamePlayUIControl:OnDestroy()
  --销毁键盘
  if self._CurrentMobileKeyPad and not Slua.IsNull(self._CurrentMobileKeyPad) then
    UnityEngine.Object.Destroy(self._CurrentMobileKeyPad.gameObject)
    self._CurrentMobileKeyPad = nil
  end
end

---创建手机端键盘
function GamePlayUIControl:ReBuildMobileKeyPad() 
  --销毁键盘
  if self._CurrentMobileKeyPad and not Slua.IsNull(self._CurrentMobileKeyPad) then
    UnityEngine.Object.Destroy(self._CurrentMobileKeyPad.gameObject)
    self._CurrentMobileKeyPad = nil
  end

  --读取键盘设置
  local settings = GameSettingsManager.GetSettings('core')
  local controlKeypadSettting = settings:GetString('control.keypad', 'BaseLeft')

  local keyPad = KeypadUIManager.GetKeypad(controlKeypadSettting)
  if keyPad == nil then
    Log.E('GamePlayUIControl', 'Keypad in setting \''..controlKeypadSettting..'\' not found, use default keypad insted.')
    keyPad = KeypadUIManager.GetKeypad('BaseLeft')
  end

  --创建键盘
  self._CurrentMobileKeyPad = GameUIManager:InitViewToCanvas(keyPad.prefrab, 'GameMobileKeypad', false)

  if not self._CurrentMobileKeyPadShow then
    self._CurrentMobileKeyPad.gameObject:SetActive(false)
  end
end

---闪烁分数面板
function GamePlayUIControl:TwinklePoint() 
  self._ScoreBoardActive.gameObject:SetActive(true)
  GameUIManager.UIFadeManager:AddFadeOut(self._ScoreBoardActive, 1, true)
end
---设置分数面板文字
---@param score number|string 分数面板文字
function GamePlayUIControl:SetPointText(score) 
  if type(score) == "number" then
    self._ScoreText.text = string.format("%d", score)
  elseif type(score) == "string" then 
    self._ScoreText.text = score
  end
end

---当前显示的生命球数 +1
function GamePlayUIControl:AddLifeBall()
  if self._CurrentShowLifeBallCount ~= -1 then
    self._CurrentShowLifeBallCount = self._CurrentShowLifeBallCount + 1

    local ball = CloneUtils.CloneNewObjectWithParent(self._LifeBoardBallPrefab, self._LifeBalls):GetComponent(Image) ---@type Image
    ball.rectTransform:SetAsFirstSibling()
    ball.color = Color(1,1,1,0)
    coroutine.resume(coroutine.create(function()
      self:_MoveLifeLeftBaffle()
      Yield(WaitForSeconds(0.4))
      GameUIManager.UIFadeManager:AddFadeIn(ball, 0.4)
    end))
  end
end
---当前显示的生命球数 -1
function GamePlayUIControl:RemoveLifeBall()
  if self._CurrentShowLifeBallCount > 0 then
    self._CurrentShowLifeBallCount = self._CurrentShowLifeBallCount - 1

    local ball = self._LifeBalls:GetChild(self._LifeBalls.childCount - 1 - self._CurrentShowLifeBallCount)
    GameUIManager.UIFadeManager:AddFadeOut(ball:GetComponent(Image), 0.4, true)
    coroutine.resume(coroutine.create(function()
      Yield(WaitForSeconds(0.4))
      self:_MoveLifeLeftBaffle()
      UnityEngine.Object.Destroy(ball.gameObject)
    end))
  end
end
---直接设置当前显示的生命球数（无动画效果）
---@param count any
function GamePlayUIControl:SetLifeBallCount(count)
  if count ~= self._CurrentShowLifeBallCount then
    self._CurrentShowLifeBallCount = count
    if count == -1 then
      --负1就显示无限大图标
      for i = self._LifeBalls.childCount - 1, 0, -1 do
        UnityEngine.Object.Destroy(self._LifeBalls:GetChild(i).gameObject)
      end
      CloneUtils.CloneNewObjectWithParent(self._LifeBoardBallInfPrefab, self._LifeBalls)
      self:_MoveLifeLeftBaffle()
    else
      if self._LifeBalls.childCount > count then
        --显示数量大于目标，删除多余的
        for i = self._LifeBalls.childCount - 1, count, -1 do
          UnityEngine.Object.Destroy(self._LifeBalls:GetChild(i).gameObject)
        end
        self:_MoveLifeLeftBaffle()
      elseif self._LifeBalls.childCount < count then
        --显示数量小于目标，添加
        for i = 1, count - self._LifeBalls.childCount, 1 do
          CloneUtils.CloneNewObjectWithParent(self._LifeBoardBallPrefab, self._LifeBalls)
        end
        self:_MoveLifeLeftBaffle()
      end
    end
  end
end

function GamePlayUIControl:_MoveLifeLeftBaffle() 
  if self._CurrentShowLifeBallCount < 0 then
    self._CurrentMoveBaffleTarget =  -27
  else
    self._CurrentMoveBaffleTarget =  -(self._CurrentShowLifeBallCount * 27)
  end
  self._CurrentMoveBaffleTick = 0
  self._CurrentMoveBaffleStart = self._LifeBoardLeftBaffle.rectTransform.anchoredPosition.x
end

function CreateClass:GamePlayUIControl() 
  return GamePlayUIControl()
end