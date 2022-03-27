---@gendoc
--[[
---@gen_remark_start

这是路面的物理参数。


---@gen_remark_end
]]--

---游戏路面物理参数
GamePhysFloor = {
  ---普通路面的物理参数
  Phys_Floors = {
    Friction = 0.7,
    Elasticity = 0.3,
    Layer = GameLayers.LAYER_PHY_FLOOR,
    CollisionLayerName = 'Stone',
  },
  ---木制路面的物理参数
  Phys_FloorWoods = {
    Friction = 0.7,
    Elasticity = 0.3,
    Layer = GameLayers.LAYER_PHY_FLOOR_WOODS,
    CollisionLayerName = 'Wood',
  },
  ---钢轨的物理参数
  Phys_FloorRails = {
    Friction = 0.7,
    Elasticity = 0.3,
    Layer = GameLayers.LAYER_PHY_FLOOR_RAIL,
    CollisionLayerName = 'Metal',
  },
  ---挡板的物理参数
  Phys_FloorStopper = {
    Friction = 0.7,
    Elasticity = 0.5,
    Layer = GameLayers.LAYER_PHY_FLOOR_STOPPER,
    CollisionLayerName = 'Wood',
    HitSound = 'core.sounds:Hit_WoodenFlap.wav',
  },
}