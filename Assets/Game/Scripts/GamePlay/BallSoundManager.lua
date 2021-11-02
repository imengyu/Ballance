---背景音乐管理器
---@class BallSoundManager : GameLuaObjectHostClass
BallSoundManager = ClassicObject:extend()

local TAG = 'BallSoundManager'

function BallSoundManager:new() 
  GamePlay.BallSoundManager = self

  self._CurrentSoundLayerId = 32
  self._CustomSoundLayer = {}
  self._CustomSoundLayerLockTick = {}
  self._CustomSoundLayerHandlers = {}
  self._CustomSoundLayerLockTick[GameLayers.LAYER_PHY_FLOOR] = 0
  self._CustomSoundLayerLockTick[GameLayers.LAYER_PHY_FLOOR_WOODS] = 0
  self._CustomSoundLayerLockTick[GameLayers.LAYER_PHY_FLOOR_RAIL] = 0
end

---通过名称获取自定义的声音组
---@param name string
function BallSoundManager:GetCustomSoundLayerByName(name) 
  if type(self._CustomSoundLayer[name]) == 'number' then
    return self._CustomSoundLayer[name]
  else
    local id = self._CurrentSoundLayerId
    self._CurrentSoundLayerId = self._CurrentSoundLayerId + 1
    self._CustomSoundLayer[name] = id
    return id
  end
end

function BallSoundManager:Update()
  for index, value in pairs(self._CustomSoundLayerLockTick) do
    if value and value > 0 then
      self._CustomSoundLayerLockTick[index] = value - 1
    end
  end
end
---添加自定义声音组处理函数
---@param name string 声音组名称
---@param handlerFn function 回调函数定义 function (type, ball, vol) type：类型(roll滚动声音/hit撞击声音) ball：当前发出声音的球 vol：声音音量
function BallSoundManager:AddCustomSoundLayerHandler(name, handlerFn) 
  local key = self:GetCustomSoundLayerByName(name)
  self._CustomSoundLayerLockTick[key] = 0
  self._CustomSoundLayerHandlers[key] = handlerFn
end
---移除自定义声音组处理函数
---@param name string 声音组名称
function BallSoundManager:RemoveCustomSoundLayerHandler(name) 
  local key = self:GetCustomSoundLayerByName(name)
  self._CustomSoundLayerLockTick[key] = nil
  self._CustomSoundLayerHandlers[key] = nil
end

---碰撞声音处理
---@param ball Ball
---@param body PhysicsBody
---@param other PhysicsBody
---@param speedMeter SpeedMeter
---@param info PhysicsBodyCollisionInfo
function BallSoundManager:HandlerBallCollisionEnter(ball, speedMeter, body, other, info)
  if other == nil then
    return
  end

  local voc = math.abs(info.separatingVelocity)

  --使用速度计算声音音量
  local vol = (voc - ball._HitSound.MinSpeed) / (ball._HitSound.MaxSpeed - ball._HitSound.MinSpeed)

  if ball._HitSound.SoundAll == nil then

    local layer = other.CustomLayer
    if layer > 31 then
      --判断自定义声音层
      if self._CustomSoundLayerLockTick[layer] and self._CustomSoundLayerLockTick[layer] <= 0 then
        self._CustomSoundLayerLockTick[layer] = ball._HitSound.SoundDelay

        local customHandler = self._CustomSoundLayerHandlers[layer]
        if type(customHandler) == 'function' then
          customHandler('hit', ball, vol)
        end

      end
      return
    end

    --判断路面层
    layer = other.Layer
    if self._CustomSoundLayerLockTick[layer] and self._CustomSoundLayerLockTick[layer] <= 0 then
      self._CustomSoundLayerLockTick[layer] = ball._HitSound.SoundDelay

      local sound = nil
      if layer == GameLayers.LAYER_PHY_FLOOR then
        sound = ball._HitSound.SoundStone
      elseif layer == GameLayers.LAYER_PHY_FLOOR_WOODS then
        sound = ball._HitSound.SoundWood
      elseif layer == GameLayers.LAYER_PHY_FLOOR_RAIL then
        sound = ball._HitSound.SoundMetal
      end

      if sound then
        sound.volume = vol
        if not sound.isPlaying then
          sound:Play() 
        end
      end
    end
  else
    local layer = other.Layer
    if self._CustomSoundLayerLockTick[layer] and self._CustomSoundLayerLockTick[layer] <= 0 then 
      self._CustomSoundLayerLockTick[layer] = ball._HitSound.SoundDelay
      
      local sound = ball._HitSound.SoundAll
      if sound then
        sound.volume = vol
        if not sound.isPlaying then
          sound:Play() 
        end
      end
    end
  end
