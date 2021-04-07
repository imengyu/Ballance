---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaVoidDelegate
local LuaVoidDelegate={ }
---
---@public
---@param self LuaTable 
---@return void 
function LuaVoidDelegate:Invoke(self) end
---
---@public
---@param self LuaTable 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaVoidDelegate:BeginInvoke(self, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaVoidDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaVoidDelegate = LuaVoidDelegate