using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.GamePlay
{
  /// <summary>
  /// 球物理参数定义
  /// </summary>
  /// <remarks>
  /// 如何添加自己的球物理参数：请在注册球之前插入物理参数至 Data 字典中，键是你的球名称，与注册球时的名称一致。
  /// </remarks>
  public static class GamePhysBall 
  {
    /// <summary>
    /// 获取数据
    /// </summary>
    public static Dictionary<string, GamePhysBallData> Data;

    static GamePhysBall() {
      var dataAsset = Resources.Load<TextAsset>("GamePhysBallData");
      if (dataAsset != null)
        Data = JsonConvert.DeserializeObject<Dictionary<string, GamePhysBallData>>(dataAsset.text);
      else
        Data = new Dictionary<string, GamePhysBallData>();

      UnityEngine.Debug.Log("GamePhysBallData: " + Data.Count);
    }
  }

  [JsonObject]
  public struct GamePhysBallPiecesPhysicsData 
  {
    /// <summary>
    /// 摩擦力系数
    /// </summary>
    [JsonProperty]
    public float Friction;
    /// <summary>
    /// 弹力系数
    /// </summary>
    [JsonProperty]
    public float Elasticity;
    /// <summary>
    /// 质量
    /// </summary>
    [JsonProperty]
    public float Mass;
    /// <summary>
    /// 线性阻尼
    /// </summary>
    [JsonProperty]
    public float LinearDamp;
    /// <summary>
    /// 旋转阻尼
    /// </summary>
    [JsonProperty]
    public float RotDamp;
  }
  [JsonObject]
  public struct GamePhysBallData 
  {
    /// <summary>
    /// 球推动力
    /// </summary>
    [JsonProperty]
    public float Force; 
    /// <summary>
    /// 球的摩擦力系数
    /// </summary>
    [JsonProperty]
    public float Friction; 
    /// <summary>
    /// 球的弹力系数
    /// </summary>
    [JsonProperty]
    public float Elasticity; 
    /// <summary>
    /// 球的质量
    /// </summary>
    [JsonProperty]
    public float Mass; 
    /// <summary>
    /// 线性阻尼
    /// </summary>
    [JsonProperty]
    public float LinearDamp; 
    /// <summary>
    /// 碰撞层
    /// </summary>
    [JsonProperty]
    public int Layer; 
    /// <summary>
    /// 旋转阻尼
    /// </summary>
    [JsonProperty]
    public float RotDamp; 
    /// <summary>
    /// 碎片爆炸最小的力
    /// </summary>
    [JsonProperty]
    public float PiecesMinForce; 
    /// <summary>
    /// 碎片爆炸最大的力
    /// </summary>
    [JsonProperty]
    public float PiecesMaxForce; 
    /// <summary>
    /// 碎片的物理化参数
    /// </summary>
    [JsonProperty]
    public GamePhysBallPiecesPhysicsData PiecesPhysicsData; 
    /// <summary>
    /// 球向上的力。仅调试中有效
    /// </summary>
    [JsonProperty]
    public float UpForce; 
    /// <summary>
    /// 球半径。如果为0则使用convex mesh
    /// </summary>
    [JsonProperty]
    public float BallRadius; 
    /// <summary>
    /// 用于Tigger检测球半径。默认是2
    /// </summary>
    [JsonProperty]
    public float TiggerBallRadius; 
    /// <summary>
    /// 球向下的力。仅调试中有效
    /// </summary>
    [JsonProperty]
    public float DownForce; 
  }
}