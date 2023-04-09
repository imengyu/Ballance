namespace Ballance2.Game.GamePlay
{
  /// <summary>
  /// 游戏物理碰撞层定义
  /// </summary>
  /// <remarks>
  /// 物理碰撞层最大是32个，14之后的都可以用，但是还是推荐使用已经定义的碰撞层, 不要自定义添加，碰撞层的相互碰撞设置可以在 Unity 菜单 Ballance Physics Settings 中查看。
  /// </remarks>
  public static class GameLayers 
  {
    /// <summary>
    /// 默认碰撞层 与所有物体碰撞
    /// </summary>
    public const int LAYER_PHY_GLOBAL = 0;
    /// <summary>
    /// 球的碰撞层
    /// </summary>
    public const int LAYER_PHY_BALL = 1;
    /// <summary>
    /// 球碎片的碰撞层
    /// </summary>
    public const int LAYER_PHY_BALL_PEICES = 2;
    /// <summary>
    /// 此碰撞层机关专用，与球、路面碰撞
    /// </summary>
    public const int LAYER_PHY_MODUL_COL_BALL = 3;
    /// <summary>
    /// 此碰撞层机关专用，不与球碰撞
    /// </summary>
    public const int LAYER_PHY_MODUL_NOCOL_BALL = 4;
    /// <summary>
    /// 此碰撞层机关专用，不与路面碰撞
    /// </summary>
    public const int LAYER_PHY_MODUL_NOCOL_FLOOR = 5;
    /// <summary>
    /// 此碰撞层机关专用，仅与其他机关碰撞层碰撞，不与路面、球碰撞
    /// </summary>
    public const int LAYER_PHY_MODUL_NOCOL = 6;
    /// <summary>
    /// 此碰撞层机关专用，不与其他机关碰撞层、路面碰撞，仅与球碰撞
    /// </summary>
    public const int LAYER_PHY_MODUL_ONLY_COL_BALL = 7;
    /// <summary>
    /// 仅与球碰撞的触发器
    /// </summary>
    public const int LAYER_PHY_TRANFO_TIGGER = 8;
    /// <summary>
    /// 未使用层（预留）
    /// </summary>
    public const int LAYER_UNUSED_9 = 9;
    /// <summary>
    /// 未使用层（预留）
    /// </summary>
    public const int LAYER_UNUSED_10 = 10;
    /// <summary>
    /// 路面层
    /// </summary>
    public const int LAYER_PHY_FLOOR = 11;
    /// <summary>
    /// 钢轨层
    /// </summary>
    public const int LAYER_PHY_FLOOR_RAIL = 12;
    /// <summary>
    /// 木制路面层
    /// </summary>
    public const int LAYER_PHY_FLOOR_WOODS = 13;
    /// <summary>
    /// 挡板层
    /// </summary>
    public const int LAYER_PHY_FLOOR_STOPPER = 14;
  }
}