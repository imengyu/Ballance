---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CoveredMethodStats : ValueType
---@field public method MethodBase 
---@field public totalSequencePoints number 
---@field public uncoveredSequencePoints number 
local CoveredMethodStats={ }
---
---@public
---@return string 
function CoveredMethodStats:ToString() end
---
UnityEngine.TestTools.CoveredMethodStats = CoveredMethodStats