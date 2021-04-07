---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaActionStoreReturnBoolDelegate
local LuaActionStoreReturnBoolDelegate={ }
---
---@public
---@param self LuaTable 
---@param store GameActionStore 
---@return boolean 
function LuaActionStoreReturnBoolDelegate:Invoke(self, store) end
---
---@public
---@param self LuaTable 
---@param store GameActionStore 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaActionStoreReturnBoolDelegate:BeginInvoke(self, store, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return boolean 
function LuaActionStoreReturnBoolDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaActionStoreReturnBoolDelegate = LuaActionStoreReturnBoolDelegate