# Ballance2.Services.InputManager.KeyListener 
键盘按键事件事件侦听器

## 注解

此类可以方便的侦听键盘按键事件。

```lua
local lisnster = KeyListener.Get(self.gameObject)
lisnster:AddKeyListen(KeyCode.A, function(key, downed) 
  if downed then
    ---当 A 键按下时将会发出此事件
  end
end)
```

如果你只需要单次监听某个按键，无须单独使用此类，可以直接使用 GameUIManager 上的 WaitKey，具体参见 [GameUIManager](Ballance2.Services.GameUIManager) 。


## 字段

|名称|类型|说明|
|---|---|---|
|DisableWhenUIFocused|boolean |如果UI激活时是否禁用键盘事件|
|AllowMultipleKey|boolean |指定是否允许同时发出1个以上的键盘事件，否则同时只能发送一个键盘事件。以后注册的先发送|
## 属性

|名称|类型|说明|
|---|---|---|
|IsListenKey|boolean |是否开启监听|

## 方法



### `静态` Get(go)

从 指定 GameObject 创建键事件侦听器


#### 参数


`go` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>指定 GameObject



#### 返回值

[KeyListener](./Ballance2.Services.InputManager.KeyListener.md) <br/>返回事件侦听器实例


### ReSendPressingKey()

重新发送当前已按下的按键事件



### AddKeyListen(key, key2, callBack)

添加侦听器侦听键，可以一次监听两个键。


#### 参数


`key` number [KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html)<br/>键值

`key2` number [KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html)<br/>键值2

`callBack` `回调` KeyDelegate(key: [KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html), downed: [Boolean](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean)) <br/>回调函数



#### 返回值

number [int](../types.md)<br/>返回一个ID, 可使用 DeleteKeyListen 删除侦听器


### AddKeyListen(key, callBack)

添加侦听器侦听键。


#### 参数


`key` number [KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html)<br/>键值

`callBack` `回调` KeyDelegate(key: [KeyCode](https://docs.unity3d.com/ScriptReference/KeyCode.html), downed: [Boolean](https://docs.microsoft.com/zh-cn/dotnet/api/System.Boolean)) <br/>回调函数



#### 返回值

number [int](../types.md)<br/>返回一个ID, 可使用 DeleteKeyListen 删除侦听器


### DeleteKeyListen(id)

删除指定侦听器。


#### 参数


`id` number [int](../types.md)<br/>AddKeyListen 返回的ID




### ClearKeyListen()

清空事件侦听器所有侦听键。

