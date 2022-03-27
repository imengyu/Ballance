# Ball

基础球定义, 可继承此类来重写你自己的球

## 属性

|名称|类型|说明|
|---|---|---|
|CurrentBallName|string|获取当前的球名称|


## 配置

这些配置都是可以更改的，需要在 new 函数中修改，可以使用 `self.*` 来访问。

也可以在编辑器中手动添加变量来设置。

|名称|值|说明|
|---|---|---|
|_PiecesData|`table or nil`|当前球的碎片数据|
|_Pieces|`GameObject`|当前球的碎片父级组对象|
|_PiecesMinForce|number|碎片抛出最小推力|
|_PiecesMaxForce|number|碎片抛出最大推力|
|_PiecesPhysicsData|`table or nil`|碎片物理数据|
|_PiecesPhysCallback|`function`|物理化碎片自定义处理回调 (gameObject, physicsData) -> PhysicsObject，如果为nil，则碎片将使用默认参数来物理化|
|_PiecesHaveColSound|`string[]`|指定要为碎片添加碰撞声音的物体名称|
|_UpForce|number|球调试升高力|
|_DownForce|number|球调试下降力|
|_HitSound|table|球的碰撞声音配置|
|_RollSound|table|球的滚动声音配置|

### _HitSound 定义

|名称|值|说明|
|---|---|---|
|Names|`table`|球与其他声音组碰撞的声音配置，参见下方|
|MinSpeed|`string`|碰撞最小速度|
|MaxSpeed|`string`|碰撞最大速度，最终碰撞声音的音量是 `(absSpeed - MinSpeed) / (MaxSpeed - MinSpeed)` |

### _RollSound 定义

|名称|值|说明|
|---|---|---|
|Names|`table`|球与其他声音组滚动的声音配置，参见下方|
|PitchBase|`string`|最低声音变速|
|PitchFactor|`string`|声音变速系数，声音变速计算方式是 `PitchBase + (speed * PitchFactor)` |
|VolumeBase|`string`|最低声音音量|
|VolumeFactor|`string`|声音音量系数，声音音量计算方式是 `VolumeBase + (speed * VolumeFactor)` |

### 声音 Names 定义

这是一个table，key是当前球与哪个声音组碰撞，value与之碰撞播放的声音。

All是特殊的配置，如果有配置All，球与任何声音组碰撞，都会播放All配置的声音。

```lua
Names = {
  All = '',
  Dome = '',
  Metal = '',
  Stone = '',
  Wood = '',
  Paper = '',
},
```

## 可继承方法

### Active()

当前球激活事件

### Deactive()

当前球取消激活事件

### GetPieces()

当前球获取碎片

#### 返回

GameObject <br>需要返回当前球所属碎片的父级组实例，这是一个游戏对象，游戏对象一级下是所有碎片的物体。不返回则此球没有碎片相关功能。

### ThrowPieces(pos)

丢出此类的碎片事件

#### 参数

`pos` Vector3 <br>碎片抛出位置

?>  如果不继承此方法，则会调用 `BallPiecesControll:ThrowPieces` 运行默认的碎片效果。

### ResetPieces()

回收此类的碎片事件

?> 如果不继承此方法，则会调用 `BallPiecesControll:ResetPieces` 运行默认的碎片效果。