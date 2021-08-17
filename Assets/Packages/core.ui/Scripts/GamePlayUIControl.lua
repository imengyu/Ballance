local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local Image = UnityEngine.UI.Image
local Vector2 = UnityEngine.Vector2
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

---创建主游戏菜单UI
---@param package GamePackage
function CreateGamePlayUI(package)

  local PageGamePause = GameUIManager:RegisterPage('PageGamePause', 'PageCommon')
  local PageGameQuitAsk = GameUIManager:RegisterPage('PageGameQuitAsk', 'PageCommon')
  local PageGameRestartAsk = GameUIManager:RegisterPage('PageGameRestartAsk', 'PageCommon')

  PageGamePause:CreateContent(package)
  PageGameQuitAsk:CreateContent(package)
  PageGameRestartAsk:CreateContent(package)
  PageGamePause:CreateContent(package)

  MessageCenter:SubscribeEvent('BtnGameRestartClick', function () GameUIManager:GoPage('PageGameRestartAsk') end)
  MessageCenter:SubscribeEvent('PageGameQuitAsk', function () GameUIManager:GoPage('PageGameQuitAsk') end)
  MessageCenter:SubscribeEvent('BtnGameFailRestartClick', function ()   
    GameUIManager:HideCurrentPage()
    Game.GamePlay.GamePlayManager:RestartLevel()
  end)
  MessageCenter:SubscribeEvent('BtnGameFailQuitClick', function ()   
    GameUIManager:HideCurrentPage()
    Game.GamePlay.GamePlayManager:QuitLevel()
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
local GamePlayUIControl = ClassicObject:extend()

function GamePlayUIControl:new() 
  self._CurrentShowLifeBallCount = 0
  self._CurrentIsMoveLifeLeftBaffle = false
  self._CurrentMoveBaffleCoroutine = nil
  self._CurrentMoveBaffleCurrentVelocity = Vector2.zero
  self._CurrentMoveBaffleTarget = Vector2.zero
end
function GamePlayUIControl:Start() 
  self._ScoreBoardActive.gameObject:SetActive(false)
end
function GamePlayUIControl:FixedUpdate()
  if self._CurrentIsMoveLifeLeftBaffle then
    self._LifeBoardLeftBaffle.rectTransform.anchoredPosition, self._CurrentMoveBaffleCurrentVelocity = Vector2.SmoothDamp(
      self._LifeBoardLeftBaffle.rectTransform.anchoredPosition, self._CurrentMoveBaffleTarget, self._CurrentMoveBaffleCurrentVelocity, 1
    )
  end
end

---闪烁分数面板
function GamePlayUIControl:TwinkleScore() 
  self._ScoreBoardActive.gameObject:SetActive(true)
  GameUIManager.UIFadeManager:AddFadeOut(self._ScoreBoardActive, 1, true, nil)
end
---设置分数面板文字
---@param score number|string 分数面板文字
function GamePlayUIControl:SetScoreText(score) 
  if type(score) == "number" then
    self._ScoreText.text = string.format("%d", score)
  elseif type(score) == "string" then 
    self._ScoreText.text = score;
  end
end

---当前显示的生命球数 +1
function GamePlayUIControl:AddLifeBall()
  if self._CurrentShowLifeBallCount ~= -1 then
    self._CurrentShowLifeBallCount = self._CurrentShowLifeBallCount + 1

    local ball = CloneUtils.CloneNewObjectWithParent(self._LifeBoardBallPrefab, self._LifeBalls)
    GameUIManager.UIFadeManager:AddFadeIn(ball:GetComponent(Image), 1)
  end
end
---当前显示的生命球数 -1
function GamePlayUIControl:RemoveLifeBall()
  if self._CurrentShowLifeBallCount > 0 then
    self._CurrentShowLifeBallCount = self._CurrentShowLifeBallCount - 1

    local ball = self._LifeBalls:GetChild(self._LifeBalls.childCount - 1 - self._CurrentShowLifeBallCount)
    GameUIManager.UIFadeManager:AddFadeOut(ball:GetComponent(Image), 1, true)
    coroutine.resume(coroutine.create(function()
      Yield(WaitForSeconds(1))
      UnityEngine.Object.Destroy(ball)
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
      self:_MoveLifeLeftBaffle();
    else
      if self._LifeBalls.childCount > count then
        --显示数量大于目标，删除多余的
        for i = self._LifeBalls.childCount - 1, count + 1, -1 do
          UnityEngine.Object.Destroy(self._LifeBalls:GetChild(i).gameObject)
        end
        self:_MoveLifeLeftBaffle();
      elseif self._LifeBalls.childCount < count then
        --显示数量小于目标，添加
        for i = 1, count - self._LifeBalls.childCount, 1 do
          CloneUtils.CloneNewObjectWithParent(self._LifeBoardBallPrefab, self._LifeBalls)
        end
        self:_MoveLifeLeftBaffle();
      end
    end
  end
end

function GamePlayUIControl:_MoveLifeLeftBaffle() 
  if self._CurrentMoveBaffleCoroutine ~= nil then
    coroutine.close(self._CurrentMoveBaffleCoroutine)
  end

  self._CurrentMoveBaffleTarget = Vector2(self._LifeBalls.childCount * -27, 0)
  self._CurrentIsMoveLifeLeftBaffle = true
  self._CurrentMoveBaffleCoroutine = coroutine.create(function()
    Yield(WaitForSeconds(1))
    self._CurrentIsMoveLifeLeftBaffle = false
    self._CurrentMoveBaffleCoroutine = nil
  end)

  coroutine.resume(self._CurrentMoveBaffleCoroutine)
end

function CreateClass_GamePlayUIControl() 
  return GamePlayUIControl()
end