local LuaUtils = Ballance2.Utils.LuaUtils
local GameSoundType = Ballance2.Sys.Services.GameSoundType
local PhysicsBody = PhysicsRT.PhysicsBody
local Vector3 = UnityEngine.Vector3
local Table = require('Table')

---纸球定义
---@class Ball : GameLuaObjectHostClass
---@field _CamMgr CamManager
---@field _Rigidbody PhysicsBody
---@field _Pieces GameObject
---@field _PiecesData BallPiecesData
---@field _PiecesSoundName string
---@field _PiecesMinForce number
---@field _PiecesMaxForce number
---@field _Force number
---@field _UpForce number
---@field _DownForce number
Ball = ClassicObject:extend()

function Ball:new()
  self._CamMgr = nil
  self._Rigidbody = nil
  self._Pieces = nil
  self._PiecesMinForce = 0
  self._PiecesMaxForce = 5
  self._PiecesHaveColSound = {}
  self._PiecesColSoundMaxSpeed = 25
  self._PiecesColSoundMinSpeed = 2
  self._Force = 0
  self._UpForce = 0
  self._DownForce = 0
  self._HitSound = {
    Names = {
      All = '',
      Dome = '',
      Metal = '',
      Stone = '',
      Wood = '',
    },
    MaxSpeed = 15,
    MinSpeed = 2,
    ---声音播放延时，多个声音一起播放时如果未到达延时，则后面的声音不会播放
    SoundDelay = 10
  }
  self._RollSoundLockTick = 0
  self._RollSound = {
    Names = {
      All = '',
      Metal = '',
      Stone = '',
      Wood = '',
    },
    MaxSpeed = 0.35,
    MinSpeed = 0.01,
    ---声音播放延时，多个声音一起播放时如果未到达延时，则后面的声音不会播放
    SoundDelay = 8,
  }
end

function Ball:Start()
  self._CamMgr = GamePlay.CamManager
  self._Rigidbody = self.gameObject:GetComponent(PhysicsBody)
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
      local body = child.gameObject:GetComponent(PhysicsBody) ---@type PhysicsBody
      table.insert(data.bodys, body);

      if piecesSound ~= nil and Table.IndexOf(self._PiecesHaveColSound, body.name) ~= -1 then
        local sound = Game.SoundManager:RegisterSoundPlayer(GameSoundType.BallEffect, piecesSound, false, true, 'PiecesSound'..child.name)
        sound.spatialBlend = 1
        sound.maxDistance = 30
        sound.minDistance = 10
        sound.dopplerLevel = 0
        sound.volume = 1
        body.AddContactListener = true
        ---@param this PhysicsBody
        ---@param other PhysicsBody
        ---@param info PhysicsBodyCollisionInfo
        body.onCollisionEnter = function (this, other, info)
          if data.throwed then
            sound.volume = (info.separatingVelocity - self._PiecesColSoundMinSpeed) / (self._PiecesColSoundMaxSpeed - self._PiecesColSoundMinSpeed)
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
      local sound = SoundManager:RegisterSoundPlayer(GameSoundType.BallEffect, value, false, true, 'BallSound'..key)
      self._HitSound['Sound'..key] = sound
    end
  end
  for key, value in pairs(self._RollSound.Names) do
    if type(value) == 'string' and value ~= '' then
      local sound = SoundManager:RegisterSoundPlayer(GameSoundType.BallEffect, value, true, true, 'BallSound'..key)
      sound.loop = true
      sound.volume = 0
      self._RollSound['Sound'..key] = sound
    end
  end
end

---推动
---@param pushType number
function Ball:Push(pushType)
  if self._CamMgr == nil then
    self._CamMgr = GamePlay.CamManager
  end
  if LuaUtils.And(pushType, BallPushType.Left) == BallPushType.Left then
    self._Rigidbody:ApplyLinearImpulse(self._CamMgr.CamLeftVector * self._Force)
  elseif LuaUtils.And(pushType, BallPushType.Right) == BallPushType.Right then
    self._Rigidbody:ApplyLinearImpulse(self._CamMgr.CamRightVector * self._Force)
  end
  if LuaUtils.And(pushType, BallPushType.Forward) == BallPushType.Forward then
    self._Rigidbody:ApplyLinearImpulse(self._CamMgr.CamForwerdVector * self._Force)
  elseif LuaUtils.And(pushType, BallPushType.Back) == BallPushType.Back then
    self._Rigidbody:ApplyLinearImpulse(self._CamMgr.CamBackVector * self._Force)
  end
  if LuaUtils.And(pushType, BallPushType.Up) == BallPushType.Up then
    self._Rigidbody:ApplyLinearImpulse(Vector3.up * self._UpForce)
  elseif LuaUtils.And(pushType, BallPushType.Down) == BallPushType.Down then
    self._Rigidbody:ApplyLinearImpulse(Vector3.down * self._DownForce)
  end
end

function Ball:Update()
  if self._RollSoundLockTick > 0 then
    self._RollSoundLockTick = self._RollSoundLockTick - 1
  end
end
---激活时
function Ball:Active()
  for key, value in pairs(self._RollSound.Names) do
    if type(value) == 'string' and value ~= '' then
      self._RollSound['Sound'..key]:Play()
    end
  end
end
---取消激活时
function Ball:Deactive()
  for key, value in pairs(self._RollSound.Names) do
    if type(value) == 'string' and value ~= '' then
      self._RollSound['Sound'..key]:Stop()
    end
  end
end
---获取碎片
function Ball:GetPieces()
  return self._Pieces
end
---丢出此作类的碎片时
---@param pos Vector3
function Ball:ThrowPieces(pos)
  GamePlay.BallPiecesControll:ThrowPieces(self._PiecesData, pos, self._PiecesMinForce, self._PiecesMaxForce)
end
---回收此作类的碎片时
function Ball:ResetPieces() 
  GamePlay.BallPiecesControll:ResetPieces(self._PiecesData)
end

function CreateClass_Ball()
  return Ball()
end