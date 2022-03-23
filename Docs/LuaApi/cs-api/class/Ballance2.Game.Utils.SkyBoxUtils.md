# Ballance2.Game.Utils.SkyBoxUtils 
天空盒生成器


## 方法



### `静态` MakeSkyBox(s)

创建预制的天空盒


#### 参数


`s` string <br/>天空盒名字，（必须是 A~K ，对应原版游戏11个天空）



#### 返回值

[Material](https://docs.unity3d.com/ScriptReference/Material.html) <br/>返回创建好的天空盒材质


### `静态` MakeCustomSkyBox(SkyLeft, SkyRight, SkyFront, SkyBack, SkyDown, SkyTop)

创建自定义天空盒


#### 参数


`SkyLeft` [Texture](https://docs.unity3d.com/ScriptReference/Texture.html) <br/>左边的图像

`SkyRight` [Texture](https://docs.unity3d.com/ScriptReference/Texture.html) <br/>右边的图像

`SkyFront` [Texture](https://docs.unity3d.com/ScriptReference/Texture.html) <br/>前边的图像

`SkyBack` [Texture](https://docs.unity3d.com/ScriptReference/Texture.html) <br/>后边的图像

`SkyDown` [Texture](https://docs.unity3d.com/ScriptReference/Texture.html) <br/>下边的图像

`SkyTop` [Texture](https://docs.unity3d.com/ScriptReference/Texture.html) <br/>上边的图像



#### 返回值

[Material](https://docs.unity3d.com/ScriptReference/Material.html) <br/>返回创建好的天空盒材质
