local CloneUtils = Ballance2.Sys.Utils.CloneUtils

local LevelBuilderGameObject = nil

function LevelBuilderInit()
  LevelBuilderGameObject = CloneUtils.CloneNewObject(Game.SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Core/LevelBuilder.prefab'), 'GameLevelBuilder')
  return LevelBuilderGameObject
end
function LevelBuilderDestroy()
  if (not Slua.IsNull(LevelBuilderGameObject)) then UnityEngine.Object.Destroy(LevelBuilderGameObject) end 

  LevelBuilderGameObject = nil
end