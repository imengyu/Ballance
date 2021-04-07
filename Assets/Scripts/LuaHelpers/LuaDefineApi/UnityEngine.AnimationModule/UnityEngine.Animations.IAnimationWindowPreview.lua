---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IAnimationWindowPreview
local IAnimationWindowPreview={ }
---
---@public
---@return void 
function IAnimationWindowPreview:StartPreview() end
---
---@public
---@return void 
function IAnimationWindowPreview:StopPreview() end
---
---@public
---@param graph PlayableGraph 
---@return void 
function IAnimationWindowPreview:UpdatePreviewGraph(graph) end
---
---@public
---@param graph PlayableGraph 
---@param inputPlayable Playable 
---@return Playable 
function IAnimationWindowPreview:BuildPreviewGraph(graph, inputPlayable) end
---
UnityEngine.Animations.IAnimationWindowPreview = IAnimationWindowPreview