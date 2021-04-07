---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ParentConstraint : Behaviour
---@field public weight number 
---@field public constraintActive boolean 
---@field public locked boolean 
---@field public sourceCount number 
---@field public translationAtRest Vector3 
---@field public rotationAtRest Vector3 
---@field public translationOffsets Vector3[] 
---@field public rotationOffsets Vector3[] 
---@field public translationAxis number 
---@field public rotationAxis number 
local ParentConstraint={ }
---
---@public
---@param index number 
---@return Vector3 
function ParentConstraint:GetTranslationOffset(index) end
---
---@public
---@param index number 
---@param value Vector3 
---@return void 
function ParentConstraint:SetTranslationOffset(index, value) end
---
---@public
---@param index number 
---@return Vector3 
function ParentConstraint:GetRotationOffset(index) end
---
---@public
---@param index number 
---@param value Vector3 
---@return void 
function ParentConstraint:SetRotationOffset(index, value) end
---
---@public
---@param sources List`1 
---@return void 
function ParentConstraint:GetSources(sources) end
---
---@public
---@param sources List`1 
---@return void 
function ParentConstraint:SetSources(sources) end
---
---@public
---@param source ConstraintSource 
---@return number 
function ParentConstraint:AddSource(source) end
---
---@public
---@param index number 
---@return void 
function ParentConstraint:RemoveSource(index) end
---
---@public
---@param index number 
---@return ConstraintSource 
function ParentConstraint:GetSource(index) end
---
---@public
---@param index number 
---@param source ConstraintSource 
---@return void 
function ParentConstraint:SetSource(index, source) end
---
UnityEngine.Animations.ParentConstraint = ParentConstraint