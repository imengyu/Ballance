---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GamePackageStatus
---@field public value__ number 
---@field public NotLoad number 未加载
---@field public Registing number 正在注册
---@field public Loading number 正在加载
---@field public LoadSuccess number 加载成功
---@field public LoadFailed number 加载失败
---@field public UnloadWaiting number 正在等待卸载
---@field public Registered number 已经注册但未加载
local GamePackageStatus={ }
---模块加载状态
Ballance2.Sys.Package.GamePackageStatus = GamePackageStatus