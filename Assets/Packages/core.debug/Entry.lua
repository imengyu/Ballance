--[[
Copyright(c) 2021  mengyu
* 模块名：     
Entry.lua
* 用途：
调试包入口
* 作者：
mengyu
* 

]]--


-- 调试工具类
local GameManager = Ballance2.Sys.GameManager
local GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
local CloneUtils = Ballance2.Sys.Utils.CloneUtils
local DebugCamera = Ballance2.DebugCamera
local WindowType = Ballance2.Sys.UI.WindowType
local GamePackage = Ballance2.Sys.Package.GamePackage
local GameStaticResourcesPool = Ballance2.Sys.Res.GameStaticResourcesPool

local KeyCode = UnityEngine.KeyCode
local GameObject = UnityEngine.GameObject

GlobalDebugToolbar = nil ---@type GameLuaObjectHost
GlobalDebugWindow = nil ---@type Window|MonoBehaviour
GlobalDebugOptWindow = nil ---@type Window|MonoBehaviour
local GlobalRuntimeHierarchyWindow = nil ---@type Window|MonoBehaviour
local GlobalRuntimeInspectorWindow = nil ---@type Window|MonoBehaviour

DebugOptEvent = 'core.debug.OptNotify'
DebugOptStandByEvent = 'core.debug.OptStandByNotify'

local PackageEntryDebugOptStandByHandler = nil
local PackageDebugOptEntryHandler = nil
local F12KeyListen = nil

return {
  ---模块入口函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageEntry = function(thisGamePackage)
    GameManager.GameMediator:RegisterGlobalEvent(DebugOptEvent)
    GameManager.GameMediator:RegisterGlobalEvent(DebugOptStandByEvent)

    --创建窗口
    GlobalDebugToolbar = GameUIManager:InitViewToCanvas(thisGamePackage:GetPrefabAsset('DebugToolbar.prefab'), 'DebugToolbar', true)
    GlobalDebugToolbar:SetAsLastSibling()
    GlobalDebugToolbar = GlobalDebugToolbar.gameObject:GetComponent(Ballance2.Sys.Bridge.LuaWapper.GameLuaObjectHost)

    local DebugWindow = thisGamePackage:GetPrefabAsset('Assets/Packages/core.debug/Prefabs/DebugWindow.prefab')
    GlobalDebugWindow = GameUIManager:CreateWindow('Console', 
        CloneUtils.CloneNewObjectWithParent(DebugWindow, GameManager.Instance.GameCanvas, 'DebugWindow').transform, 
        false, 9, -140, 600, 400)
    GlobalDebugWindow.CloseAsHide = true
    GlobalDebugWindow.WindowType = WindowType.TopWindow
    GlobalDebugWindow.gameObject.tag = 'DebugWindow'
    local DebugOptWindow = thisGamePackage:GetPrefabAsset('Assets/Packages/core.debug/Prefabs/DebugOptWindow.prefab')
    GlobalDebugOptWindow = GameUIManager:CreateWindow('Debug options', 
        CloneUtils.CloneNewObjectWithParent(DebugOptWindow, GameManager.Instance.GameCanvas, 'DebugOptWindow').transform, 
        false, 675, -70, 210, 320)
    GlobalDebugOptWindow.CloseAsHide = true

    GlobalRuntimeHierarchyWindow = GameUIManager:CreateWindow('Hierarchy', 
        CloneUtils.CloneNewObjectWithParent(GameStaticResourcesPool.FindStaticPrefabs('DebugHierarchy'), GameManager.Instance.GameCanvas, 'DebugHierarchy').transform, 
        false, 20, -205, 225, 406)
        GlobalRuntimeHierarchyWindow.CloseAsHide = true
    GlobalRuntimeInspectorWindow = GameUIManager:CreateWindow('Inspector', 
        CloneUtils.CloneNewObjectWithParent(GameStaticResourcesPool.FindStaticPrefabs('DebugInspector'), GameManager.Instance.GameCanvas, 'DebugInspector').transform, 
        false, 925, -90, 350, 500)
    GlobalRuntimeInspectorWindow.CloseAsHide = true
    GlobalRuntimeInspectorWindow.WindowType = WindowType.TopWindow

    DebugCamera.Instance.GameDebugInspectorWindow = GlobalRuntimeInspectorWindow
    DebugCamera.Instance.GameDebugHierarchyWindow = GlobalRuntimeHierarchyWindow
    DebugCamera.Instance:PrepareWindow()

    --添加选项菜单
    PackageEntryDebugOptStandByHandler = GameManager.GameMediator:RegisterEventHandler(thisGamePackage, DebugOptStandByEvent, "PackageDebugEntryOptStandByHandler", function ()
        GameManager.Instance.GameActionStore:CallAction('DebugOptAddOption', { 'optHierarchy', 'Hierarchy', 'Button' })
        GameManager.Instance.GameActionStore:CallAction('DebugOptAddOption', { 'optInspector', 'Inspector', 'Button' })
        return false
    end)
    PackageDebugOptEntryHandler = GameManager.GameMediator:RegisterEventHandler(thisGamePackage, DebugOptEvent, "PackageDebugOptEntryHandler", function (evtName, params)
        local name = params[1]
        if name == 'optHierarchy' then
            GlobalRuntimeHierarchyWindow:Show()
        elseif name == 'optInspector' then
            GlobalRuntimeInspectorWindow:Show()
        end
        return false
    end)

    local GameDebugBeginStats = GameObject.Find("GameDebugBeginStats")
    if(GameDebugBeginStats ~= nil) then
        GameDebugBeginStats:SetActive(false)
    end

    F12KeyListen = GameUIManager:ListenKey(KeyCode.F12, function (key, down)
      if(down) then
        if (GlobalDebugWindow:GetVisible()) then GlobalDebugWindow:Hide()
        else GlobalDebugWindow:Show() end
      end
    end)

    GameManager.GameMediator:SubscribeSingleEvent(GamePackage:GetSystemPackage(), "DebugToolsOpenOptWindow", "DebugTools", function () 
      GlobalDebugOptWindow:Show()
      return false
    end);

    return true
  end,
  ---模块卸载前函数
  ---@param thisGamePackage GamePackage
  ---@return boolean
  PackageBeforeUnLoad = function(thisGamePackage)

    GameUIManager:DeleteKeyListen(F12KeyListen)
    GameManager.GameMediator:UnRegisterSingleEvent("DebugToolsOpenOptWindow")
    GameManager.GameMediator:UnRegisterEventHandler(DebugOptStandByEvent, PackageEntryDebugOptStandByHandler)
    GameManager.GameMediator:UnRegisterEventHandler(DebugOptEvent, PackageDebugOptEntryHandler)

    if (not Slua.IsNull(GlobalDebugToolbar)) then UnityEngine.Object.Destroy(GlobalDebugToolbar) end
    if (not Slua.IsNull(GlobalDebugWindow)) then UnityEngine.Object.Destroy(GlobalDebugWindow) end
    if (not Slua.IsNull(GlobalDebugOptWindow)) then UnityEngine.Object.Destroy(GlobalDebugOptWindow) end
    if (not Slua.IsNull(GlobalRuntimeHierarchyWindow)) then UnityEngine.Object.Destroy(GlobalRuntimeHierarchyWindow) end
    if (not Slua.IsNull(GlobalRuntimeInspectorWindow)) then UnityEngine.Object.Destroy(GlobalRuntimeInspectorWindow) end

    GameManager.GameMediator:UnRegisterGlobalEvent(DebugOptEvent)
    return true
  end
}
