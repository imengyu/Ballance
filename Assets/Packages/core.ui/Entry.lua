local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager

GameUIPackage = nil ---@type GamePackage

return {
  ---模块入口函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageEntry = function(thisGamePackage)
    GameUIPackage = thisGamePackage
    MessageCenter = GameUIManager:CreateUIMessageCenter('GameUIGloobalMessageCenter')
    thisGamePackage:RequireLuaFile('MenuLevelUIControl.lua')
    thisGamePackage:RequireLuaFile('SettingsUIControl.lua')
    CreateSettingsUI(thisGamePackage)
    CreateMenuLevelUI(thisGamePackage)
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
