using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.GamePlay
{
  /// <summary>
  /// 这是游戏路面的物理参数。
  /// </summary>
  /// <remarks>
  /// 你可以在 Data 里添加参数, 就相当于成功注册自己的物理路面。
  /// 要修改默认路面的参数，只需要在进入游戏关卡之前修改对应数据参数即可。
  /// </remarks>
  public static class GamePhysFloor 
  {
    /// <summary>
    /// 获取数据
    /// </summary>
    public static Dictionary<string, GamePhysFloorData> Data;

    static GamePhysFloor() {
      var dataAsset = Resources.Load<TextAsset>("GamePhysFloorData");
      if (dataAsset != null)
        Data = JsonConvert.DeserializeObject<Dictionary<string, GamePhysFloorData>>(dataAsset.text);
      else
        Data = new Dictionary<string, GamePhysFloorData>();
    }
  }

  [JsonObject]
  public struct GamePhysFloorData 
  {
    /// <summary>
    /// 路面的摩擦力系数
    /// </summary>
    [JsonProperty]
    public float Friction; 
    /// <summary>
    /// 路面的弹力系数
    /// </summary>
    [JsonProperty]
    public float Elasticity; 
    /// <summary>
    /// 路面的碰撞层
    /// </summary>
    [JsonProperty]
    public int Layer; 
    /// <summary>
    /// 路面的碰撞层
    /// </summary>
    [JsonProperty]
    public string HitSound; 
    /// <summary>
    /// 路面的碰撞层声音名称
    /// </summary>
    [JsonProperty]
    public string CollisionLayerName;
  }
}