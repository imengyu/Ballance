---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LayoutRebuilder
---@field public transform Transform 
local LayoutRebuilder={ }
---
---@public
---@return boolean 
function LayoutRebuilder:IsDestroyed() end
---
---@public
---@param layoutRoot RectTransform 
---@return void 
function LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot) end
---
---@public
---@param executing number 
---@return void 
function LayoutRebuilder:Rebuild(executing) end
---
---@public
---@param rect RectTransform 
---@return void 
function LayoutRebuilder.MarkLayoutForRebuild(rect) end
---
---@public
---@return void 
function LayoutRebuilder:LayoutComplete() end
---
---@public
---@return void 
function LayoutRebuilder:GraphicUpdateComplete() end
---
---@public
---@return number 
function LayoutRebuilder:GetHashCode() end
---
---@public
---@param obj Object 
---@return boolean 
function LayoutRebuilder:Equals(obj) end
---
---@public
---@return string 
function LayoutRebuilder:ToString() end
---
UnityEngine.UI.LayoutRebuilder = LayoutRebuilder