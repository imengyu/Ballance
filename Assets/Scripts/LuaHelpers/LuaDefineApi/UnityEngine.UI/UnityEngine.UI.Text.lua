---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Text : MaskableGraphic
---@field public cachedTextGenerator TextGenerator 
---@field public cachedTextGeneratorForLayout TextGenerator 
---@field public mainTexture Texture 
---@field public font Font 
---@field public text string 
---@field public supportRichText boolean 
---@field public resizeTextForBestFit boolean 
---@field public resizeTextMinSize number 
---@field public resizeTextMaxSize number 
---@field public alignment number 
---@field public alignByGeometry boolean 
---@field public fontSize number 
---@field public horizontalOverflow number 
---@field public verticalOverflow number 
---@field public lineSpacing number 
---@field public fontStyle number 
---@field public pixelsPerUnit number 
---@field public minWidth number 
---@field public preferredWidth number 
---@field public flexibleWidth number 
---@field public minHeight number 
---@field public preferredHeight number 
---@field public flexibleHeight number 
---@field public layoutPriority number 
local Text={ }
---
---@public
---@return void 
function Text:FontTextureChanged() end
---
---@public
---@param extents Vector2 
---@return TextGenerationSettings 
function Text:GetGenerationSettings(extents) end
---
---@public
---@param anchor number 
---@return Vector2 
function Text.GetTextAnchorPivot(anchor) end
---
---@public
---@return void 
function Text:CalculateLayoutInputHorizontal() end
---
---@public
---@return void 
function Text:CalculateLayoutInputVertical() end
---
---@public
---@return void 
function Text:OnRebuildRequested() end
---
UnityEngine.UI.Text = Text