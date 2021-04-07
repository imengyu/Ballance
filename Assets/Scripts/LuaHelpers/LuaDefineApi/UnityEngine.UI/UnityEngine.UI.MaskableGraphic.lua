---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class MaskableGraphic : Graphic
---@field public onCullStateChanged CullStateChangedEvent 
---@field public maskable boolean 
---@field public isMaskingGraphic boolean 
local MaskableGraphic={ }
---
---@public
---@param baseMaterial Material 
---@return Material 
function MaskableGraphic:GetModifiedMaterial(baseMaterial) end
---
---@public
---@param clipRect Rect 
---@param validRect boolean 
---@return void 
function MaskableGraphic:Cull(clipRect, validRect) end
---
---@public
---@param clipRect Rect 
---@param validRect boolean 
---@return void 
function MaskableGraphic:SetClipRect(clipRect, validRect) end
---
---@public
---@param clipSoftness Vector2 
---@return void 
function MaskableGraphic:SetClipSoftness(clipSoftness) end
---
---@public
---@return void 
function MaskableGraphic:ParentMaskStateChanged() end
---
---@public
---@return void 
function MaskableGraphic:RecalculateClipping() end
---
---@public
---@return void 
function MaskableGraphic:RecalculateMasking() end
---
UnityEngine.UI.MaskableGraphic = MaskableGraphic