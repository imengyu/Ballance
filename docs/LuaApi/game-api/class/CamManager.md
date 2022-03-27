# CamManager

摄像机管理器，负责游戏中的摄像机运动。

## 属性

|名称|类型|说明|
|---|---|---|
|CamDirectionRef|Transform|获取球参照的摄像机旋转方向变换|
|CamRightVector|Vector3|获取摄像机右侧向量|
|CamLeftVector|Vector3|获取摄像机左侧向量|
|CamForwerdVector|Vector3|获取摄像机向前向量|
|CamBackVector|Vector3|获取摄像机向后向量|
|CamFollowSpeed|number|获取或设置摄像机跟随速度|
|CamIsSpaced|boolean|获取摄像机是否空格键升高了|
|Target|Transform|获取或者设置摄像机跟随物体|
|CamRotateValue|number|获取当前摄像机方向（0-3, CamRotateType）设置请使用 `RotateTo` 方法|
|CamFollow|[CamFollow](../../cs-api/class/Ballance2.Game.CamFollow)|获取摄像机跟随脚本|

## 定义

### CamRotateType 摄像机旋转方向

|名称|值|说明|
|---|---|---|
|North|0|北。面向+X轴。|
|East|1|东。面向-Z轴。|
|South|2|南。面向-X轴。|
|West|3|西。面向+Z轴。|

## 方法

### CamManager:ResetVector()

摄像机面对向量重置

### CamManager:GetRotateDegreeByType(type)

通过旋转方向获取目标角度

#### 参数

`type` number <br/>旋转方向 CamRotateType

### CamManager:SetPosAndDirByRestPoint(go)

通过RestPoint占位符设置摄像机的方向和位置

?> 注意，摄像机旋转是固定90°一个方向，如果RestPoint占位符旋转不能被90整除，此函数会选择一个最接近占位符的旋转度数。

#### 参数

`go` GameObject <br/>RestPoint占位符

### CamManager:RotateUp(enable)

空格键向上旋转

#### 参数

`enable` boolean <br/>状态

### CamManager:RotateTo(val)

摄像机旋转指定角度

#### 参数

`val` number <br/>旋转方向 CamRotateType

### CamManager:RotateRight()

摄像机向右旋转

### CamManager:RotateLeft()

摄像机向左旋转

### CamManager:SetSkyBox(mat)

设置主摄像机天空盒材质

#### 参数

`mat` Material <br/>

### CamManager:SetCamFollow(enable)

指定摄像机是否开启跟随球

#### 参数

`enable` boolean <br/>

### CamManager:SetCamLook(enable)

指定摄像机是否开启看着球

#### 参数

`enable` boolean <br/>

### CamManager:SetTarget(enable)

指定当前摄像机跟踪的目标

#### 参数

`target` Transform <br/>

### CamManager:DisbleAll()

禁用所有摄像机功能