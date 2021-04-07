---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ProfilerMarker : ValueType
---@field public Handle IntPtr 
local ProfilerMarker={ }
---
---@public
---@return void 
function ProfilerMarker:Begin() end
---
---@public
---@param contextUnityObject Object 
---@return void 
function ProfilerMarker:Begin(contextUnityObject) end
---
---@public
---@return void 
function ProfilerMarker:End() end
---
---@public
---@return AutoScope 
function ProfilerMarker:Auto() end
---
Unity.Profiling.ProfilerMarker = ProfilerMarker