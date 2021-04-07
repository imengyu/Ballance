---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class EditorScreen
---@field public width number 
---@field public height number 
---@field public dpi number 
---@field public orientation number 
---@field public sleepTimeout number 
---@field public autorotateToPortrait boolean 
---@field public autorotateToPortraitUpsideDown boolean 
---@field public autorotateToLandscapeLeft boolean 
---@field public autorotateToLandscapeRight boolean 
---@field public currentResolution Resolution 
---@field public fullScreen boolean 
---@field public fullScreenMode number 
---@field public safeArea Rect 
---@field public cutouts Rect[] 
---@field public resolutions Resolution[] 
---@field public brightness number 
local EditorScreen={ }
---
---@public
---@param width number 
---@param height number 
---@param fullscreenMode number 
---@param preferredRefreshRate number 
---@return void 
function EditorScreen.SetResolution(width, height, fullscreenMode, preferredRefreshRate) end
---
---@public
---@param width number 
---@param height number 
---@param fullscreenMode number 
---@return void 
function EditorScreen.SetResolution(width, height, fullscreenMode) end
---
---@public
---@param width number 
---@param height number 
---@param fullscreen boolean 
---@param preferredRefreshRate number 
---@return void 
function EditorScreen.SetResolution(width, height, fullscreen, preferredRefreshRate) end
---
---@public
---@param width number 
---@param height number 
---@param fullscreen boolean 
---@return void 
function EditorScreen.SetResolution(width, height, fullscreen) end
---
UnityEngine.EditorScreen = EditorScreen