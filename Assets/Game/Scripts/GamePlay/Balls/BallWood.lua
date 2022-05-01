---纸球定义
---@class BallWood : Ball
---@field super Ball
BallWood = Ball:extend()

function BallWood:new()
  BallWood.super.new(self)
  self._PiecesSoundName = 'core.sounds:Pieces_Wood.wav'
  self._PiecesHaveColSound = {
    "Ball_Wood_piece01", "Ball_Wood_piece04", "Ball_Wood_piece07",
    "Ball_Wood_piece11", "Ball_Wood_piece14", "Ball_Wood_piece17",
  }
  self._HitSound.Names = {
    All = nil,
    Dome = 'core.sounds:Hit_Wood_Dome.wav',
    Metal = 'core.sounds:Hit_Wood_Metal.wav',
    Stone = 'core.sounds:Hit_Wood_Stone.wav',
    Wood = 'core.sounds:Hit_Wood_Wood.wav',
    Paper = 'core.sounds:Hit_Paper.wav',
  }
  self._RollSound.Names = {
    All = nil,
    Metal = 'core.sounds:Roll_Wood_Metal.wav',
    Stone = 'core.sounds:Roll_Wood_Stone.wav',
    Wood = 'core.sounds:Roll_Wood_Wood.wav',
  }
  self._RollSound.VolumeFactor = 0.09
end

function CreateClass:BallWood()
  return BallWood(nil)
end