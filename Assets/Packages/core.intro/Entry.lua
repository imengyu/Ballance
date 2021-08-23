--[[
Copyright(c) 2021  mengyu
* 模块名：     
Entry.lua
* 用途：
游戏进入动画入口
* 作者：
mengyu
]]--

local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
local GameSoundManager = GameManager.Instance:GetSystemService('GameSoundManager') ---@type GameSoundManager
local GameEventNames = Ballance2.Sys.Bridge.GameEventNames
local GameSoundType = Ballance2.Sys.Services.GameSoundType
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local Log = Ballance2.Utils.Log
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds

local IntroUI = nil
local introFinished = false
local baseFinished = false
local TAG = 'Intro:Entry'

---进入Intro场景
---@param thisGamePackage GamePackage
local function OnEnterIntro(thisGamePackage)

  Log.D(TAG, 'Into intro ui')

  if IntroUI == nil then
    IntroUI = GameUIManager:InitViewToCanvas(thisGamePackage:GetPrefabAsset('IntroUI.prefab'), 'IntroUI', true)
    IntroUI:SetAsFirstSibling()    
  end

  GameUIManager:MaskBlackFadeIn(0.3)

  introFinished = false
  baseFinished = false

  --进入音乐
  GameSoundManager:PlayFastVoice(thisGamePackage, "Assets/Packages/core.intro/Sounds/MusicIntro.wav", GameSoundType.Background)

  --延时5s
  coroutine.resume(coroutine.create(function()
    Yield(WaitForSeconds(5))
 
    --黑色渐变进入
    GameUIManager:MaskBlackFadeIn(1)
    Yield(WaitForSeconds(1))

    if not baseFinished then introFinished = true else
      GameManager.Instance:RequestEnterLogicScense('MenuLevel')
    end
  end))
end
local function OnQuitIntro()
  Log.D(TAG, 'Quit intro ui')
  if IntroUI ~= nil then
    IntroUI.gameObject:SetActive(false)
  end
end

return {
  ---模块入口函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageEntry = function(thisGamePackage)
    GameManager.GameMediator:RegisterEventHandler(thisGamePackage, GameEventNames.EVENT_GAME_MANAGER_INIT_FINISHED, "Intro", function ()
      if not introFinished then baseFinished = true else
        GameManager.Instance:RequestEnterLogicScense('MenuLevel')
      end
      return false
    end)
    GameManager.GameMediator:RegisterEventHandler(thisGamePackage, GameEventNames.EVENT_LOGIC_SECNSE_ENTER, "Intro", function (evtName, params)
      local scense = params[1]
      if(scense == 'Intro') then OnEnterIntro(thisGamePackage) end
      return false
    end)    
    GameManager.GameMediator:RegisterEventHandler(thisGamePackage, GameEventNames.EVENT_LOGIC_SECNSE_QUIT, "Intro", function (evtName, params)
      local scense = params[1]
      if(scense == 'Intro') then OnQuitIntro() end
      return false
    end)
    return true
  end,
  ---模块卸载前函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageBeforeUnLoad = function(thisGamePackage)
    if (not Slua.IsNull(IntroUI)) then UnityEngine.Object.Destroy(IntroUI.gameObject) end 
    return true
  end
}