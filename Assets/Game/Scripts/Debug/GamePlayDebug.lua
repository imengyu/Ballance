local GamePackage = Ballance2.Sys.Package.GamePackage
local SystemPackage = GamePackage.GetSystemPackage()
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
local SkyBoxUtils = Ballance2.Game.Utils.SkyBoxUtils

function CoreDebugGameGamePlay()

  --Init floors
  CloneUtils.CloneNewObject(SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Debug/TestFloor.prefab'), 'TestFloor')

  --Hide base floor
  GameManager.Instance:SetGameBaseCameraVisible(false)

  GamePlayInit(function ()
    --Init sky
    Game.GamePlay.CamManager:SetSkyBox(SkyBoxUtils.MakeSkyBox('A'))

    --Hide black
    GameUIManager:MaskBlackFadeOut(1)
  end)

end