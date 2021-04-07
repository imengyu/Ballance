---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameHandler
---@field public BelongPackage GamePackage 所属模块
---@field public Name string 接收器名称
---@field public Destroyed boolean 获取是否被释放
local GameHandler={ }
---调用自定义接收器
---@public
---@param pararms Object[] 参数
---@return Object 返回自定义对象
function GameHandler:CallCustomHandler(pararms) end
---调用接收器
---@public
---@param evtName string 事件名称
---@param pararms Object[] 参数
---@return boolean 返回是否中断剩余事件分发/返回Action是否成功
function GameHandler:CallEventHandler(evtName, pararms) end
---调用操作接收器
---@public
---@param pararms Object[] 参数
---@return GameActionCallResult 返回是否中断剩余事件分发/返回Action是否成功
function GameHandler:CallActionHandler(pararms) end
---
---@public
---@param gamePackage GamePackage 
---@param name string 
---@param actionHandler GameActionHandlerDelegate 
---@return GameHandler 
function GameHandler.CreateCsActionHandler(gamePackage, name, actionHandler) end
---
---@public
---@param gamePackage GamePackage 
---@param name string 
---@param eventHandler GameEventHandlerDelegate 
---@return GameHandler 
function GameHandler.CreateCsEventHandler(gamePackage, name, eventHandler) end
---
---@public
---@param gamePackage GamePackage 
---@param name string 
---@param handler GameCustomHandlerDelegate 
---@return GameHandler 
function GameHandler.CreateCsCustomHandler(gamePackage, name, handler) end
---创建 Lua 通用接收器
---@public
---@param gamePackage GamePackage 所在包
---@param name string 接收器名称
---@param luaFunction LuaFunction Lua函数
---@param self LuaTable Lua self
---@return GameHandler 返回创建的 GameHandler，如果创建失败，则返回null
function GameHandler.CreateLuaHandler(gamePackage, name, luaFunction, self) end
---创建 Lua 静态或 GameLuaObjectHost 接收器
---@public
---@param gamePackage GamePackage 所在包
---@param name string 接收器名称
---@param luaModulHandler string 接收器标识符字符串
---@return GameHandler 返回创建的 GameHandler，如果创建失败，则返回null
function GameHandler.CreateLuaStaticHandler(gamePackage, name, luaModulHandler) end
---游戏通用接收器
Ballance2.Sys.Bridge.Handler.GameHandler = GameHandler