---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class GameUIManager : GameService
---@field public UIRoot RectTransform UI 根
---@field public UIFadeManager UIFadeManager 渐变管理器
---@field public UIRootRectTransform RectTransform UI 根 RectTransform
local GameUIManager={ }
---显示全局土司提示
---@public
---@param text string 提示文字
---@return void 
function GameUIManager:GlobalToast(text) end
---显示全局土司提示
---@public
---@param text string 提示文字
---@param showSec number 显示时长（秒）
---@return void 
function GameUIManager:GlobalToast(text, showSec) end
---显示全局 Alert 对话框（窗口模式）
---@public
---@param text string 内容
---@param title string 标题
---@param okText string OK 按钮文字
---@return number 
function GameUIManager:GlobalAlertWindow(text, title, okText) end
---显示全局 Confirm 对话框（窗口模式）
---@public
---@param text string 内容
---@param title string 标题
---@param okText string OK 按钮文字
---@param cancelText string Cancel 按钮文字
---@return number 
function GameUIManager:GlobalConfirmWindow(text, title, okText, cancelText) end
---创建自定义窗口（默认不显示）
---@public
---@param title string 标题
---@param customView RectTransform 窗口自定义View
---@return Window 
function GameUIManager:CreateWindow(title, customView) end
---创建自定义窗口
---@public
---@param title string 标题
---@param customView RectTransform 窗口自定义View
---@param show boolean 创建后是否立即显示
---@return Window 
function GameUIManager:CreateWindow(title, customView, show) end
---创建自定义窗口
---@public
---@param title string 标题
---@param customView RectTransform 窗口自定义View
---@param show boolean 创建后是否立即显示
---@param x number X 坐标
---@param y number Y 坐标
---@param w number 宽度，0 使用默认
---@param h number 高度，0 使用默认
---@return Window 
function GameUIManager:CreateWindow(title, customView, show, x, y, w, h) end
---注册窗口到管理器中
---@public
---@param window Window 窗口
---@return Window 
function GameUIManager:RegisterWindow(window) end
---通过 ID 查找窗口
---@public
---@param windowId number 窗口ID
---@return Window 
function GameUIManager:FindWindowById(windowId) end
---
---@public
---@param window Window 
---@return void 
function GameUIManager:ShowWindow(window) end
---
---@public
---@param window Window 
---@return void 
function GameUIManager:HideWindow(window) end
---
---@public
---@param window Window 
---@return void 
function GameUIManager:CloseWindow(window) end
---全局黑色遮罩隐藏
---@public
---@param show boolean 
---@return void 
function GameUIManager:MaskBlackSet(show) end
---全局黑色遮罩隐藏
---@public
---@param show boolean 
---@return void 
function GameUIManager:MaskWhiteSet(show) end
---全局黑色遮罩渐变淡入
---@public
---@param second number 耗时（秒）
---@return void 
function GameUIManager:MaskBlackFadeIn(second) end
---全局白色遮罩渐变淡入
---@public
---@param second number 耗时（秒）
---@return void 
function GameUIManager:MaskWhiteFadeIn(second) end
---全局黑色遮罩渐变淡出
---@public
---@param second number 耗时（秒）
---@return void 
function GameUIManager:MaskBlackFadeOut(second) end
---全局白色遮罩渐变淡出
---@public
---@param second number 耗时（秒）
---@return void 
function GameUIManager:MaskWhiteFadeOut(second) end
---获取 UI 控件预制体
---@public
---@param name number 名称
---@return GameObject 
function GameUIManager:GetUIPrefab(name) end
---注册 UI 控件预制体
---@public
---@param name number 名称
---@param prefab number 预制体
---@return boolean 
function GameUIManager:RegisterUIPrefab(name, prefab) end
---清除已注册的 UI 控件预制体
---@public
---@param name number 名称
---@return boolean 
function GameUIManager:RemoveUIPrefab(name) end
---
---@public
---@param view RectTransform 
---@return void 
function GameUIManager:SetViewToTemporarily(view) end
---
---@public
---@param view RectTransform 
---@return void 
function GameUIManager:AttatchViewToCanvas(view) end
---
---@public
---@param prefab GameObject 
---@param name string 
---@return RectTransform 
function GameUIManager:InitViewToCanvas(prefab, name) end
---UI 管理器
Ballance2.Sys.Services.GameUIManager = GameUIManager