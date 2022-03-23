# Ballance2.Services.GameUIManager 
UI 管理器，用于管理UI通用功能

## 注解

UI 管理器提供了者几种种UI通用功能：
* Window 为游戏提供了一个可以拖拽，调整大小的窗口，用于游戏内部某些UI的使用。要创建窗口，可以调用 `GameUIManager.CreateWindow` 函数。
* Page 页与窗口不太一样，窗口可以同时打开多个，页只能同时显示一个，相当于独占的全屏窗口。要创建页，可以调用 `GameUIManager.RegisterPage` 函数。
* MaskBlack 全局的黑色转场渐变遮罩。
* MaskWhite 全局的白色转场渐变遮罩。
* GlobalAlert 全局的弹出独占对话框。


## 字段

|名称|类型|说明|
|---|---|---|
|UIRoot|[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) |UI 根 Canvas 的 RectTransform|
|UIFadeManager|[UIFadeManager](./Ballance2.UI.Utils.UIFadeManager.md) |渐变管理器|
## 属性

|名称|类型|说明|
|---|---|---|
|UIRootRectTransform|[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) |UI 根 RectTransform|

## 方法



### SetUIOverlayVisible(visible)

切换遮罩UI是否显示


#### 参数


`visible` boolean <br/>是否显示




### WaitKey(code, pressedOrReleased, callback)

侦听某个键盘按键一次


#### 参数


`code` number [KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html)<br/>

`pressedOrReleased` boolean <br/>如果为true，则侦听按下事件，否则侦听松开事件

`callback` `回调` VoidDelegate() <br/>



#### 返回值

number [int](../types.md)<br/>返回一个ID, 可使用 DeleteKeyListen 删除侦听


### ListenKey(key, callBack)

添加键盘按键侦听


#### 参数


`key` number [KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html)<br/>键值

`callBack` `回调` KeyDelegate(key: [KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html), downed: [Boolean](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean)) <br/>回调函数



#### 返回值

number [int](../types.md)<br/>返回一个ID, 可使用 DeleteKeyListen 删除侦听


### DeleteKeyListen(id)

删除侦听按键


#### 参数


`id` number [int](../types.md)<br/>AddKeyListen 返回的ID




### IsUiFocus()

获取当前鼠标是否在UI上


#### 返回值

boolean <br/>


### GetUIPrefab(name, type)

获取 UI 控件预制体


#### 参数


`name` string <br/>名称

`type` number [GameUIPrefabType](./Ballance2.Services.GameUIPrefabType.md)<br/>



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>如果找到指定名称预制体，则返回其实例，如果未找到，则返回 null


### RegisterUIPrefab(name, type, perfab)

注册 UI 控件预制体


#### 参数


`name` string <br/>名称

`type` number [GameUIPrefabType](./Ballance2.Services.GameUIPrefabType.md)<br/>

`perfab` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>预制体



#### 返回值

boolean <br/>返回注册是否成功


### RemoveUIPrefab(name)

清除已注册的 UI 控件预制体


#### 参数


`name` string <br/>名称



#### 返回值

boolean <br/>返回是否成功


### FindPage(name)

查找页


#### 参数


`name` string <br/>页名称



#### 返回值

[GameUIPage](./Ballance2.UI.Core.GameUIPage.md) <br/>返回找到的页实例，如果找不到则返回null


### RegisterPage(name, prefabName)

注册页


#### 参数


`name` string <br/>页名称

`prefabName` string <br/>页模板名称



#### 返回值

[GameUIPage](./Ballance2.UI.Core.GameUIPage.md) <br/>返回新创建的页实例，如果失败则返回null，请查看LastError


### GoPage(name)

跳转到页


#### 参数


`name` string <br/>页名称



#### 返回值

boolean <br/>返回跳转是否成功


### GoPageWithOptions(name, options)

跳转到页并携带参数


#### 参数


`name` string <br/>页名称

`options` table <br/>打开页所需要携带的参数，在页中可以使用 `GameUIPage.LastOption` 读取到传递进入页的参数。



#### 返回值

boolean <br/>返回跳转是否成功


### GetCurrentPage()

获取当前显示的页实例


#### 返回值

[GameUIPage](./Ballance2.UI.Core.GameUIPage.md) <br/>


### HideCurrentPage()

隐藏当前显示的页



### CloseAllPage()

关闭所有显示的页



### BackPreviusPage()

返回上一页


#### 返回值

boolean <br/>如果可以返回，则返回true，否则返回false


### BackPreviusPageWithOptions(options)

返回上一页并携带参数


#### 参数


`options` table <br/>传递返回上一页参数，页中可以使用 `GameUIPage.LastBackOptions` 读取到传递进入上一页的参数。



#### 返回值

boolean <br/>如果可以返回，则返回true，否则返回false


### UnRegisterPage(name)

取消注册页


#### 参数


`name` string <br/>页名称



#### 返回值

boolean <br/>返回是否成功


### GlobalToast(text)

显示全局土司提示


#### 参数


`text` string <br/>提示文字




### GlobalToast(text, showSec)

显示全局土司提示


#### 参数


`text` string <br/>提示文字

`showSec` number [float](../types.md)<br/>显示时长（秒）




