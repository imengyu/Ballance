# Ballance2.Game.CamFollow 
摄像机跟随脚本

## 注解

摄像机跟随脚本，用于Ballance球摄像机跟随的核心内容。

## 字段

|名称|类型|说明|
|---|---|---|
|SmoothTime|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) ||
|CamTargetSmoothTime|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) ||
|CamLookTargetSmoothTime|[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html) ||
|CamLookTarget|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) ||
|CamTarget|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) ||
|CamOrient|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) ||
|CamPos|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) ||
|InGameCam|[Camera](https://docs.unity3d.com/ScriptReference/Camera.html) ||
|cameraMovePosGetPosTime|number [float](../types.md)||
## 属性

|名称|类型|说明|
|---|---|---|
|Follow|boolean |指定摄像机跟随球是否开启|
|Look|boolean |指定摄像机看着球是否开启|
|Target|[Transform](https://docs.unity3d.com/ScriptReference/Transform.html) |指定当前跟踪的目标|

## 方法



### SetTargetWithoutUpdatePos(t)

设置跟踪的目标，不更新位置


#### 参数


`t` [Transform](https://docs.unity3d.com/ScriptReference/Transform.html) <br/>


