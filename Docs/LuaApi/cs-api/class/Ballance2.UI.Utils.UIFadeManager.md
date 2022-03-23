# Ballance2.UI.Utils.UIFadeManager 
渐变自管理类

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
