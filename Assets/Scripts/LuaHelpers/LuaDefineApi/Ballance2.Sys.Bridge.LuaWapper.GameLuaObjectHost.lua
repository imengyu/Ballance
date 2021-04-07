---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameLuaObjectHost
---@field public TAG string 
---@field public Name string Lua 对象名字，用于 FindLuaObject 查找
---@field public LuaClassName string 获取或设置 Lua类的文件名（eg MenuLevel）
---@field public LuaPackageName string 获取或设置 Lua 类所在的模块包名（该模块类型必须是 Module 并可运行）。设置后该对象会自动注册到 LuaObject 中
---@field public LuaInitialVars List`1 设置 Lua 初始参数，用于方便地从 Unity 编辑器直接引入初始参数至 Lua，这些变量会设置到 Lua self 上，可直接获取。
---@field public CreateStore boolean 
---@field public CreateActionStore boolean 
---@field public LuaSelf LuaTable lua self
---@field public LuaState LuaState 获取当前虚拟机
---@field public Package GamePackage 获取对应 模组包
---@field public Store Store 获取此Lua脚本的共享数据仓库
---@field public ActionStore GameActionStore 获取此Lua脚本的共享操作仓库
---@field public PackageName string 获取该 Lua 脚本所属包包名
local GameLuaObjectHost={ }
---更新 lua 脚本的 InitialVars 至 LuaInitialVars 脚本上
---@public
---@return void 
function GameLuaObjectHost:UpdateAllVarToLua() end
---更新所有 LuaInitialVars 至 lua 脚本上
---@public
---@return void 
function GameLuaObjectHost:UpdateAllVarFromLua() end
---更新 LuaVarObjectInfo 至 lua 脚本上
---@public
---@param v LuaVarObjectInfo 
---@return void 
function GameLuaObjectHost:UpdateVarToLua(v) end
---从 lua 脚本上获取 lua 变量更新至 LuaVarObjectInfo
---@public
---@param v LuaVarObjectInfo 
---@return void 
function GameLuaObjectHost:UpdateVarFromLua(v) end
---将指定名字的 lua 变量更新至 LuaVarObjectInfo
---@public
---@param paramName string 变量名称
---@return boolean 如果没有找到变量，则返回false，否则返回true。
function GameLuaObjectHost:UpdateVarFromLua(paramName) end
---获取当前 Lua 类
---@public
---@return LuaTable 
function GameLuaObjectHost:GetLuaClass() end
---获取当前 Object 的指定函数
---@public
---@param funName string 函数名
---@return LuaFunction 返回函数，未找到返回null
function GameLuaObjectHost:GetLuaFun(funName) end
---调用的lua无参函数
---@public
---@param funName string lua函数名称
---@return void 
function GameLuaObjectHost:CallLuaFun(funName) end
---调用的lua函数
---@public
---@param funName string lua函数名称
---@param pararms Object[] 参数
---@return void 
function GameLuaObjectHost:CallLuaFun(funName, pararms) end
---简易 Lua 脚本承载组件（感觉很像 Lua 的 MonoBehaviour）
Ballance2.Sys.Bridge.LuaWapper.GameLuaObjectHost = GameLuaObjectHost