---纸球定义
---@class BallStone : Ball
---@field super Ball
BallStone = Ball:extend()

function BallStone:new()
  BallStone.super.new(self)
  self._PiecesSoundName = 'core.sounds:Pieces_Stone.wav'
  self._PiecesHaveColSound = {
    "Ball_Stone_piece01", "Ball_Stone_piece04", "Ball_Stone_piece07",
    "Ball_Stone_piece11", "Ball_Stone_piece14", "Ball_Stone_piece17",
  }
  self._HitSound.Names = {
    All = nil,
    Dome = 'core.sounds:Hit_Stone_Kuppel.wav',
    Metal = 'core.sounds:Hit_Stone_Metal.wav',
    Stone = 'core.sounds:Hit_Stone_Stone.wav',
    Wood = 'core.sounds:Hit_Stone_Wood.wav',
  }
  self._RollSound.Names = {
    All = nil,
    Metal = 'core.sounds:Roll_Stone_Metal.wav',
    Stone = 'core.sounds:Roll_Stone_Stone.wav',
    Wood = 'core.sounds:Roll_Stone_Wood.wav',
  }
end

function CreateClass:BallStone()
  return BallStone(nil)
end