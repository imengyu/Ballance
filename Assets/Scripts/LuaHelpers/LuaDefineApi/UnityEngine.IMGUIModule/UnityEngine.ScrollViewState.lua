---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ScrollViewState
---@field public position Rect 
---@field public visibleRect Rect 
---@field public viewRect Rect 
---@field public scrollPosition Vector2 
---@field public apply boolean 
---@field public isDuringTouchScroll boolean 
---@field public touchScrollStartMousePosition Vector2 
---@field public touchScrollStartPosition Vector2 
---@field public velocity Vector2 
---@field public previousTimeSinceStartup number 
local ScrollViewState={ }
---
---@public
---@param pos Rect 
---@return void 
function ScrollViewState:ScrollTo(pos) end
---
---@public
---@param pos Rect 
---@param maxDelta number 
---@return boolean 
function ScrollViewState:ScrollTowards(pos, maxDelta) end
---
UnityEngine.ScrollViewState = ScrollViewState