end

---滚动声音处理
---@param ball Ball
---@param body PhysicsBody
---@param other PhysicsBody
---@param speedMeter SpeedMeter
---@param info PhysicsBodyCollisionInfo
function BallSoundManager:HandlerBallCollisionStay(ball, speedMeter, body, other, info)

  --增加延时，防止多个碰撞事件发出多个碰撞声音
  if ball._RollSoundLockTick <= 0 then
    ball._RollSoundLockTick = ball._RollSound.SoundDelay

    if other == nil then
      return
    end

    --使用速度计算声音音量
    local vol = (speedMeter.NowAbsoluteSpeed - ball._RollSound.MinSpeed) / (ball._RollSound.MaxSpeed - ball._RollSound.MinSpeed)
    if ball._RollSound.SoundAll == nil then

      local layer = other.CustomLayer
      if layer > 31 then
        --判断自定义声音层
        local customHandler = self._CustomSoundLayerHandlers[layer]
        if type(customHandler) == 'function' then
          customHandler('hit', ball, vol)
        end
        return
      end

      --判断路面层
      local sound = nil
      layer = other.Layer
      if layer == GameLayers.LAYER_PHY_FLOOR then
        sound = ball._RollSound.SoundStone
      elseif layer == GameLayers.LAYER_PHY_FLOOR_WOODS then
        sound = ball._RollSound.SoundWood
      elseif layer == GameLayers.LAYER_PHY_FLOOR_RAIL then
        sound = ball._RollSound.SoundMetal   
      end
      if sound then
        sound.volume = vol
      end
    else
      if ball._RollSound.SoundAll then
        ball._RollSound.SoundAll.volume = vol
      end
    end   
  end
end

---滚动声音处理
---@param ball Ball
---@param body PhysicsBody
---@param other PhysicsBody
---@param speedMeter SpeedMeter
function BallSoundManager:HandlerBallCollisionLeave(ball, speedMeter, body, other)

  if ball._RollSound.SoundAll == nil then
    --判断路面层
    local layer = other.Layer
    if layer == GameLayers.LAYER_PHY_FLOOR then
      if ball._RollSound.SoundStone then
        ball._RollSound.SoundStone.volume = 0
      end
    elseif layer == GameLayers.LAYER_PHY_FLOOR_WOODS then
      if ball._RollSound.SoundWood then
        ball._RollSound.SoundWood.volume = 0
      end
    elseif layer == GameLayers.LAYER_PHY_FLOOR_RAIL then
      if ball._RollSound.SoundMetal then
        ball._RollSound.SoundMetal.volume = 0
      end
    else
      --如果不是路面层，则判断自定义声音层
      layer = other.CustomLayer
      local customHandler = self._CustomSoundLayerHandlers[layer]
      if type(customHandler) == 'function' then
        customHandler('roll', ball, 0)
      end
    end
  else
    if ball._RollSound.SoundAll then
      ball._RollSound.SoundAll.volume = 0
    end
  end
end

---强制禁止指定球所有声音
---@param ball Ball 球
function BallSoundManager:ForceDisableBallAllSound(ball) 
  if ball._RollSound.SoundAll == nil then
    if ball._RollSound.SoundStone then ball._RollSound.SoundStone.volume = 0 end
    if ball._RollSound.SoundWood then ball._RollSound.SoundWood.volume = 0 end
    if ball._RollSound.SoundMetal then ball._RollSound.SoundMetal.volume = 0 end
  else
    ball._RollSound.SoundAll.volume = 0
  end   
end

function CreateClass_BallSoundManager() 
  return BallSoundManager()
end