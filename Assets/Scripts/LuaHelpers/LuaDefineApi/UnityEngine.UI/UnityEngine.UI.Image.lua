---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Image : MaskableGraphic
---@field public sprite Sprite 
---@field public overrideSprite Sprite 
---@field public type number 
---@field public preserveAspect boolean 
---@field public fillCenter boolean 
---@field public fillMethod number 
---@field public fillAmount number 
---@field public fillClockwise boolean 
---@field public fillOrigin number 
---@field public eventAlphaThreshold number 
---@field public alphaHitTestMinimumThreshold number 
---@field public useSpriteMesh boolean 
---@field public defaultETC1GraphicMaterial Material 
---@field public mainTexture Texture 
---@field public hasBorder boolean 
---@field public pixelsPerUnitMultiplier number 
---@field public pixelsPerUnit number 
---@field public material Material 
---@field public minWidth number 
---@field public preferredWidth number 
---@field public flexibleWidth number 
---@field public minHeight number 
---@field public preferredHeight number 
---@field public flexibleHeight number 
---@field public layoutPriority number 
local Image={ }
---
---@public
---@return void 
function Image:DisableSpriteOptimizations() end
---
---@public
---@return void 
function Image:OnBeforeSerialize() end
---
---@public
---@return void 
function Image:OnAfterDeserialize() end
---
---@public
---@return void 
function Image:SetNativeSize() end
---
---@public
---@return void 
function Image:CalculateLayoutInputHorizontal() end
---
---@public
---@return void 
function Image:CalculateLayoutInputVertical() end
---
---@public
---@param screenPoint Vector2 
---@param eventCamera Camera 
---@return boolean 
function Image:IsRaycastLocationValid(screenPoint, eventCamera) end
---
UnityEngine.UI.Image = Image