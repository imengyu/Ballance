---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IMeshModifier
local IMeshModifier={ }
---
---@public
---@param mesh Mesh 
---@return void 
function IMeshModifier:ModifyMesh(mesh) end
---
---@public
---@param verts VertexHelper 
---@return void 
function IMeshModifier:ModifyMesh(verts) end
---
UnityEngine.UI.IMeshModifier = IMeshModifier