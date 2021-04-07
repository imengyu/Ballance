---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class StaticBatchingUtility
local StaticBatchingUtility={ }
---
---@public
---@param staticBatchRoot GameObject 
---@return void 
function StaticBatchingUtility.Combine(staticBatchRoot) end
---
---@public
---@param gos GameObject[] 
---@param staticBatchRoot GameObject 
---@return void 
function StaticBatchingUtility.Combine(gos, staticBatchRoot) end
---
UnityEngine.StaticBatchingUtility = StaticBatchingUtility