---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IClippable
---@field public gameObject GameObject 
---@field public rectTransform RectTransform 
local IClippable={ }
---
---@public
---@return void 
function IClippable:RecalculateClipping() end
---
---@public
---@param clipRect Rect 
---@param validRect boolean 
---@return void 
function IClippable:Cull(clipRect, validRect) end
---
---@public
---@param value Rect 
---@param validRect boolean 
---@return void 
function IClippable:SetClipRect(value, validRect) end
---
---@public
---@param clipSoftness Vector2 
---@return void 
function IClippable:SetClipSoftness(clipSoftness) end
---
UnityEngine.UI.IClippable = IClippable