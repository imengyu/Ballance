using System.Collections.Generic;
using System.Text;
using Ballance2.Services;
using Ballance2.UI.Utils;
using BallancePhysics.Wapper;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.GamePlay.Balls
{
  /// <summary>
  /// Ballance 基础球定义。
  /// 可继承此类来重写你自己的球。
  /// </summary>
  public class Ball : MonoBehaviour
  {
    /// <summary>
    /// 球对应刚体
    /// </summary>
    public PhysicsObject _Rigidbody = null;
    /// <summary>
    /// 球碎片
    /// </summary>
    public GameObject _Pieces = null;
    /// <summary>
    /// 球碎片配置数据
    /// </summary>
    public BallPiecesData _PiecesData;
    
    public float _UpForce = 0; 
    public float _DownForce = 0; 
    public float _Force = 0; 

    #region 球声音配置

    [JsonObject]
    public class BallSoundConfig {
      /// <summary>
      /// 球的碰撞声音配置
      /// </summary>
      [JsonProperty]
      public BallHitSoundConfigHit HitSound = new BallHitSoundConfigHit(); 
      /// <summary>
      /// 球的滚动声音配置
      /// </summary>
      [JsonProperty]
      public BallHitSoundConfigRoll RollSound = new BallHitSoundConfigRoll();
      /// <summary>
      /// 指定当前球是否存在滚动声音
      /// </summary>
      [JsonProperty]
      public bool HasRollSound = true;
      /// <summary>
      /// 指定当前球是否存在滚动声音
      /// </summary> 
      [JsonIgnore]
      public StringBuilder _SoundManagerDebugString = new StringBuilder();
    }

    /// <summary>
    /// 球的碰撞声音配置
    /// </summary>
    public BallSoundConfig _SoundConfig = new BallSoundConfig();

    #endregion

    #region 碎片配置

    /// <summary>
    /// 球碎片声音
    /// </summary>
    public string _PiecesSoundName = "";
    /// <summary>
    /// 碎片抛出最小推力
    /// </summary>
    public float _PiecesMinForce = 0;
    /// <summary>
    /// 碎片抛出最大推力
    /// </summary>
    public float _PiecesMaxForce = 5; 
    /// <summary>
    /// 碎片存活时间（秒）
    /// </summary>
    public float _PiecesTimeLive = 30; 
    /// <summary>
    /// 碎片碰撞最高速度，用于计算碰撞声音音量
    /// </summary>
    public float _PiecesColSoundMaxSpeed = 10; 
    /// <summary>
    /// 碎片碰撞最低速度，用于计算碰撞声音音量
    /// </summary>
    public float _PiecesColSoundMinSpeed = 1; 
    /// <summary>
    /// 碎片物理数据
    /// </summary>
    public GamePhysBallPiecesPhysicsData _PiecesPhysicsData = default(GamePhysBallPiecesPhysicsData); 
    /// <summary>
    /// 物理化碎片自定义处理回调，如果为nil，则碎片将使用默认参数来物理化
    /// </summary>
    protected PiecesPhysCallback _PiecesPhysCallback = null;  
    /// <summary>
    /// 配置存在声音的碎片
    /// </summary>
    public List<string> _PiecesHaveColSound = new List<string>();

    protected delegate PhysicsObject PiecesPhysCallback(GameObject gameObject, GamePhysBallPiecesPhysicsData physicsData);

    #endregion

    #region 球功能初始化

    //初始化碎片
    private void _InitPeices() {
      if (_Pieces != null) {
        AudioClip piecesSound = null;
        if (!string.IsNullOrEmpty(_PiecesSoundName)) {
          piecesSound = GameSoundManager.Instance.LoadAudioResource(_PiecesSoundName);
        }
      
        var parent = _Pieces;
        var data = new BallPiecesData();
        data.parent = parent;
        data.bodys = new List<PhysicsObject>();
        data.fadeObjects = new List<FadeObject>();

        for (int i = 0; i < parent.transform.childCount; i++) {
          var child = parent.transform.GetChild(i);
          PhysicsObject body = null;
          if (_PiecesPhysCallback != null)
            body = _PiecesPhysCallback(child.gameObject, _PiecesPhysicsData);
          else {
            body = child.gameObject.AddComponent<PhysicsObject>();
            if(_PiecesPhysicsData.Mass > 0) {
              //Mesh
              var meshFilter = child.GetComponent<MeshFilter>();
              if (meshFilter != null && meshFilter.mesh != null) {
                body.Mass = _PiecesPhysicsData.Mass;
                body.Elasticity = _PiecesPhysicsData.Elasticity;
                body.Friction = _PiecesPhysicsData.Friction;
                body.LinearSpeedDamping = _PiecesPhysicsData.LinearDamp;
                body.RotSpeedDamping = _PiecesPhysicsData.RotDamp;
                body.AutoControlActive = false;
                body.DoNotAutoCreateAtAwake = true;
                body.EnableCollision = true;
                body.AutoMassCenter = false;
                body.UseExistsSurface = true;
                body.Layer = GameLayers.LAYER_PHY_BALL_PEICES;
                body.Convex.Add(meshFilter.mesh);
              }
              else {
                Log.W($"Ball {gameObject.name}", $"Not found MeshFilter or mesh in peices '{child.name}'");
              }
            }
            else {
              Log.W($"Ball {gameObject.name}", "No _PiecesPhysCallback or valid _PiecesPhysicsData found for this ball");
            }
            body.DoNotAutoCreateAtAwake = true;

            //碎片声音
            if (piecesSound != null && _PiecesHaveColSound.IndexOf(body.name) != -1) {
              var sound = GameSoundManager.Instance.RegisterSoundPlayer(GameSoundType.BallEffect, piecesSound, false, true, "PiecesSound" + child.name);
              sound.spatialBlend = 1;
              sound.maxDistance = 30;
              sound.minDistance = 10;
              sound.dopplerLevel = 0;
              sound.volume = 1;
              body.CollisionEventCallSleep = 0.5f;
              body.EnableCollisionEvent = true;
              body.OnPhysicsCollision = (self, other, contact_point_ws, speed, surf_normal) => {
                if (data.throwed) {
                  sound.volume = (speed.sqrMagnitude - _PiecesColSoundMinSpeed) / (_PiecesColSoundMaxSpeed - _PiecesColSoundMinSpeed);
                  sound.Play();
                }
              };
            }
          }
          data.bodys.Add(body);
        }
        _PiecesData = data;
      }
    }
  
    #endregion

    #region 公共可继承方法

    /// <summary>
    /// 开始事件
    /// </summary>
    protected virtual void Start() {
      _Rigidbody = gameObject.GetComponent<PhysicsObject>();
      _InitPeices();
    }
    /// <summary>
    /// 当前球激活事件
    /// </summary>
    public virtual void Active() {}
    /// <summary>
    /// 当前球取消激活事件
    /// </summary>
    public virtual void Deactive() {}
    /// <summary>
    /// 当前球获取碎片
    /// </summary>
    public virtual GameObject GetPieces() { return _Pieces; }
    /// <summary>
    /// 自定义获取碎片控制器
    /// </summary>
    public virtual IBallPiecesControllManager GetBallPiecesControllManager() { return GamePlayManager.Instance.BallManager.BallPiecesControllManager; }
    /// <summary>
    /// 丢出此类球的碎片事件
    /// </summary>
    public virtual void ThrowPieces(Vector3 pos) {
      if (_PiecesData != null)
        GetBallPiecesControllManager().ThrowPieces(_PiecesData, pos, _PiecesMinForce, _PiecesMaxForce, _PiecesTimeLive);
    }
    /// <summary>
    /// 回收此类的碎片事件
    /// </summary>
    public virtual void ResetPieces() {
      if (_PiecesData != null)
         GetBallPiecesControllManager().ResetPieces(_PiecesData);
    }
  
    #endregion
  }

  [JsonObject]
  public class BallHitSoundConfigBase {
    /// <summary>
    /// 声音配置
    /// key 是当前球碰撞的层名称
    /// value 是播放声音资源名称
    /// </summary>
    [JsonProperty]
    public Dictionary<string, string> Names = new Dictionary<string, string>();
  }
  [JsonObject]
  public class BallHitSoundConfigRoll : BallHitSoundConfigBase {
    /// <summary>
    /// 滚动声音起始延时
    /// </summary>
    [JsonProperty]
    public float TimeDelayStart = 0.5f;
    /// <summary>
    /// 滚动声音末尾延时
    /// </summary>
    [JsonProperty]
    public float TimeDelayEnd = 0.5f;
    /// <summary>
    /// 滚动声音变速基数
    /// </summary>
    [JsonProperty]
    public float PitchBase = 0.6f;
    /// <summary>
    /// 滚动声音变速乘数
    /// </summary>
    [JsonProperty]
    public float PitchFactor = 0.03f;
    /// <summary>
    /// 滚动声音音量基数
    /// </summary>
    [JsonProperty]
    public float VolumeBase = 0f;
    /// <summary>
    /// 滚动声音音量乘数
    /// </summary>
    [JsonProperty]
    public float VolumeFactor = 0.05f;
  }  
  [JsonObject]
  public class BallHitSoundConfigHit : BallHitSoundConfigBase {
    /// <summary>
    /// 碰撞最高速度，用于计算碰撞声音音量
    /// </summary>
    [JsonProperty]
    public float MaxSpeed = 20;
    /// <summary>
    /// 碰撞最低速度，用于计算碰撞声音音量
    /// </summary>
    [JsonProperty]
    public float MinSpeed = 2;
  }
  /// <summary>
  /// 球碎片数据结构定义
  /// </summary>
  public class BallPiecesData {
    /// <summary>
    /// 所有的碎片物理体
    /// </summary>
    public List<PhysicsObject> bodys; 
    /// <summary>
    /// 父级游戏对象
    /// </summary>
    public GameObject parent;
    /// <summary>
    /// 淡出延时定时器
    /// </summary>
    public int fadeOutTimerID = 0;
    /// <summary>
    /// 隐藏延时定时器
    /// </summary>
    public int delayHideTimerID = 0;
    /// <summary>
    /// 获取碎片是否已经抛出了
    /// </summary>
    public bool throwed = false; 
    /// <summary>
    /// 淡出控制对象
    /// </summary>
    public List<FadeObject> fadeObjects;
  }
}