# GamePhysFloor

游戏路面物理参数

这是游戏路面的物理参数。

你可以在这里添加参数, 就成功注册自己的物理路面：
```lua
--添加物理参数
GamePhysFloor['Phys_MyFloor'] = {
  Friction = 0.7,
  Elasticity = 0.3,
  Layer = GameLayers.LAYER_PHY_FLOOR,
  CollisionLayerName = 'Stone',
}
```
注意，GamePhysFloor['key'] 是路面的名称，不能重复。

要修改默认路面的参数，只需要在进入游戏关卡之前修改参数即可：
```lua
GamePhysFloor['Phys_Floors'].Elasticity = 0.8
```

## 定义

|名称|类型|说明|
|---|---|---|
|Friction|number|路面的摩擦系数|
|Elasticity|number|路面的弹力系数|
|Layer|number|路面的碰撞层|
|CollisionLayerName|string|路面的声音组名称|
|HitSound|string|用于挡板层，指定挡板碰撞时发出的声音资源|
