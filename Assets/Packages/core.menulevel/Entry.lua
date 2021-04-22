--[[
Copyright(c) 2021  mengyu
* 模块名：     
Entry.lua
* 用途：
主菜单进入入口
* 作者：
mengyu
* 更改历史：
2021-4-18 创建
]]--

GameManager = Ballance2.Sys.GameManager
Log = Ballance2.Utils.Log
GameLuaObjectHost = Ballance2.Sys.Bridge.LuaWapper.GameLuaObjectHost
GameMenuLevel = nil
GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
MenuLevelEventUiEntryHandler = nil
GameMenuLevelPackage = nil ---@type GamePackage

return {
  ---模块入口函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageEntry = function(thisGamePackage)
    GameMenuLevelPackage = thisGamePackage
    MenuLevelEventUiEntryHandler = GameManager.GameMediator:SubscribeSingleEvent(thisGamePackage, 'event_ui_entry', 'MenuLevel', function ()

      GameUIManager:MaskBlackSet(true)
      Log.D(thisGamePackage.TAG, "Enter menulevel")

      GameManager.Instance:SetGameBaseCameraVisible(false)
      GameMenuLevel = CloneUtils.CloneNewObject(thisGamePackage:GetPrefabAsset('GameMenuLevel.prefab'), 'GameMenuLevel')

      
      CreateMenuLevelUI(GameUIPackage)
      CreateSettingsUI(GameUIPackage)
      return true
    end)
    return true
  end,
  ---模块卸载前函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageBeforeUnLoad = function(thisGamePackage)
    GameManager.GameMediator:UnsubscribeSingleEvent(thisGamePackage, 'event_ui_entry', MenuLevelEventUiEntryHandler)
    if (not Slua.IsNull(GameMenuLevel)) then UnityEngine.Object.Destroy(GameMenuLevel) end 
    return true
  end
}
