---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class BeforeRenderHelper
local BeforeRenderHelper={ }
---
---@public
---@param callback UnityAction 
---@return void 
function BeforeRenderHelper.RegisterCallback(callback) end
---
---@public
---@param callback UnityAction 
---@return void 
function BeforeRenderHelper.UnregisterCallback(callback) end
---
---@public
---@return void 
function BeforeRenderHelper.Invoke() end
---
UnityEngine.BeforeRenderHelper = BeforeRenderHelper