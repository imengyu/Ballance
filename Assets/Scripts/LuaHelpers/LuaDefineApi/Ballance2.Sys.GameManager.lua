---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameManager
---@field public Instance GameManager 实例
---@field public GameMediator GameMediator GameMediator 实例
---@field public GameMainLuaState MainState 游戏全局Lua虚拟机
---@field public GameMainLuaSvr LuaSvr 游戏全局Lua虚拟机
---@field public GameBaseCamera Camera 基础摄像机
---@field public GameCanvas RectTransform 根Canvas
---@field public GameActionStore GameActionStore 游戏内核ActionStore
---@field public GameStore Store 游戏内核Store
local GameManager={ }
---开始退出游戏流程
---@public
---@return void 
function GameManager:QuitGame() end
---获取系统服务
---@public
---@param name string 服务名称
---@return GameService 返回服务实例，如果没有找到，则返回null
function GameManager:GetSystemService(name) end
---设置基础摄像机状态
---@public
---@param visible boolean 是否显示
---@return void 
function GameManager:SetGameBaseCameraVisible(visible) end
---获取游戏版本
---@public
---@return number 
function GameManager.LuaBindingCallback() end
---游戏管理器
Ballance2.Sys.GameManager = GameManager