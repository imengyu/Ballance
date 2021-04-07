---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaIntDelegate
local LuaIntDelegate={ }
---
---@public
---@param self LuaTable 
---@param v number 
---@return void 
function LuaIntDelegate:Invoke(self, v) end
---
---@public
---@param self LuaTable 
---@param v number 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaIntDelegate:BeginInvoke(self, v, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaIntDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaIntDelegate = LuaIntDelegate