---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AimConstraint : Behaviour
---@field public weight number 
---@field public constraintActive boolean 
---@field public locked boolean 
---@field public rotationAtRest Vector3 
---@field public rotationOffset Vector3 
---@field public rotationAxis number 
---@field public aimVector Vector3 
---@field public upVector Vector3 
---@field public worldUpVector Vector3 
---@field public worldUpObject Transform 
---@field public worldUpType number 
---@field public sourceCount number 
local AimConstraint={ }
---
---@public
---@param sources List`1 
---@return void 
function AimConstraint:GetSources(sources) end
---
---@public
---@param sources List`1 
---@return void 
function AimConstraint:SetSources(sources) end
---
---@public
---@param source ConstraintSource 
---@return number 
function AimConstraint:AddSource(source) end
---
---@public
---@param index number 
---@return void 
function AimConstraint:RemoveSource(index) end
---
---@public
---@param index number 
---@return ConstraintSource 
function AimConstraint:GetSource(index) end
---
---@public
---@param index number 
---@param source ConstraintSource 
---@return void 
function AimConstraint:SetSource(index, source) end
---
UnityEngine.Animations.AimConstraint = AimConstraint