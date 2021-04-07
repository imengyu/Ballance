---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ProfilerRecorder : ValueType
---@field public Valid boolean 
---@field public DataType number 
---@field public UnitType number 
---@field public CurrentValue number 
---@field public CurrentValueAsDouble number 
---@field public LastValue number 
---@field public LastValueAsDouble number 
---@field public Capacity number 
---@field public Count number 
---@field public IsRunning boolean 
---@field public WrappedAround boolean 
local ProfilerRecorder={ }
---
---@public
---@param category ProfilerCategory 
---@param statName string 
---@param capacity number 
---@param options number 
---@return ProfilerRecorder 
function ProfilerRecorder.StartNew(category, statName, capacity, options) end
---
---@public
---@param marker ProfilerMarker 
---@param capacity number 
---@param options number 
---@return ProfilerRecorder 
function ProfilerRecorder.StartNew(marker, capacity, options) end
---
---@public
---@return void 
function ProfilerRecorder:Start() end
---
---@public
---@return void 
function ProfilerRecorder:Stop() end
---
---@public
---@return void 
function ProfilerRecorder:Reset() end
---
---@public
---@param index number 
---@return ProfilerRecorderSample 
function ProfilerRecorder:GetSample(index) end
---
---@public
---@param outSamples List`1 
---@param reset boolean 
---@return void 
function ProfilerRecorder:CopyTo(outSamples, reset) end
---
---@public
---@param dest ProfilerRecorderSample* 
---@param destSize number 
---@param reset boolean 
---@return number 
function ProfilerRecorder:CopyTo(dest, destSize, reset) end
---
---@public
---@return ProfilerRecorderSample[] 
function ProfilerRecorder:ToArray() end
---
---@public
---@return void 
function ProfilerRecorder:Dispose() end
---
Unity.Profiling.ProfilerRecorder = ProfilerRecorder