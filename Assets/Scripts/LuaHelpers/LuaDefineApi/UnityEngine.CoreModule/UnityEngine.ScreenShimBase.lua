---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ScreenShimBase
---@field public width number 
---@field public height number 
---@field public dpi number 
---@field public currentResolution Resolution 
---@field public resolutions Resolution[] 
---@field public fullScreen boolean 
---@field public fullScreenMode number 
---@field public safeArea Rect 
---@field public cutouts Rect[] 
---@field public autorotateToPortrait boolean 
---@field public autorotateToPortraitUpsideDown boolean 
---@field public autorotateToLandscapeLeft boolean 
---@field public autorotateToLandscapeRight boolean 
---@field public orientation number 
---@field public sleepTimeout number 
---@field public brightness number 
local ScreenShimBase={ }
---
---@public
---@return void 
function ScreenShimBase:Dispose() end
---
---@public
---@return boolean 
function ScreenShimBase:IsActive() end
---
---@public
---@param width number 
---@param height number 
---@param fullscreenMode number 
---@param preferredRefreshRate number 
---@return void 
function ScreenShimBase:SetResolution(width, height, fullscreenMode, preferredRefreshRate) end
---
UnityEngine.ScreenShimBase = ScreenShimBase