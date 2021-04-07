---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameSettingsManager
local GameSettingsManager={ }
---获取设置执行器
---@public
---@param packageName string 设置所使用包名
---@return GameSettingsActuator 
function GameSettingsManager.GetSettings(packageName) end
---还原默认设置
---@public
---@return void 
function GameSettingsManager.ResetDefaultSettings() end
---设置管理器
Ballance2.Config.GameSettingsManager = GameSettingsManager