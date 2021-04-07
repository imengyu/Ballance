---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameMediator : GameService
---@field public SYSTEM_ACTION_STORE_NAME string 内核的 ActinoStore 名称
---@field public Events Dictionary`2 
---@field public SystemActionStore GameActionStore 内核的 ActinoStore
---@field public GlobalStore Dictionary`2 
local GameMediator={ }
---
---@public
---@return void 
function GameMediator:Destroy() end
---
---@public
---@return boolean 
function GameMediator:Initialize() end
---注册事件
---@public
---@param evtName string 事件名称
---@return GameEvent 
function GameMediator:RegisterGlobalEvent(evtName) end
---取消注册事件
---@public
---@param evtName string 事件名称
---@return void 
function GameMediator:UnRegisterGlobalEvent(evtName) end
---获取事件是否注册
---@public
---@param evtName string 事件名称
---@return boolean 是否注册
function GameMediator:IsGlobalEventRegistered(evtName) end
---
---@public
---@param evtName string 
---@param e GameEvent& 
---@return boolean 
function GameMediator:IsGlobalEventRegistered(evtName, e) end
---获取事件实例
---@public
---@param evtName string 事件名称
---@return GameEvent 返回的事件实例
function GameMediator:GetRegisteredGlobalEvent(evtName) end
---执行事件分发
---@public
---@param gameEvent GameEvent 事件实例
---@param handlerFilter string 指定事件可以接收到的名字（这里可以用正则）
---@param pararms Object[] 事件参数
---@return number 返回已经发送的接收器个数
function GameMediator:DispatchGlobalEvent(gameEvent, handlerFilter, pararms) end
---执行事件分发
---@public
---@param evtName string 事件名称
---@param handlerFilter string 指定事件可以接收到的名字（这里可以用正则）
---@param pararms Object[] 事件参数
---@return number 返回已经发送的接收器个数
function GameMediator:DispatchGlobalEvent(evtName, handlerFilter, pararms) end
---注册命令接收器（Lua）
---@public
---@param package GamePackage 
---@param evtName string 事件名称
---@param name string 接收器名字
---@param luaFunction LuaFunction 
---@param luaSelf LuaTable 
---@return GameHandler 
function GameMediator:RegisterEventHandler(package, evtName, name, luaFunction, luaSelf) end
---注册命令接收器（Delegate）
---@public
---@param package GamePackage 
---@param evtName string 事件名称
---@param name string 接收器名字
---@param gameHandlerDelegate GameEventHandlerDelegate 回调
---@return GameHandler 
function GameMediator:RegisterEventHandler(package, evtName, name, gameHandlerDelegate) end
---注册事件接收器
---@public
---@param package GamePackage 
---@param evtName string 事件名称
---@param name string 接收器名字
---@param luaModulHandler string 模块接收器函数标识符
---@return GameHandler 返回接收器类
function GameMediator:RegisterEventHandler(package, evtName, name, luaModulHandler) end
---取消注册事件接收器
---@public
---@param evtName string 事件名称
---@param handler GameHandler 接收器类
---@return void 
function GameMediator:UnRegisterEventHandler(evtName, handler) end
---注册全局共享数据存储池
---@public
---@param package GamePackage 所属包
---@param name string 池名称
---@return GameActionStore 如果注册成功，返回池对象；如果已经注册，则返回已经注册的池对象
function GameMediator:RegisterActionStore(package, name) end
---获取全局共享数据存储池
---@public
---@param package GamePackage 所属包
---@param name string 池名称
---@return GameActionStore 
function GameMediator:GetActionStore(package, name) end
---释放已注册的全局共享数据存储池
---@public
---@param package GamePackage 所属包
---@param name string 池名称
---@return boolean 
function GameMediator:UnRegisterActionStore(package, name) end
---释放已注册的全局共享数据存储池
---@public
---@param store GameActionStore 
---@return boolean 
function GameMediator:UnRegisterActionStore(store) end
---调用操作
---@public
---@param package GamePackage 所属包
---@param storeName string 操作仓库名称
---@param name string 操作名称
---@param param Object[] 调用参数
---@return GameActionCallResult 
function GameMediator:CallAction(package, storeName, name, param) end
---调用操作
---@public
---@param store GameActionStore 操作仓库名称
---@param name string 操作名称
---@param param Object[] 调用参数
---@return GameActionCallResult 
function GameMediator:CallAction(store, name, param) end
---调用操作
---@public
---@param action GameAction 操作实体
---@param param Object[] 调用参数
---@return GameActionCallResult 
function GameMediator:CallAction(action, param) end
---注册全局共享数据存储池
---@public
---@param name string 池名称
---@return Store 如果注册成功，返回池对象；如果已经注册，则返回已经注册的池对象
function GameMediator:RegisterGlobalDataStore(name) end
---获取全局共享数据存储池
---@public
---@param name string 池名称
---@return Store 
function GameMediator:GetGlobalDataStore(name) end
---释放已注册的全局共享数据存储池
---@public
---@param name string 池名称
---@return boolean 
function GameMediator:UnRegisterGlobalDataStore(name) end
---释放已注册的全局共享数据存储池
---@public
---@param store Store 
---@return boolean 
function GameMediator:UnRegisterGlobalDataStore(store) end
---游戏中介者
Ballance2.Sys.Services.GameMediator = GameMediator