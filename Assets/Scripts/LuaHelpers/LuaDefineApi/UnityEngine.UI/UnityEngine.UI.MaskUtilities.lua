---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class MaskUtilities
local MaskUtilities={ }
---
---@public
---@param mask Component 
---@return void 
function MaskUtilities.Notify2DMaskStateChanged(mask) end
---
---@public
---@param mask Component 
---@return void 
function MaskUtilities.NotifyStencilStateChanged(mask) end
---
---@public
---@param start Transform 
---@return Transform 
function MaskUtilities.FindRootSortOverrideCanvas(start) end
---
---@public
---@param transform Transform 
---@param stopAfter Transform 
---@return number 
function MaskUtilities.GetStencilDepth(transform, stopAfter) end
---
---@public
---@param father Transform 
---@param child Transform 
---@return boolean 
function MaskUtilities.IsDescendantOrSelf(father, child) end
---
---@public
---@param clippable IClippable 
---@return RectMask2D 
function MaskUtilities.GetRectMaskForClippable(clippable) end
---
---@public
---@param clipper RectMask2D 
---@param masks List`1 
---@return void 
function MaskUtilities.GetRectMasksForClip(clipper, masks) end
---
UnityEngine.UI.MaskUtilities = MaskUtilities