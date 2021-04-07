---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GarbageCollector
---@field public GCMode number 
---@field public isIncremental boolean 
---@field public incrementalTimeSliceNanoseconds number 
local GarbageCollector={ }
---
---@public
---@param value Action`1 
---@return void 
function GarbageCollector.add_GCModeChanged(value) end
---
---@public
---@param value Action`1 
---@return void 
function GarbageCollector.remove_GCModeChanged(value) end
---
---@public
---@param nanoseconds number 
---@return boolean 
function GarbageCollector.CollectIncremental(nanoseconds) end
---
UnityEngine.Scripting.GarbageCollector = GarbageCollector