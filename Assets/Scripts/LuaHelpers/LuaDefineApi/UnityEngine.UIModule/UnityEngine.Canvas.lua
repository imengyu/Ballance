---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Canvas : Behaviour
---@field public renderMode number 
---@field public isRootCanvas boolean 
---@field public pixelRect Rect 
---@field public scaleFactor number 
---@field public referencePixelsPerUnit number 
---@field public overridePixelPerfect boolean 
---@field public pixelPerfect boolean 
---@field public planeDistance number 
---@field public renderOrder number 
---@field public overrideSorting boolean 
---@field public sortingOrder number 
---@field public targetDisplay number 
---@field public sortingLayerID number 
---@field public cachedSortingLayerValue number 
---@field public additionalShaderChannels number 
---@field public sortingLayerName string 
---@field public rootCanvas Canvas 
---@field public worldCamera Camera 
---@field public normalizedSortingGridSize number 
---@field public sortingGridNormalizedSize number 
local Canvas={ }
---
---@public
---@param value WillRenderCanvases 
---@return void 
function Canvas.add_willRenderCanvases(value) end
---
---@public
---@param value WillRenderCanvases 
---@return void 
function Canvas.remove_willRenderCanvases(value) end
---
---@public
---@return Material 
function Canvas.GetDefaultCanvasTextMaterial() end
---
---@public
---@return Material 
function Canvas.GetDefaultCanvasMaterial() end
---
---@public
---@return Material 
function Canvas.GetETC1SupportedCanvasMaterial() end
---
---@public
---@return void 
function Canvas.ForceUpdateCanvases() end
---
UnityEngine.Canvas = Canvas