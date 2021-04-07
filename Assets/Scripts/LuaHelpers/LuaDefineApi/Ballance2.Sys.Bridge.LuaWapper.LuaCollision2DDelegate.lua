---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaCollision2DDelegate
local LuaCollision2DDelegate={ }
---
---@public
---@param self LuaTable 
---@param collision Collision2D 
---@return void 
function LuaCollision2DDelegate:Invoke(self, collision) end
---
---@public
---@param self LuaTable 
---@param collision Collision2D 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaCollision2DDelegate:BeginInvoke(self, collision, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaCollision2DDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaCollision2DDelegate = LuaCollision2DDelegate