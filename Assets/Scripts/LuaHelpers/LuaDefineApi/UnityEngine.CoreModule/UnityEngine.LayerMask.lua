---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class LayerMask : ValueType
---@field public value number 
local LayerMask={ }
---
---@public
---@param mask LayerMask 
---@return number 
function LayerMask.op_Implicit(mask) end
---
---@public
---@param intVal number 
---@return LayerMask 
function LayerMask.op_Implicit(intVal) end
---
---@public
---@param layer number 
---@return string 
function LayerMask.LayerToName(layer) end
---
---@public
---@param layerName string 
---@return number 
function LayerMask.NameToLayer(layerName) end
---
---@public
---@param layerNames String[] 
---@return number 
function LayerMask.GetMask(layerNames) end
---
UnityEngine.LayerMask = LayerMask