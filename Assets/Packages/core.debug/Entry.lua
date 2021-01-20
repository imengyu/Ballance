--- 调试工具类

GameManager = Ballance2.System.GameManager
CloneUtils = Ballance2.System.Utils.CloneUtils
Log = Ballance2.Utils.Log

GamePackage = nil
DebugToolbar = nil
local TAG = "GameDebugTools:Main"

-- 模块入口函数
function PackageEntry(thisGamePackage)
  GamePackage = thisGamePackage
  thisGamePackage:RequireLuaFile("DebugUtils")
  DebugToolbar = thisGamePackage:GetPrefabAsset("Assets/Packages/core.debug/Prefabs/DebugToolbar.prefab")
  DebugToolbar = CloneUtils.CloneNewObjectWithParent(DebugToolbar, GameManager.Instance.GameCanvas, "DebugToolbar")
  return true
end

-- 模块卸载前函数
function PackageBeforeUnLoad(thisGamePackage)
	GamePackage = thisGamePackage
  UnityEngine.Object.Destroy(DebugToolbar)
  return true
end
