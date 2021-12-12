---背景音乐管理器
---@class BallSoundManager : GameLuaObjectHostClass
BallSoundManager = ClassicObject:extend()

local Table = require('Table')
local TAG = 'BallSoundManager'

function BallSoundManager:new() 
  GamePlay.BallSoundManager = self
  self._CurrentSoundLayerId = 32
  self._CustomSoundLayer = {}
  self._CustomSoundLayerHandlers = {}
  self._CustomSoundLayerRollHandlers = {}

  --正在播放的滚动声音组
  self._CurrentPlayingRollSounds = {}
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
---添加自定义声音组处理函数
---@param name string 声音组名称
---@param roll boolean 此声音组是否有滚动音效
---@param handlerFn function 回调函数定义 function (type, ball, vol) type：类型(roll滚动声音/hit撞击声音) ball：当前发出声音的球 vol：声音音量
function BallSoundManager:AddCustomSoundLayerHandler(name, roll, handlerFn) 
  local key = self:GetCustomSoundLayerByName(name)
  self._CustomSoundLayerHandlers[key] = handlerFn
  if roll then
    table.insert(self._CustomSoundLayerRollHandlers, key)
  end
end
---移除自定义声音组处理函数
---@param name string 声音组名称
function BallSoundManager:RemoveCustomSoundLayerHandler(name) 
  local key = self:GetCustomSoundLayerByName(name)
  self._CustomSoundLayerHandlers[key] = nil

  local index = Table.IndexOf(self._CustomSoundLayerRollHandlers, key)
  if index >= 0 then
    table.remove(self._CustomSoundLayerRollHandlers, index)
  end
end


---碰撞声音处理
---@param ball Ball
---@param body PhysicsObject
---@param other PhysicsObject
---@param contact_point_ws Vector3
---@param speed Vector3
---@param surf_normal Vector3
function BallSoundManager:HandlerBallHitEvent(ball, body, other, contact_point_ws, speed, surf_normal)
  if other == nil then
    return
  end

  --速度低于限定值，不播放声音
  local voc = (math.abs(speed.x) + math.abs(speed.z)) / 2
  if voc < ball._HitSound.MinSpeed then
    return
  end

  --使用速度计算声音音量
  local vol = (voc - ball._HitSound.MinSpeed) / (ball._HitSound.MaxSpeed - ball._HitSound.MinSpeed)
  local sound = nil

  if ball._HitSound.Sounds.All == nil then

    local layer = other.CustomLayer
    if layer > 31 then
      --判断自定义声音层
      local customHandler = self._CustomSoundLayerHandlers[layer]
      if type(customHandler) == 'function' then
        sound = customHandler('hit', ball, { vol, body, other, contact_point_ws, speed, surf_normal })
      end
    else
      --判断路面层
      layer = other.Layer

      if layer == GameLayers.LAYER_PHY_FLOOR then
        sound = ball._HitSound.Sounds.Stone
      elseif layer == GameLayers.LAYER_PHY_FLOOR_WOODS then
        sound = ball._HitSound.Sounds.Wood
      elseif layer == GameLayers.LAYER_PHY_FLOOR_RAIL then
        sound = ball._HitSound.Sounds.Metal
      end
    end
  else
    sound = ball._HitSound.Sounds.All
  end

  if sound then
    --这里是切换了两个sound的播放，因为碰撞声音很可能
    --一个没有播放完成另一个就来了
    if sound.isSound1 then
      sound.isSound1 = false
      sound.sound1.volume = vol
      if not sound.sound1.isPlaying then
        sound.sound1:Play() 
      end
    else
      sound.isSound1 = true
      sound.sound2.volume = vol
      if not sound.sound2.isPlaying then
        sound.sound2:Play() 
      end
    end
  end

end

---球接触声音处理
---@param ball Ball
---@param isOn boolean
---@param body PhysicsBody
---@param other PhysicsObject
function BallSoundManager:HandlerBallContract(ball, isOn, body, other)

  if other == nil then
    return
  end

  local sound = nil ---@type AudioSource
  local soundId = 0

  if ball._RollSound.Sounds.All == nil then

    local layer = other.CustomLayer
    if layer > 31 then
      --判断自定义声音层
      local customHandler = self._CustomSoundLayerHandlers[layer]
      if type(customHandler) == 'function' then
        soundId = layer
        sound = customHandler('contact', ball, { isOn, body, other })
      end
    else
      --判断路面层
      layer = other.Layer
      if layer == GameLayers.LAYER_PHY_FLOOR then
        sound = ball._RollSound.Sounds.Stone
      elseif layer == GameLayers.LAYER_PHY_FLOOR_WOODS then
        sound = ball._RollSound.Sounds.Wood
      elseif layer == GameLayers.LAYER_PHY_FLOOR_RAIL then
        sound = ball._RollSound.Sounds.Metal   
      end
      soundId = layer
    end
  elseif ball._RollSound.Sounds.All then
    sound = ball._RollSound.Sounds.All
    soundId = 0
  end   
  
  if sound then
    if isOn then
      --加入正在播放声音中
      self._CurrentPlayingRollSounds[soundId] = sound
    else
      self._CurrentPlayingRollSounds[soundId] = nil
      sound.volume = 0
    end
  end

end

---滚动声音音量与速度处理
---@param ball Ball
---@param speedMeter SpeedMeter
function BallSoundManager:HandlerBallRollSpeedChange(ball, speedMeter)
  
  local speed = speedMeter.NowAbsoluteSpeed;
  local vol = 0
  if speed > ball._RollSound.MinSpeed then
    vol = (speed - ball._RollSound.MinSpeed) / (ball._RollSound.MaxSpeed - ball._RollSound.MinSpeed);
  end
  local pit = 0.9 + (vol * 0.1);

  --将音量设置到正在播放的声音中
  for _, value in pairs(self._CurrentPlayingRollSounds) do
    if value then
      value.volume = vol
      value.pitch = pit
    end
  end
end

---强制禁止指定球所有声音
---@param ball Ball 球
function BallSoundManager:ForceDisableBallAllSound(ball) 
  for i, value in pairs(self._CurrentPlayingRollSounds) do
    if value then
      value:Stop()
      self._CurrentPlayingRollSounds[i] = nil
    end
  end
end

function CreateClass:BallSoundManager() 
  return BallSoundManager()
end