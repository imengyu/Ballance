---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class UILayout : UIElement
---@field public Elements List`1 子元素
local UILayout={ }
---添加元素
---@public
---@param element UIElement UI元素
---@param doLayout boolean 
---@return UIElement 
function UILayout:AddElement(element, doLayout) end
---移除元素
---@public
---@param element UIElement 元素
---@param destroy boolean 是否释放元素，否则隐藏元素
---@param doLayout boolean 是否立即重新布局
---@return void 
function UILayout:RemoveElement(element, destroy, doLayout) end
---插入元素
---@public
---@param element UIElement UI元素
---@param index number 插入的索引
---@param doLayout boolean 
---@return UIElement 
function UILayout:InsertElement(element, index, doLayout) end
---通过名字查找元素
---@public
---@param name string 名字
---@return UIElement 
function UILayout:FindElementByName(name) end
---通过名字查找布局内部的元素
---@public
---@param name string 名字
---@return UIElement 
function UILayout:FindElementInLayoutByName(name) end
---
---@public
---@param name string 
---@param val string 
---@param intital boolean 
---@return void 
function UILayout:SetProp(name, val, intital) end
---布局
Ballance2.Sys.UI.UISystem.Layout.UILayout = UILayout