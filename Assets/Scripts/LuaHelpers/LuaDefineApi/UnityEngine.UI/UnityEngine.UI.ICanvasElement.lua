---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ICanvasElement
---@field public transform Transform 
local ICanvasElement={ }
---
---@public
---@param executing number 
---@return void 
function ICanvasElement:Rebuild(executing) end
---
---@public
---@return void 
function ICanvasElement:LayoutComplete() end
---
---@public
---@return void 
function ICanvasElement:GraphicUpdateComplete() end
---
---@public
---@return boolean 
function ICanvasElement:IsDestroyed() end
---
UnityEngine.UI.ICanvasElement = ICanvasElement