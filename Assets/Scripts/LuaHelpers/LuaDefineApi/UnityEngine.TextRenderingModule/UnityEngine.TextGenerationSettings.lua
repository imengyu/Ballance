---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class TextGenerationSettings : ValueType
---@field public font Font 
---@field public color Color 
---@field public fontSize number 
---@field public lineSpacing number 
---@field public richText boolean 
---@field public scaleFactor number 
---@field public fontStyle number 
---@field public textAnchor number 
---@field public alignByGeometry boolean 
---@field public resizeTextForBestFit boolean 
---@field public resizeTextMinSize number 
---@field public resizeTextMaxSize number 
---@field public updateBounds boolean 
---@field public verticalOverflow number 
---@field public horizontalOverflow number 
---@field public generationExtents Vector2 
---@field public pivot Vector2 
---@field public generateOutOfBounds boolean 
local TextGenerationSettings={ }
---
---@public
---@param other TextGenerationSettings 
---@return boolean 
function TextGenerationSettings:Equals(other) end
---
UnityEngine.TextGenerationSettings = TextGenerationSettings