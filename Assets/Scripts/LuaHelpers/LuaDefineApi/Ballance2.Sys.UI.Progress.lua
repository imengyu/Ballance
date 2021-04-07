---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Progress
---@field public fillArea RectTransform 
---@field public fillRect RectTransform 
---@field public value number 
local Progress={ }
---
---@public
---@return void 
function Progress:UpdateVal() end
---进度条组件
Ballance2.Sys.UI.Progress = Progress