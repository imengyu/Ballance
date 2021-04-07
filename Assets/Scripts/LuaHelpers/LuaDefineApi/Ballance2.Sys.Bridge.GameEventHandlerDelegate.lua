---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameEventHandlerDelegate
local GameEventHandlerDelegate={ }
---
---@public
---@param evtName string 
---@param pararms Object[] 
---@return boolean 
function GameEventHandlerDelegate:Invoke(evtName, pararms) end
---
---@public
---@param evtName string 
---@param pararms Object[] 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function GameEventHandlerDelegate:BeginInvoke(evtName, pararms, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return boolean 
function GameEventHandlerDelegate:EndInvoke(result) end
---事件接收器内核回调
Ballance2.Sys.Bridge.GameEventHandlerDelegate = GameEventHandlerDelegate