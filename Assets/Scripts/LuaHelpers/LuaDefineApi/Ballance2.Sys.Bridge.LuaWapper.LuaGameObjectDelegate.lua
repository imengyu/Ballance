---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaGameObjectDelegate
local LuaGameObjectDelegate={ }
---
---@public
---@param self LuaTable 
---@param gameObject GameObject 
---@return void 
function LuaGameObjectDelegate:Invoke(self, gameObject) end
---
---@public
---@param self LuaTable 
---@param gameObject GameObject 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaGameObjectDelegate:BeginInvoke(self, gameObject, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaGameObjectDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaGameObjectDelegate = LuaGameObjectDelegate