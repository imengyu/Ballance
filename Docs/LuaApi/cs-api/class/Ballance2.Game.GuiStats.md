# Ballance2.Game.GuiStats 
一个用于GUI显示状态的脚本。

## 注解

此类的作用是，因为在 Lua 中有时需要显示一些调试参数，显示调试参数一般在 C#中是使用 GUI.Label 显示一些数据的，
但在 Lua 中调用 GUI 相关函数是非常消耗性能的，所以就有了这个小工具。

它的作用是，当Lua需要显示某些调试数据在UI上是，Lua只需要告诉它需要显示什么数据，
然后它就会帮助你在C#端显示GUI或者设置Text，Lua可以等到数据变化时才更新到UI上，也可以选择定时更新，
不需要在 Lua 中频繁调用 GUI 而造成性能开销。


## 示例


使用示例，例如，我可以在UI上绑定一个 GuiStats 组件，然后在代码中获取它，添加相应的数据条目：
```lua
---添加相应的数据条目
local statPos = self._GuiStats:AddStat('Pos')

---当数据有更新，调用更新显示状态
statPos:SetVector3Value(self.transform.position)

---当不需要再显示这个数据，可以调用删除
statPos:Delete()
```


## 字段

|名称|类型|说明|
|---|---|---|
|text|[Text](https://docs.unity3d.com/ScriptReference/UI.Text.html) |如果使用Text模式，请设置要设置数据文字的目标Text|
|SetToText|boolean |是否使用Text模式，否则使用GUI模式，Text模式会将最终数据文字设置到Text上，而GUI模式是直接调用GUI显示在屏幕上|
|DisplayArea|[Rect](https://docs.unity3d.com/ScriptReference/Rect.html) |如果使用GUI模式，你可以通过设置这个来控制GUI显示区域|

## 方法



### AddStat(name)

添加状态条目


#### 参数


`name` string <br/>条目名称



#### 返回值

[GuiStatsValue](./Ballance2.Game.GuiStatsValue.md) <br/>返回条目的实例，可以使用它来更新数据


### DeleteStat(stat)

删除指定条目


#### 参数


`stat` [GuiStatsValue](./Ballance2.Game.GuiStatsValue.md) <br/>


