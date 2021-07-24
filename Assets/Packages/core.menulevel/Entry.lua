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

local GameMenuLevelEnterHandler = nil
local GameMenuLevelQuitHandler = nil

---进入MenuLevel场景
---@param thisGamePackage GamePackage
local function OnEnterMenuLevel(thisGamePackage)
  Log.D(thisGamePackage.TAG, 'Enter menuLevel')

  GameUIManager:MaskBlackFadeOut(1)
  GameManager.Instance:SetGameBaseCameraVisible(false)

  if(GameMenuLevel == nil) then
    GameMenuLevel = CloneUtils.CloneNewObject(thisGamePackage:GetPrefabAsset('GameMenuLevel.prefab'), 'GameMenuLevel')
  end
end
---退出MenuLevel场景
---@param thisGamePackage GamePackage
local function OnQuitMenuLevel(thisGamePackage)
  Log.D(thisGamePackage.TAG, 'Quit menuLevel')

  if (not Slua.IsNull(GameMenuLevel)) then 
    GameMenuLevel:SetActive(false)
    GameManager.Instance:SetGameBaseCameraVisible(true)
  end 
end

return {
  ---模块入口函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageEntry = function(thisGamePackage)
    GameMenuLevelPackage = thisGamePackage
    GameMenuLevelEnterHandler = GameManager.GameMediator:RegisterEventHandler(thisGamePackage, GameEventNames.EVENT_LOGIC_SECNSE_ENTER, "Intro", function (evtName, params)
      local scense = params[1]
      if(scense == 'MenuLevel') then OnEnterMenuLevel(thisGamePackage) end
      return false
    end)    
    GameMenuLevelQuitHandler = GameManager.GameMediator:RegisterEventHandler(thisGamePackage, GameEventNames.EVENT_LOGIC_SECNSE_QUIT, "Intro", function (evtName, params)
      local scense = params[1]
      if(scense == 'MenuLevel') then OnQuitMenuLevel(thisGamePackage) end
      return false
    end)
    return true
  end,
  ---模块卸载前函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageBeforeUnLoad = function(thisGamePackage)
    if (not Slua.IsNull(GameMenuLevel)) then UnityEngine.Object.Destroy(GameMenuLevel) end 
    GameManager.GameMediator:UnRegisterEventHandler(GameEventNames.EVENT_LOGIC_SECNSE_ENTER, GameMenuLevelEnterHandler)
    GameManager.GameMediator:UnRegisterEventHandler(GameEventNames.EVENT_LOGIC_SECNSE_QUIT, GameMenuLevelQuitHandler)
    return true
  end
}
