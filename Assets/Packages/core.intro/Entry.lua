--[[
Copyright(c) 2021  mengyu
* 模块名：     
Entry.lua
* 用途：
游戏进入动画入口
* 作者：
mengyu
* 更改历史：
2021-4-18 创建
]]--

GameManager = Ballance2.Sys.GameManager
GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
GameSoundManager = GameManager.Instance:GetSystemService('GameSoundManager') ---@type GameSoundManager
GameEventNames = Ballance2.Sys.Bridge.GameEventNames
GameSoundType = Ballance2.Sys.Services.GameSoundType
Log = Ballance2.Utils.Log
Yield = UnityEngine.Yield
WaitForSeconds = UnityEngine.WaitForSeconds

return {
  ---模块入口函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageEntry = function(thisGamePackage)
    Log.D('Intro', 'Into intro ui')
    GameManager.GameMediator:RegisterSingleEvent("event_ui_entry")
    GameManager.GameMediator:RegisterEventHandler(thisGamePackage, GameEventNames.EVENT_GAME_MANAGER_INIT_FINISHED, "Intro", function ()
      IntroUI = CloneUtils.CloneNewObjectWithParent(thisGamePackage:GetPrefabAsset('Assets/Packages/core.intro/Prefabs/IntroUI.prefab'), GameManager.Instance.GameCanvas, 'IntroUI')
      IntroUI.transform:SetAsFirstSibling()

      --进入音乐
      GameSoundManager:PlayFastVoice(thisGamePackage, "Assets/Packages/core.intro/Sounds/MusicIntro.wav", GameSoundType.Background)
      GameUIManager:MaskWhiteFadeOut(0.3)

      --延时5s
      coroutine.resume(coroutine.create(function()
        Yield(WaitForSeconds(5))
        
        Log.D('Intro', 'Quit intro ui')

        --黑色渐变进入
        GameUIManager:MaskWhiteFadeOut(1)
        Yield(WaitForSeconds(1))

        IntroUI.gameObject:SetActive(false)

        GameManager.GameMediator:NotifySingleEvent("event_ui_entry", nil)
      end))
      return false
    end)
    return true
  end,
  ---模块卸载前函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageBeforeUnLoad = function(thisGamePackage)
    return true
  end
}