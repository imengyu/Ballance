local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local WaitUntil = UnityEngine.WaitUntil
local Text = UnityEngine.UI.Text
local GameSoundType = Ballance2.Sys.Services.GameSoundType
local I18N = Ballance2.Sys.Language.I18N

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
    if self._GamePlayManager.CurrentPoint > 0 then
      if self._GamePlayManager.CurrentPoint > 4 then
        self._GamePlayManager.CurrentPoint = self._GamePlayManager.CurrentPoint - 4
        self._ScoreNTimePoints = self._ScoreNTimePoints + 4
        self._ScoreNTotal = self._ScoreNTotal + 4
      else
        self._ScoreNTimePoints = self._ScoreNTimePoints + self._GamePlayManager.CurrentPoint
        self._ScoreNTotal = self._ScoreNTotal + self._GamePlayManager.CurrentPoint
        self._GamePlayManager.CurrentPoint = 0
      end
      
      GameUI.GamePlayUI:SetPointText(self._GamePlayManager.CurrentPoint)
      self._CountSound:Stop()
      self._CountSound:Play()
      self.ScoreTimePoints.text = tostring(self._ScoreNTimePoints)
      self.ScoreTotal.text = tostring(self._ScoreNTotal)
    else
      self._IsCountingPoint = false
    end
  end
end

---开始分数统计序列
function WinScoreUIControl:StartSeq() 
  self._GamePlayManager = GamePlay.GamePlayManager
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

    Yield(WaitForSeconds(5))

    Game.UIManager:GoPage('PageEndScore')

    Yield(WaitForSeconds(1.7))
    if self._Skip then return end
    
    --关卡分数
    self._SwitchSound:Play()
    self.HighlightBar1:SetActive(true)
    self.ScoreBouns.text = tostring(self._GamePlayManager.LevelScore)
    self._ScoreNTotal = self._GamePlayManager.LevelScore
    self.ScoreTotal.text = tostring(self._ScoreNTotal)

    Yield(WaitForSeconds(1.7))
    if self._Skip then return end

    --额外时间点

    self._SwitchSound:Play()
    self.HighlightBar1:SetActive(false)
    self.HighlightBar2:SetActive(true)
    self._IsCountingPoint = true
    
    Yield(WaitUntil(function () return not self._IsCountingPoint end))
    if self._Skip then return end

    Yield(WaitForSeconds(1.5))

    --额外生命点
    
    self._SwitchSound:Play()
    self.HighlightBar2:SetActive(false)
    self.HighlightBar3:SetActive(true)

    for i = self._GamePlayManager.CurrentLife, 1, -1 do

      Yield(WaitForSeconds(0.6))

      GameUI.GamePlayUI:RemoveLifeBall()
      self._ScoreNExtraLives = self._ScoreNExtraLives + 200
      self._ScoreNTotal = self._ScoreNTotal + 200
      self.ScoreExtraLives.text = tostring(self._ScoreNExtraLives)
      self.ScoreTotal.text = tostring(self._ScoreNTotal)

      if self._Skip then return end
    end

    Yield(WaitForSeconds(1.5))

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

  local GamePlayManager = self._GamePlayManager

  self._ScoreNTimePoints = self._ScoreNTimePoints + GamePlayManager.CurrentPoint
  self._ScoreNExtraLives = self._ScoreNExtraLives + 200 * GamePlayManager.CurrentLife
  self._ScoreNTotal = GamePlayManager.LevelScore + self._ScoreNTimePoints + self._ScoreNExtraLives

  GamePlayManager.CurrentLife = 0
  GamePlayManager.CurrentPoint = 0
  GameUI.GamePlayUI:SetPointText(GamePlayManager.CurrentPoint)
  GameUI.GamePlayUI:SetLifeBallCount(0)
  
  self.ScoreTimePoints.text = tostring(self._ScoreNTimePoints)
  self.ScoreBouns.text = tostring(GamePlayManager.LevelScore)
  self.ScoreExtraLives.text = tostring(self._ScoreNExtraLives)
  
  LuaTimer.Add(1000, function ()
    self:_ShowHighscore()
  end)
end
function WinScoreUIControl:SaveHighscore(entryName) 
  HighscoreManagerAddItem(GamePlay.GamePlayManager.CurrentLevelName, entryName, self._ScoreNTotal)
end
function WinScoreUIControl:_ShowHighscore() 
  Game.UIManager:GoPage('PageHighscoreEntry')

  --检查是不是新的高分
  local PageHighscoreEntry = Game.UIManager:GetCurrentPage()
  local HighscoreEntryNameTextScore = PageHighscoreEntry.Content:Find('TextScore'):GetComponent(Text) ---@type Text
  
  HighscoreEntryNameTextScore.text = tostring(self._ScoreNTotal)..' <size=20>'..I18N.Tr('ui.gameWin.points')..'</size>';
  
  self._HighscoreSound:Play()
  if HighscoreManagerCheckLevelHighScore(GamePlay.GamePlayManager.CurrentLevelName, self._ScoreNTotal) then
    PageHighscoreEntry.Content:Find('TextNewHighScore').gameObject:SetActive(true)
    PageHighscoreEntry.Content:Find('TextWin').gameObject:SetActive(false)
  else
    PageHighscoreEntry.Content:Find('TextNewHighScore').gameObject:SetActive(false)
    PageHighscoreEntry.Content:Find('TextWin').gameObject:SetActive(true)
  end
end

function CreateClass_WinScoreUIControl() 
  return WinScoreUIControl
end