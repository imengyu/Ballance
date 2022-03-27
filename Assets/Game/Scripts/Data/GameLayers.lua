---@gendoc
--[[
---@gen_remark_start
物理碰撞层最大是32个，14之后的都可以用，但是还是推荐使用已经定义的碰撞层, 不要自定义添加，碰撞层的相互碰撞设置可以在 Unity 菜单 Ballance Physics Settings 中查看。
---@gen_remark_end
]]--
---游戏物理碰撞层定义
GameLayers = {
  LAYER_PHY_GLOBAL = 0, --默认碰撞层 与所有物体碰撞
  LAYER_PHY_BALL = 1, --球的碰撞层
  LAYER_PHY_BALL_PEICES = 2,--球碎片的碰撞层
  LAYER_PHY_MODUL_COL_BALL = 3,--此碰撞层机关专用，与球、路面碰撞
  LAYER_PHY_MODUL_NOCOL_BALL = 4, --此碰撞层机关专用，不与球碰撞
  LAYER_PHY_MODUL_NOCOL_FLOOR = 5, --此碰撞层机关专用，不与路面碰撞
  LAYER_PHY_MODUL_NOCOL = 6, --此碰撞层机关专用，仅与其他机关碰撞层碰撞，不与路面、球碰撞
  LAYER_PHY_MODUL_ONLY_COL_BALL = 7,  --此碰撞层机关专用，不与其他机关碰撞层、路面碰撞，仅与球碰撞
  LAYER_PHY_TRANFO_TIGGER = 8, --仅与球碰撞的触发器
  LAYER_UNUSED_9 = 9, --未使用层
  LAYER_UNUSED_10 = 10, --未使用层
  LAYER_PHY_FLOOR = 11, --路面层
  LAYER_PHY_FLOOR_RAIL = 12, --钢轨层
  LAYER_PHY_FLOOR_WOODS = 13, --木制路面层
  LAYER_PHY_FLOOR_STOPPER = 14, --挡板层
}