---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Recorder
---@field public isValid boolean 
---@field public enabled boolean 
---@field public elapsedNanoseconds number 
---@field public gpuElapsedNanoseconds number 
---@field public sampleBlockCount number 
---@field public gpuSampleBlockCount number 
local Recorder={ }
---
---@public
---@param samplerName string 
---@return Recorder 
function Recorder.Get(samplerName) end
---
---@public
---@return void 
function Recorder:FilterToCurrentThread() end
---
---@public
---@return void 
function Recorder:CollectFromAllThreads() end
---
UnityEngine.Profiling.Recorder = Recorder