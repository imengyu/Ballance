---@gendoc
--[[
---@gen_remark_start

如何添加自己的球物理参数：
请在注册球之前插入物理参数，BallName是你的球名称，与注册球时的名称一致：
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

---@gen_remark_end
]]--

---球的物理参数定义结构
---@class GamePhysBallData 
---@field Force number 球推动力
---@field Friction number 球的摩擦力系数
---@field Elasticity number 球的弹力系数
---@field Mass number 球的质量
---@field LinearDamp number 线性阻尼
---@field Layer number 碰撞层
---@field RotDamp number 旋转阻尼
---@field PiecesMinForce number 碎片爆炸最小的力
---@field PiecesMaxForce number 碎片爆炸最大的力
---@field PiecesPhysicsData table 碎片的物理化参数
---@field UpForce number 球向上的力。仅调试中有效
---@field BallRadius number 球半径。如果为0则使用convex mesh
---@field TiggerBallRadius number 用于Tigger检测球半径。默认是2
---@field DownForce number 球向下的力。仅调试中有效
GamePhysBallData = {}

---游戏球物理参数
---子属性名称为球的名字
---@type GamePhysBallData[]
GamePhysBall = {
  BallWood = {
    Force = 0.43,
    Friction = 0.8,
    Elasticity = 0.2,
    Mass = 1.9,
    LinearDamp = 0.9,
    RotDamp = 0.1,
    PiecesPhysicsData = {
      Friction = 2,
      Elasticity = 1,
      Mass = 0.2,
      LinearDamp = 0.3,
      RotDamp = 0.2,
    },
    PiecesMinForce = 1.5,
    PiecesMaxForce = 3.0,
    UpForce = 0.6,
    DownForce = 0.3,
    BallRadius = 2,
    TiggerBallRadius = 2,
    Layer = GameLayers.LAYER_PHY_BALL,
  },
  BallStone = {
    Force = 0.92,
    Friction = 0.5,
    Elasticity = 0.1,
    Mass = 10,
    LinearDamp = 0.3,
    RotDamp = 0.1,
    PiecesPhysicsData = {
      Friction = 0.8,
      Elasticity = 1,
      Mass = 0.8,
      LinearDamp = 0.3,
      RotDamp = 0.2,
    },
    PiecesMinForce = 8,
    PiecesMaxForce = 16,
    UpForce = 3,
    DownForce = 0.05,
    BallRadius = 2,
    TiggerBallRadius = 2,
    Layer = GameLayers.LAYER_PHY_BALL,
  },
  BallPaper = {
    Force = 0.065,
    Friction = 0.5,
    Elasticity = 0.4,
    Mass = 0.2,
    LinearDamp = 1.3,
    RotDamp = 0.1,
    PiecesPhysicsData = {
      Elasticity = 1,
      LinearDamp = 6,
      RotDamp = 0.5,
    },
    PiecesMinForce = 0.3,
    PiecesMaxForce = 1.3,
    UpForce = 0.08,
    DownForce = 0.01,
    BallRadius = 0,
    TiggerBallRadius = 2,
    Layer = GameLayers.LAYER_PHY_BALL,
  },
}