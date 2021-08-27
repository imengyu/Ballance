local GamePackage = Ballance2.Sys.Package.GamePackage
local SystemPackage = GamePackage.GetSystemPackage()
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local GameManager = Ballance2.Sys.GameManager
local StringUtils = Ballance2.Utils.StringUtils

function CoreDebugGameGamePlay()
  --Hide base Cam
  GameManager.Instance:SetGameBaseCameraVisible(false)
  
  GamePlayInit(function ()
    --Init floors
    CloneUtils.CloneNewObject(SystemPackage:GetPrefabAsset('Assets/Game/Prefabs/Debug/TestFloor.prefab'), 'GameTestFloor')
    GamePlay.GamePlayManager:CreateSkyAndLight('L', nil, StringUtils.StringToColor('#6D6050'))
    GamePlay.GamePlayManager:_InitAndStart()
  end)

end