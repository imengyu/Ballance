---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GraphicRegistry
---@field public instance GraphicRegistry 
local GraphicRegistry={ }
---
---@public
---@param c Canvas 
---@param graphic Graphic 
---@return void 
function GraphicRegistry.RegisterGraphicForCanvas(c, graphic) end
---
---@public
---@param c Canvas 
---@param graphic Graphic 
---@return void 
function GraphicRegistry.RegisterRaycastGraphicForCanvas(c, graphic) end
---
---@public
---@param c Canvas 
---@param graphic Graphic 
---@return void 
function GraphicRegistry.UnregisterGraphicForCanvas(c, graphic) end
---
---@public
---@param c Canvas 
---@param graphic Graphic 
---@return void 
function GraphicRegistry.UnregisterRaycastGraphicForCanvas(c, graphic) end
---
---@public
---@param canvas Canvas 
---@return IList`1 
function GraphicRegistry.GetGraphicsForCanvas(canvas) end
---
---@public
---@param canvas Canvas 
---@return IList`1 
function GraphicRegistry.GetRaycastableGraphicsForCanvas(canvas) end
---
UnityEngine.UI.GraphicRegistry = GraphicRegistry