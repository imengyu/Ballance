---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameStaticResourcesPool
---@field public PrefabUIEmpty GameObject 
---@field public PrefabEmpty GameObject 
---@field public GamePrefab List`1 静态引入 Prefab
---@field public GameAssets List`1 静态引入资源
local GameStaticResourcesPool={ }
---在静态引入资源中查找
---@public
---@param name string 资源名称
---@return GameObject 
function GameStaticResourcesPool.FindStaticPrefabs(name) end
---在静态引入资源中查找
---@public
---@param name string 资源名称
---@return Object 
function GameStaticResourcesPool.FindStaticAssets(name) end
---游戏静态资源池
Ballance2.Sys.Res.GameStaticResourcesPool = GameStaticResourcesPool