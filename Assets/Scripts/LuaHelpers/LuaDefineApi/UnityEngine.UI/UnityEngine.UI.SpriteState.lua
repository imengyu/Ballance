---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SpriteState : ValueType
---@field public highlightedSprite Sprite 
---@field public pressedSprite Sprite 
---@field public selectedSprite Sprite 
---@field public disabledSprite Sprite 
local SpriteState={ }
---
---@public
---@param other SpriteState 
---@return boolean 
function SpriteState:Equals(other) end
---
UnityEngine.UI.SpriteState = SpriteState