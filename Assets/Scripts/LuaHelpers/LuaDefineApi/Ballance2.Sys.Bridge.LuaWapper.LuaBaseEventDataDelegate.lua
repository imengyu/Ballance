---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaBaseEventDataDelegate
local LuaBaseEventDataDelegate={ }
---
---@public
---@param self LuaTable 
---@param baseEvent BaseEventData 
---@return void 
function LuaBaseEventDataDelegate:Invoke(self, baseEvent) end
---
---@public
---@param self LuaTable 
---@param baseEvent BaseEventData 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaBaseEventDataDelegate:BeginInvoke(self, baseEvent, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaBaseEventDataDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaBaseEventDataDelegate = LuaBaseEventDataDelegate