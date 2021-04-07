---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Screen
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
---@field public GetResolution Resolution[] 
---@field public showCursor boolean 
---@field public lockCursor boolean 
local Screen={ }
---
---@public
---@param width number 
---@param height number 
---@param fullscreenMode number 
---@param preferredRefreshRate number 
---@return void 
function Screen.SetResolution(width, height, fullscreenMode, preferredRefreshRate) end
---
---@public
---@param width number 
---@param height number 
---@param fullscreenMode number 
---@return void 
function Screen.SetResolution(width, height, fullscreenMode) end
---
---@public
---@param width number 
---@param height number 
---@param fullscreen boolean 
---@param preferredRefreshRate number 
---@return void 
function Screen.SetResolution(width, height, fullscreen, preferredRefreshRate) end
---
---@public
---@param width number 
---@param height number 
---@param fullscreen boolean 
---@return void 
function Screen.SetResolution(width, height, fullscreen) end
---
UnityEngine.Screen = Screen