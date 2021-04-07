---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameHandlerList
local GameHandlerList={ }
---
---@public
---@param evtName string 
---@param parm Object[] 
---@return void 
function GameHandlerList:CallEventHandler(evtName, parm) end
---
---@public
---@return void 
function GameHandlerList:Dispose() end
---GameHandler的一个List包装类
Ballance2.Sys.Bridge.Handler.GameHandlerList = GameHandlerList