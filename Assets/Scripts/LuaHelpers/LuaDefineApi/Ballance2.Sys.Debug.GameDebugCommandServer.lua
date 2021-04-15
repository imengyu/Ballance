---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameDebugCommandServer
---@field public Instance GameDebugCommandServer 实例
local GameDebugCommandServer={ }

---执行调试命令
---@param cmd string 命令字符串
function GameDebugCommandServer:ExecuteCommand(cmd) end
---注册调试命令
---@param keyword string 命令单词
---@param callback function 命令回调
---@param limitArgCount number 命令最低参数，默认 0 表示无参数或不限制
---@param helpText string 命令帮助文字
---@return number 成功返回命令ID，不成功返回-1
function GameDebugCommandServer:RegisterCommand(keyword, callback, limitArgCount, helpText) end
---取消注册命令
---@param cmdId number 命令ID
function GameDebugCommandServer:UnRegisterCommand(cmdId) end
---获取命令是否注册
---@param keyword string 命令单词
function GameDebugCommandServer:IsCommandRegistered(keyword) end

---调试命令服务
Ballance2.Sys.Debug.GameDebugCommandServer = GameDebugCommandServer