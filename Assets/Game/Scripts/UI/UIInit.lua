require('GamePlayUIControl')
require('MenuLevelUIControl')
require('SettingsUIControl')

local GameManager = Ballance2.Services.GameManager
local GamePackage = Ballance2.Package.GamePackage
local GameUIManager = GameManager.GetSystemService('GameUIManager') ---@type GameUIManager

GameUIPackage = nil ---@type GamePackage

GameUI = {
  HighscoreUIControl = nil, ---@type HighscoreUIControl
  WinScoreUIControl = nil, ---@type WinScoreUIControl
}

return {
  Init = function ()
    local thisGamePackage = GamePackage.GetCorePackage()
    GameUIPackage = thisGamePackage
    MessageCenter = GameUIManager:CreateUIMessageCenter('GameUIGloobalMessageCenter')
    MessageCenter:SubscribeEvent('BtnBackClick', function () GameUIManager:BackPreviusPage() end)
    CreateSettingsUI(thisGamePackage)
    CreateMenuLevelUI(thisGamePackage)
    CreateGamePlayUI(thisGamePackage)
  end,
  Unload = function ()
    GameUIManager:DestroyUIMessageCenter('GameUIGloobalMessageCenter')
  end
}