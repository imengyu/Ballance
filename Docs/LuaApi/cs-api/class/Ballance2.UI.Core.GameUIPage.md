# Ballance2.UI.Core.GameUIPage 
UI页实例

## 注解

页与窗口不太一样，窗口可以同时打开多个，页只能同时显示一个，相当于独占的全屏窗口。要创建页，可以调用 `GameUIManager.RegisterPage` 函数。

## 字段

|名称|类型|说明|
|---|---|---|
|Content|[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) ||
|ContentHost|[RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) ||
|VerticalLayoutGroup|[VerticalLayoutGroup](https://docs.unity3d.com/ScriptReference/UI.VerticalLayoutGroup.html) ||
|HorizontalLayoutGroup|[HorizontalLayoutGroup](https://docs.unity3d.com/ScriptReference/UI.HorizontalLayoutGroup.html) ||
|PageName|string |页名称|
|CanEscBack|boolean |获取或设置是否可以按ESC键返回上一页|
|OnShow|`回调` OptionsDelegate(options: [Dictionary`2](https://docs.microsoft.com/zh-cn/dotnet/api/System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]])) |显示页事件|
|OnBackFromChild|`回调` OptionsDelegate(options: [Dictionary`2](https://docs.microsoft.com/zh-cn/dotnet/api/System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]])) |页由上一页返回时事件|
|OnHide|`回调` VoidDelegate() |隐藏页事件|
|LastOptions|table |页面上一次打开时所设置的参数|
|LastBackOptions|table |页面上由子页所返回设置的参数|

## 方法



### Show()

显示页。直接调用此函数会导致脱离 GameUIManager 的栈管理，推荐使用 GameUIManager 来控制页的显示与隐藏。



### Hide()

隐藏页。直接调用此函数会导致脱离 GameUIManager 的栈管理，推荐使用 GameUIManager 来控制页的显示与隐藏。



### CreateContent(package, resourceName)

创建指定资源的内容


#### 参数


`package` [GamePackage](./Ballance2.Package.GamePackage.md) <br/>资源所在模块

`resourceName` string <br/>资源模板路径




### CreateContent(package)

使用页名称自动创建指定资源的内容


#### 参数


`package` [GamePackage](./Ballance2.Package.GamePackage.md) <br/>资源所在模块




### CreateContent(prefab)

使用页预制体自动创建指定资源的内容


#### 参数


`prefab` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>预制体




### SetContent(content)

将内容设置至当前页的内容容器中


#### 参数


`content` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>内容RectTransform


