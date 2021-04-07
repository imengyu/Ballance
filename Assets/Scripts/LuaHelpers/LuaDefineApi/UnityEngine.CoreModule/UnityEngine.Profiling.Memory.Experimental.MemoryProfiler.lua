---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class MemoryProfiler
local MemoryProfiler={ }
---
---@public
---@param value Action`1 
---@return void 
function MemoryProfiler.add_createMetaData(value) end
---
---@public
---@param value Action`1 
---@return void 
function MemoryProfiler.remove_createMetaData(value) end
---
---@public
---@param path string 
---@param finishCallback Action`2 
---@param captureFlags number 
---@return void 
function MemoryProfiler.TakeSnapshot(path, finishCallback, captureFlags) end
---
---@public
---@param path string 
---@param finishCallback Action`2 
---@param screenshotCallback Action`3 
---@param captureFlags number 
---@return void 
function MemoryProfiler.TakeSnapshot(path, finishCallback, screenshotCallback, captureFlags) end
---
---@public
---@param finishCallback Action`2 
---@param captureFlags number 
---@return void 
function MemoryProfiler.TakeTempSnapshot(finishCallback, captureFlags) end
---
UnityEngine.Profiling.Memory.Experimental.MemoryProfiler = MemoryProfiler