---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Texture2D : Texture
---@field public format number 
---@field public whiteTexture Texture2D 
---@field public blackTexture Texture2D 
---@field public redTexture Texture2D 
---@field public grayTexture Texture2D 
---@field public linearGrayTexture Texture2D 
---@field public normalTexture Texture2D 
---@field public isReadable boolean 
---@field public vtOnly boolean 
---@field public streamingMipmaps boolean 
---@field public streamingMipmapsPriority number 
---@field public requestedMipmapLevel number 
---@field public minimumMipmapLevel number 
---@field public calculatedMipmapLevel number 
---@field public desiredMipmapLevel number 
---@field public loadingMipmapLevel number 
---@field public loadedMipmapLevel number 
---@field public alphaIsTransparency boolean 
local Texture2D={ }
---
---@public
---@param highQuality boolean 
---@return void 
function Texture2D:Compress(highQuality) end
---
---@public
---@return void 
function Texture2D:ClearRequestedMipmapLevel() end
---
---@public
---@return boolean 
function Texture2D:IsRequestedMipmapLevelLoaded() end
---
---@public
---@return void 
function Texture2D:ClearMinimumMipmapLevel() end
---
---@public
---@param nativeTex IntPtr 
---@return void 
function Texture2D:UpdateExternalTexture(nativeTex) end
---
---@public
---@return Byte[] 
function Texture2D:GetRawTextureData() end
---
---@public
---@param x number 
---@param y number 
---@param blockWidth number 
---@param blockHeight number 
---@param miplevel number 
---@return Color[] 
function Texture2D:GetPixels(x, y, blockWidth, blockHeight, miplevel) end
---
---@public
---@param x number 
---@param y number 
---@param blockWidth number 
---@param blockHeight number 
---@return Color[] 
function Texture2D:GetPixels(x, y, blockWidth, blockHeight) end
---
---@public
---@param miplevel number 
---@return Color32[] 
function Texture2D:GetPixels32(miplevel) end
---
---@public
---@return Color32[] 
function Texture2D:GetPixels32() end
---
---@public
---@param textures Texture2D[] 
---@param padding number 
---@param maximumAtlasSize number 
---@param makeNoLongerReadable boolean 
---@return Rect[] 
function Texture2D:PackTextures(textures, padding, maximumAtlasSize, makeNoLongerReadable) end
---
---@public
---@param textures Texture2D[] 
---@param padding number 
---@param maximumAtlasSize number 
---@return Rect[] 
function Texture2D:PackTextures(textures, padding, maximumAtlasSize) end
---
---@public
---@param textures Texture2D[] 
---@param padding number 
---@return Rect[] 
function Texture2D:PackTextures(textures, padding) end
---
---@public
---@param width number 
---@param height number 
---@param format number 
---@param mipChain boolean 
---@param linear boolean 
---@param nativeTex IntPtr 
---@return Texture2D 
function Texture2D.CreateExternalTexture(width, height, format, mipChain, linear, nativeTex) end
---
---@public
---@param x number 
---@param y number 
---@param color Color 
---@return void 
function Texture2D:SetPixel(x, y, color) end
---
---@public
---@param x number 
---@param y number 
---@param color Color 
---@param mipLevel number 
---@return void 
function Texture2D:SetPixel(x, y, color, mipLevel) end
---
---@public
---@param x number 
---@param y number 
---@param blockWidth number 
---@param blockHeight number 
---@param colors Color[] 
---@param miplevel number 
---@return void 
function Texture2D:SetPixels(x, y, blockWidth, blockHeight, colors, miplevel) end
---
---@public
---@param x number 
---@param y number 
---@param blockWidth number 
---@param blockHeight number 
---@param colors Color[] 
---@return void 
function Texture2D:SetPixels(x, y, blockWidth, blockHeight, colors) end
---
---@public
---@param colors Color[] 
---@param miplevel number 
---@return void 
function Texture2D:SetPixels(colors, miplevel) end
---
---@public
---@param colors Color[] 
---@return void 
function Texture2D:SetPixels(colors) end
---
---@public
---@param x number 
---@param y number 
---@return Color 
function Texture2D:GetPixel(x, y) end
---
---@public
---@param x number 
---@param y number 
---@param mipLevel number 
---@return Color 
function Texture2D:GetPixel(x, y, mipLevel) end
---
---@public
---@param u number 
---@param v number 
---@return Color 
function Texture2D:GetPixelBilinear(u, v) end
---
---@public
---@param u number 
---@param v number 
---@param mipLevel number 
---@return Color 
function Texture2D:GetPixelBilinear(u, v, mipLevel) end
---
---@public
---@param data IntPtr 
---@param size number 
---@return void 
function Texture2D:LoadRawTextureData(data, size) end
---
---@public
---@param data Byte[] 
---@return void 
function Texture2D:LoadRawTextureData(data) end
---
---@public
---@param updateMipmaps boolean 
---@param makeNoLongerReadable boolean 
---@return void 
function Texture2D:Apply(updateMipmaps, makeNoLongerReadable) end
---
---@public
---@param updateMipmaps boolean 
---@return void 
function Texture2D:Apply(updateMipmaps) end
---
---@public
---@return void 
function Texture2D:Apply() end
---
---@public
---@param width number 
---@param height number 
---@return boolean 
function Texture2D:Resize(width, height) end
---
---@public
---@param width number 
---@param height number 
---@param format number 
---@param hasMipMap boolean 
---@return boolean 
function Texture2D:Resize(width, height, format, hasMipMap) end
---
---@public
---@param width number 
---@param height number 
---@param format number 
---@param hasMipMap boolean 
---@return boolean 
function Texture2D:Resize(width, height, format, hasMipMap) end
---
---@public
---@param source Rect 
---@param destX number 
---@param destY number 
---@param recalculateMipMaps boolean 
---@return void 
function Texture2D:ReadPixels(source, destX, destY, recalculateMipMaps) end
---
---@public
---@param source Rect 
---@param destX number 
---@param destY number 
---@return void 
function Texture2D:ReadPixels(source, destX, destY) end
---
---@public
---@param sizes Vector2[] 
---@param padding number 
---@param atlasSize number 
---@param results List`1 
---@return boolean 
function Texture2D.GenerateAtlas(sizes, padding, atlasSize, results) end
---
---@public
---@param colors Color32[] 
---@param miplevel number 
---@return void 
function Texture2D:SetPixels32(colors, miplevel) end
---
---@public
---@param colors Color32[] 
---@return void 
function Texture2D:SetPixels32(colors) end
---
---@public
---@param x number 
---@param y number 
---@param blockWidth number 
---@param blockHeight number 
---@param colors Color32[] 
---@param miplevel number 
---@return void 
function Texture2D:SetPixels32(x, y, blockWidth, blockHeight, colors, miplevel) end
---
---@public
---@param x number 
---@param y number 
---@param blockWidth number 
---@param blockHeight number 
---@param colors Color32[] 
---@return void 
function Texture2D:SetPixels32(x, y, blockWidth, blockHeight, colors) end
---
---@public
---@param miplevel number 
---@return Color[] 
function Texture2D:GetPixels(miplevel) end
---
---@public
---@return Color[] 
function Texture2D:GetPixels() end
---
UnityEngine.Texture2D = Texture2D