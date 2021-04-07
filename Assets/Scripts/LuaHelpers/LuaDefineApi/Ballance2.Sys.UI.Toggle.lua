---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Toggle
---@field public Drag RectTransform 
---@field public Background RectTransform 
---@field public DragImage Image 
---@field public ActiveColor Color 
---@field public DeactiveColor Color 
---@field public onValueChanged ToggleEvent 
---@field public isOn boolean 
local Toggle={ }
---
---@public
---@return void 
function Toggle:UpdateOn() end
---一个开关组件
Ballance2.Sys.UI.Toggle = Toggle