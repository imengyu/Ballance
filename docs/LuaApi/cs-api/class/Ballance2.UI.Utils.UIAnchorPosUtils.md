# Ballance2.UI.Utils.UIAnchorPosUtils 
UI 组件锚点位置工具

## 注解

UI 组件锚点位置工具可以方便的为您设置 
RectTransform 的 anchorMin、anchorMax、offsetMax、offsetMin
这几个令人头疼的变量了，在代码里也可以像编辑器里一样的参数来设置RectTransform了。

具体分为：Pivot（枢轴）、Anchor（锚点）、Left/Right/Top/Bottom. 与编辑器里显示的一致。

具体可参考每个函数的注释。



## 方法



### `静态` SetUIAnchor(rectTransform, x, y)

设置 UI 组件锚点


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件

`x` number [UIAnchor](./Ballance2.UI.Utils.UIAnchor.md)<br/>X 轴锚点

`y` number [UIAnchor](./Ballance2.UI.Utils.UIAnchor.md)<br/>Y 轴锚点




### `静态` GetUIAnchor(rectTransform)

获取 UI 组件锚点


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件



#### 返回值

[UIAnchor[]](./Ballance2.UI.Utils.UIAnchor[].md) <br/>


### `静态` GetUIAnchor(rectTransform, axis)

获取 UI 组件锚点


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件

`axis` number [Axis](https://docs.unity3d.com/ScriptReference/RectTransform+Axis.html)<br/>要获取的轴



#### 返回值

number [UIAnchor](./Ballance2.UI.Utils.UIAnchor.md)<br/>aaa


### `静态` SetUIRightTop(rectTransform, right, top)

设置 UI 组件 上 右 坐标


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>

`right` number [float](../types.md)<br/>

`top` number [float](../types.md)<br/>




### `静态` SetUILeftBottom(rectTransform, left, bottom)

设置 UI 组件 左 下坐标


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>

`left` number [float](../types.md)<br/>

`bottom` number [float](../types.md)<br/>




### `静态` SetUIPos(rectTransform, left, top, right, bottom)

设置 UI 组件 左 上 右 下坐标


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>

`left` number [float](../types.md)<br/>

`top` number [float](../types.md)<br/>

`right` number [float](../types.md)<br/>

`bottom` number [float](../types.md)<br/>




### `静态` SetUIRight(rectTransform, right)




#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>

`right` number [float](../types.md)<br/>




### `静态` SetUITop(rectTransform, top)

设置 UI 组件 上 坐标


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件

`top` number [float](../types.md)<br/>




### `静态` SetUILeft(rectTransform, left)

设置 UI 组件左坐标


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件

`left` number [float](../types.md)<br/>




### `静态` SetUIBottom(rectTransform, bottom)

设置 UI 组件下坐标


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件

`bottom` number [float](../types.md)<br/>




### `静态` GetUIRight(rectTransform)

获取组件的Right


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件



#### 返回值

number [float](../types.md)<br/>


### `静态` GetUITop(rectTransform)

获取组件的Top


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件



#### 返回值

number [float](../types.md)<br/>


### `静态` GetUILeft(rectTransform)

获取组件的Left


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件



#### 返回值

number [float](../types.md)<br/>


### `静态` GetUIBottom(rectTransform)

获取组件的Bottom


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件



#### 返回值

number [float](../types.md)<br/>


### `静态` SetUIPivot(rectTransform, pivot)

设置 UI 组件枢轴


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件

`pivot` number [UIPivot](./Ballance2.UI.Utils.UIPivot.md)<br/>轴锚点




### `静态` SetUIPivot(rectTransform, pivot, axis)

设置 UI 组件枢轴


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件

`pivot` number [UIPivot](./Ballance2.UI.Utils.UIPivot.md)<br/>

`axis` number [Axis](https://docs.unity3d.com/ScriptReference/RectTransform+Axis.html)<br/>




### `静态` GetUIPivot(rectTransform)

获取 UI 组件枢轴


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>UI 组件



#### 返回值

number [UIPivot](./Ballance2.UI.Utils.UIPivot.md)<br/>


### `静态` GetUIPivotLocationOffest(rectTransform, value, axis)

计算 UI 坐标值枢轴的偏移


#### 参数


`rectTransform` [RectTransform](https://docs.unity3d.com/ScriptReference/RectTransform.html) <br/>

`value` number [float](../types.md)<br/>

`axis` number [Axis](https://docs.unity3d.com/ScriptReference/RectTransform+Axis.html)<br/>



#### 返回值

number [float](../types.md)<br/>
