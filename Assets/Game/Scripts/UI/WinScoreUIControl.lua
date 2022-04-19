local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local KeyCode = UnityEngine.KeyCode
local Text = UnityEngine.UI.Text
local GameSoundType = Ballance2.Services.GameSoundType
local GameUIManager = Ballance2.Services.GameManager.GetSystemService('GameUIManager') ---@type GameUIManager
local I18N = Ballance2.Services.I18N.I18N

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
---@field _GamePlayManager GamePlayManager
---@field _CountSound AudioSource
---@field _HighscoreSound AudioSource
---@field _SwitchSound AudioSource
---@field _CountingPointEndCallback function
---@field _IsInSeq boolean
---@field _ScoreNTotal number
WinScoreUIControl = ClassicObject:extend()

function WinScoreUIControl:new() 
  self._ThisTimeHasNewHighscore = false
  self._CountingPointEndCallback = nil ---@type function
  GameUI.WinScoreUIControl = self
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
      if self._GamePlayManager.CurrentPoint > 4000 then
        self._GamePlayManager.CurrentPoint = self._GamePlayManager.CurrentPoint - 20
        self._ScoreNTimePoints = self._ScoreNTimePoints + 20
        self._ScoreNTotal = self._ScoreNTotal + 20
      elseif self._GamePlayManager.CurrentPoint > 2000 then
        self._GamePlayManager.CurrentPoint = self._GamePlayManager.CurrentPoint - 10
        self._ScoreNTimePoints = self._ScoreNTimePoints + 10
        self._ScoreNTotal = self._ScoreNTotal + 10
      elseif self._GamePlayManager.CurrentPoint > 2000 then
        self._GamePlayManager.CurrentPoint = self._GamePlayManager.CurrentPoint - 10
        self._ScoreNTimePoints = self._ScoreNTimePoints + 10
        self._ScoreNTotal = self._ScoreNTotal + 10
      elseif self._GamePlayManager.CurrentPoint > 4 then
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
      if type(self._CountingPointEndCallback) == "function" then
        self._CountingPointEndCallback()
      end
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
    
    self.EscKeyID = GameUIManager:WaitKey(KeyCode.Escape, false, function () self:Skip() end)
    self.ReturnKeyID = GameUIManager:WaitKey(KeyCode.Return, false, function () self:Skip() end)

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
    
  end))

  self._CountingPointEndCallback = function ()   
    coroutine.resume(coroutine.create(function()
      if self._Skip then return end

      Yield(WaitForSeconds(1.5))

      --额外生命点
      
      self._SwitchSound:Play()
      self.HighlightBar2:SetActive(false)
      self.HighlightBar3:SetActive(true)

      for i = self._GamePlayManager.CurrentLife, 1, -1 do

        Yield(WaitForSeconds(0.6))
        if self._Skip then return end

        GameUI.GamePlayUI:RemoveLifeBall()
        self._ScoreNExtraLives = self._ScoreNExtraLives + 200
        self._ScoreNTotal = self._ScoreNTotal + 200
        self.ScoreExtraLives.text = tostring(self._ScoreNExtraLives)
        self.ScoreTotal.text = tostring(self._ScoreNTotal)

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
end
function WinScoreUIControl:IsInSeq() return self._IsInSeq end
---跳过分数统计序列
function WinScoreUIControl:Skip() 
  if self._Skip then return end
  
  self._IsInSeq = false
  self._Skip = true
  self._IsCountingPoint = false
  self._SwitchSound:Play()
  self.HighlightBar1:SetActive(false)
  self.HighlightBar2:SetActive(false)
  self.HighlightBar3:SetActive(false)
  self.HighlightBar4:SetActive(true)

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
  self.ScoreTotal.text = tostring(self._ScoreNTotal)

  LuaTimer.Add(3500, function() self:_ShowHighscore() end)
end
function WinScoreUIControl:SaveHighscore(entryName) 
  Game.HighScoreManager.AddItem(GamePlay.GamePlayManager.CurrentLevelName, entryName, self._ScoreNTotal)
  Game.HighScoreManager.AddLevelPassState(GamePlay.GamePlayManager.NextLevelName)
end
function WinScoreUIControl:_ShowHighscore() 
  if self.EscKeyID ~= nil then
    GameUIManager:DeleteKeyListen(self.EscKeyID)
    self.EscKeyID = nil
  end
  if self.ReturnKeyID ~= nil then
    GameUIManager:DeleteKeyListen(self.ReturnKeyID)
    self.ReturnKeyID = nil
  end

  Game.UIManager:GoPage('PageHighscoreEntry')

  --重置文字为0
  self.ScoreTimePoints.text = '0'
  self.ScoreBouns.text = '0'
  self.ScoreExtraLives.text = '0'
  self.ScoreTotal.text = '0'

  --检查是不是新的高分
  local PageHighscoreEntry = Game.UIManager:GetCurrentPage()
  local HighscoreEntryNameTextScore = PageHighscoreEntry.Content:Find('TextScore'):GetComponent(Text) ---@type Text
  
  HighscoreEntryNameTextScore.text = tostring(self._ScoreNTotal)..' <size=20>'..I18N.Tr('core.ui.WinUIPoints')..'</size>'
  
  self._HighscoreSound:Play()
  if Game.HighScoreManager.CheckLevelHighScore(GamePlay.GamePlayManager.CurrentLevelName, self._ScoreNTotal) then
    PageHighscoreEntry.Content:Find('TextNewHighScore').gameObject:SetActive(true)
    PageHighscoreEntry.Content:Find('TextWin').gameObject:SetActive(false)
    self._ThisTimeHasNewHighscore = true
  else
    PageHighscoreEntry.Content:Find('TextNewHighScore').gameObject:SetActive(false)
    PageHighscoreEntry.Content:Find('TextWin').gameObject:SetActive(true)
    self._ThisTimeHasNewHighscore = false
  end
end

function CreateClass:WinScoreUIControl() 
  return WinScoreUIControl
end