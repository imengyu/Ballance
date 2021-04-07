---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class EdgeCollider2D : Collider2D
---@field public edgeRadius number 
---@field public edgeCount number 
---@field public pointCount number 
---@field public points Vector2[] 
---@field public useAdjacentStartPoint boolean 
---@field public useAdjacentEndPoint boolean 
---@field public adjacentStartPoint Vector2 
---@field public adjacentEndPoint Vector2 
local EdgeCollider2D={ }
---
---@public
---@return void 
function EdgeCollider2D:Reset() end
---
---@public
---@param points List`1 
---@return number 
function EdgeCollider2D:GetPoints(points) end
---
---@public
---@param points List`1 
---@return boolean 
function EdgeCollider2D:SetPoints(points) end
---
UnityEngine.EdgeCollider2D = EdgeCollider2D