---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AnimatorUtility
local AnimatorUtility={ }
---
---@public
---@param go GameObject 
---@param exposedTransforms String[] 
---@return void 
function AnimatorUtility.OptimizeTransformHierarchy(go, exposedTransforms) end
---
---@public
---@param go GameObject 
---@return void 
function AnimatorUtility.DeoptimizeTransformHierarchy(go) end
---
UnityEngine.AnimatorUtility = AnimatorUtility