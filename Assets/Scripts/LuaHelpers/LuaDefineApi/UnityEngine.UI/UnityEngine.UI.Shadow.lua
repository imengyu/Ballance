---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Shadow : BaseMeshEffect
---@field public effectColor Color 
---@field public effectDistance Vector2 
---@field public useGraphicAlpha boolean 
local Shadow={ }
---
---@public
---@param vh VertexHelper 
---@return void 
function Shadow:ModifyMesh(vh) end
---
UnityEngine.UI.Shadow = Shadow