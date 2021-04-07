---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class IConstraint
---@field public weight number 
---@field public constraintActive boolean 
---@field public locked boolean 
---@field public sourceCount number 
local IConstraint={ }
---
---@public
---@param source ConstraintSource 
---@return number 
function IConstraint:AddSource(source) end
---
---@public
---@param index number 
---@return void 
function IConstraint:RemoveSource(index) end
---
---@public
---@param index number 
---@return ConstraintSource 
function IConstraint:GetSource(index) end
---
---@public
---@param index number 
---@param source ConstraintSource 
---@return void 
function IConstraint:SetSource(index, source) end
---
---@public
---@param sources List`1 
---@return void 
function IConstraint:GetSources(sources) end
---
---@public
---@param sources List`1 
---@return void 
function IConstraint:SetSources(sources) end
---
UnityEngine.Animations.IConstraint = IConstraint