---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Mask : UIBehaviour
---@field public rectTransform RectTransform 
---@field public showMaskGraphic boolean 
---@field public graphic Graphic 
local Mask={ }
---
---@public
---@return boolean 
function Mask:MaskEnabled() end
---
---@public
---@return void 
function Mask:OnSiblingGraphicEnabledDisabled() end
---
---@public
---@param sp Vector2 
---@param eventCamera Camera 
---@return boolean 
function Mask:IsRaycastLocationValid(sp, eventCamera) end
---
---@public
---@param baseMaterial Material 
---@return Material 
function Mask:GetModifiedMaterial(baseMaterial) end
---
UnityEngine.UI.Mask = Mask