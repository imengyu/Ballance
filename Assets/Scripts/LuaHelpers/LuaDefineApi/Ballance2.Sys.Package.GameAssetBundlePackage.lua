---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameAssetBundlePackage : GamePackage
local GameAssetBundlePackage={ }
---
---@public
---@return void 
function GameAssetBundlePackage:Destroy() end
---
---@public
---@param filePath string 
---@return Task`1 
function GameAssetBundlePackage:LoadInfo(filePath) end
---模块包 AssetBundle
Ballance2.Sys.Package.GameAssetBundlePackage = GameAssetBundlePackage