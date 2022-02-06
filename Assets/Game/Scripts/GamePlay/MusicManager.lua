local GameSoundType = Ballance2.Services.GameSoundType
local Log = Ballance2.Log
local CommonUtils = Ballance2.Utils.CommonUtils

---背景音乐管理器
---@class MusicManager : GameLuaObjectHostClass
MusicManager = ClassicObject:extend()

local TAG = 'MusicManager'

---@class MusicThemeDataStorage
---@field musics AudioClip[]
---@field atmos AudioClip[]
---@field baseInterval number
---@field maxInterval number
---@field atmoInterval number
---@field atmoMaxInterval number
MusicThemeDataStorage = {}

function MusicManager:new() 
  self.Musics = {} ---@type MusicThemeDataStorage
  self.CurrentAudioSource = Game.SoundManager:RegisterSoundPlayer(GameSoundType.Background, nil, false, true, "MusicManagerBackgroundMusic")
  self.CurrentAudioSource.maxDistance = 2000
  self.CurrentAudioTheme = nil ---@type MusicThemeDataStorage
  self.CurrentAudioEnabled = false

  self._CurrentIsAtmo = false
  self._CurrentAudioTick = 0
  self._LastMusicIndex = 0
end
function MusicManager:Start() 
  ---加载内置音乐
  local atmo = {
    Game.SoundManager:LoadAudioResource('core.sounds.music:Music_Atmo_1.wav'),
    Game.SoundManager:LoadAudioResource('core.sounds.music:Music_Atmo_2.wav'),
    Game.SoundManager:LoadAudioResource('core.sounds.music:Music_Atmo_3.wav')
  }

  for i = 1, 5, 1 do
    self.Musics[i] = {
      atmos = atmo,
      musics = {
        Game.SoundManager:LoadAudioResource('core.sounds.music:Music_Theme_'..i..'_1.wav'),
        Game.SoundManager:LoadAudioResource('core.sounds.music:Music_Theme_'..i..'_2.wav'),
        Game.SoundManager:LoadAudioResource('core.sounds.music:Music_Theme_'..i..'_3.wav')
      },
      baseInterval = 20,
      maxInterval = 30,
      atmoInterval = 10,
      atmoMaxInterval = 20,
    }
  end

  GamePlay.MusicManager = self

  self._CommandId = Game.Manager.GameDebugCommandServer:RegisterCommand('bgm', function (eyword, fullCmd, argsCount, args)
    local type = args[1]
    if type == 'enable' then
      self:EnableBackgroundMusic()
      Log.D(TAG, 'EnableBackgroundMusic');
    elseif type == 'disable' then
      self:DisableBackgroundMusic()
      Log.D(TAG, 'DisableBackgroundMusic');
    else
      Log.W(TAG, 'Unknow option '..type);
      return false
    end
    return true
  end, 1, "bgm <enable/disable> 背景音乐管理器命令"..
          "  enable  ▶ 开启背景音乐"..
          "  disable ▶ 关闭背景音乐"
  )
end
function MusicManager:OnDestroy() 
  Game.Manager.GameDebugCommandServer:UnRegisterCommand(self._CommandId)
end

function MusicManager:FixedUpdate() 
  if self.CurrentAudioEnabled and self.CurrentAudioTheme ~= nil then

    --随机时间播放随机音乐
    if self._CurrentAudioTick > 0 then
      self._CurrentAudioTick = self._CurrentAudioTick - 1
    else
      if self._CurrentIsAtmo then
        self._CurrentIsAtmo = false
        if not self.CurrentAudioSource.isPlaying then
          self.CurrentAudioSource.clip = self.CurrentAudioTheme.atmos[math.random(#self.CurrentAudioTheme.atmos)]
          self.CurrentAudioSource.volume = CommonUtils.RandomFloat(0.5, 1)
          self.CurrentAudioSource:Play()
        end
        self._CurrentAudioTick = math.random(self.CurrentAudioTheme.atmoInterval, self.CurrentAudioTheme.atmoMaxInterval)
      else
        self._CurrentIsAtmo = math.random() > 0.3
        local musicIndex = math.random(#self.CurrentAudioTheme.musics)
        if musicIndex == self._LastMusicIndex then
          if musicIndex < #self.CurrentAudioTheme.musics then
            musicIndex = musicIndex + 1
          else
            musicIndex = 1
          end
        end
        self._LastMusicIndex = musicIndex
        if not self.CurrentAudioSource.isPlaying then 
          self.CurrentAudioSource:Stop() 
          self.CurrentAudioSource.clip = self.CurrentAudioTheme.musics[musicIndex]
          self.CurrentAudioSource.volume = 1
          self.CurrentAudioSource:Play()
        end
      end
      

      self._CurrentIsAtom = false
      self._CurrentAudioTick = math.random(self.CurrentAudioTheme.baseInterval, self.CurrentAudioTheme.maxInterval)
    end

    --随机播放atmo音效
    if self._CurrentAudioTick2 > 0 then
      self._CurrentAudioTick2 = self._CurrentAudioTick2 - 1
    else
      
    end

  end
end

---设置当前背景音乐预设
---@param theme number
function MusicManager:SetCurrentTheme(theme) 
  if self.Musics[theme] ~= nil then 
    self.CurrentAudioTheme = self.Musics[theme] 
    return true
  else
    if theme ~= 0 then
      Log.E(TAG, 'Not found music theme '..theme..' , music disabled')
    end
    self.CurrentAudioEnabled = false
    return false
  end
end
function MusicManager:EnableBackgroundMusic() 
  if self.CurrentAudioTheme then
    self.CurrentAudioEnabled = true 
    self._CurrentAudioTick = math.random(2, self.CurrentAudioTheme.maxInterval / 2)
    self._CurrentAudioTick2 = math.random(10, self.CurrentAudioTheme.atmoMaxInterval)

    --淡入当前正在播放的音乐
    if self.CurrentAudioSource.isPlaying then
      Game.UIManager.UIFadeManager:AddAudioFadeIn(self.CurrentAudioSource, 1)
    else
      self.CurrentAudioSource.volume = 1
    end
  end
end
function MusicManager:DisableBackgroundMusic() 
  self.CurrentAudioEnabled = false 
  --淡出当前正在播放的音乐
  if self.CurrentAudioSource.isPlaying then
    Game.UIManager.UIFadeManager:AddAudioFadeOut(self.CurrentAudioSource, 1)
  else
    self.CurrentAudioSource.volume = 1
  end
end
---从当前时间开始暂停音乐指定秒
---@param sec number
function MusicManager:DisableInSec(sec) 
  if sec <= 0 or not self.CurrentAudioEnabled then
    return
  end
  --淡出当前正在播放的音乐
  if self.CurrentAudioSource.isPlaying then
    Game.UIManager.UIFadeManager:AddAudioFadeOut(self.CurrentAudioSource, 1)
  end

  self.CurrentAudioEnabled = false 
  LuaTimer.Add(1000, function ()
    self.CurrentAudioSource:Stop()
  end)
  LuaTimer.Add(sec*1000, function ()
    self.CurrentAudioEnabled = true 
  end)
end

function CreateClass:MusicManager() 
  return MusicManager()
end