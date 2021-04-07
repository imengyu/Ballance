---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class HumanTrait
---@field public MuscleCount number 
---@field public MuscleName String[] 
---@field public BoneCount number 
---@field public BoneName String[] 
---@field public RequiredBoneCount number 
local HumanTrait={ }
---
---@public
---@param i number 
---@param dofIndex number 
---@return number 
function HumanTrait.MuscleFromBone(i, dofIndex) end
---
---@public
---@param i number 
---@return number 
function HumanTrait.BoneFromMuscle(i) end
---
---@public
---@param i number 
---@return boolean 
function HumanTrait.RequiredBone(i) end
---
---@public
---@param i number 
---@return number 
function HumanTrait.GetMuscleDefaultMin(i) end
---
---@public
---@param i number 
---@return number 
function HumanTrait.GetMuscleDefaultMax(i) end
---
---@public
---@param i number 
---@return number 
function HumanTrait.GetBoneDefaultHierarchyMass(i) end
---
---@public
---@param i number 
---@return number 
function HumanTrait.GetParentBone(i) end
---
UnityEngine.HumanTrait = HumanTrait