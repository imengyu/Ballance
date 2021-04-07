---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Texture3D : Texture
---@field public depth number 
---@field public format number 
---@field public isReadable boolean 
local Texture3D={ }
---
---@public
---@param nativeTex IntPtr 
---@return void 
function Texture3D:UpdateExternalTexture(nativeTex) end
---
---@public
---@param miplevel number 
---@return Color[] 
function Texture3D:GetPixels(miplevel) end
---
---@public
---@return Color[] 
function Texture3D:GetPixels() end
---
---@public
---@param miplevel number 
---@return Color32[] 
function Texture3D:GetPixels32(miplevel) end
---
---@public
---@return Color32[] 
function Texture3D:GetPixels32() end
---
---@public
---@param colors Color[] 
---@param miplevel number 
---@return void 
function Texture3D:SetPixels(colors, miplevel) end
---
---@public
---@param colors Color[] 
---@return void 
function Texture3D:SetPixels(colors) end
---
---@public
---@param colors Color32[] 
---@param miplevel number 
---@return void 
function Texture3D:SetPixels32(colors, miplevel) end
---
---@public
---@param colors Color32[] 
---@return void 
function Texture3D:SetPixels32(colors) end
---
---@public
---@param width number 
---@param height number 
---@param depth number 
---@param format number 
---@param mipChain boolean 
---@param nativeTex IntPtr 
---@return Texture3D 
function Texture3D.CreateExternalTexture(width, height, depth, format, mipChain, nativeTex) end
---
---@public
---@param updateMipmaps boolean 
---@param makeNoLongerReadable boolean 
---@return void 
function Texture3D:Apply(updateMipmaps, makeNoLongerReadable) end
---
---@public
---@param updateMipmaps boolean 
---@return void 
function Texture3D:Apply(updateMipmaps) end
---
---@public
---@return void 
function Texture3D:Apply() end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@param color Color 
---@return void 
function Texture3D:SetPixel(x, y, z, color) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@param color Color 
---@param mipLevel number 
---@return void 
function Texture3D:SetPixel(x, y, z, color, mipLevel) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@return Color 
function Texture3D:GetPixel(x, y, z) end
---
---@public
---@param x number 
---@param y number 
---@param z number 
---@param mipLevel number 
---@return Color 
function Texture3D:GetPixel(x, y, z, mipLevel) end
---
---@public
---@param u number 
---@param v number 
---@param w number 
---@return Color 
function Texture3D:GetPixelBilinear(u, v, w) end
---
---@public
---@param u number 
---@param v number 
---@param w number 
---@param mipLevel number 
---@return Color 
function Texture3D:GetPixelBilinear(u, v, w, mipLevel) end
---
UnityEngine.Texture3D = Texture3D