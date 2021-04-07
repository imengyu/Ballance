---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IMaterialModifier
local IMaterialModifier={ }
---
---@public
---@param baseMaterial Material 
---@return Material 
function IMaterialModifier:GetModifiedMaterial(baseMaterial) end
---
UnityEngine.UI.IMaterialModifier = IMaterialModifier