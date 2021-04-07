---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class StoreOnDataChanged
local StoreOnDataChanged={ }
---
---@public
---@param data StoreData 
---@param oldV Object 
---@param newV Object 
---@return void 
function StoreOnDataChanged:Invoke(data, oldV, newV) end
---
---@public
---@param data StoreData 
---@param oldV Object 
---@param newV Object 
---@param callback AsyncCallback 
---@param object Object 
---@return IAsyncResult 
function StoreOnDataChanged:BeginInvoke(data, oldV, newV, callback, object) end
---
---@public
---@param result IAsyncResult 
---@return void 
function StoreOnDataChanged:EndInvoke(result) end
---数据改变回调
Ballance2.Sys.Bridge.StoreOnDataChanged = StoreOnDataChanged