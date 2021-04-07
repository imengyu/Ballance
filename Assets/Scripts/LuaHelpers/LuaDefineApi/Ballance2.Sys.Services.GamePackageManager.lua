---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GamePackageManager : GameService
---@field public SYSTEM_PACKAGE_NAME string 
local GamePackageManager={ }
---
---@public
---@return void 
function GamePackageManager:Destroy() end
---
---@public
---@return boolean 
function GamePackageManager:Initialize() end
---注册包
---@public
---@param packageName string 包名
---@return Task`1 返回是否加载成功。要获得错误代码，请获取
function GamePackageManager:RegisterPackage(packageName) end
---查找已注册的模块
---@public
---@param packageName string 包名
---@return GamePackage 
function GamePackageManager:FindRegisteredPackage(packageName) end
---取消注册模块
---@public
---@param packageName string 包名
---@param unLoadImmediately boolean 是否立即卸载
---@return boolean 
function GamePackageManager:UnRegisterPackage(packageName, unLoadImmediately) end
---获取包是否正在加载
---@public
---@param packageName string 包名
---@return boolean 
function GamePackageManager:IsPackageLoading(packageName) end
---获取包是否正在注册
---@public
---@param packageName string 包名
---@return boolean 
function GamePackageManager:IsPackageRegistering(packageName) end
---获取包是否已加载
---@public
---@param packageName string 包名
---@return boolean 
function GamePackageManager:IsPackageLoaded(packageName) end
---通知模块运行
---@public
---@param packageNameFilter string 
---@return void 
function GamePackageManager:NotifyAllPackageRun(packageNameFilter) end
---加载模块
---@public
---@param packageName string 模块包名
---@return Task`1 返回加载是否成功
function GamePackageManager:LoadPackage(packageName) end
---卸载模块
---@public
---@param packageName string 模块包名
---@param unLoadImmediately boolean 是否立即卸载，如果为false，此模块            将等待至依赖它的模块全部卸载之后才会卸载
---@return boolean 返回是否成功
function GamePackageManager:UnLoadPackage(packageName, unLoadImmediately) end
---查找已加载的模块
---@public
---@param packageName string 模块包名
---@return GamePackage 
function GamePackageManager:FindPackage(packageName) end
---框架包管理器
Ballance2.Sys.Services.GamePackageManager = GamePackageManager