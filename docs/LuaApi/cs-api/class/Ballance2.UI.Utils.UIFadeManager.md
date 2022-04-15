# Ballance2.UI.Utils.UIFadeManager 
渐变自管理类

## 注解

负责渐变自动化脚本的管理与执行。

该组件有绑定一个实例至 [GameUIManager](Ballance2.Services.GameUIManager) 上，可使用 `GameUIManager.UIFadeManager` 快速访问。

## 示例

渐变工具，可以对多种物体进行渐变，显示/隐藏。
目前支持Material，Material数组，Image，Text，AudioSource的淡出淡入效果。

注意，如果多个物体共享同一个材质，运行渐变后会导致全部使用此材质的物体都会改变效果，如果需要单独渐变，要在对象上用一个单独的材质。

使用方法，可以调用

```lua
--产生一个渐变，对象是当前物体，并且渐变完成后自动隐藏物体
--最后一个参数为null表示它会自动去当前物体的MeshRenderer上查找材质来进行渐变。
local fadeObject = GameUIManager.UIFadeManager:AddFadeOut(self.gameObject, 1.0, true, null);

--创建的fadeObject随时可以停止，或者设置当前的渐变值。
fadeObject:Delete(); --停止
fadeObject:ResetTo(0.5f); --设置当前的渐变值为 0.5f
```

## 字段

|名称|类型|说明|
|---|---|---|
|fadeObjects|table |当前的渐变对象|

## 方法



### AddFadeOut(gameObject, timeInSecond, hidden, material)

运行淡出动画


#### 参数


`gameObject` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>执行对象

`timeInSecond` number [float](../types.md)<br/>执行时间

`hidden` boolean <br/>执行完毕是否将对象设置为不激活

`material` [Material](https://docs.unity3d.com/ScriptReference/Material.html) <br/>执行材质



#### 返回值

[FadeObject](./Ballance2.UI.Utils.FadeObject.md) <br/>


### AddFadeIn(gameObject, timeInSecond, material)

运行淡入动画


#### 参数


`gameObject` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>执行对象

`timeInSecond` number [float](../types.md)<br/>执行时间

`material` [Material](https://docs.unity3d.com/ScriptReference/Material.html) <br/>执行材质



#### 返回值

[FadeObject](./Ballance2.UI.Utils.FadeObject.md) <br/>


### AddFadeOut2(gameObject, timeInSecond, hidden, materials)

运行淡出动画


#### 参数


`gameObject` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>执行对象

`timeInSecond` number [float](../types.md)<br/>执行时间

`hidden` boolean <br/>执行完毕是否将对象设置为不激活

`materials` [Material[]](https://docs.unity3d.com/ScriptReference/Material[].html) <br/>执行材质数组



#### 返回值

[FadeObject](./Ballance2.UI.Utils.FadeObject.md) <br/>


### AddFadeIn2(gameObject, timeInSecond, materials)

运行淡入动画


#### 参数


`gameObject` [GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) <br/>执行对象

`timeInSecond` number [float](../types.md)<br/>执行时间

`materials` [Material[]](https://docs.unity3d.com/ScriptReference/Material[].html) <br/>执行材质数组



#### 返回值

[FadeObject](./Ballance2.UI.Utils.FadeObject.md) <br/>


### AddFadeOut(image, timeInSecond, hidden)

运行淡出动画


#### 参数


`image` [Image](https://docs.unity3d.com/ScriptReference/UI.Image.html) <br/>执行对象

`timeInSecond` number [float](../types.md)<br/>执行时间

`hidden` boolean <br/>执行完毕是否将对象设置为不激活



#### 返回值

[FadeObject](./Ballance2.UI.Utils.FadeObject.md) <br/>


### AddFadeIn(image, timeInSecond)

运行淡入动画


#### 参数


`image` [Image](https://docs.unity3d.com/ScriptReference/UI.Image.html) <br/>执行对象

`timeInSecond` number [float](../types.md)<br/>执行时间



#### 返回值

[FadeObject](./Ballance2.UI.Utils.FadeObject.md) <br/>


### AddFadeOut(text, timeInSecond, hidden)

运行淡出动画


#### 参数


`text` [Text](https://docs.unity3d.com/ScriptReference/UI.Text.html) <br/>执行对象

`timeInSecond` number [float](../types.md)<br/>执行时间

`hidden` boolean <br/>执行完毕是否将对象设置为不激活



#### 返回值

[FadeObject](./Ballance2.UI.Utils.FadeObject.md) <br/>


### AddFadeIn(text, timeInSecond)

运行淡入动画


#### 参数


`text` [Text](https://docs.unity3d.com/ScriptReference/UI.Text.html) <br/>执行对象

`timeInSecond` number [float](../types.md)<br/>执行时间



#### 返回值

[FadeObject](./Ballance2.UI.Utils.FadeObject.md) <br/>


### AddAudioFadeOut(audio, timeInSecond)

运行声音淡出


#### 参数


`audio` [AudioSource](https://docs.unity3d.com/ScriptReference/AudioSource.html) <br/>执行对象

`timeInSecond` number [float](../types.md)<br/>执行时间



#### 返回值

[FadeObject](./Ballance2.UI.Utils.FadeObject.md) <br/>


### AddAudioFadeIn(audio, timeInSecond)

运行声音淡入


#### 参数


`audio` [AudioSource](https://docs.unity3d.com/ScriptReference/AudioSource.html) <br/>执行对象

`timeInSecond` number [float](../types.md)<br/>执行时间



#### 返回值

[FadeObject](./Ballance2.UI.Utils.FadeObject.md) <br/>
