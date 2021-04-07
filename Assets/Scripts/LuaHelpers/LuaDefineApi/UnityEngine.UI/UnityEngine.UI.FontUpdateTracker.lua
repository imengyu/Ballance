---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class FontUpdateTracker
local FontUpdateTracker={ }
---
---@public
---@param t Text 
---@return void 
function FontUpdateTracker.TrackText(t) end
---
---@public
---@param t Text 
---@return void 
function FontUpdateTracker.UntrackText(t) end
---
UnityEngine.UI.FontUpdateTracker = FontUpdateTracker