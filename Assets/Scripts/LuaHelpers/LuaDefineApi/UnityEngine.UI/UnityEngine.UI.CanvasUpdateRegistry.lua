---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class CanvasUpdateRegistry
---@field public instance CanvasUpdateRegistry 
local CanvasUpdateRegistry={ }
---
---@public
---@param element ICanvasElement 
---@return void 
function CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(element) end
---
---@public
---@param element ICanvasElement 
---@return boolean 
function CanvasUpdateRegistry.TryRegisterCanvasElementForLayoutRebuild(element) end
---
---@public
---@param element ICanvasElement 
---@return void 
function CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(element) end
---
---@public
---@param element ICanvasElement 
---@return boolean 
function CanvasUpdateRegistry.TryRegisterCanvasElementForGraphicRebuild(element) end
---
---@public
---@param element ICanvasElement 
---@return void 
function CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(element) end
---
---@public
---@return boolean 
function CanvasUpdateRegistry.IsRebuildingLayout() end
---
---@public
---@return boolean 
function CanvasUpdateRegistry.IsRebuildingGraphics() end
---
UnityEngine.UI.CanvasUpdateRegistry = CanvasUpdateRegistry