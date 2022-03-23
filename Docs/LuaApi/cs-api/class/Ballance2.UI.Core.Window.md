# Ballance2.UI.Core.Window 
基础 UI 窗口

## 注解

为游戏提供了一个可以拖拽，调整大小的窗口，用于游戏内部某些UI的使用。要创建窗口，可以调用 `GameUIManager.CreateWindow` 函数。

## 字段

|名称|类型|说明|
|---|---|---|
|WindowRectTransform|[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) ||
|WindowTitleDragger|[UIDragControl](./Ballance2.UI.Utils.UIDragControl.md) ||
|TitleDefaultColor|[Color](https://docs.unity3d.com/ScriptReference/Color.html) ||
|TitleActiveColor|[Color](https://docs.unity3d.com/ScriptReference/Color.html) ||
|TitleDefaultSprite|[Sprite](https://docs.unity3d.com/ScriptReference/Sprite.html) ||
|TitleMinSprite|[Sprite](https://docs.unity3d.com/ScriptReference/Sprite.html) ||
|WindowButtonClose|[Button](https://docs.unity3d.com/ScriptReference/UI.Button.html) ||
|WindowButtonMin|[Button](https://docs.unity3d.com/ScriptReference/UI.Button.html) ||
|WindowButtonMax|[Button](https://docs.unity3d.com/ScriptReference/UI.Button.html) ||
|WindowButtonRestore|[Button](https://docs.unity3d.com/ScriptReference/UI.Button.html) ||
|WindowIconImage|[Image](https://docs.unity3d.com/ScriptReference/UI.Image.html) ||
|WindowTitleImage|[Image](https://docs.unity3d.com/ScriptReference/UI.Image.html) ||
|WindowTitleText|[Text](https://docs.unity3d.com/ScriptReference/UI.Text.html) ||
|WindowClientArea|[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) ||
|WindowTitle|[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) ||
|SizeDrag|[UISizeDrag](./Ballance2.UI.Utils.UISizeDrag.md) ||
|onClose|`回调` WindowEventDelegate(windowId: [Int32](https://docs.microsoft.com/zh-cn/dotnet/api/System.Int32)) ||
|onShow|`回调` WindowEventDelegate(windowId: [Int32](https://docs.microsoft.com/zh-cn/dotnet/api/System.Int32)) ||
|onHide|`回调` WindowEventDelegate(windowId: [Int32](https://docs.microsoft.com/zh-cn/dotnet/api/System.Int32)) ||
## 属性

|名称|类型|说明|
|---|---|---|
|Size|[Vector2](https://docs.unity3d.com/ScriptReference/Vector2.html) |获取或设置窗口大小|
|MinSize|[Vector2](https://docs.unity3d.com/ScriptReference/Vector2.html) |获取或设置窗口最小大小|
|Position|[Vector2](https://docs.unity3d.com/ScriptReference/Vector2.html) |获取或设置窗口位置|
|Icon|[Sprite](https://docs.unity3d.com/ScriptReference/Sprite.html) |窗口的图标|
|CanResize|boolean |窗口是否可以拖动改变大小|
|CanDrag|boolean |窗口是否可以拖动|
|CanClose|boolean |窗口是否可关闭|
|CanMin|boolean |窗口是否可以最小化|
|CanMax|boolean |窗口是否可以最大化|
|CloseAsHide|boolean |点击窗口关闭按钮是否替换为隐藏窗口|
|Title|string |窗口标题|
|WindowState|number [WindowState](./Ballance2.UI.Core.WindowState.md)|获取窗口当前状态|
|WindowType|number [WindowType](./Ballance2.UI.Core.WindowType.md)|获取窗口类型|

## 方法



### GetVisible()

获取窗口是否显示


#### 返回值

boolean <br/>


### SetVisible(visible)

设置窗口是否显示


#### 参数


`visible` boolean <br/>是否显示




### Destroy()

销毁窗口



### GetWindowId()

获取窗口ID


#### 返回值

number [int](../types.md)<br/>


### SetView(view)

设置窗口的自定义区域视图


#### 参数


`view` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>要绑定的子视图



#### 返回值

[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>


### GetView()

获取窗口的自定义区域已绑定的视图


#### 返回值

[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>


### GetRectTransform()

获取窗口本体的 RectTransform


#### 返回值

[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>


### ActiveWindow()

激活窗口，同 UIManager.ActiveWindow



### Close()

关闭并销毁窗口



### Show()

显示窗口



### Hide()

隐藏窗口



### MoveToCenter()

窗口居中

