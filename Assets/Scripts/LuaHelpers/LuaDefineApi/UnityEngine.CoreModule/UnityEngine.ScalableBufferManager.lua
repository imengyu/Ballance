---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ScalableBufferManager
---@field public widthScaleFactor number 
---@field public heightScaleFactor number 
local ScalableBufferManager={ }
---
---@public
---@param widthScale number 
---@param heightScale number 
---@return void 
function ScalableBufferManager.ResizeBuffers(widthScale, heightScale) end
---
UnityEngine.ScalableBufferManager = ScalableBufferManager