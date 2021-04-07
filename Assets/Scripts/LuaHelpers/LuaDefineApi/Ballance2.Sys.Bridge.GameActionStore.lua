---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameActionStore
---@field public TAG string 标签
---@field public Actions Dictionary`2 获取此存储库的所有操作
---@field public Name string 获取此存储库的包名
---@field public KeyName string 获取此存储库的KeyName
---@field public Package GamePackage 获取此存储库所属的包
local GameActionStore={ }
---注册操作
---@public
---@param package GamePackage 操作所在模块包
---@param name string 操作名称
---@param handlerName string 接收器名称
---@param handler GameActionHandlerDelegate 接收函数
---@param callTypeCheck String[] 函数参数检查，数组长度规定了操作需要的参数，            组值是一个或多个允许的类型名字，例如 UnityEngine.GameObject System.String 。            如果一个参数允许多种类型，可使用/分隔。            如果不需要参数检查，也可以为null，则当前操作将不会进行类型检查
---@return GameAction 返回注册的操作实例，如果注册失败则返回 null ，请查看 LastError 的值
function GameActionStore:RegisterAction(package, name, handlerName, handler, callTypeCheck) end
---注册操作(LUA)
---@public
---@param package GamePackage 操作所在模块包
---@param name string 操作名称
---@param handlerName string 接收器名称
---@param luaFunction LuaFunction LUA接收函数
---@param self LuaTable LUA self （当前类，LuaTable），如无可填null
---@param callTypeCheck String[] 函数参数检查，数组长度规定了操作需要的参数，            数组值是一个或多个允许的类型名字，例如 UnityEngine.GameObject System.String 。            如果一个参数允许多种类型，可使用/分隔。            如果不需要参数检查，也可以为null，则当前操作将不会进行类型检查
---@return GameAction 返回注册的操作实例，如果注册失败则返回 null ，请查看 LastError 的值
function GameActionStore:RegisterAction(package, name, handlerName, luaFunction, self, callTypeCheck) end
---注册操作
---@public
---@param package GamePackage 操作所在模块包
---@param name string 操作名称
---@param handler GameHandler 接收器
---@param callTypeCheck String[] 函数参数检查，数组长度规定了操作需要的参数，            组值是一个或多个允许的类型名字，例如 UnityEngine.GameObject System.String 。            如果一个参数允许多种类型，可使用/分隔。            如果不需要参数检查，也可以为null，则当前操作将不会进行类型检查
---@return GameAction 返回注册的操作实例，如果注册失败则返回 null ，请查看 LastError 的值
function GameActionStore:RegisterAction(package, name, handler, callTypeCheck) end
---取消注册操作
---@public
---@param action GameAction 操作实例
---@return void 
function GameActionStore:UnRegisterAction(action) end
---取消注册操作
---@public
---@param name string 操作名称
---@return void 
function GameActionStore:UnRegisterAction(name) end
---取消注册多个操作
---@public
---@param names String[] 
---@return void 
function GameActionStore:UnRegisterActions(names) end
---获取操作是否注册
---@public
---@param name string 操作名称
---@return boolean 是否注册
function GameActionStore:IsActionRegistered(name) end
---
---@public
---@param name string 
---@param e GameAction& 
---@return boolean 
function GameActionStore:IsActionRegistered(name, e) end
---获取操作实例
---@public
---@param name string 操作名称
---@return GameAction 返回的操作实例
function GameActionStore:GetRegisteredAction(name) end
---注册多个操作
---@public
---@param package GamePackage 操作所在模块包
---@param names String[] 操作名称数组
---@param handlerNames String[] 接收器名称数组
---@param handlers GameActionHandlerDelegate[] 接收函数数组
---@param callTypeChecks String[][] 函数参数检查，如果不需要，也可以为null
---@return number 返回注册成功的操作个数
function GameActionStore:RegisterActions(package, names, handlerNames, handlers, callTypeChecks) end
---注册多个操作
---@public
---@param package GamePackage 操作所在模块包
---@param names String[] 操作名称数组
---@param handlerName string 
---@param handlers GameActionHandlerDelegate[] 接收函数数组
---@param callTypeChecks String[][] 函数参数检查，如果不需要，也可以为null
---@return number 返回注册成功的操作个数
function GameActionStore:RegisterActions(package, names, handlerName, handlers, callTypeChecks) end
---注册多个操作
---@public
---@param package GamePackage 操作所在模块包
---@param names String[] 操作名称数组
---@param handlerNames String[] 接收器名称数组
---@param luaFunctionHandlers LuaFunction[] LUA接收函数数组
---@param self LuaTable LUA self （当前类，LuaTable），如无可填null
---@param callTypeChecks String[][] 函数参数检查，如果不需要，也可以为null
---@return number 返回注册成功的操作个数
function GameActionStore:RegisterActions(package, names, handlerNames, luaFunctionHandlers, self, callTypeChecks) end
---注册多个操作
---@public
---@param package GamePackage 操作所在模块包
---@param names String[] 操作名称数组
---@param handlerName string 接收器名（多个接收器名字一样）
---@param luaFunctionHandlers LuaFunction[] LUA接收函数数组
---@param self LuaTable LUA self （当前类，LuaTable），如无可填null
---@param callTypeChecks String[][] 函数参数检查，如果不需要，也可以为null
---@return number 返回注册成功的操作个数
function GameActionStore:RegisterActions(package, names, handlerName, luaFunctionHandlers, self, callTypeChecks) end
---调用操作
---@public
---@param name string 目标操作名称
---@param param Object[] 调用参数
---@return GameActionCallResult 返回操作调用结果，如果未找到操作，则返回 GameActionCallResult.FailResult
function GameActionStore:CallAction(name, param) end
---调用操作
---@public
---@param action GameAction 目标操作实例
---@param param Object[] 调用参数
---@return GameActionCallResult 返回操作调用结果，如果未找到操作，则返回 GameActionCallResult.FailResult
function GameActionStore:CallAction(action, param) end
---操作存储库
Ballance2.Sys.Bridge.GameActionStore = GameActionStore