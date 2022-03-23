# Ballance2.Utils.ObjectStateBackupUtils 
对象变换状态保存器

## 注解

Virtools 中有一个叫做IC的功能，可以保存物体的初始状态，设置了IC，可以很方便的恢复物体的初始状态，Ballance中很多模块需要IC的功能以重复恢复初始状态，因此设计了此对象变换状态保存器。

目前可保存物体的旋转与位置信息，可选保存单个物体，或是对象和他的一级子对象，可满足大部分使用需求。
  

## 示例

例如，在初始化的时候使用 `BackUpObject` 保存当前物体状态：
```lua
BackUpObject(self.gameObject)
```
然后，在需要恢复当前物体状态时可以调用 `RestoreObject` 恢复：
```lua
RestoreObject(self.gameObject)
```



## 方法



### `静态` ClearAll()

由 `GameManager` 调用。手动调用将清空所有信息。



### `静态` ClearObjectBackUp(gameObject)

清除对象的备份


#### 参数


`gameObject` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>要操作的游戏对象




### `静态` BackUpObject(gameObject)

备份对象的变换状态


#### 参数


`gameObject` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>要备份的游戏对象




### `静态` BackUpObjectAndChilds(gameObject)

备份对象和他的一级子对象的变换状态


#### 参数


`gameObject` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>要备份的游戏对象




### `静态` RestoreObject(gameObject)

从备份还原对象的变换状态


#### 参数


`gameObject` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>要还原的游戏对象




### `静态` RestoreObjectAndChilds(gameObject)

从备份还原对象和他的一级子对象的变换状态


#### 参数


`gameObject` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>要还原的游戏对象


