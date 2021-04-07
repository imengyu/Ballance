---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaCollider2DDelegate
local LuaCollider2DDelegate={ }
---
---@public
---@param self LuaTable 
---@param collider Collider2D 
---@return void 
function LuaCollider2DDelegate:Invoke(self, collider) end
---
---@public
---@param self LuaTable 
---@param collider Collider2D 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaCollider2DDelegate:BeginInvoke(self, collider, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaCollider2DDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaCollider2DDelegate = LuaCollider2DDelegate