### GlobalAlertWindow(text, title, onConfirm, okText)

显示全局 Alert 独占对话框


#### 参数


`text` string <br/>内容

`title` string <br/>标题

`onConfirm` `回调` VoidDelegate() <br/>OK 按钮点击回调

`okText` string <br/>OK 按钮文字



#### 返回值

number [int](../types.md)<br/>返回对话框ID


### GlobalConfirmWindow(text, title, onConfirm, onCancel, okText, cancelText)

显示全局 Confirm 独占对话框


#### 参数


`text` string <br/>内容

`title` string <br/>标题

`onConfirm` `回调` VoidDelegate() <br/>OK 按钮点击回调

`onCancel` `回调` VoidDelegate() <br/>Cancel 按扭点击回调

`okText` string <br/>OK 按钮文字

`cancelText` string <br/>Cancel 按钮文字



#### 返回值

number [int](../types.md)<br/>返回对话框ID


### CreateWindow(title, customView)

创建自定义窗口（默认不显示）


#### 参数


`title` string <br/>标题

`customView` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>窗口自定义内容View



#### 返回值

[Window](./Ballance2.UI.Core.Window.md) <br/>返回窗口实例


### CreateWindow(title, customView, show)

创建自定义窗口


#### 参数


`title` string <br/>标题

`customView` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>窗口自定义内容View

`show` boolean <br/>创建后是否立即显示



#### 返回值

[Window](./Ballance2.UI.Core.Window.md) <br/>返回窗口实例


### CreateWindow(title, customView, show, x, y, w, h)

创建自定义窗口


#### 参数


`title` string <br/>标题

`customView` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>窗口自定义内容View

`show` boolean <br/>创建后是否立即显示

`x` number [float](../types.md)<br/>X 坐标

`y` number [float](../types.md)<br/>Y 坐标

`w` number [float](../types.md)<br/>宽度，0 使用默认

`h` number [float](../types.md)<br/>高度，0 使用默认



#### 返回值

[Window](./Ballance2.UI.Core.Window.md) <br/>返回窗口实例


### RegisterWindow(window)

注册窗口到管理器中


#### 参数


`window` [Window](./Ballance2.UI.Core.Window.md) <br/>窗口实例



#### 返回值

[Window](./Ballance2.UI.Core.Window.md) <br/>返回窗口实例


### FindWindowById(windowId)

通过 ID 查找窗口


#### 参数


`windowId` number [int](../types.md)<br/>窗口ID



#### 返回值

[Window](./Ballance2.UI.Core.Window.md) <br/>返回找到的窗口实例，如果找不到则返回null


### GetCurrentActiveWindow()

获取当前激活的窗口


#### 返回值

[Window](./Ballance2.UI.Core.Window.md) <br/>


### ShowWindow(window)

显示窗口


#### 参数


`window` [Window](./Ballance2.UI.Core.Window.md) <br/>窗口实例




### HideWindow(window)

隐藏窗口


#### 参数


`window` [Window](./Ballance2.UI.Core.Window.md) <br/>窗口实例




### CloseWindow(window)

关闭窗口


#### 参数


`window` [Window](./Ballance2.UI.Core.Window.md) <br/>窗口实例




### ActiveWindow(window)

激活窗口至最顶层


#### 参数


`window` [Window](./Ballance2.UI.Core.Window.md) <br/>窗口实例




### MaskBlackSet(show)

全局黑色遮罩隐藏（无渐变动画）


#### 参数


`show` boolean <br/>




### MaskWhiteSet(show)

全局白色遮罩控制（无渐变动画）


#### 参数


`show` boolean <br/>为true则显示遮罩，否则隐藏




### MaskBlackFadeIn(second)

全局黑色遮罩渐变淡入


#### 参数


`second` number [float](../types.md)<br/>耗时（秒）




### MaskWhiteFadeIn(second)

全局白色遮罩渐变淡入


#### 参数


`second` number [float](../types.md)<br/>耗时（秒）




### MaskBlackFadeOut(second)

全局黑色遮罩渐变淡出


#### 参数


`second` number [float](../types.md)<br/>耗时（秒）




### MaskWhiteFadeOut(second)

全局白色遮罩渐变淡出


#### 参数


`second` number [float](../types.md)<br/>耗时（秒）




### SetViewToTemporarily(view)

设置一个UI至临时区域


#### 参数


`view` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>指定UI




### AttatchViewToCanvas(view)

将一个UI附加到主Canvas


#### 参数


`view` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>指定UI




### InitViewToCanvas(prefab, name, topMost)

使用Prefab初始化一个对象并附加到主Canvas


#### 参数


`prefab` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>Prefab

`name` string <br/>新对象名称

`topMost` boolean <br/>是否置顶，置顶后会在遮罩层上出现，不会被遮挡



#### 返回值

[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>返回新对象的RectTransform


### CreateUIMessageCenter(name)

创建一个UI消息中心


#### 参数


`name` string <br/>名称



#### 返回值

[GameUIMessageCenter](./Ballance2.UI.Core.GameUIMessageCenter.md) <br/>返回UI消息中心实例


### DestroyUIMessageCenter(name)

销毁一个UI消息中心


#### 参数


`name` string <br/>名称



#### 返回值

boolean <br/>返回是否成功
