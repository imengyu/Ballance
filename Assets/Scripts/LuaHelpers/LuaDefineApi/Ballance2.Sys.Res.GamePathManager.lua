---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GamePathManager
---@field public DEBUG_PACKAGE_FOLDER string 调试模组包存放路径
---@field public ANDROID_FOLDER_PATH string 安卓系统数据目录
---@field public ANDROID_PACKAGES_PATH string 安卓系统模组目录
---@field public ANDROID_LEVELS_PATH string 安卓系统关卡目录
---@field public DEBUG_PATH string 调试路径（输出目录）（您在调试时请点击菜单 "Ballance">"开发设置">"Debug Settings" 将其更改为自己调试输出存放目录）
---@field public DEBUG_PACKAGES_PATH string 调试路径（模组目录）
---@field public DEBUG_LEVELS_PATH string 调试路径（关卡目录）
local GamePathManager={ }
---检测是否是绝对路径
---@public
---@param path string 路径
---@return boolean 
function GamePathManager.IsAbsolutePath(path) end
---将资源的相对路径转为资源真实路径
---@public
---@param type string 资源种类（gameinit、core: 核心文件、level：关卡、package：模块）
---@param pathorname string 相对路径或名称
---@param replacePlatform boolean 是否替换文件路径中的[Platform]
---@return string 
function GamePathManager.GetResRealPath(type, pathorname, replacePlatform) end
---将关卡资源的相对路径转为关卡资源真实路径
---@public
---@param pathorname string 关卡的相对路径或名称
---@return string 
function GamePathManager.GetLevelRealPath(pathorname) end
---
---@public
---@param newPath string 
---@param buf String[]& 
---@return string 
function GamePathManager.ReplacePathInResourceIdentifier(newPath, buf) end
---
---@public
---@param oldIdentifier string 
---@param outPath String& 
---@return String[] 
function GamePathManager.SplitResourceIdentifier(oldIdentifier, outPath) end
---获取路径中的文件名（不包括后缀）
---@public
---@param path string 路径
---@return string 
function GamePathManager.GetFileNameWithoutExt(path) end
---获取路径中的文件名（包括后缀）
---@public
---@param path string 路径
---@return string 
function GamePathManager.GetFileName(path) end
---获取文件是否存在
---@public
---@param path string 文件路径
---@return boolean 
function GamePathManager.Exists(path) end
---
---@public
---@param path string 
---@return string 
function GamePathManager.FixFilePathScheme(path) end
---路径管理器
Ballance2.Sys.Res.GamePathManager = GamePathManager