---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Cubemap : Texture
---@field public format number 
---@field public isReadable boolean 
---@field public streamingMipmaps boolean 
---@field public streamingMipmapsPriority number 
---@field public requestedMipmapLevel number 
---@field public desiredMipmapLevel number 
---@field public loadingMipmapLevel number 
---@field public loadedMipmapLevel number 
local Cubemap={ }
---
---@public
---@param nativeTexture IntPtr 
---@return void 
function Cubemap:UpdateExternalTexture(nativeTexture) end
---
---@public
---@param smoothRegionWidthInPixels number 
---@return void 
function Cubemap:SmoothEdges(smoothRegionWidthInPixels) end
---
---@public
---@return void 
function Cubemap:SmoothEdges() end
---
---@public
---@param face number 
---@param miplevel number 
---@return Color[] 
function Cubemap:GetPixels(face, miplevel) end
---
---@public
---@param face number 
---@return Color[] 
function Cubemap:GetPixels(face) end
---
---@public
---@param colors Color[] 
---@param face number 
---@param miplevel number 
---@return void 
function Cubemap:SetPixels(colors, face, miplevel) end
---
---@public
---@param colors Color[] 
---@param face number 
---@return void 
function Cubemap:SetPixels(colors, face) end
---
---@public
---@return void 
function Cubemap:ClearRequestedMipmapLevel() end
---
---@public
---@return boolean 
function Cubemap:IsRequestedMipmapLevelLoaded() end
---
---@public
---@param width number 
---@param format number 
---@param mipmap boolean 
---@param nativeTex IntPtr 
---@return Cubemap 
function Cubemap.CreateExternalTexture(width, format, mipmap, nativeTex) end
---
---@public
---@param face number 
---@param x number 
---@param y number 
---@param color Color 
---@return void 
function Cubemap:SetPixel(face, x, y, color) end
---
---@public
---@param face number 
---@param x number 
---@param y number 
---@return Color 
function Cubemap:GetPixel(face, x, y) end
---
---@public
---@param updateMipmaps boolean 
---@param makeNoLongerReadable boolean 
---@return void 
function Cubemap:Apply(updateMipmaps, makeNoLongerReadable) end
---
---@public
---@param updateMipmaps boolean 
---@return void 
function Cubemap:Apply(updateMipmaps) end
---
---@public
---@return void 
function Cubemap:Apply() end
---
UnityEngine.Cubemap = Cubemap