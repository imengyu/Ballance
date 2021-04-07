---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaReturnBoolDelegate
local LuaReturnBoolDelegate={ }
---
---@public
---@param self LuaTable 
---@return boolean 
function LuaReturnBoolDelegate:Invoke(self) end
---
---@public
---@param self LuaTable 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaReturnBoolDelegate:BeginInvoke(self, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return boolean 
function LuaReturnBoolDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaReturnBoolDelegate = LuaReturnBoolDelegate