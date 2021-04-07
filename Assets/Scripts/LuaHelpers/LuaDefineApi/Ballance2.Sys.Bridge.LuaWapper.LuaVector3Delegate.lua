---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaVector3Delegate
local LuaVector3Delegate={ }
---
---@public
---@param self LuaTable 
---@param vector3 Vector3 
---@return void 
function LuaVector3Delegate:Invoke(self, vector3) end
---
---@public
---@param self LuaTable 
---@param vector3 Vector3 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaVector3Delegate:BeginInvoke(self, vector3, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaVector3Delegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaVector3Delegate = LuaVector3Delegate