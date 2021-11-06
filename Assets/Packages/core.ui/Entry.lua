require('GamePlayUIControl')
require('MenuLevelUIControl')
require('SettingsUIControl')

local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager

GameUIPackage = nil ---@type GamePackage

GameUI = {
  HighscoreUIControl = nil, ---@type HighscoreUIControl
  WinScoreUIControl = nil, ---@type WinScoreUIControl
}

return {
  ---模块入口函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageEntry = function(thisGamePackage)
    GameUIPackage = thisGamePackage
    MessageCenter = GameUIManager:CreateUIMessageCenter('GameUIGloobalMessageCenter')
    MessageCenter:SubscribeEvent('BtnBackClick', function () GameUIManager:BackPreviusPage() end)
    CreateSettingsUI(thisGamePackage)
    CreateMenuLevelUI(thisGamePackage)
    CreateGamePlayUI(thisGamePackage)
    return true
  end,
  ---模块卸载前函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageBeforeUnLoad = function(thisGamePackage)
    GameUIManager:DestroyUIMessageCenter('GameUIGloobalMessageCenter')
    return true
  end
}
