---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameConst
---@field public GameVersion string 游戏版本
---@field public GameBulidVersion number 游戏编译版本
---@field public GameBulidDate string 游戏编译版本
---@field public GamePlatform string 游戏编译平台
---@field public GamePlatformIdentifier string 游戏编译平台标识符
---@field public GameScriptBackend string 游戏编译脚本后端
---@field public GameLoggerOn boolean 配置游戏日志记录器是否启动
---@field public GameLoggerLogToFile boolean 配置游戏日志记录器是否记录至文件
---@field public GameLoggerLogFile string 配置游戏日志记录器记录至文件的路径
---@field public GameLoggerBufferMax number 配置游戏日志记录器缓冲区最大条数
---@field public BallancePrivacyPolicy string 
---@field public BallanceUserAgreement string 
local GameConst={ }
---静态常量配置
Ballance2.Config.GameConst = GameConst