local BulitInModulsLoaded = false

function InitBulitInModuls()
  if BulitInModulsLoaded then
    return
  end

  Game.LevelBuilder:RegisterModul('P_Ball_Paper', Game.CorePackage:GetPrefabAsset('P_Ball_Paper.prefab'))
  Game.LevelBuilder:RegisterModul('P_Ball_Stone', Game.CorePackage:GetPrefabAsset('P_Ball_Stone.prefab'))
  Game.LevelBuilder:RegisterModul('P_Ball_Wood', Game.CorePackage:GetPrefabAsset('P_Ball_Wood.prefab'))
  Game.LevelBuilder:RegisterModul('P_Box', Game.CorePackage:GetPrefabAsset('P_Box.prefab'))
  Game.LevelBuilder:RegisterModul('P_Dome', Game.CorePackage:GetPrefabAsset('P_Dome.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_01', Game.CorePackage:GetPrefabAsset('P_Modul_01.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_03', Game.CorePackage:GetPrefabAsset('P_Modul_03.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_08', Game.CorePackage:GetPrefabAsset('P_Modul_08.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_17', Game.CorePackage:GetPrefabAsset('P_Modul_17.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_18', Game.CorePackage:GetPrefabAsset('P_Modul_18.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_19', Game.CorePackage:GetPrefabAsset('P_Modul_19.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_25', Game.CorePackage:GetPrefabAsset('P_Modul_25.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_26', Game.CorePackage:GetPrefabAsset('P_Modul_26.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_29', Game.CorePackage:GetPrefabAsset('P_Modul_29.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_30', Game.CorePackage:GetPrefabAsset('P_Modul_30.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_34', Game.CorePackage:GetPrefabAsset('P_Modul_34.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_37', Game.CorePackage:GetPrefabAsset('P_Modul_37.prefab'))
  Game.LevelBuilder:RegisterModul('P_Modul_41', Game.CorePackage:GetPrefabAsset('P_Modul_41.prefab'))
  Game.LevelBuilder:RegisterModul('P_Trafo_Paper', Game.CorePackage:GetPrefabAsset('P_Trafo_Paper.prefab'))
  Game.LevelBuilder:RegisterModul('P_Trafo_Stone', Game.CorePackage:GetPrefabAsset('P_Trafo_Stone.prefab'))
  Game.LevelBuilder:RegisterModul('P_Trafo_Wood', Game.CorePackage:GetPrefabAsset('P_Trafo_Wood.prefab'))
  Game.LevelBuilder:RegisterModul('P_Extra_Life', Game.CorePackage:GetPrefabAsset('P_Extra_Life.prefab'))
  Game.LevelBuilder:RegisterModul('P_Extra_Point', Game.CorePackage:GetPrefabAsset('P_Extra_Point.prefab'))
  Game.LevelBuilder:RegisterModul('PC_CheckPoints', Game.CorePackage:GetPrefabAsset('PC_TwoFlames.prefab'))
  Game.LevelBuilder:RegisterModul('PE_LevelEnd', Game.CorePackage:GetPrefabAsset('PE_Balloon.prefab'))
  Game.LevelBuilder:RegisterModul('PS_LevelStart', Game.CorePackage:GetPrefabAsset('PS_FourFlames.prefab'))

  BulitInModulsLoaded = true
end

---注册内置机关的自定义声音组
function InitBulitInModulCustomSounds()
  local BallSoundManager = GamePlay.BallSoundManager
  BallSoundManager:AddSoundCollData(BallSoundManager:GetSoundCollIDByName('Dome'), {
    MinSpeed = 5,
    MaxSpeed = 15,
    SleepAfterwards = 0.6,
    SpeedThreadhold = 10,
    TimeDelayStart = 0.3,
    TimeDelayEnd = 0.3,
    HasRollSound = false,
    HitSoundName = 'Dome'
  })
  BallSoundManager:AddSoundCollData(BallSoundManager:GetSoundCollIDByName('WoodOnlyHit'), {
    MinSpeed = 5,
    MaxSpeed = 20,
    SleepAfterwards = 0.6,
    SpeedThreadhold = 20,
    TimeDelayStart = 0.3,
    TimeDelayEnd = 0.3,
    HasRollSound = false,
    HitSoundName = 'Wood'
  })
  BallSoundManager:AddSoundCollData(BallSoundManager:GetSoundCollIDByName('StoneOnlyHit'), {
    MinSpeed = 5,
    MaxSpeed = 20,
    SleepAfterwards = 0.6,
    SpeedThreadhold = 20,
    TimeDelayStart = 0.3,
    TimeDelayEnd = 0.3,
    HasRollSound = false,
    HitSoundName = 'Stone'
  })
  BallSoundManager:AddSoundCollData(BallSoundManager:GetSoundCollIDByName('PaperOnlyHit'), {
    MinSpeed = 5,
    MaxSpeed = 20,
    SleepAfterwards = 0.6,
    SpeedThreadhold = 20,
    TimeDelayStart = 0.3,
    TimeDelayEnd = 0.3,
    HasRollSound = false,
    HitSoundName = 'Paper'
  })
end