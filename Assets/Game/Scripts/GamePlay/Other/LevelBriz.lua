local Time = UnityEngine.Time
local Color = UnityEngine.Color

local GamePackage = Ballance2.Package.GamePackage
local GameSoundType = Ballance2.Services.GameSoundType

---闪电控制管理器
---@class LevelBriz : GameLuaObjectHostClass
---@field _BrizCurve AnimationCurve
---@field _BrizLight Light
---@field _BrizTime number
LevelBriz = ClassicObject:extend()

function LevelBriz:new()
  self._LightEnable = false
  self._LightTick = math.random(10, 90)
  self._LightFlash = false
  self._LightFlashTick = 0
end
function LevelBriz:Start()
  self._BrizLight.color = Color(0,0,0,1)
  Game.Mediator:RegisterEventHandler(GamePackage.GetCorePackage(), 'CoreBrizLevelEventHandler', 'LevelBrizHandler', function (evtName, params)
    if params[1] == 'beforeStart' then
      GamePlay.GamePlayManager.Events:addListener('Start', function ()
        self._LightEnable = true
      end):addListener('Quit', function ()
        self._LightEnable = false
      end)
    end
    return false
  end)
end
function LevelBriz:Update()
  if self._LightFlash then
    self._LightFlashTick = self._LightFlashTick + Time.deltaTime
    local v = self._BrizCurve:Evaluate(self._LightFlashTick / self._BrizTime)
    if v > 1 then
      self._LightFlash = false
    else
      self._BrizLight.color = Color(v, v, v, 1)
    end
  end
end
function LevelBriz:FixedUpdate()
  if self._LightEnable then
    if self._LightTick > 0 then
      self._LightTick = self._LightTick - 1
    else
      self:LightFlash()
      self._LightTick = math.random(10, 90)
    end
  end
end
function LevelBriz:LightFlash()
  self._LightFlash = true
  self._LightFlashTick = 0
  Game.SoundManager:PlayFastVoice('core.sounds.music:Music_thunder.wav', GameSoundType.Normal)
end

function CreateClass:LevelBriz()
  return LevelBriz()
end