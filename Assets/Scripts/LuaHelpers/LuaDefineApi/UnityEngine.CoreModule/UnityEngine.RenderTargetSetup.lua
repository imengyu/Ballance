---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RenderTargetSetup : ValueType
---@field public color RenderBuffer[] 
---@field public depth RenderBuffer 
---@field public mipLevel number 
---@field public cubemapFace number 
---@field public depthSlice number 
---@field public colorLoad RenderBufferLoadAction[] 
---@field public colorStore RenderBufferStoreAction[] 
---@field public depthLoad number 
---@field public depthStore number 
local RenderTargetSetup={ }
---
UnityEngine.RenderTargetSetup = RenderTargetSetup