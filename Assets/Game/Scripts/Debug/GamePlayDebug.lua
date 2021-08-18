local GamePackage = Ballance2.Sys.Package.GamePackage
local SystemPackage = GamePackage.GetSystemPackage()
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local GameManager = Ballance2.Sys.GameManager
local ColorUtility = UnityEngine.ColorUtility

function CoreDebugGameGamePlay()
  --Hide base Cam
  GameManager.Instance:SetGameBaseCameraVisible(false)
  
  GamePlayInit(function ()
    --Init floors
    CloneUtils.CloneNewObject(SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Debug/TestFloor.prefab'), 'GameTestFloor')
    local _, color = ColorUtility.TryParseHtmlString('6D6050', Slua.out)
    GamePlay.GamePlayManager:CreateSkyAndLight('L', nil, color)
    GamePlay.GamePlayManager:StartLevel()
  end)

end