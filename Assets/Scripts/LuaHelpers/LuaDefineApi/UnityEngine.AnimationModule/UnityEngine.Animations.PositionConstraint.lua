---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class PositionConstraint : Behaviour
---@field public weight number 
---@field public translationAtRest Vector3 
---@field public translationOffset Vector3 
---@field public translationAxis number 
---@field public constraintActive boolean 
---@field public locked boolean 
---@field public sourceCount number 
local PositionConstraint={ }
---
---@public
---@param sources List`1 
---@return void 
function PositionConstraint:GetSources(sources) end
---
---@public
---@param sources List`1 
---@return void 
function PositionConstraint:SetSources(sources) end
---
---@public
---@param source ConstraintSource 
---@return number 
function PositionConstraint:AddSource(source) end
---
---@public
---@param index number 
---@return void 
function PositionConstraint:RemoveSource(index) end
---
---@public
---@param index number 
---@return ConstraintSource 
function PositionConstraint:GetSource(index) end
---
---@public
---@param index number 
---@param source ConstraintSource 
---@return void 
function PositionConstraint:SetSource(index, source) end
---
UnityEngine.Animations.PositionConstraint = PositionConstraint