-- 调试工具类
GameManager = Ballance2.Sys.GameManager
CloneUtils = Ballance2.Sys.Utils.CloneUtils
GameUIManager = GameManager.Instance:GetSystemService('GameUIManager') ---@type GameUIManager
Log = Ballance2.Utils.Log

GlobalDebugToolbar = nil ---@type GameLuaObjectHost
GlobalDebugWindow = nil
GlobalDebugOptWindow = nil

---模块入口函数
---@param thisGamePackage GamePackage
---@return boolean
function PackageEntry(thisGamePackage)
    thisGamePackage:RequireLuaFile('DebugUtils')

    GlobalDebugToolbar = thisGamePackage:GetPrefabAsset('Assets/Packages/core.debug/Prefabs/DebugToolbar.prefab')
    GlobalDebugToolbar = CloneUtils.CloneNewObjectWithParent(GlobalDebugToolbar, GameManager.Instance.GameCanvas, 'DebugToolbar')
    GlobalDebugToolbar.transform:SetAsFirstSibling()
    GlobalDebugToolbar = GlobalDebugToolbar:GetComponent(Ballance2.Sys.Bridge.LuaWapper.GameLuaObjectHost)
    local DebugWindow = thisGamePackage:GetPrefabAsset('Assets/Packages/core.debug/Prefabs/DebugWindow.prefab')
    GlobalDebugWindow = GameUIManager:CreateWindow('Console', 
        CloneUtils.CloneNewObjectWithParent(DebugWindow, GameManager.Instance.GameCanvas, 'DebugWindow').transform, 
        true, 9, -70, 660, 440)
    GlobalDebugWindow.CloseAsHide = true
    local DebugOptWindow = thisGamePackage:GetPrefabAsset('Assets/Packages/core.debug/Prefabs/DebugOptWindow.prefab')
    GlobalDebugOptWindow = GameUIManager:CreateWindow('Debug options', 
        CloneUtils.CloneNewObjectWithParent(DebugOptWindow, GameManager.Instance.GameCanvas, 'DebugOptWindow').transform, 
        false, 675, -70, 210, 188)
    GlobalDebugOptWindow.CloseAsHide = true
    return true
end

---模块卸载前函数
---@param thisGamePackage GamePackage
---@return boolean
function PackageBeforeUnLoad(thisGamePackage)
    if (not Slua.IsNull(GlobalDebugToolbar)) then UnityEngine.Object.Destroy(GlobalDebugToolbar) end
    if (not Slua.IsNull(GlobalDebugWindow)) then UnityEngine.Object.Destroy(GlobalDebugWindow) end
    return true
end
