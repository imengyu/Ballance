---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CubemapArray : Texture
---@field public cubemapCount number 
---@field public format number 
---@field public isReadable boolean 
local CubemapArray={ }
---
---@public
---@param face number 
---@param arrayElement number 
---@param miplevel number 
---@return Color[] 
function CubemapArray:GetPixels(face, arrayElement, miplevel) end
---
---@public
---@param face number 
---@param arrayElement number 
---@return Color[] 
function CubemapArray:GetPixels(face, arrayElement) end
---
---@public
---@param face number 
---@param arrayElement number 
---@param miplevel number 
---@return Color32[] 
function CubemapArray:GetPixels32(face, arrayElement, miplevel) end
---
---@public
---@param face number 
---@param arrayElement number 
---@return Color32[] 
function CubemapArray:GetPixels32(face, arrayElement) end
---
---@public
---@param colors Color[] 
---@param face number 
---@param arrayElement number 
---@param miplevel number 
---@return void 
function CubemapArray:SetPixels(colors, face, arrayElement, miplevel) end
---
---@public
---@param colors Color[] 
---@param face number 
---@param arrayElement number 
---@return void 
function CubemapArray:SetPixels(colors, face, arrayElement) end
---
---@public
---@param colors Color32[] 
---@param face number 
---@param arrayElement number 
---@param miplevel number 
---@return void 
function CubemapArray:SetPixels32(colors, face, arrayElement, miplevel) end
---
---@public
---@param colors Color32[] 
---@param face number 
---@param arrayElement number 
---@return void 
function CubemapArray:SetPixels32(colors, face, arrayElement) end
---
---@public
---@param updateMipmaps boolean 
---@param makeNoLongerReadable boolean 
---@return void 
function CubemapArray:Apply(updateMipmaps, makeNoLongerReadable) end
---
---@public
---@param updateMipmaps boolean 
---@return void 
function CubemapArray:Apply(updateMipmaps) end
---
---@public
---@return void 
function CubemapArray:Apply() end
---
UnityEngine.CubemapArray = CubemapArray