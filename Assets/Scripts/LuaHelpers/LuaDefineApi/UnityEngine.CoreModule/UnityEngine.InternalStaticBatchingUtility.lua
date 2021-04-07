---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class InternalStaticBatchingUtility
local InternalStaticBatchingUtility={ }
---
---@public
---@param staticBatchRoot GameObject 
---@param sorter StaticBatcherGOSorter 
---@return void 
function InternalStaticBatchingUtility.CombineRoot(staticBatchRoot, sorter) end
---
---@public
---@param staticBatchRoot GameObject 
---@param combineOnlyStatic boolean 
---@param isEditorPostprocessScene boolean 
---@param sorter StaticBatcherGOSorter 
---@return void 
function InternalStaticBatchingUtility.Combine(staticBatchRoot, combineOnlyStatic, isEditorPostprocessScene, sorter) end
---
---@public
---@param gos GameObject[] 
---@param sorter StaticBatcherGOSorter 
---@return GameObject[] 
function InternalStaticBatchingUtility.SortGameObjectsForStaticbatching(gos, sorter) end
---
---@public
---@param gos GameObject[] 
---@param staticBatchRoot GameObject 
---@param isEditorPostprocessScene boolean 
---@param sorter StaticBatcherGOSorter 
---@return void 
function InternalStaticBatchingUtility.CombineGameObjects(gos, staticBatchRoot, isEditorPostprocessScene, sorter) end
---
UnityEngine.InternalStaticBatchingUtility = InternalStaticBatchingUtility