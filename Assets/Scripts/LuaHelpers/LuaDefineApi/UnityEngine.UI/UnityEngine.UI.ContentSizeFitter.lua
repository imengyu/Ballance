---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class ContentSizeFitter : UIBehaviour
---@field public horizontalFit number 
---@field public verticalFit number 
local ContentSizeFitter={ }
---
---@public
---@return void 
function ContentSizeFitter:SetLayoutHorizontal() end
---
---@public
---@return void 
function ContentSizeFitter:SetLayoutVertical() end
---
UnityEngine.UI.ContentSizeFitter = ContentSizeFitter