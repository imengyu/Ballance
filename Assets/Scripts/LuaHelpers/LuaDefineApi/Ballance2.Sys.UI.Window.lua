---@diagnostic disable: duplicate-set-field, undefined-doc-class, undefined-doc-name, duplicate-doc-field
---@class Window
---@field public WindowButtonMax Button 
---@field public WindowButtonMin Button 
---@field public WindowButtonMinRectTransform RectTransform 
---@field public WindowButtonClose Button 
---@field public WindowIconImage Image 
---@field public WindowTitleText Text 
---@field public WindowClientArea RectTransform 
---@field public WindowTitle RectTransform 
---@field public SizeDrag UISizeDrag 
---@field public onClose WindowEventDelegate 
---@field public onShow WindowEventDelegate 
---@field public onHide WindowEventDelegate 
---@field public Size Vector2 获取或设置窗口大小
---@field public MinSize Vector2 获取或设置窗口最小大小
---@field public Position Vector2 获取或设置窗口位置
---@field public Icon Sprite 窗口的图标
---@field public CanResize boolean 窗口是否可以拖动改变大小
---@field public CanDrag boolean 窗口是否可以拖动
---@field public CanClose boolean 窗口是否可关闭
---@field public CanMin boolean 窗口是否可以拖动改变大小
---@field public CanMax boolean 窗口是否可以拖动改变大小
---@field public CloseAsHide boolean 点击窗口关闭按钮是否替换为隐藏窗口
---@field public Title string 窗口标题
---@field public WindowState number 获取窗口当前状态
---@field public WindowType number 获取窗口类型
local Window={ }
---获取窗口是否显示
---@public
---@return boolean 
function Window:GetVisible() end
---设置窗口是否显示
---@public
---@param visible boolean 是否显示
---@return void 
function Window:SetVisible(visible) end
---销毁窗口
---@public
---@return void 
function Window:Destroy() end
---获取窗口ID
---@public
---@return number 
function Window:GetWindowId() end
---设置窗口的自定义区域视图
---@public
---@param view RectTransform 要绑定的子视图
---@return RectTransform 
function Window:SetView(view) end
---获取窗口的自定义区域已绑定的视图
---@public
---@return RectTransform 
function Window:GetView() end
---获取窗口本体的 RectTransform
---@public
---@return RectTransform 
function Window:GetRectTransform() end
---关闭并销毁窗口
---@public
---@return void 
function Window:Close() end
---显示窗口
---@public
---@return void 
function Window:Show() end
---隐藏窗口
---@public
---@return void 
function Window:Hide() end
---窗口剧中
---@public
---@return void 
function Window:MoveToCenter() end
---基础 UI 窗口
Ballance2.Sys.UI.Window = Window