---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PolygonCollider2D : Collider2D
---@field public autoTiling boolean 
---@field public points Vector2[] 
---@field public pathCount number 
local PolygonCollider2D={ }
---
---@public
---@return number 
function PolygonCollider2D:GetTotalPointCount() end
---
---@public
---@param index number 
---@return Vector2[] 
function PolygonCollider2D:GetPath(index) end
---
---@public
---@param index number 
---@param points Vector2[] 
---@return void 
function PolygonCollider2D:SetPath(index, points) end
---
---@public
---@param index number 
---@param points List`1 
---@return number 
function PolygonCollider2D:GetPath(index, points) end
---
---@public
---@param index number 
---@param points List`1 
---@return void 
function PolygonCollider2D:SetPath(index, points) end
---
---@public
---@param sides number 
---@return void 
function PolygonCollider2D:CreatePrimitive(sides) end
---
---@public
---@param sides number 
---@param scale Vector2 
---@return void 
function PolygonCollider2D:CreatePrimitive(sides, scale) end
---
---@public
---@param sides number 
---@param scale Vector2 
---@param offset Vector2 
---@return void 
function PolygonCollider2D:CreatePrimitive(sides, scale, offset) end
---
UnityEngine.PolygonCollider2D = PolygonCollider2D