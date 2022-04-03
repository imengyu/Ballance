# 修改内置球物理数据

球的物理参数存放在 `Assets\Game\Scripts\Data\GamePhysBall.lua` 中。

球的物理参数不是实时生效的，因此，你需要在关卡初始化之前设置这些参数。
通常，你可以在你的模组加载时修改。因为模组加载时游戏玩模块还未初始化。

下面是修改纸球物理参数的实例：

```lua
GamePhysBall['BallPaper'].Force = 0.065 --修改球的推力
GamePhysBall['BallPaper'].Friction = 0.5 --修改球的摩擦力
GamePhysBall['BallPaper'].Elasticity = 0.4 --修改球的弹力
GamePhysBall['BallPaper'].Mass = 0.2 --修改球的质量
GamePhysBall['BallPaper'].LinearDamp = 1.5 --修改球的线性阻尼
GamePhysBall['BallPaper'].RotDamp = 0.1 --修改球的旋转阻尼
```

内置球有 `BallPaper`、`BallWood` 和 `BallStone`。
