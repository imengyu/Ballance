---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class UIBehaviour : MonoBehaviour
local UIBehaviour={ }
---
---@public
---@return boolean 
function UIBehaviour:IsActive() end
---
---@public
---@return boolean 
function UIBehaviour:IsDestroyed() end
---
UnityEngine.EventSystems.UIBehaviour = UIBehaviour