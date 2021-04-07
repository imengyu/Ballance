---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaAxisEventDataDelegate
local LuaAxisEventDataDelegate={ }
---
---@public
---@param self LuaTable 
---@param baseEvent AxisEventData 
---@return void 
function LuaAxisEventDataDelegate:Invoke(self, baseEvent) end
---
---@public
---@param self LuaTable 
---@param baseEvent AxisEventData 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaAxisEventDataDelegate:BeginInvoke(self, baseEvent, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaAxisEventDataDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaAxisEventDataDelegate = LuaAxisEventDataDelegate