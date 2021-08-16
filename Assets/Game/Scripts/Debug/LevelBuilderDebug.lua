local GamePackage = Ballance2.Sys.Package.GamePackage
local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager

function CoreDebugLevelBuliderEntry()
  --Hide base Cam
  GameManager.Instance:SetGameBaseCameraVisible(false)

  GamePlayInit(function ()
    
    --Hide black
    GameUIManager:MaskBlackFadeOut(1)
  end)

end