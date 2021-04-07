---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameCustomHandlerDelegate
local GameCustomHandlerDelegate={ }
---
---@public
---@param pararms Object[] 
---@return Object 
function GameCustomHandlerDelegate:Invoke(pararms) end
---
---@public
---@param pararms Object[] 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function GameCustomHandlerDelegate:BeginInvoke(pararms, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return Object 
function GameCustomHandlerDelegate:EndInvoke(result) end
---自定义接收器内核回调
Ballance2.Sys.Bridge.GameCustomHandlerDelegate = GameCustomHandlerDelegate