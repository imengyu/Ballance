---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class AspectRatioFitter : UIBehaviour
---@field public aspectMode number 
---@field public aspectRatio number 
local AspectRatioFitter={ }
---
---@public
---@return void 
function AspectRatioFitter:SetLayoutHorizontal() end
---
---@public
---@return void 
function AspectRatioFitter:SetLayoutVertical() end
---
---@public
---@return boolean 
function AspectRatioFitter:IsComponentValidOnObject() end
---
---@public
---@return boolean 
function AspectRatioFitter:IsAspectModeValid() end
---
UnityEngine.UI.AspectRatioFitter = AspectRatioFitter