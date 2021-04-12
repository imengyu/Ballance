-- 调试工具类
GameManager = Ballance2.Sys.GameManager
CloneUtils = Ballance2.Sys.Utils.CloneUtils
GameUIManager = Ballance2.Sys.Services.GameUIManager
Log = Ballance2.Utils.Log

GlobalDebugToolbar = nil
GlobalDebugWindow = nil

---模块入口函数
---@param thisGamePackage GamePackage
---@return boolean
function PackageEntry(thisGamePackage)
    thisGamePackage:RequireLuaFile('DebugUtils')

    GlobalDebugToolbar = thisGamePackage:GetPrefabAsset('Assets/Packages/core.debug/Prefabs/DebugToolbar.prefab')
    GlobalDebugToolbar = CloneUtils.CloneNewObjectWithParent(GlobalDebugToolbar, GameManager.Instance.GameCanvas, 'DebugToolbar')
    GlobalDebugWindow = thisGamePackage:GetPrefabAsset('Assets/Packages/core.debug/Prefabs/DebugWindow.prefab')
    GlobalDebugWindow = GameUIManager:CreateWindow('Console', CloneUtils.CloneNewObjectWithParent(DebugWindow, GameManager.Instance.GameCanvas, 'DebugWindow'), true)
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
