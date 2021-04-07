---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Graphic : UIBehaviour
---@field public defaultGraphicMaterial Material 
---@field public color Color 
---@field public raycastTarget boolean 
---@field public raycastPadding Vector4 
---@field public depth number 
---@field public rectTransform RectTransform 
---@field public canvas Canvas 
---@field public canvasRenderer CanvasRenderer 
---@field public defaultMaterial Material 
---@field public material Material 
---@field public materialForRendering Material 
---@field public mainTexture Texture 
local Graphic={ }
---
---@public
---@return void 
function Graphic:SetAllDirty() end
---
---@public
---@return void 
function Graphic:SetLayoutDirty() end
---
---@public
---@return void 
function Graphic:SetVerticesDirty() end
---
---@public
---@return void 
function Graphic:SetMaterialDirty() end
---
---@public
---@return void 
function Graphic:OnCullingChanged() end
---
---@public
---@param update number 
---@return void 
function Graphic:Rebuild(update) end
---
---@public
---@return void 
function Graphic:LayoutComplete() end
---
---@public
---@return void 
function Graphic:GraphicUpdateComplete() end
---
---@public
---@return void 
function Graphic:OnRebuildRequested() end
---
---@public
---@return void 
function Graphic:SetNativeSize() end
---
---@public
---@param sp Vector2 
---@param eventCamera Camera 
---@return boolean 
function Graphic:Raycast(sp, eventCamera) end
---
---@public
---@param point Vector2 
---@return Vector2 
function Graphic:PixelAdjustPoint(point) end
---
---@public
---@return Rect 
function Graphic:GetPixelAdjustedRect() end
---
---@public
---@param targetColor Color 
---@param duration number 
---@param ignoreTimeScale boolean 
---@param useAlpha boolean 
---@return void 
function Graphic:CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha) end
---
---@public
---@param targetColor Color 
---@param duration number 
---@param ignoreTimeScale boolean 
---@param useAlpha boolean 
---@param useRGB boolean 
---@return void 
function Graphic:CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, useRGB) end
---
---@public
---@param alpha number 
---@param duration number 
---@param ignoreTimeScale boolean 
---@return void 
function Graphic:CrossFadeAlpha(alpha, duration, ignoreTimeScale) end
---
---@public
---@param action UnityAction 
---@return void 
function Graphic:RegisterDirtyLayoutCallback(action) end
---
---@public
---@param action UnityAction 
---@return void 
function Graphic:UnregisterDirtyLayoutCallback(action) end
---
---@public
---@param action UnityAction 
---@return void 
function Graphic:RegisterDirtyVerticesCallback(action) end
---
---@public
---@param action UnityAction 
---@return void 
function Graphic:UnregisterDirtyVerticesCallback(action) end
---
---@public
---@param action UnityAction 
---@return void 
function Graphic:RegisterDirtyMaterialCallback(action) end
---
---@public
---@param action UnityAction 
---@return void 
function Graphic:UnregisterDirtyMaterialCallback(action) end
---
UnityEngine.UI.Graphic = Graphic