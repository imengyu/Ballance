local GameSoundType = Ballance2.Sys.Services.GameSoundType
local CommonUtils = Ballance2.Utils.CommonUtils
local Vector3 = UnityEngine.Vector3

---纸球定义
---@class BallPaper : Ball
---@field super Ball
---@field _PaperPiecesSound AudioSource
BallPaper = Ball:extend()

function BallPaper:new()
  BallPaper.super.new(self)
  self._PaperPiecesSound = nil
  self._HitSound.Names.All = 'core.sounds:Hit_Paper.wav'
  self._RollSound.Names.All = 'core.sounds:Roll_Paper.wav'
  ---自定义物理化碎片
  ---@param go GameObject
  ---@param data table
  self._PiecesPhysCallback = function (go, data)
    local body = go:AddComponent(BallancePhysics.Wapper.PhysicsObject) ---@type PhysicsObject
    body.Mass = CommonUtils.RandomFloat(0.02, 0.09)
    body.Elasticity = data.Elasticity
    body.Friction = CommonUtils.RandomFloat(1, 5)
    body.LinearSpeedDamping = data.LinearDamp
    body.RotSpeedDamping = data.RotDamp
    return body
  end
end

function BallPaper:Start()
  Ball.Start(self)
  self._PaperPiecesSound = Game.SoundManager:RegisterSoundPlayer(GameSoundType.BallEffect,
    Game.SoundManager:LoadAudioResource('core.sounds:Pieces_Paper.wav'), false, true, 'Pieces_Paper')
end

function BallPaper:ThrowPieces(pos)
  self._PaperPiecesSound:Play()
  Ball.ThrowPieces(self, pos)

  --纸球碎片将施加一个恒力，以达到被风吹走的效果
  for _, body in ipairs(self._PiecesData.bodys) do
    body:AddConstantForce(Vector3(-0.03, 0, 0.03)) --施加力
  end
  
end

function BallPaper:ResetPieces()
  Ball.ResetPieces(self)

  --去掉纸球碎片恒力
  for _, body in ipairs(self._PiecesData.bodys) do
    body:ClearConstantForce()
  end
end

function CreateClass:BallPaper()
  return BallPaper(nil)
end