# Ballance2.UI.Utils.FadeObject 
渐变管理对象信息

## 字段

|名称|类型|说明|
|---|---|---|
|gameObject|[GameObject](https://docs.unity3d.com/ScriptReference/GameObject.html) |渐变所属对象|
|material|[Material](https://docs.unity3d.com/ScriptReference/Material.html) |渐变控制材质|
|materials|[Material[]](https://docs.unity3d.com/ScriptReference/Material[].html) |渐变控制材质数组|
|image|[Image](https://docs.unity3d.com/ScriptReference/UI.Image.html) |渐变控制UI图像|
|text|[Text](https://docs.unity3d.com/ScriptReference/UI.Text.html) |渐变控制UI文字|
|audio|[AudioSource](https://docs.unity3d.com/ScriptReference/AudioSource.html) |渐变控制音量|
|alpha|number [float](../types.md)|当前透明度值|
|timeInSecond|number [float](../types.md)|渐变时长(秒)|
|endReactive|boolean |结束后是否重新激活对象|
|runEnd|boolean |渐变是否已经结束|
|oldMatRenderMode|number [RenderingMode](./Ballance2.Utils.MaterialUtils+RenderingMode.md)|渐变开始之前的材质渲染类型|
|fadeType|number [FadeType](./Ballance2.UI.Utils.FadeType.md)|渐变类型|
## 属性

|名称|类型|说明|
|---|---|---|
|fadeManager|[UIFadeManager](./Ballance2.UI.Utils.UIFadeManager.md) |渐变所属管理器|

## 方法



### ResetTo(alpha)

强制重置当前渐变透明度至指定的值


#### 参数


`alpha` number [float](../types.md)<br/>透明度值




### Delete()

移除当前渐变

