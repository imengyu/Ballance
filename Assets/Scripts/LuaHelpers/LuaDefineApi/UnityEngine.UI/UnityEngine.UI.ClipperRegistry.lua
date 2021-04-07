---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ClipperRegistry
---@field public instance ClipperRegistry 
local ClipperRegistry={ }
---
---@public
---@return void 
function ClipperRegistry:Cull() end
---
---@public
---@param c IClipper 
---@return void 
function ClipperRegistry.Register(c) end
---
---@public
---@param c IClipper 
---@return void 
function ClipperRegistry.Unregister(c) end
---
UnityEngine.UI.ClipperRegistry = ClipperRegistry