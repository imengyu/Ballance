---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Scene : ValueType
---@field public handle number 
---@field public path string 
---@field public name string 
---@field public isLoaded boolean 
---@field public buildIndex number 
---@field public isDirty boolean 
---@field public rootCount number 
---@field public isSubScene boolean 
local Scene={ }
---
---@public
---@return boolean 
function Scene:IsValid() end
---
---@public
---@return GameObject[] 
function Scene:GetRootGameObjects() end
---
---@public
---@param rootGameObjects List`1 
---@return void 
function Scene:GetRootGameObjects(rootGameObjects) end
---
---@public
---@param lhs Scene 
---@param rhs Scene 
---@return boolean 
function Scene.op_Equality(lhs, rhs) end
---
---@public
---@param lhs Scene 
---@param rhs Scene 
---@return boolean 
function Scene.op_Inequality(lhs, rhs) end
---
---@public
---@return number 
function Scene:GetHashCode() end
---
---@public
---@param other Object 
---@return boolean 
function Scene:Equals(other) end
---
UnityEngine.SceneManagement.Scene = Scene