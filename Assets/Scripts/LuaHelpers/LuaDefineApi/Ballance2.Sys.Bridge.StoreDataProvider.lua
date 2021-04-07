---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class StoreDataProvider
local StoreDataProvider={ }
---
---@public
---@return Object 
function StoreDataProvider:Invoke() end
---
---@public
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function StoreDataProvider:BeginInvoke(callback, object) end
---
---@public
---@param result IAsyncResult 
---@return Object 
function StoreDataProvider:EndInvoke(result) end
---用于自己提供数据的回调
Ballance2.Sys.Bridge.StoreDataProvider = StoreDataProvider