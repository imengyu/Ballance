---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class UIElement
---@field public Id number 元素ID
---@field public Name string 元素名称
---@field public Parent UIElement 获取元素的父元素
---@field public RectTransform RectTransform RectTransform
---@field public Visible number 获取或设置显示状态
---@field public LayoutParams LayoutParams 布局属性
---@field public IsInLayout boolean 获取是否正在布局
---@field public Width number 获取当前元素宽度
---@field public Height number 获取当前元素高度
---@field public Padding number 
---@field public PaddingLeft number 
---@field public PaddingRight number 
---@field public PaddingTop number 
---@field public PaddingBottom number 
---@field public MinWidth number 最小宽度（如果设置了ContentSizeFitter则此属性无效）
---@field public MinHeight number 最小高度（如果设置了ContentSizeFitter则此属性无效）
local UIElement={ }
---
---@public
---@return number 
function UIElement:GetSuggestedMinimumWidth() end
---
---@public
---@return number 
function UIElement:GetSuggestedMinimumHeight() end
---布局
---@public
---@param l number 
---@param t number 
---@param r number 
---@param b number 
---@return void 
function UIElement:Layout(l, t, r, b) end
---请求当前视图进行布局
---@public
---@return void 
function UIElement:RequestLayout() end
---指示是否在下一个层次结构布局过程中请求此视图的布局。
---@public
---@return boolean 如果在下一个布局过程中强制布局，则为true
function UIElement:IsLayoutRequested() end
---强制在下一个布局过程中布局此视图。此方法不对父对象调用RequestLayout() 或 ForceLayout()。
---@public
---@return void 
function UIElement:ForceLayout() end
---
---@public
---@param name string 
---@param val string 
---@param intital boolean 
---@return void 
function UIElement:SetProp(name, val, intital) end
---UI布局系统元素
Ballance2.Sys.UI.UISystem.UIElement = UIElement