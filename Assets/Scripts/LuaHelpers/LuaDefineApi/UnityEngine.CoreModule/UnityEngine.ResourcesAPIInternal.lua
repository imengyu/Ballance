---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ResourcesAPIInternal
local ResourcesAPIInternal={ }
---
---@public
---@param type Type 
---@return Object[] 
function ResourcesAPIInternal.FindObjectsOfTypeAll(type) end
---
---@public
---@param name string 
---@return Shader 
function ResourcesAPIInternal.FindShaderByName(name) end
---
---@public
---@param path string 
---@param systemTypeInstance Type 
---@return Object 
function ResourcesAPIInternal.Load(path, systemTypeInstance) end
---
---@public
---@param path string 
---@param systemTypeInstance Type 
---@return Object[] 
function ResourcesAPIInternal.LoadAll(path, systemTypeInstance) end
---
---@public
---@param assetToUnload Object 
---@return void 
function ResourcesAPIInternal.UnloadAsset(assetToUnload) end
---
UnityEngine.ResourcesAPIInternal = ResourcesAPIInternal