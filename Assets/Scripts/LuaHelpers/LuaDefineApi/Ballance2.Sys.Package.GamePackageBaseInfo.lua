---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GamePackageBaseInfo
---@field public Name string 模块名称
---@field public Author string 模块作者
---@field public Introduction string 模块介绍
---@field public Logo string 模块Logo
---@field public LogoTexture Sprite 模块Logo Sprite
---@field public Link string 模块链接
---@field public VersionName string 显示给用户看的版本
---@field public Dependencies List`1 模块依赖
local GamePackageBaseInfo={ }
---模块基础信息
Ballance2.Sys.Package.GamePackageBaseInfo = GamePackageBaseInfo