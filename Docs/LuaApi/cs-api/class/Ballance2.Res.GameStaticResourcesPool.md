# Ballance2.Res.GameStaticResourcesPool 
游戏静态资源池

## 注解

可在 GameEntry 中配置静态引用资源，打包后无需加载，可使用本工具类直接获取。但太多静态引用资源会导致游戏启动变慢。

## 属性

|名称|类型|说明|
|---|---|---|
|PrefabUIEmpty|[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) ||
|PrefabEmpty|[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) ||
|GamePrefab|table |静态引入 Prefab|
|GameAssets|table |静态引入资源|

## 方法



### `静态` FindStaticPrefabs(name)

在静态引入资源中查找指定名称的预制体资源


#### 参数


`name` string <br/>资源名称



#### 返回值

[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>如果找到指定预制体资源，则返回预制体实例，否则返回null


### `静态` FindStaticAssets(name)

在静态引入资源中查找指定名称的资源


#### 参数


`name` string <br/>资源名称



#### 返回值

[Object](https://docs.unity3d.com/ScriptReference/Object.html) <br/>如果找到指定资源，则返回资源实例，否则返回null
