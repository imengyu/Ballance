local GamePackage = Ballance2.Sys.Package.GamePackage
local SystemPackage = GamePackage.GetSystemPackage()
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager

function CoreDebugGameGamePlay()
  CloneUtils.CloneNewObject(SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Debug/TestFloor.prefab'), 'TestFloor')
  GameManager.Instance:SetGameBaseCameraVisible(false)
  GamePlayInit()
  GameUIManager:MaskBlackFadeOut(1)
end