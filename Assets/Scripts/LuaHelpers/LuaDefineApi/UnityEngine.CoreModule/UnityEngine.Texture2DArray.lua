---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Texture2DArray : Texture
---@field public allSlices number 
---@field public depth number 
---@field public format number 
---@field public isReadable boolean 
local Texture2DArray={ }
---
---@public
---@param arrayElement number 
---@param miplevel number 
---@return Color[] 
function Texture2DArray:GetPixels(arrayElement, miplevel) end
---
---@public
---@param arrayElement number 
---@return Color[] 
function Texture2DArray:GetPixels(arrayElement) end
---
---@public
---@param arrayElement number 
---@param miplevel number 
---@return Color32[] 
function Texture2DArray:GetPixels32(arrayElement, miplevel) end
---
---@public
---@param arrayElement number 
---@return Color32[] 
function Texture2DArray:GetPixels32(arrayElement) end
---
---@public
---@param colors Color[] 
---@param arrayElement number 
---@param miplevel number 
---@return void 
function Texture2DArray:SetPixels(colors, arrayElement, miplevel) end
---
---@public
---@param colors Color[] 
---@param arrayElement number 
---@return void 
function Texture2DArray:SetPixels(colors, arrayElement) end
---
---@public
---@param colors Color32[] 
---@param arrayElement number 
---@param miplevel number 
---@return void 
function Texture2DArray:SetPixels32(colors, arrayElement, miplevel) end
---
---@public
---@param colors Color32[] 
---@param arrayElement number 
---@return void 
function Texture2DArray:SetPixels32(colors, arrayElement) end
---
---@public
---@param updateMipmaps boolean 
---@param makeNoLongerReadable boolean 
---@return void 
function Texture2DArray:Apply(updateMipmaps, makeNoLongerReadable) end
---
---@public
---@param updateMipmaps boolean 
---@return void 
function Texture2DArray:Apply(updateMipmaps) end
---
---@public
---@return void 
function Texture2DArray:Apply() end
---
UnityEngine.Texture2DArray = Texture2DArray