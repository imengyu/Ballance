---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RectTransform : Transform
---@field public rect Rect 
---@field public anchorMin Vector2 
---@field public anchorMax Vector2 
---@field public anchoredPosition Vector2 
---@field public sizeDelta Vector2 
---@field public pivot Vector2 
---@field public anchoredPosition3D Vector3 
---@field public offsetMin Vector2 
---@field public offsetMax Vector2 
local RectTransform={ }
---
---@public
---@param value ReapplyDrivenProperties 
---@return void 
function RectTransform.add_reapplyDrivenProperties(value) end
---
---@public
---@param value ReapplyDrivenProperties 
---@return void 
function RectTransform.remove_reapplyDrivenProperties(value) end
---
---@public
---@return void 
function RectTransform:ForceUpdateRectTransforms() end
---
---@public
---@param fourCornersArray Vector3[] 
---@return void 
function RectTransform:GetLocalCorners(fourCornersArray) end
---
---@public
---@param fourCornersArray Vector3[] 
---@return void 
function RectTransform:GetWorldCorners(fourCornersArray) end
---
---@public
---@param edge number 
---@param inset number 
---@param size number 
---@return void 
function RectTransform:SetInsetAndSizeFromParentEdge(edge, inset, size) end
---
---@public
---@param axis number 
---@param size number 
---@return void 
function RectTransform:SetSizeWithCurrentAnchors(axis, size) end
---
UnityEngine.RectTransform = RectTransform