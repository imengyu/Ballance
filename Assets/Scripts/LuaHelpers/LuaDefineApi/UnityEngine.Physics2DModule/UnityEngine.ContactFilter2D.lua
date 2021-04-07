---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ContactFilter2D : ValueType
---@field public useTriggers boolean 
---@field public useLayerMask boolean 
---@field public useDepth boolean 
---@field public useOutsideDepth boolean 
---@field public useNormalAngle boolean 
---@field public useOutsideNormalAngle boolean 
---@field public layerMask LayerMask 
---@field public minDepth number 
---@field public maxDepth number 
---@field public minNormalAngle number 
---@field public maxNormalAngle number 
---@field public NormalAngleUpperLimit number 
---@field public isFiltering boolean 
local ContactFilter2D={ }
---
---@public
---@return ContactFilter2D 
function ContactFilter2D:NoFilter() end
---
---@public
---@return void 
function ContactFilter2D:ClearLayerMask() end
---
---@public
---@param layerMask LayerMask 
---@return void 
function ContactFilter2D:SetLayerMask(layerMask) end
---
---@public
---@return void 
function ContactFilter2D:ClearDepth() end
---
---@public
---@param minDepth number 
---@param maxDepth number 
---@return void 
function ContactFilter2D:SetDepth(minDepth, maxDepth) end
---
---@public
---@return void 
function ContactFilter2D:ClearNormalAngle() end
---
---@public
---@param minNormalAngle number 
---@param maxNormalAngle number 
---@return void 
function ContactFilter2D:SetNormalAngle(minNormalAngle, maxNormalAngle) end
---
---@public
---@param collider Collider2D 
---@return boolean 
function ContactFilter2D:IsFilteringTrigger(collider) end
---
---@public
---@param obj GameObject 
---@return boolean 
function ContactFilter2D:IsFilteringLayerMask(obj) end
---
---@public
---@param obj GameObject 
---@return boolean 
function ContactFilter2D:IsFilteringDepth(obj) end
---
---@public
---@param normal Vector2 
---@return boolean 
function ContactFilter2D:IsFilteringNormalAngle(normal) end
---
---@public
---@param angle number 
---@return boolean 
function ContactFilter2D:IsFilteringNormalAngle(angle) end
---
UnityEngine.ContactFilter2D = ContactFilter2D