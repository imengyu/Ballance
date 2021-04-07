---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaStartDelegate
local LuaStartDelegate={ }
---
---@public
---@param self LuaTable 
---@param gameObject GameObject 
---@return void 
function LuaStartDelegate:Invoke(self, gameObject) end
---
---@public
---@param self LuaTable 
---@param gameObject GameObject 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaStartDelegate:BeginInvoke(self, gameObject, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaStartDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaStartDelegate = LuaStartDelegate