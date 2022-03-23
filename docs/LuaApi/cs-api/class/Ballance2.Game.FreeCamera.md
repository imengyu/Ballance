# Ballance2.Game.FreeCamera 
自由摄像机脚本，包含了方向键移动，鼠标左键拖动，右键旋转视图，滚轮缩放功能。

## 字段

|名称|类型|说明|
|---|---|---|
|Wireframe|boolean |是否启用线框渲染|
|Audio|boolean |是否启用当前摄像机上的 AudioListener|
|Fog|boolean |是否启用雾渲染|
|SkyBox|boolean |是否启用天空盒渲染|
|Instance|[FreeCamera](./Ballance2.Game.FreeCamera.md) |获取当前摄像机的一个实例。|
|onCamSpeedChanged|`回调` VoidDelegate() |摄像机速度更改事件|
|cameraSpeed|number [float](../types.md)|摄像机键盘移动速度|
|m_moveSensitivityX|number [float](../types.md)|摄像机鼠标水平移动速度|
|m_moveSensitivityY|number [float](../types.md)|摄像机鼠标垂直移动速度|
|m_moveSensitivityZoom|number [float](../types.md)|摄像机缩放速度|
|m_sensitivityX|number [float](../types.md)|摄像机水平移动速度|
|m_sensitivityY|number [float](../types.md)|摄像机垂直移动速度|
|m_minimumX|number [float](../types.md)|摄像机转向最小角度|
|m_maximumX|number [float](../types.md)|摄像机转向最大角度|
|m_minimumY|number [float](../types.md)|镜头转向垂直最小仰角|
|m_maximumY|number [float](../types.md)|镜头转向垂直最大仰角|
