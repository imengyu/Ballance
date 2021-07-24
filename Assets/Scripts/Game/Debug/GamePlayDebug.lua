

local SystemPackage = GamePackage.GetSystemPackage()

function CoreDebugGameGamePlay()
  CloneUtils.CloneNewObject(SystemPackage:GetPrefabAsset('Debug/TestFloor.prefab'), 'TestFloor')
  GamePlayInit()
  
end