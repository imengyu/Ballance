---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Coverage
---@field public enabled boolean 
local Coverage={ }
---
---@public
---@param method MethodBase 
---@return CoveredSequencePoint[] 
function Coverage.GetSequencePointsFor(method) end
---
---@public
---@param method MethodBase 
---@return CoveredMethodStats 
function Coverage.GetStatsFor(method) end
---
---@public
---@param methods MethodBase[] 
---@return CoveredMethodStats[] 
function Coverage.GetStatsFor(methods) end
---
---@public
---@param type Type 
---@return CoveredMethodStats[] 
function Coverage.GetStatsFor(type) end
---
---@public
---@return CoveredMethodStats[] 
function Coverage.GetStatsForAllCoveredMethods() end
---
---@public
---@param method MethodBase 
---@return void 
function Coverage.ResetFor(method) end
---
---@public
---@return void 
function Coverage.ResetAll() end
---
UnityEngine.TestTools.Coverage = Coverage