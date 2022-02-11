---游戏路面物理参数
GamePhysFloor = {
  Phys_Floors = {
    Friction = 0.7,
    Elasticity = 0.3,
    Layer = GameLayers.LAYER_PHY_FLOOR,
    CollisionLayerName = 'Stone',
  },
  Phys_FloorWoods = {
    Friction = 0.7,
    Elasticity = 0.3,
    Layer = GameLayers.LAYER_PHY_FLOOR_WOODS,
    CollisionLayerName = 'Wood',
  },
  Phys_FloorRails = {
    Friction = 0.7,
    Elasticity = 0.3,
    Layer = GameLayers.LAYER_PHY_FLOOR_RAIL,
    CollisionLayerName = 'Metal',
  },
  Phys_FloorStopper = {
    Friction = 0.7,
    Elasticity = 0.3,
    Layer = GameLayers.LAYER_PHY_FLOOR_STOPPER,
    CollisionLayerName = 'Wood',
    HitSound = 'core.sounds:Hit_WoodenFlap.wav',
  },
}