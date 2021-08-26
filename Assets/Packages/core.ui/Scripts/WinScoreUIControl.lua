local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local WaitUntil = UnityEngine.WaitUntil
local GameSoundType = Ballance2.Sys.Services.GameSoundType

WinScoreUIControl = ClassicObject:extend()

---过关之后的分数统计界面控制
---@class WinScoreUIControl : GameLuaObjectHostClass
---@field ScoreTotal Text
---@field ScoreExtraLives Text
---@field ScoreBouns Text
---@field ScoreTimePoints Text
---@field HighlightBar1 GameObject
---@field HighlightBar2 GameObject
---@field HighlightBar3 GameObject
---@field HighlightBar4 GameObject
function WinScoreUIControl:new() 
end
function WinScoreUIControl:Start() 
  self._SwitchSound = Game.SoundManager:RegisterSoundPlayer(GameSoundType.UI, "core.sounds:Menu_dong.wav", false, true, 'WinScoreUISwitch')
  self._HighscoreSound = Game.SoundManager:RegisterSoundPlayer(GameSoundType.UI, "core.sounds.music:Music_Highscore.wav", false, true, 'WinScoreUISwitch')
  self._CountSound = Game.SoundManager:RegisterSoundPlayer(GameSoundType.UI, "core.sounds:Menu_counter.wav", false, true, 'WinScoreUICount')

  GameUI.WinScoreUIControl = self
end
function WinScoreUIControl:Update() 
  if self._IsCountingPoint then
    if GamePlayManager.CurrentPoint > 0 then
      GamePlayManager.CurrentPoint = GamePlayManager.CurrentPoint - 1
      GamePlay.GamePlayUI:SetPointText(GamePlayManager.CurrentPoint)
      self._CountSound:Stop()
      self._CountSound:Play()
      self._ScoreNTimePoints = self._ScoreNTimePoints + 1
      self._ScoreNTotal = self._ScoreNTotal + 1
      self.ScoreTimePoints.text = tostring(self._ScoreNTimePoints)
      self.ScoreTotal.text = tostring(self._ScoreNTotal)
    else
      self._IsCountingPoint = false
    end
  end
end

---开始分数统计序列
function WinScoreUIControl:StartSeq() 
  local GamePlayManager = GamePlay.GamePlayManager

  self._IsInSeq = true
  self._ScoreNExtraLives = 0
  self._ScoreNTimePoints = 0
  self._ScoreNTotal = 0
  self._Skip = false
  self.HighlightBar1:SetActive(false)
  self.HighlightBar2:SetActive(false)
  self.HighlightBar3:SetActive(false)
  self.HighlightBar4:SetActive(false)
  coroutine.resume(coroutine.create(function()

    Yield(WaitForSeconds(1))
    if self._Skip then return end
    
    --关卡分数
    self._SwitchSound:Play()
    self.HighlightBar1:SetActive(true)
    self.ScoreBouns.text = tostring(GamePlayManager.LevelScore)
    self._ScoreNTotal = GamePlayManager.LevelScore
    self.ScoreTotal.text = tostring(self._ScoreNTotal)

    Yield(WaitForSeconds(1))
    if self._Skip then return end

    --额外时间点

    self._SwitchSound:Play()
    self.HighlightBar1:SetActive(false)
    self.HighlightBar2:SetActive(true)
    self._IsCountingPoint = true
    
    Yield(WaitUntil(function () return not self._IsCountingPoint end))
    if self._Skip then return end

    --额外生命点
    
    self._SwitchSound:Play()
    self.HighlightBar2:SetActive(false)
    self.HighlightBar3:SetActive(true)

    for i = GamePlayManager.CurrentPoint, 1, -1 do
      GamePlay.GamePlayUI:RemoveLifeBall()
      self._ScoreNExtraLives = self._ScoreNExtraLives + 200
      self._ScoreNTotal = self._ScoreNTotal + 200
      self.ScoreExtraLives.text = tostring(self._ScoreNExtraLives)
      self.ScoreTotal.text = tostring(self._ScoreNTotal)

      Yield(WaitForSeconds(0.5))
      if self._Skip then return end
    end

    --完整分数
    if self._Skip then return end

    self._SwitchSound:Play()
    self.HighlightBar3:SetActive(false)
    self.HighlightBar4:SetActive(true)

    Yield(WaitForSeconds(5))
    if self._Skip then return end

    self._IsInSeq = false
    self:_ShowHighscore()
  end))
end
function WinScoreUIControl:IsInSeq() return self._IsInSeq end
---跳过分数统计序列
function WinScoreUIControl:Skip() 
  self._IsInSeq = false
  self._Skip = true
  self._SwitchSound:Play()
  self.HighlightBar1:SetActive(false)
  self.HighlightBar2:SetActive(false)
  self.HighlightBar3:SetActive(false)
  self.HighlightBar4:SetActive(false)

  self._ScoreNTimePoints = self._ScoreNTimePoints + GamePlay.GamePlayManager.CurrentPoint
  self._ScoreNExtraLives = self._ScoreNExtraLives + 200 * GamePlayManager.CurrentLife
  self._ScoreNTotal = GamePlayManager.LevelScore + self._ScoreNTimePoints + self._ScoreNExtraLives

  GamePlayManager.CurrentLife = 0
  GamePlayManager.CurrentPoint = 0
  GamePlay.GamePlayUI:SetPointText(GamePlayManager.CurrentPoint)
  GamePlay.GamePlayUI:SetLifeBallCount(0)
  
  self.ScoreTimePoints.text = tostring(self._ScoreNTimePoints)
  self.ScoreBouns.text = tostring(GamePlayManager.LevelScore)
  self.ScoreExtraLives.text = tostring(self._ScoreNExtraLives)
  
  coroutine.resume(coroutine.create(function()
    Yield(WaitForSeconds(1))
    self:_ShowHighscore()
  end))
end
function WinScoreUIControl:SaveHighscore(entryName) 
  HighscoreManagerAddItem(GamePlay.GamePlayManager.CurrentLevelName, entryName, self._ScoreNTotal)
end
function WinScoreUIControl:_ShowHighscore() 
  Game.UIManager:GoPage('PageGameWin')

  --检查是不是新的高分
  local PageGameWin = Game.UIManager:GetCurrentPage()
  if HighscoreManagerCheckLevelHighScore(GamePlay.GamePlayManager.CurrentLevelName, self._ScoreNTotal) then
    self._HighscoreSound:Play()
    PageGameWin.Content:Find('TextNewHighScore').gameObject:SetActive(true)
    PageGameWin.Content:Find('TextWin').gameObject:SetActive(false)
  else
    PageGameWin.Content:Find('TextNewHighScore').gameObject:SetActive(false)
    PageGameWin.Content:Find('TextWin').gameObject:SetActive(true)
  end
end

function CreateClass_WinScoreUIControl() 
  return WinScoreUIControl
end