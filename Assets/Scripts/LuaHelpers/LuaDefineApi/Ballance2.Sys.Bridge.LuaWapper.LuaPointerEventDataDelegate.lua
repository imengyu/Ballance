---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LuaPointerEventDataDelegate
local LuaPointerEventDataDelegate={ }
---
---@public
---@param self LuaTable 
---@param pointerEventData PointerEventData 
---@return void 
function LuaPointerEventDataDelegate:Invoke(self, pointerEventData) end
---
---@public
---@param self LuaTable 
---@param pointerEventData PointerEventData 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function LuaPointerEventDataDelegate:BeginInvoke(self, pointerEventData, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function LuaPointerEventDataDelegate:EndInvoke(result) end
---
Ballance2.Sys.Bridge.LuaWapper.LuaPointerEventDataDelegate = LuaPointerEventDataDelegate