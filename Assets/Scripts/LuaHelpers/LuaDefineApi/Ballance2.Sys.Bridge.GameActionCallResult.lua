---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameActionCallResult
---@field public SuccessResult GameActionCallResult 成功的无其他参数的调用返回结果
---@field public FailResult GameActionCallResult 失败的无其他参数的调用返回结果
---@field public Success boolean 获取是否成功
---@field public ReturnParams Object[] 获取操作返回的数据
local GameActionCallResult={ }
---创建操作调用结果
---@public
---@param success boolean 是否成功
---@param returnParams Object[] 返回的数据
---@return GameActionCallResult 操作调用结果
function GameActionCallResult.CreateActionCallResult(success, returnParams) end
---操作调用结果
Ballance2.Sys.Bridge.GameActionCallResult = GameActionCallResult