---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameActionHandlerDelegate
local GameActionHandlerDelegate={ }
---
---@public
---@param pararms Object[] 
---@return GameActionCallResult 
function GameActionHandlerDelegate:Invoke(pararms) end
---
---@public
---@param pararms Object[] 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function GameActionHandlerDelegate:BeginInvoke(pararms, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return GameActionCallResult 
function GameActionHandlerDelegate:EndInvoke(result) end
---操作接收器内核回调
Ballance2.Sys.Bridge.GameActionHandlerDelegate = GameActionHandlerDelegate