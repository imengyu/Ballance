---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BaseMeshEffect : UIBehaviour
local BaseMeshEffect={ }
---
---@public
---@param mesh Mesh 
---@return void 
function BaseMeshEffect:ModifyMesh(mesh) end
---
---@public
---@param vh VertexHelper 
---@return void 
function BaseMeshEffect:ModifyMesh(vh) end
---
UnityEngine.UI.BaseMeshEffect = BaseMeshEffect