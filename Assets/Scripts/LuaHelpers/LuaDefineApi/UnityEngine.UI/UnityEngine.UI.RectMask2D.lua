---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RectMask2D : UIBehaviour
---@field public padding Vector4 
---@field public softness Vector2Int 
---@field public canvasRect Rect 
---@field public rectTransform RectTransform 
local RectMask2D={ }
---
---@public
---@param sp Vector2 
---@param eventCamera Camera 
---@return boolean 
function RectMask2D:IsRaycastLocationValid(sp, eventCamera) end
---
---@public
---@return void 
function RectMask2D:PerformClipping() end
---
---@public
---@return void 
function RectMask2D:UpdateClipSoftness() end
---
---@public
---@param clippable IClippable 
---@return void 
function RectMask2D:AddClippable(clippable) end
---
---@public
---@param clippable IClippable 
---@return void 
function RectMask2D:RemoveClippable(clippable) end
---
UnityEngine.UI.RectMask2D = RectMask2D