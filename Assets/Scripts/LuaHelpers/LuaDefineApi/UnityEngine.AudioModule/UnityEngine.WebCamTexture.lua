---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class WebCamTexture : Texture
---@field public devices WebCamDevice[] 
---@field public isPlaying boolean 
---@field public deviceName string 
---@field public requestedFPS number 
---@field public requestedWidth number 
---@field public requestedHeight number 
---@field public videoRotationAngle number 
---@field public videoVerticallyMirrored boolean 
---@field public didUpdateThisFrame boolean 
---@field public autoFocusPoint Nullable`1 
---@field public isDepth boolean 
local WebCamTexture={ }
---
---@public
---@return void 
function WebCamTexture:Play() end
---
---@public
---@return void 
function WebCamTexture:Pause() end
---
---@public
---@return void 
function WebCamTexture:Stop() end
---
---@public
---@param x number 
---@param y number 
---@return Color 
function WebCamTexture:GetPixel(x, y) end
---
---@public
---@return Color[] 
function WebCamTexture:GetPixels() end
---
---@public
---@param x number 
---@param y number 
---@param blockWidth number 
---@param blockHeight number 
---@return Color[] 
function WebCamTexture:GetPixels(x, y, blockWidth, blockHeight) end
---
---@public
---@return Color32[] 
function WebCamTexture:GetPixels32() end
---
---@public
---@param colors Color32[] 
---@return Color32[] 
function WebCamTexture:GetPixels32(colors) end
---
UnityEngine.WebCamTexture = WebCamTexture