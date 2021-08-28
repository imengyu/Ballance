local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local UIAnchorPosUtils = Ballance2.Sys.UI.Utils.UIAnchorPosUtils
local Image = UnityEngine.UI.Image
local Color = UnityEngine.Color
local Vector2 = UnityEngine.Vector2
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
  local PageGameWin = GameUIManager:RegisterPage('PageGameWin', 'PageCommon')
  local PageEndScore = GameUIManager:RegisterPage('PageEndScore', 'PageTransparent')
  local PageHighscoreEntry = GameUIManager:RegisterPage('PageHighscoreEntry', 'PageCommon')

  PageGamePause:CreateContent(package)
  PageGameQuitAsk:CreateContent(package)
  PageGameRestartAsk:CreateContent(package)
  PageGamePause:CreateContent(package)
  PageGameWin:CreateContent(package)
  PageGameWin.CanEscBack = false
  PageEndScore:CreateContent(package)
  PageEndScore.CanEscBack = false
  PageHighscoreEntry:CreateContent(package)
  PageHighscoreEntry.CanEscBack = false
  PageGameFail:CreateContent(package)
  PageGameFail.CanEscBack = false

  MessageCenter:SubscribeEvent('BtnGameHomeClick', function () GamePlay.GamePlayManager:QuitLevel() end)
  MessageCenter:SubscribeEvent('BtnNextLevellick', function () GamePlay.GamePlayManager:NextLevel() end)
  MessageCenter:SubscribeEvent('BtnGameRestartClick', function () GameUIManager:GoPage('PageGameRestartAsk') end)
  MessageCenter:SubscribeEvent('BtnGameQuitClick', function () GameUIManager:GoPage('PageGameQuitAsk') end)
  MessageCenter:SubscribeEvent('BtnGameFailRestartClick', function ()
    GameUIManager:HideCurrentPage()
    Game.GamePlay.GamePlayManager:RestartLevel()
  end)
  MessageCenter:SubscribeEvent('BtnGameFailQuitClick', function ()
    GameUIManager:HideCurrentPage()
    Game.GamePlay.GamePlayManager:QuitLevel()
  end)
  MessageCenter:SubscribeEvent('BtnResumeClick', function () 
    Game.GamePlay.GamePlayManager:ResumeLevel()
  end)
  local highscoreEntryName = ''
  local HighscoreEntryName = MessageCenter:SubscribeValueBinder('HighscoreEntryName', function(val)
    highscoreEntryName = val
  end)
  PageHighscoreEntry.OnShow = function ()
    HighscoreEntryName:Invoke(PlayerPrefs.GetString('LastEnterHighscoreEntry', 'NAME'))
  end

  MessageCenter:SubscribeEvent('BtnHighscrollEnterClick', function () 
    PlayerPrefs.SetString('LastEnterHighscoreEntry', highscoreEntryName)
    GamePlay.WinScoreUIControl:SaveHighscore(highscoreEntryName)
    GameUIManager:GoPage('PageGameWin') 
  end)
end

---主游戏菜单控制器类
---@class GamePlayUIControl : GameLuaObjectHostClass
---@field _ScoreBoardActive Image
---@field _ScoreText Text
---@field _LifeBoardLeftBaffle Image
---@field _LifeBoardBallPrefab GameObject
---@field _LifeBoardBallInfPrefab GameObject
---@field _LifeBalls RectTransform
GamePlayUIControl = ClassicObject:extend()

function GamePlayUIControl:new() 
  self._CurrentShowLifeBallCount = 0
  self._CurrentMoveBaffleTick = 0.3
  self._MoveBaffleSec = 0.3
  self._CurrentMoveBaffleStart = 0
  self._CurrentMoveBaffleTarget = 0
end
function GamePlayUIControl:Start() 
  self._ScoreBoardActive.gameObject:SetActive(false)
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
        for i = self._LifeBalls.childCount - 1, count + 1, -1 do
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

function CreateClass_GamePlayUIControl() 
  return GamePlayUIControl()
end