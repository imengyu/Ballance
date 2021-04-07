---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class RawImage : MaskableGraphic
---@field public mainTexture Texture 
---@field public texture Texture 
---@field public uvRect Rect 
local RawImage={ }
---
---@public
---@return void 
function RawImage:SetNativeSize() end
---
UnityEngine.UI.RawImage = RawImage