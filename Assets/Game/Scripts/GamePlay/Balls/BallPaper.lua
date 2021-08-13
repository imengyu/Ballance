local GameManager = Ballance2.Sys.GameManager
local GameSoundManager = GameManager.Instance:GetSystemService('GameSoundManager') ---@type GameSoundManager
local GameSoundType = Ballance2.Sys.Services.GameSoundType

---纸球定义
---@class BallPaper : Ball
---@field _PaperPiecesSound AudioSource
BallPaper = Ball:extend()

function BallPaper:new()
  BallPaper.super.new(self)
  self._PaperPiecesSound = nil
end

function BallPaper:Start()
  Ball.Start(self)
  self._PaperPiecesSound = GameSoundManager:RegisterSoundPlayer(GameSoundType.BallEffect,
    GameSoundManager:LoadAudioResource('core.sounds:Pieces_Paper.wav'), false, true, 'Pieces_Paper')
end

function BallPaper:ThrowPieces()
  self._PaperPiecesSound:Play()
  Ball.ThrowPieces(self)
end

function CreateClass_BallPaper()
  return BallPaper(nil)
end