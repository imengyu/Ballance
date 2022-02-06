local CloneUtils = Ballance2.Utils.CloneUtils
local GameLuaObjectHost = Ballance2.Services.LuaService.LuaWapper.GameLuaObjectHost

local LevelBuilderGameObject = nil

function LevelBuilderInit()
  LevelBuilderGameObject = CloneUtils.CloneNewObject(Game.CorePackage:GetPrefabAsset('LevelBuilder.prefab'), 'GameLevelBuilder')
  Game.LevelBuilder = GameLuaObjectHost.GetLuaClassFromGameObject(LevelBuilderGameObject)
  return LevelBuilderGameObject
end
function LevelBuilderDestroy()
  if (not Slua.IsNull(LevelBuilderGameObject)) then UnityEngine.Object.Destroy(LevelBuilderGameObject) end 
  Game.LevelBuilder = nil
  LevelBuilderGameObject = nil
end