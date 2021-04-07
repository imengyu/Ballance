---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IConstraintInternal
---@field public transform Transform 
local IConstraintInternal={ }
---
---@public
---@return void 
function IConstraintInternal:ActivateAndPreserveOffset() end
---
---@public
---@return void 
function IConstraintInternal:ActivateWithZeroOffset() end
---
---@public
---@return void 
function IConstraintInternal:UserUpdateOffset() end
---
UnityEngine.Animations.IConstraintInternal = IConstraintInternal