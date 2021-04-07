---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaColliderDelegate
local LuaColliderDelegate={ }
---
---@public
---@param self LuaTable 
---@param collider Collider 
---@return void 
function LuaColliderDelegate:Invoke(self, collider) end
---
---@public
---@param self LuaTable 
---@param collider Collider 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaColliderDelegate:BeginInvoke(self, collider, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaColliderDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaColliderDelegate = LuaColliderDelegate