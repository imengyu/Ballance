---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaStoreReturnBoolDelegate
local LuaStoreReturnBoolDelegate={ }
---
---@public
---@param self LuaTable 
---@param store Store 
---@return boolean 
function LuaStoreReturnBoolDelegate:Invoke(self, store) end
---
---@public
---@param self LuaTable 
---@param store Store 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaStoreReturnBoolDelegate:BeginInvoke(self, store, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return boolean 
function LuaStoreReturnBoolDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaStoreReturnBoolDelegate = LuaStoreReturnBoolDelegate