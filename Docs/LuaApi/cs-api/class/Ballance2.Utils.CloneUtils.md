# Ballance2.Utils.CloneUtils 
克隆工具类

## 注解

提供了一些工具方法，可方便的克隆出空物体，或是使用已有Prefab克隆出新物体。


## 方法



### `静态` CloneNewObject(prefab, name)

使用 Prefab 克隆一个新的对象


#### 参数


`prefab` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>Prefab

`name` string <br/>新的对象的名字



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回生成的新对象


### `静态` CloneNewObjectWithParent(prefab, parent, name)

使用 Prefab 克隆一个新的对象，并添加至指定变换的子级


#### 参数


`prefab` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>Prefab

`parent` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>父级对象

`name` string <br/>新的对象的名字



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回生成的新对象


### `静态` CloneNewObjectWithParent(prefab, parent)

使用 Prefab 克隆一个新的对象，并添加至指定变换的子级


#### 参数


`prefab` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>Prefab

`parent` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>父级对象



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回生成的新对象


### `静态` CloneNewObjectWithParent(prefab, parent, name, active)

使用 Prefab 克隆一个新的对象，并添加至指定变换的子级


#### 参数


`prefab` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>Prefab

`parent` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>父级对象

`name` string <br/>新的对象的名字

`active` boolean <br/>是否激活该对象



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回生成的新对象


### `静态` CreateEmptyObject(name)

克隆一个新的空对象


#### 参数


`name` string <br/>新的对象的名字



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回生成的新对象


### `静态` CreateEmptyObjectWithParent(parent, name)

克隆一个新的空对象，并添加至指定变换的子级


#### 参数


`parent` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>指定父级对象

`name` string <br/>新的对象的名字



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回生成的新对象


### `静态` CreateEmptyObjectWithParent(parent)

克隆一个新的空对象，并添加至指定变换的子级


#### 参数


`parent` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>指定父级对象



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>指定父级对象


### `静态` CreateEmptyUIObject(name)

生成一个空的UI对象


#### 参数


`name` string <br/>指定新对象的名称



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回生成的新对象


### `静态` CreateEmptyUIObjectWithParent(parent, name)

生成一个空的UI对象并添加至指定变换的子级


#### 参数


`parent` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>指定父级对象

`name` string <br/>指定新对象的名称



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回生成的新对象


### `静态` CreateEmptyUIObjectWithParent(parent)

生成一个空的UI对象并添加至指定变换的子级


#### 参数


`parent` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>指定父级对象



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>返回生成的新对象
