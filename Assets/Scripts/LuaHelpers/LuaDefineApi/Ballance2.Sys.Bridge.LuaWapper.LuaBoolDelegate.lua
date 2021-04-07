---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaBoolDelegate
local LuaBoolDelegate={ }
---
---@public
---@param self LuaTable 
---@param b boolean 
---@return void 
function LuaBoolDelegate:Invoke(self, b) end
---
---@public
---@param self LuaTable 
---@param b boolean 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaBoolDelegate:BeginInvoke(self, b, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaBoolDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaBoolDelegate = LuaBoolDelegate