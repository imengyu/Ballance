---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RenderTexture : Texture
---@field public width number 
---@field public height number 
---@field public dimension number 
---@field public graphicsFormat number 
---@field public useMipMap boolean 
---@field public sRGB boolean 
---@field public vrUsage number 
---@field public memorylessMode number 
---@field public format number 
---@field public stencilFormat number 
---@field public autoGenerateMips boolean 
---@field public volumeDepth number 
---@field public antiAliasing number 
---@field public bindTextureMS boolean 
---@field public enableRandomWrite boolean 
---@field public useDynamicScale boolean 
---@field public isPowerOfTwo boolean 
---@field public active RenderTexture 
---@field public colorBuffer RenderBuffer 
---@field public depthBuffer RenderBuffer 
---@field public depth number 
---@field public descriptor RenderTextureDescriptor 
---@field public generateMips boolean 
---@field public isCubemap boolean 
---@field public isVolume boolean 
---@field public enabled boolean 
local RenderTexture={ }
---
---@public
---@return IntPtr 
function RenderTexture:GetNativeDepthBufferPtr() end
---
---@public
---@param discardColor boolean 
---@param discardDepth boolean 
---@return void 
function RenderTexture:DiscardContents(discardColor, discardDepth) end
---
---@public
---@return void 
function RenderTexture:MarkRestoreExpected() end
---
---@public
---@return void 
function RenderTexture:DiscardContents() end
---
---@public
---@return void 
function RenderTexture:ResolveAntiAliasedSurface() end
---
---@public
---@param target RenderTexture 
---@return void 
function RenderTexture:ResolveAntiAliasedSurface(target) end
---
---@public
---@param propertyName string 
---@return void 
function RenderTexture:SetGlobalShaderProperty(propertyName) end
---
---@public
---@return boolean 
function RenderTexture:Create() end
---
---@public
---@return void 
function RenderTexture:Release() end
---
---@public
---@return boolean 
function RenderTexture:IsCreated() end
---
---@public
---@return void 
function RenderTexture:GenerateMips() end
---
---@public
---@param equirect RenderTexture 
---@param eye number 
---@return void 
function RenderTexture:ConvertToEquirect(equirect, eye) end
---
---@public
---@param rt RenderTexture 
---@return boolean 
function RenderTexture.SupportsStencil(rt) end
---
---@public
---@param temp RenderTexture 
---@return void 
function RenderTexture.ReleaseTemporary(temp) end
---
---@public
---@param desc RenderTextureDescriptor 
---@return RenderTexture 
function RenderTexture.GetTemporary(desc) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@param format number 
---@param antiAliasing number 
---@param memorylessMode number 
---@param vrUsage number 
---@param useDynamicScale boolean 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer, format, antiAliasing, memorylessMode, vrUsage, useDynamicScale) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@param format number 
---@param antiAliasing number 
---@param memorylessMode number 
---@param vrUsage number 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer, format, antiAliasing, memorylessMode, vrUsage) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@param format number 
---@param antiAliasing number 
---@param memorylessMode number 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer, format, antiAliasing, memorylessMode) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@param format number 
---@param antiAliasing number 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer, format, antiAliasing) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@param format number 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer, format) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@param format number 
---@param readWrite number 
---@param antiAliasing number 
---@param memorylessMode number 
---@param vrUsage number 
---@param useDynamicScale boolean 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage, useDynamicScale) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@param format number 
---@param readWrite number 
---@param antiAliasing number 
---@param memorylessMode number 
---@param vrUsage number 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@param format number 
---@param readWrite number 
---@param antiAliasing number 
---@param memorylessMode number 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@param format number 
---@param readWrite number 
---@param antiAliasing number 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@param format number 
---@param readWrite number 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@param format number 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer, format) end
---
---@public
---@param width number 
---@param height number 
---@param depthBuffer number 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height, depthBuffer) end
---
---@public
---@param width number 
---@param height number 
---@return RenderTexture 
function RenderTexture.GetTemporary(width, height) end
---
---@public
---@param color Color 
---@return void 
function RenderTexture:SetBorderColor(color) end
---
---@public
---@return Vector2 
function RenderTexture:GetTexelOffset() end
---
UnityEngine.RenderTexture = RenderTexture