---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class DataUtility
local DataUtility={ }
---
---@public
---@param sprite Sprite 
---@return Vector4 
function DataUtility.GetInnerUV(sprite) end
---
---@public
---@param sprite Sprite 
---@return Vector4 
function DataUtility.GetOuterUV(sprite) end
---
---@public
---@param sprite Sprite 
---@return Vector4 
function DataUtility.GetPadding(sprite) end
---
---@public
---@param sprite Sprite 
---@return Vector2 
function DataUtility.GetMinSize(sprite) end
---
UnityEngine.Sprites.DataUtility = DataUtility