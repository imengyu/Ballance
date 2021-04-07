---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class HybridV2ShaderReflection
local HybridV2ShaderReflection={ }
---
---@public
---@return number 
function HybridV2ShaderReflection.GetDOTSReflectionVersionNumber() end
---
---@public
---@param shader Shader 
---@return NativeArray`1 
function HybridV2ShaderReflection.GetDOTSInstancingCbuffers(shader) end
---
---@public
---@param shader Shader 
---@return NativeArray`1 
function HybridV2ShaderReflection.GetDOTSInstancingProperties(shader) end
---
Unity.Rendering.HybridV2.HybridV2ShaderReflection = HybridV2ShaderReflection