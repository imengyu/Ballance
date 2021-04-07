---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Sprite : Object
---@field public bounds Bounds 
---@field public rect Rect 
---@field public border Vector4 
---@field public texture Texture2D 
---@field public pixelsPerUnit number 
---@field public spriteAtlasTextureScale number 
---@field public associatedAlphaSplitTexture Texture2D 
---@field public pivot Vector2 
---@field public packed boolean 
---@field public packingMode number 
---@field public packingRotation number 
---@field public textureRect Rect 
---@field public textureRectOffset Vector2 
---@field public vertices Vector2[] 
---@field public triangles UInt16[] 
---@field public uv Vector2[] 
local Sprite={ }
---
---@public
---@return number 
function Sprite:GetPhysicsShapeCount() end
---
---@public
---@param shapeIdx number 
---@return number 
function Sprite:GetPhysicsShapePointCount(shapeIdx) end
---
---@public
---@param shapeIdx number 
---@param physicsShape List`1 
---@return number 
function Sprite:GetPhysicsShape(shapeIdx, physicsShape) end
---
---@public
---@param physicsShapes IList`1 
---@return void 
function Sprite:OverridePhysicsShape(physicsShapes) end
---
---@public
---@param vertices Vector2[] 
---@param triangles UInt16[] 
---@return void 
function Sprite:OverrideGeometry(vertices, triangles) end
---
---@public
---@param texture Texture2D 
---@param rect Rect 
---@param pivot Vector2 
---@param pixelsPerUnit number 
---@param extrude number 
---@param meshType number 
---@param border Vector4 
---@param generateFallbackPhysicsShape boolean 
---@return Sprite 
function Sprite.Create(texture, rect, pivot, pixelsPerUnit, extrude, meshType, border, generateFallbackPhysicsShape) end
---
---@public
---@param texture Texture2D 
---@param rect Rect 
---@param pivot Vector2 
---@param pixelsPerUnit number 
---@param extrude number 
---@param meshType number 
---@param border Vector4 
---@return Sprite 
function Sprite.Create(texture, rect, pivot, pixelsPerUnit, extrude, meshType, border) end
---
---@public
---@param texture Texture2D 
---@param rect Rect 
---@param pivot Vector2 
---@param pixelsPerUnit number 
---@param extrude number 
---@param meshType number 
---@return Sprite 
function Sprite.Create(texture, rect, pivot, pixelsPerUnit, extrude, meshType) end
---
---@public
---@param texture Texture2D 
---@param rect Rect 
---@param pivot Vector2 
---@param pixelsPerUnit number 
---@param extrude number 
---@return Sprite 
function Sprite.Create(texture, rect, pivot, pixelsPerUnit, extrude) end
---
---@public
---@param texture Texture2D 
---@param rect Rect 
---@param pivot Vector2 
---@param pixelsPerUnit number 
---@return Sprite 
function Sprite.Create(texture, rect, pivot, pixelsPerUnit) end
---
---@public
---@param texture Texture2D 
---@param rect Rect 
---@param pivot Vector2 
---@return Sprite 
function Sprite.Create(texture, rect, pivot) end
---
UnityEngine.Sprite = Sprite