---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CustomRenderTextureManager
local CustomRenderTextureManager={ }
---
---@public
---@param value Action`1 
---@return void 
function CustomRenderTextureManager.add_textureLoaded(value) end
---
---@public
---@param value Action`1 
---@return void 
function CustomRenderTextureManager.remove_textureLoaded(value) end
---
---@public
---@param value Action`1 
---@return void 
function CustomRenderTextureManager.add_textureUnloaded(value) end
---
---@public
---@param value Action`1 
---@return void 
function CustomRenderTextureManager.remove_textureUnloaded(value) end
---
---@public
---@param currentCustomRenderTextures List`1 
---@return void 
function CustomRenderTextureManager.GetAllCustomRenderTextures(currentCustomRenderTextures) end
---
---@public
---@param value Action`2 
---@return void 
function CustomRenderTextureManager.add_updateTriggered(value) end
---
---@public
---@param value Action`2 
---@return void 
function CustomRenderTextureManager.remove_updateTriggered(value) end
---
---@public
---@param value Action`1 
---@return void 
function CustomRenderTextureManager.add_initializeTriggered(value) end
---
---@public
---@param value Action`1 
---@return void 
function CustomRenderTextureManager.remove_initializeTriggered(value) end
---
UnityEngine.CustomRenderTextureManager = CustomRenderTextureManager