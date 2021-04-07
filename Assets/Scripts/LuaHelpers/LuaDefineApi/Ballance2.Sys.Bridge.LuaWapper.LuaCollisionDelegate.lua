---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaCollisionDelegate
local LuaCollisionDelegate={ }
---
---@public
---@param self LuaTable 
---@param collision Collision 
---@return void 
function LuaCollisionDelegate:Invoke(self, collision) end
---
---@public
---@param self LuaTable 
---@param collision Collision 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaCollisionDelegate:BeginInvoke(self, collision, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaCollisionDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaCollisionDelegate = LuaCollisionDelegate