---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ScriptableObject : Object
local ScriptableObject={ }
---
---@public
---@return void 
function ScriptableObject:SetDirty() end
---
---@public
---@param className string 
---@return ScriptableObject 
function ScriptableObject.CreateInstance(className) end
---
---@public
---@param type Type 
---@return ScriptableObject 
function ScriptableObject.CreateInstance(type) end
---
UnityEngine.ScriptableObject = ScriptableObject