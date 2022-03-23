# Ballance2.Services.InputManager.EventTriggerListener 
UI 事件侦听器

## 注解

不再推荐使用。推荐使用UGUI的事件进行绑定。

## 字段

|名称|类型|说明|
|---|---|---|
|onClick|`回调` GameObjectDelegate(go: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)) |鼠标点击事件|
|onDown|`回调` GameObjectDelegate(go: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)) |鼠标按下事件|
|onEnter|`回调` GameObjectDelegate(go: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)) |鼠标进入事件|
|onExit|`回调` GameObjectDelegate(go: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)) |鼠标离开事件|
|onUp|`回调` GameObjectDelegate(go: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)) |鼠标离开事件|
|onSelect|`回调` GameObjectDelegate(go: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)) ||
|onUpdateSelect|`回调` GameObjectDelegate(go: [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html)) ||

## 方法



### `静态` Get(go)

从 指定 GameObject 创建事件侦听器


#### 参数


`go` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>指定 GameObject



#### 返回值

[EventTriggerListener](./Ballance2.Services.InputManager.EventTriggerListener.md) <br/>返回事件侦听器实例
