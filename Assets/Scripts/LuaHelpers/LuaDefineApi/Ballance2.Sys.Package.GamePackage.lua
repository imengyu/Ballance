---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GamePackage
---@field public CSharpAssembly Assembly C# 程序集
---@field public PackageLuaState LuaState Lua 虚拟机
---@field public PackageFilePath string 模块文件路径
---@field public PackageName string 模块包名
---@field public PackageVersion number 模块版本号
---@field public BaseInfo GamePackageBaseInfo 基础信息
---@field public LoadError string 加载错误
---@field public PackageDef XmlDocument PackageDef文档
---@field public AssetBundle AssetBundle AssetBundle
---@field public TargetVersion number 表示模块目标游戏内核版本
---@field public MinVersion number 表示模块可以正常使用的最低游戏内核版本
---@field public EntryCode string 入口代码
---@field public Type number 模块类型
---@field public CodeType number 代码类型
---@field public ShareLuaState string 共享Lua虚拟机
---@field public Status number 获取模块状态
local GamePackage={ }
---获取系统的模块结构
---@public
---@return GamePackage 
function GamePackage.GetSystemPackage() end
---注册lua虚拟脚本到物体上
---@public
---@param name string lua虚拟脚本的名称
---@param gameObject GameObject 要附加的物体
---@param className string 目标代码类名
---@return void 
function GamePackage:RegisterLuaObject(name, gameObject, className) end
---
---@public
---@param name string 
---@param gameLuaObjectHost GameLuaObjectHost& 
---@return boolean 
function GamePackage:FindLuaObject(name, gameLuaObjectHost) end
---获取模组启动代码是否已经执行
---@public
---@return boolean 
function GamePackage:IsEntryCodeExecuted() end
---获取Lua虚拟机是否初始化完成
---@public
---@return boolean 
function GamePackage:IsLuaStateInitFinished() end
---运行模块初始化代码
---@public
---@return boolean 
function GamePackage:RunPackageExecutionCode() end
---运行模块卸载回调
---@public
---@return boolean 
function GamePackage:RunPackageBeforeUnLoadCode() end
---导入 Lua 类到当前模组虚拟机中。            注意，类函数以 “@类名” 开头，            关于 Lua 类，请参考 Docs/LuaClass 。
---@public
---@param className string 类名
---@return LuaFunction 类创建函数
function GamePackage:RequireLuaClass(className) end
---导入Lua文件到当前模组虚拟机中
---@public
---@param fileName string LUA文件名
---@return boolean 
function GamePackage:RequireLuaFile(fileName) end
---获取当前 模块主代码 的指定函数
---@public
---@param funName string 函数名
---@return LuaFunction 返回函数，未找到返回null
function GamePackage:GetLuaFun(funName) end
---调用模块主代码的lua无参函数
---@public
---@param funName string lua函数名称
---@return void 
function GamePackage:CallLuaFun(funName) end
---调用模块主代码的lua函数
---@public
---@param funName string lua函数名称
---@param pararms Object[] 参数
---@return void 
function GamePackage:CallLuaFun(funName, pararms) end
---调用指定的lua虚拟脚本中的lua无参函数
---@public
---@param luaObjectName string lua虚拟脚本名称
---@param funName string lua函数名称
---@return void 
function GamePackage:CallLuaFun(luaObjectName, funName) end
---调用指定的lua虚拟脚本中的lua函数
---@public
---@param luaObjectName string lua虚拟脚本名称
---@param funName string lua函数名称
---@param pararms Object[] 参数
---@return void 
function GamePackage:CallLuaFun(luaObjectName, funName, pararms) end
---
---@public
---@return string 
function GamePackage:ToString() end
---读取 资源包 中的文字资源
---@public
---@param pathorname string 资源路径
---@return TextAsset 
function GamePackage:GetTextAsset(pathorname) end
---读取 资源包 中的 Prefab 资源
---@public
---@param pathorname string 资源路径
---@return GameObject 
function GamePackage:GetPrefabAsset(pathorname) end
---
---@public
---@param pathorname string 
---@return Texture 
function GamePackage:GetTextureAsset(pathorname) end
---
---@public
---@param pathorname string 
---@return Texture2D 
function GamePackage:GetTexture2DAsset(pathorname) end
---
---@public
---@param pathorname string 
---@return Sprite 
function GamePackage:GetSpriteAsset(pathorname) end
---
---@public
---@param pathorname string 
---@return Material 
function GamePackage:GetMaterialAsset(pathorname) end
---
---@public
---@param pathorname string 
---@return PhysicMaterial 
function GamePackage:GetPhysicMaterialAsset(pathorname) end
---读取包中的Lua代码资源
---@public
---@param pathorname string 文件名称或路径
---@return string 如果读取成功则返回代码内容，否则返回null
function GamePackage:GetCodeLuaAsset(pathorname) end
---加载包中的c#代码资源
---@public
---@param pathorname string 
---@return Assembly 
function GamePackage:LoadCodeCSharp(pathorname) end
---
---@public
---@param name string 
---@param data Object 
---@return Object 
function GamePackage:AddCustomProp(name, data) end
---
---@public
---@param name string 
---@return Object 
function GamePackage:GetCustomProp(name) end
---
---@public
---@param name string 
---@param data Object 
---@return Object 
function GamePackage:SetCustomProp(name, data) end
---
---@public
---@param name string 
---@return boolean 
function GamePackage:RemoveCustomProp(name) end
---模块包实例
Ballance2.Sys.Package.GamePackage = GamePackage