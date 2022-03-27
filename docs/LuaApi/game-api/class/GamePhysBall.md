# BallManager

游戏球物理参数

如何添加自己的球物理参数：
请在注册球之前插入物理参数，BallName是你的球名称，与注册球时的名称一致：
```lua
GamePhysBall['BallName'] = {
  Force = 0.065,
  Friction = 0.5,
  Elasticity = 0.4,
  Mass = 0.2,
  LinearDamp = 1.5,
  RotDamp = 0.1,
  PiecesMinForce = 0.1,
  PiecesMaxForce = 0.1,
  UpForce = 0.1,
  DownForce = 0.01,
}
```

默认包含了三种原版球的物理定义：BallWood、BallStone和BallPaper。

## 球的物理参数定义结构

|名称|值|说明|
|---|---|---|
|Force|number|球推动力|
|Friction|number|球的摩擦力系数|
|Elasticity|number|球的弹力系数|
|Mass|number|球的质量|
|LinearDamp|number|线性阻尼|
|Layer|number|碰撞层。默认`GameLayers.LAYER_PHY_BALL`|
|RotDamp|number|旋转阻尼|
|PiecesMinForce|number|碎片爆炸最小的力|
|PiecesMaxForce|number|碎片爆炸最大的力|
|PiecesPhysicsData|table|碎片的物理化参数,见下方|
|UpForce|number|球向上的力。仅调试中有效|
|BallRadius|number|球半径。如果为0则使用convex mesh|
|TiggerBallRadius|number|用于Tigger检测球半径。默认是2|
|DownForce|number|球向下的力。仅调试中有效|

## 碎片的物理化参数

|名称|值|说明|
|---|---|---|
|Friction|number|摩擦力系数|
|Elasticity|number|弹力系数|
|Mass|number|质量|
|LinearDamp|number|线性阻尼|
|RotDamp|number|旋转阻尼|
