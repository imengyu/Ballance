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
  Game.LevelBuilder:RegisterModul('P_Modul_03', Game.SystemPackage:GetPrefabAsset('P_Modul_03.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_08', Game.SystemPackage:GetPrefabAsset('P_Modul_08.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_17', Game.SystemPackage:GetPrefabAsset('P_Modul_17.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_18', Game.SystemPackage:GetPrefabAsset('P_Modul_18.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_19', Game.SystemPackage:GetPrefabAsset('P_Modul_19.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_25', Game.SystemPackage:GetPrefabAsset('P_Modul_25.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_26', Game.SystemPackage:GetPrefabAsset('P_Modul_26.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_29', Game.SystemPackage:GetPrefabAsset('P_Modul_29.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_30', Game.SystemPackage:GetPrefabAsset('P_Modul_30.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_34', Game.SystemPackage:GetPrefabAsset('P_Modul_34.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_37', Game.SystemPackage:GetPrefabAsset('P_Modul_37.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_41', Game.SystemPackage:GetPrefabAsset('P_Modul_41.prefab'))
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
  ---@param data table
  GamePlay.BallSoundManager:AddCustomSoundLayerHandler('Dome', false, function (type, ball, data)
    if type == 'hit' then
      return ball._HitSound.Sounds.Dome
    end
  end)
  ---@param type string
  ---@param ball Ball
  ---@param data table
  GamePlay.BallSoundManager:AddCustomSoundLayerHandler('Wood',true, function (type, ball, data)
    if type == 'hit' then
      return ball._HitSound.Sounds.Wood
    elseif type == 'contact' then
      return ball._RollSound.Sounds.Wood
    end
  end)
  ---@param type string
  ---@param ball Ball
  ---@param data table
  GamePlay.BallSoundManager:AddCustomSoundLayerHandler('Stone', true, function (type, ball, data)
    if type == 'hit' then
      return ball._HitSound.Sounds.Stone
    elseif type == 'contact' then
      return ball._RollSound.Sounds.Stone
    end
  end)
  ---@param type string
  ---@param ball Ball
  ---@param data table
  GamePlay.BallSoundManager:AddCustomSoundLayerHandler('WoodOnlyHit', false, function (type, ball, data)
    if type == 'hit' then
      return ball._HitSound.Sounds.Wood
    end
  end)
  ---@param type string
  ---@param ball Ball
  ---@param data table
  GamePlay.BallSoundManager:AddCustomSoundLayerHandler('StoneOnlyHit', false, function (type, ball, data)
    if type == 'hit' then
      return ball._HitSound.Sounds.Stone
    end
  end)
end