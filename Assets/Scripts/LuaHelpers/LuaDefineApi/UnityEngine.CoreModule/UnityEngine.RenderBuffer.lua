---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RenderBuffer : ValueType
local RenderBuffer={ }
---
---@public
---@return IntPtr 
function RenderBuffer:GetNativeRenderBufferPtr() end
---
UnityEngine.RenderBuffer = RenderBuffer