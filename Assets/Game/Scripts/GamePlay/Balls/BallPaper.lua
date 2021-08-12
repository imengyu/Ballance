local GameManager = Ballance2.Sys.GameManager
local GameSoundManager = GameManager.Instance:GetSystemService('GameSoundManager') ---@type GameSoundManager
local GameSoundType = Ballance2.Sys.Services.GameSoundType

---球定义
---@class BallPaper : Ball
BallPaper = {
  _PaperPiecesSound = nil, ---@type AudioSource,
} 

function CreateClass_BallPaper()
  
  function BallPaper:new(o)
    o = o or Ball:new(nil)
    setmetatable(o, self)
    self.__index = self
    return o
  end

  function BallPaper:Start()
    Ball.Start(self)
    self._PaperPiecesSound = GameSoundManager:RegisterSoundPlayer(GameSoundType.BallEffect,
      GameSoundManager:LoadAudioResource('core.sounds:Pieces_Paper.wav'), false, true, 'Pieces_Paper')
  end
  function BallPaper:ThrowPieces()
    Ball.ThrowPieces(self)
    self._PaperPiecesSound:Play()
  end

  return BallPaper:new(nil)
end