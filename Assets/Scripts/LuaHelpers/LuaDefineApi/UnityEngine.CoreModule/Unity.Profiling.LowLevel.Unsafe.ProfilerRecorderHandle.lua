---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ProfilerRecorderHandle : ValueType
---@field public Valid boolean 
local ProfilerRecorderHandle={ }
---
---@public
---@param handle ProfilerRecorderHandle 
---@return ProfilerRecorderDescription 
function ProfilerRecorderHandle.GetDescription(handle) end
---
---@public
---@param outRecorderHandleList List`1 
---@return void 
function ProfilerRecorderHandle.GetAvailable(outRecorderHandleList) end
---
Unity.Profiling.LowLevel.Unsafe.ProfilerRecorderHandle = ProfilerRecorderHandle