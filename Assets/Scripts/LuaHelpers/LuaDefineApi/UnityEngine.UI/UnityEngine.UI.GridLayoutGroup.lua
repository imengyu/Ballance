---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GridLayoutGroup : LayoutGroup
---@field public startCorner number 
---@field public startAxis number 
---@field public cellSize Vector2 
---@field public spacing Vector2 
---@field public constraint number 
---@field public constraintCount number 
local GridLayoutGroup={ }
---
---@public
---@return void 
function GridLayoutGroup:CalculateLayoutInputHorizontal() end
---
---@public
---@return void 
function GridLayoutGroup:CalculateLayoutInputVertical() end
---
---@public
---@return void 
function GridLayoutGroup:SetLayoutHorizontal() end
---
---@public
---@return void 
function GridLayoutGroup:SetLayoutVertical() end
---
UnityEngine.UI.GridLayoutGroup = GridLayoutGroup