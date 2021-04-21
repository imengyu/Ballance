--[[
Copyright(c) 2021  mengyu
* 模块名：     
Entry.lua
* 用途：
调试包入口
* 作者：
mengyu
* 更改历史：
2021-1-11 创建
]]--


-- 调试工具类
GameManager = Ballance2.Sys.GameManager
CloneUtils = Ballance2.Sys.Utils.CloneUtils
GameObject = UnityEngine.GameObject
GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
Log = Ballance2.Utils.Log
DebugCamera = Ballance2.DebugCamera

GlobalDebugToolbar = nil ---@type GameLuaObjectHost
GlobalDebugWindow = nil ---@type Window|MonoBehaviour
GlobalDebugOptWindow = nil ---@type Window|MonoBehaviour
GlobalRuntimeHierarchyWindow = nil ---@type Window|MonoBehaviour
GlobalRuntimeInspectorWindow = nil ---@type Window|MonoBehaviour

DebugOptEvent = 'core.debug.OptNotify'
DebugOptStandByEvent = 'core.debug.OptStandByNotify'

local PackageEntryDebugOptStandByHandler = nil
local PackageDebugOptEntryHandler = nil

return {
    ---模块入口函数
    ---@param thisGamePackage GamePackage
    ---@return boolean
    PackageEntry = function(thisGamePackage)
        thisGamePackage:RequireLuaFile('DebugUtils')

        GameManager.GameMediator:RegisterGlobalEvent(DebugOptEvent)
        GameManager.GameMediator:RegisterGlobalEvent(DebugOptStandByEvent)
    
        --创建窗口
        GlobalDebugToolbar = thisGamePackage:GetPrefabAsset('Assets/Packages/core.debug/Prefabs/DebugToolbar.prefab')
        GlobalDebugToolbar = CloneUtils.CloneNewObjectWithParent(GlobalDebugToolbar, GameManager.Instance.GameCanvas, 'DebugToolbar')
        GlobalDebugToolbar.transform:SetAsLastSibling()
        GlobalDebugToolbar = GlobalDebugToolbar:GetComponent(Ballance2.Sys.Bridge.LuaWapper.GameLuaObjectHost)
        local DebugWindow = thisGamePackage:GetPrefabAsset('Assets/Packages/core.debug/Prefabs/DebugWindow.prefab')
        GlobalDebugWindow = GameUIManager:CreateWindow('Console', 
            CloneUtils.CloneNewObjectWithParent(DebugWindow, GameManager.Instance.GameCanvas, 'DebugWindow').transform, 
            true, 9, -70, 660, 440)
        GlobalDebugWindow.CloseAsHide = true
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

        GameObject.Find("GameDebugBeginStats"):SetActive(false);
        return true
    end,
    ---模块卸载前函数
    ---@param thisGamePackage GamePackage
    ---@return boolean
    PackageBeforeUnLoad = function(thisGamePackage)
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
