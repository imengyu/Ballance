local BulitInModulsLoaded = false

function InitBulitInModuls()
  if BulitInModulsLoaded then
    return
  end

  Game.LevelBuilder:RegisterModul('P_Ball_Paper', Game.SystemPackage:GetPrefabAsset('P_Ball_Paper.prefab'))
  Game.LevelBuilder:RegisterModul('P_Ball_Stone', Game.SystemPackage:GetPrefabAsset('P_Ball_Stone.prefab'))
  Game.LevelBuilder:RegisterModul('P_Ball_Wood', Game.SystemPackage:GetPrefabAsset('P_Ball_Wood.prefab'))
  Game.LevelBuilder:RegisterModul('P_Box', Game.SystemPackage:GetPrefabAsset('P_Box.prefab'))
  Game.LevelBuilder:RegisterModul('P_Dome', Game.SystemPackage:GetPrefabAsset('P_Dome.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_01', Game.SystemPackage:GetPrefabAsset('P_Modul_01.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_34', Game.SystemPackage:GetPrefabAsset('P_Modul_34.prefab'))
  Game.LevelBuilder:RegisterModul('P_Trafo_Paper', Game.SystemPackage:GetPrefabAsset('P_Trafo_Paper.prefab'))
  Game.LevelBuilder:RegisterModul('P_Trafo_Stone', Game.SystemPackage:GetPrefabAsset('P_Trafo_Stone.prefab'))
  Game.LevelBuilder:RegisterModul('P_Trafo_Wood', Game.SystemPackage:GetPrefabAsset('P_Trafo_Wood.prefab'))
  Game.LevelBuilder:RegisterModul('P_Extra_Life', Game.SystemPackage:GetPrefabAsset('P_Extra_Life.prefab'))
  Game.LevelBuilder:RegisterModul('P_Extra_Point', Game.SystemPackage:GetPrefabAsset('P_Extra_Point.prefab'))
  Game.LevelBuilder:RegisterModul('PC_CheckPoints', Game.SystemPackage:GetPrefabAsset('PC_TwoFlames.prefab'))
  Game.LevelBuilder:RegisterModul('PE_LevelEnd', Game.SystemPackage:GetPrefabAsset('PE_Balloon.prefab'))
  Game.LevelBuilder:RegisterModul('PS_LevelStart', Game.SystemPackage:GetPrefabAsset('PS_FourFlames.prefab'))

  BulitInModulsLoaded = true
end

---注册内置机关的自定义声音组
function InitBulitInModulCustomSounds()
  ---@param type string
  ---@param ball Ball
  ---@param vol number
  GamePlay.BallSoundManager:AddCustomSoundLayerHandler('Dome', function (type, ball, vol)
    if type == 'hit' then
      local sound = ball._HitSound.SoundDome ---@type AudioSource
      if sound then
        sound.volume = vol
        if not sound.isPlaying then
          sound:Play()
        end
      end
    end
  end)
  ---@param type string
  ---@param ball Ball
  ---@param vol number
  GamePlay.BallSoundManager:AddCustomSoundLayerHandler('Wood', function (type, ball, vol)
    if type == 'hit' then
      if ball._HitSound.SoundWood then
        ball._HitSound.SoundWood.volume = vol
        ball._HitSound.SoundWood:Play()
      end
    elseif type == 'roll' then
      if ball._RollSound.SoundWood then
        ball._RollSound.SoundWood.volume = vol
      end
    end
  end)
  ---@param type string
  ---@param ball Ball
  ---@param vol number
  GamePlay.BallSoundManager:AddCustomSoundLayerHandler('Stone', function (type, ball, vol)
    if type == 'hit' then
      if ball._HitSound.SoundStone then
        ball._HitSound.SoundStone.volume = vol
        ball._HitSound.SoundStone:Play()
      end
    elseif type == 'roll' then
      if ball._RollSound.SoundStone then
        ball._RollSound.SoundStone.volume = vol
      end
    end
  end)
  ---@param type string
  ---@param ball Ball
  ---@param vol number
  GamePlay.BallSoundManager:AddCustomSoundLayerHandler('WoodOnlyHit', function (type, ball, vol)
    if type == 'hit' then
      local sound = ball._HitSound.SoundWood
      if sound then
        sound.volume = vol
        if not sound.isPlaying then
          sound:Play()
        end
      end
    end
  end)
  ---@param type string
  ---@param ball Ball
  ---@param vol number
  GamePlay.BallSoundManager:AddCustomSoundLayerHandler('StoneOnlyHit', function (type, ball, vol)
    if type == 'hit' then
      local sound = ball._HitSound.SoundStone
      if sound then
        sound.volume = vol
        if not sound.isPlaying then
          sound:Play()
        end
      end
    end
  end)
end