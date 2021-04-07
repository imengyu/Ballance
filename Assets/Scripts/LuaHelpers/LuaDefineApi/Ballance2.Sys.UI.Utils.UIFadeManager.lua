---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class UIFadeManager
local UIFadeManager={ }
---运行淡出动画
---@public
---@param gameObject GameObject 执行对象
---@param timeInSecond number 执行时间
---@param hidden boolean 执行完毕是否将对象设置为不激活
---@param material Material 执行材质
---@return FadeObject 
function UIFadeManager:AddFadeOut(gameObject, timeInSecond, hidden, material) end
---运行淡入动画
---@public
---@param gameObject GameObject 执行对象
---@param timeInSecond number 执行时间
---@param material Material 执行材质
---@return FadeObject 
function UIFadeManager:AddFadeIn(gameObject, timeInSecond, material) end
---运行淡出动画
---@public
---@param gameObject GameObject 执行对象
---@param timeInSecond number 执行时间
---@param hidden boolean 执行完毕是否将对象设置为不激活
---@param materials Material[] 
---@return FadeObject 
function UIFadeManager:AddFadeOut(gameObject, timeInSecond, hidden, materials) end
---运行淡入动画
---@public
---@param gameObject GameObject 执行对象
---@param timeInSecond number 执行时间
---@param materials Material[] 
---@return FadeObject 
function UIFadeManager:AddFadeIn(gameObject, timeInSecond, materials) end
---运行淡出动画
---@public
---@param image Image 
---@param timeInSecond number 执行时间
---@param hidden boolean 执行完毕是否将对象设置为不激活
---@return FadeObject 
function UIFadeManager:AddFadeOut(image, timeInSecond, hidden) end
---运行淡入动画
---@public
---@param image Image 
---@param timeInSecond number 执行时间
---@return FadeObject 
function UIFadeManager:AddFadeIn(image, timeInSecond) end
---运行淡出动画
---@public
---@param text Text 
---@param timeInSecond number 执行时间
---@param hidden boolean 执行完毕是否将对象设置为不激活
---@return FadeObject 
function UIFadeManager:AddFadeOut(text, timeInSecond, hidden) end
---运行淡入动画
---@public
---@param text Text 
---@param timeInSecond number 执行时间
---@return FadeObject 
function UIFadeManager:AddFadeIn(text, timeInSecond) end
---
Ballance2.Sys.UI.Utils.UIFadeManager = UIFadeManager