---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameEventNames
---@field public EVENT_BASE_INIT_FINISHED string 全局（基础管理器）全部初始化完成时触发该事件
---@field public EVENT_GLOBAL_ALERT_CLOSE string 全局对话框（Alert，Confirm）关闭时触发该事件
---@field public EVENT_BEFORE_GAME_QUIT string 游戏即将退出时触发该事件
---@field public EVENT_PACKAGE_LOAD_SUCCESS string 模块加载成功
---@field public EVENT_PACKAGE_LOAD_FAILED string 模块加载成功
---@field public EVENT_PACKAGE_REGISTERED string 模块注册
---@field public EVENT_PACKAGE_UNLOAD string 模块卸载
local GameEventNames={ }
---
Ballance2.Sys.Bridge.GameEventNames = GameEventNames