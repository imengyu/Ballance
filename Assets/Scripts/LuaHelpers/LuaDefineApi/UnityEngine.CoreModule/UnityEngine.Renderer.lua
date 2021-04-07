---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Renderer : Component
---@field public lightmapTilingOffset Vector4 
---@field public lightProbeAnchor Transform 
---@field public castShadows boolean 
---@field public motionVectors boolean 
---@field public useLightProbes boolean 
---@field public bounds Bounds 
---@field public enabled boolean 
---@field public isVisible boolean 
---@field public shadowCastingMode number 
---@field public receiveShadows boolean 
---@field public forceRenderingOff boolean 
---@field public motionVectorGenerationMode number 
---@field public lightProbeUsage number 
---@field public reflectionProbeUsage number 
---@field public renderingLayerMask number 
---@field public rendererPriority number 
---@field public rayTracingMode number 
---@field public sortingLayerName string 
---@field public sortingLayerID number 
---@field public sortingOrder number 
---@field public allowOcclusionWhenDynamic boolean 
---@field public isPartOfStaticBatch boolean 
---@field public worldToLocalMatrix Matrix4x4 
---@field public localToWorldMatrix Matrix4x4 
---@field public lightProbeProxyVolumeOverride GameObject 
---@field public probeAnchor Transform 
---@field public lightmapIndex number 
---@field public realtimeLightmapIndex number 
---@field public lightmapScaleOffset Vector4 
---@field public realtimeLightmapScaleOffset Vector4 
---@field public materials Material[] 
---@field public material Material 
---@field public sharedMaterial Material 
---@field public sharedMaterials Material[] 
local Renderer={ }
---
---@public
---@return boolean 
function Renderer:HasPropertyBlock() end
---
---@public
---@param properties MaterialPropertyBlock 
---@return void 
function Renderer:SetPropertyBlock(properties) end
---
---@public
---@param properties MaterialPropertyBlock 
---@param materialIndex number 
---@return void 
function Renderer:SetPropertyBlock(properties, materialIndex) end
---
---@public
---@param properties MaterialPropertyBlock 
---@return void 
function Renderer:GetPropertyBlock(properties) end
---
---@public
---@param properties MaterialPropertyBlock 
---@param materialIndex number 
---@return void 
function Renderer:GetPropertyBlock(properties, materialIndex) end
---
---@public
---@param m List`1 
---@return void 
function Renderer:GetMaterials(m) end
---
---@public
---@param m List`1 
---@return void 
function Renderer:GetSharedMaterials(m) end
---
---@public
---@param result List`1 
---@return void 
function Renderer:GetClosestReflectionProbes(result) end
---
UnityEngine.Renderer = Renderer