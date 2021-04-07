---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SpriteAtlas : Object
---@field public isVariant boolean 
---@field public tag string 
---@field public spriteCount number 
local SpriteAtlas={ }
---
---@public
---@param sprite Sprite 
---@return boolean 
function SpriteAtlas:CanBindTo(sprite) end
---
---@public
---@param name string 
---@return Sprite 
function SpriteAtlas:GetSprite(name) end
---
---@public
---@param sprites Sprite[] 
---@return number 
function SpriteAtlas:GetSprites(sprites) end
---
---@public
---@param sprites Sprite[] 
---@param name string 
---@return number 
function SpriteAtlas:GetSprites(sprites, name) end
---
UnityEngine.U2D.SpriteAtlas = SpriteAtlas