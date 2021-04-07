---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CustomRenderTexture : RenderTexture
---@field public material Material 
---@field public initializationMaterial Material 
---@field public initializationTexture Texture 
---@field public initializationSource number 
---@field public initializationColor Color 
---@field public updateMode number 
---@field public initializationMode number 
---@field public updateZoneSpace number 
---@field public shaderPass number 
---@field public cubemapFaceMask number 
---@field public doubleBuffered boolean 
---@field public wrapUpdateZones boolean 
---@field public updatePeriod number 
local CustomRenderTexture={ }
---
---@public
---@param count number 
---@return void 
function CustomRenderTexture:Update(count) end
---
---@public
---@return void 
function CustomRenderTexture:Update() end
---
---@public
---@return void 
function CustomRenderTexture:Initialize() end
---
---@public
---@return void 
function CustomRenderTexture:ClearUpdateZones() end
---
---@public
---@param updateZones List`1 
---@return void 
function CustomRenderTexture:GetUpdateZones(updateZones) end
---
---@public
---@return RenderTexture 
function CustomRenderTexture:GetDoubleBufferRenderTexture() end
---
---@public
---@return void 
function CustomRenderTexture:EnsureDoubleBufferConsistency() end
---
---@public
---@param updateZones CustomRenderTextureUpdateZone[] 
---@return void 
function CustomRenderTexture:SetUpdateZones(updateZones) end
---
UnityEngine.CustomRenderTexture = CustomRenderTexture