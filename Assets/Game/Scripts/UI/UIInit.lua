local GameManager = Ballance2.Services.GameManager
local GameUIPrefabType = Ballance2.Services.GameUIPrefabType
local GameStaticResourcesPool = Ballance2.Res.GameStaticResourcesPool
local GamePackage = Ballance2.Package.GamePackage
local GameUIManager = GameManager.GetSystemService('GameUIManager') ---@type GameUIManager

GameUIPackage = nil ---@type GamePackage

GameUI = {
  HighscoreUIControl = nil, ---@type HighscoreUIControl
  WinScoreUIControl = nil, ---@type WinScoreUIControl
  GamePreviewUI = nil, ---@type GamePlayPreviewUIControl
}

return {
  Init = function ()
    local thisGamePackage = GamePackage.GetCorePackage()
    GameUIPackage = thisGamePackage

    require('GamePlayUIControl')
    require('MenuLevelUIControl')
    require('SettingsUIControl')

    GameUIManager:RegisterUIPrefab('PageTransparent', GameUIPrefabType.Page, GameStaticResourcesPool.FindStaticPrefabs('GameUIPageBallanceTransparent'))
    GameUIManager:RegisterUIPrefab('PageWide', GameUIPrefabType.Page, GameStaticResourcesPool.FindStaticPrefabs('GameUIPageBallanceWide'))
    GameUIManager:RegisterUIPrefab('PageCommon', GameUIPrefabType.Page, GameStaticResourcesPool.FindStaticPrefabs('GameUIPageBallanceCommon'))

    MessageCenter = GameUIManager:CreateUIMessageCenter('GameUIGlobalMessageCenter')
    MessageCenter:SubscribeEvent('BtnBackClick', function () GameUIManager:BackPreviusPage() end)
    CreateSettingsUI(thisGamePackage)
    CreateMenuLevelUI(thisGamePackage)
    CreateGamePlayUI(thisGamePackage)
  end,
  Unload = function ()
    GameUIManager:DestroyUIMessageCenter('GameUIGlobalMessageCenter')
  end
}