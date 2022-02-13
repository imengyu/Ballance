local GameSoundType = Ballance2.Services.GameSoundType
local PhysicsObject = BallancePhysics.Wapper.PhysicsObject
local MeshFilter = UnityEngine.MeshFilter
local Table = require('Table')
local Log = Ballance2.Log

---Ballance 基础球定义
---可继承此类来重写你自己的球
---@class Ball : GameLuaObjectHostClass
---@field _CamMgr CamManager
---@field _Rigidbody PhysicsObject
---@field _Pieces GameObject
---@field _PiecesData BallPiecesData
---@field _PiecesSoundName string
---@field _PiecesMinForce number 碎片抛出最小推力
---@field _PiecesMaxForce number 碎片抛出最大推力
---@field _PiecesPhysicsData table 碎片物理数据
---@field _PiecesPhysCallback function 物理化碎片自定义处理回调 (gameObject, physicsData) => PhysicsObject，如果为nil，则碎片将使用默认参数来物理化
---@field _UpForce number 球的向上推力（调试时Q按键）
---@field _DownForce number 球的向下推力（调试时E按键）
Ball = ClassicObject:extend()

function Ball:new()
  self._Rigidbody = nil
  self._Pieces = nil
  self._PiecesData = nil
  self._PiecesMinForce = 0
  self._PiecesMaxForce = 5
  self._PiecesMass = 1
  self._PiecesHaveColSound = {}
  self._PiecesPhysCallback = nil
  self._PiecesColSoundMaxSpeed = 25
  self._PiecesColSoundMinSpeed = 2
  self._UpForce = 0
  self._DownForce = 0
  self._HitSound = {
    Names = {
      All = '',
      Dome = '',
      Metal = '',
      Stone = '',
      Wood = '',
      Paper = '',
    },
    Sounds = {}, --请勿设置此字段
    MaxSpeed = 20,
    MinSpeed = 2,
  }
  self._RollSound = {
    Names = {
      All = '',
      Metal = '',
      Stone = '',
      Wood = '',
    },
    Sounds = {}, --请勿设置此字段
    PitchBase = 0.6,
    PitchFactor = 0.9,
    VolumeBase = 0,
    VolumeFactor = 1.9,
  }
end

function Ball:Start()
  self._Rigidbody = self.gameObject:GetComponent(PhysicsObject)
  self:_InitPeices()
  self:_InitSounds()
end

function Ball:_InitPeices() 
  --初始化碎片
  if self._Pieces then

    local piecesSound = nil
    if not IsNilOrEmpty(self._PiecesSoundName) then
      piecesSound = Game.SoundManager:LoadAudioResource(self._PiecesSoundName)
    end
  
    local parent = self._Pieces
    local data = {
      parent = parent,
      bodys = {},
      throwed = false
    }
    for i = 0, parent.transform.childCount - 1 do
      local child = parent.transform:GetChild(i)
      local body = nil ---@type PhysicsObject
      if self._PiecesPhysCallback then
        body = self._PiecesPhysCallback(child.gameObject, self._PiecesPhysicsData)
      else
        body = child.gameObject:AddComponent(PhysicsObject)
        if(self._PiecesPhysicsData) then
          --Mesh
          local meshFilter = child:GetComponent(MeshFilter) ---@type MeshFilter
          if meshFilter ~= nil and meshFilter.mesh  ~= nil then
            body.Mass = self._PiecesPhysicsData.Mass
            body.Elasticity = self._PiecesPhysicsData.Elasticity
            body.Friction = self._PiecesPhysicsData.Friction
            body.LinearSpeedDamping = self._PiecesPhysicsData.LinearDamp
            body.RotSpeedDamping = self._PiecesPhysicsData.RotDamp
            body.AutoControlActive = false
            body.DoNotAutoCreateAtAwake = true
            body.EnableCollision = true
            body.AutoMassCenter = false
            body.UseExistsSurface = true
            body.Layer = GameLayers.LAYER_PHY_BALL_PEICES
            body.Convex:Add(meshFilter.mesh)
          else
            Log.W('Ball '..self.gameObject.name, 'Not found MeshFilter or mesh in peices  \''..child.name..'\'')
          end
        else
          Log.W('Ball '..self.gameObject.name, 'No _PiecesPhysCallback or _PiecesPhysicsData found for this ball')
        end
      end
      body.DoNotAutoCreateAtAwake = true
      table.insert(data.bodys, body)

      --碎片声音
      if piecesSound ~= nil and Table.IndexOf(self._PiecesHaveColSound, body.name) ~= -1 then
        local sound = Game.SoundManager:RegisterSoundPlayer(GameSoundType.BallEffect, piecesSound, false, true, 'PiecesSound'..child.name)
        sound.spatialBlend = 1
        sound.maxDistance = 30
        sound.minDistance = 10
        sound.dopplerLevel = 0
        sound.volume = 1
        body.CollisionEventCallSleep = 0.5
        body.EnableCollisionEvent = true
        ---@param this PhysicsObject
        ---@param other PhysicsObject
        ---@param contact_point_ws Vector3
        ---@param speed Vector3
        ---@param surf_normal Vector3
        body.OnPhysicsCollision = function (this, other, contact_point_ws, speed, surf_normal)
          if data.throwed then
            sound.volume = (speed.sqrMagnitude - self._PiecesColSoundMinSpeed) / (self._PiecesColSoundMaxSpeed - self._PiecesColSoundMinSpeed)
            sound:Play()
          end
        end
      end
    end

    self._PiecesData = data
  end
end
function Ball:_InitSounds() 
  --加载球的撞击和滚动声音
  local SoundManager = Game.SoundManager
  for key, value in pairs(self._HitSound.Names) do
    if type(value) == 'string' and value ~= '' then
      local sound2 = SoundManager:RegisterSoundPlayer(GameSoundType.BallEffect, value, false, true, 'BallSoundHit'..key..'2')
      local sound1 = SoundManager:RegisterSoundPlayer(GameSoundType.BallEffect, value, false, true, 'BallSoundHit'..key..'1')
      self._HitSound.Sounds[key] = {
        isSound1 = true,
        sound2 = sound2,
        sound1 = sound1,
      }
    end
  end
  for key, value in pairs(self._RollSound.Names) do
    if type(value) == 'string' and value ~= '' then
      local sound = SoundManager:RegisterSoundPlayer(GameSoundType.BallEffect, value, true, true, 'BallSoundRoll'..key)
      sound.loop = true
      sound.playOnAwake = true
      sound.volume = 0
      sound:Play()
      self._RollSound.Sounds[key] = sound
    end
  end
end

---激活时
function Ball:Active()
end
---取消激活时
function Ball:Deactive()
end
---获取碎片
function Ball:GetPieces()
  return self._Pieces
end
---丢出此作类的碎片时
---@param pos Vector3
function Ball:ThrowPieces(pos)
  if self._PiecesData then
    GamePlay.BallPiecesControll:ThrowPieces(self._PiecesData, pos, self._PiecesMinForce, self._PiecesMaxForce)
  end
end
---回收此作类的碎片时
function Ball:ResetPieces() 
  if self._PiecesData then
    GamePlay.BallPiecesControll:ResetPieces(self._PiecesData)
  end
end

function CreateClass:Ball()
  return Ball()
end