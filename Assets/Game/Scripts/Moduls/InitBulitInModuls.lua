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