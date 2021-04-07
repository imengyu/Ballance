---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class SortingLayer : ValueType
---@field public id number 
---@field public name string 
---@field public value number 
---@field public layers SortingLayer[] 
local SortingLayer={ }
---
---@public
---@param id number 
---@return number 
function SortingLayer.GetLayerValueFromID(id) end
---
---@public
---@param name string 
---@return number 
function SortingLayer.GetLayerValueFromName(name) end
---
---@public
---@param name string 
---@return number 
function SortingLayer.NameToID(name) end
---
---@public
---@param id number 
---@return string 
function SortingLayer.IDToName(id) end
---
---@public
---@param id number 
---@return boolean 
function SortingLayer.IsValid(id) end
---
UnityEngine.SortingLayer = SortingLayer