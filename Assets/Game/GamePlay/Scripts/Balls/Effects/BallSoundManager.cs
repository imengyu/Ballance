using System.Collections.Generic;
using Ballance2.Base;
using Ballance2.Package;
using Ballance2.Services;
using BallancePhysics.Wapper;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.GamePlay.Balls
{
  /// <summary>
  /// 球声音管理器，也可用于其他物体的碰撞声音
  /// </summary>
  public class BallSoundManager : MonoBehaviour 
  {
    private const string TAG = "BallSoundManager";

    public AnimationCurve BallRollSpeedVolumeCruve = null;

    [JsonObject]
    public class BallSoundCollData {
      /// <summary>
      /// 碰撞检测最低速度
      /// </summary>
      [JsonProperty]
      public float MinSpeed; 
      /// <summary>
      /// 碰撞检测最高速度
      /// </summary>
      [JsonProperty]
      public float MaxSpeed; 
      /// <summary>
      /// 碰撞检测延时
      /// </summary>
      [JsonProperty]
      public float SleepAfterwards; 
      /// <summary>
      /// 碰撞检测最低速度阈值
      /// </summary>
      [JsonProperty]
      public float SpeedThreadhold; 
      /// <summary>
      /// 滚动检测起始延时
      /// </summary>
      [JsonProperty]
      public float TimeDelayStart;
      /// <summary>
      /// 滚动检测末尾延时
      /// </summary>
      [JsonProperty]
      public float TimeDelayEnd;
      /// <summary>
      /// 是否有滚动声音
      /// </summary>
      [JsonProperty]
      public bool HasRollSound; 
      /// <summary>
      /// 滚动声音名称，放在球的RollSound信息中
      /// </summary>
      [JsonProperty]
      public string RollSoundName; 
      /// <summary>
      /// 撞击声音名称，放在球的HitSound信息中
      /// </summary>
      [JsonProperty]
      public string HitSoundName; 
    }

    private List<GameEventHandler> _MyEventHandlers = new List<GameEventHandler>();
    private Dictionary<int, BallSoundCollData> _SoundCollData = new Dictionary<int, BallSoundCollData>();
    private Dictionary<string, int> _CollIDNames = new Dictionary<string, int>();
    private int _CurrentCollId = 1;

    //加载与卸载
    private void Start() {
      var sysPackage = GamePackage.GetSystemPackage();
      this._MyEventHandlers.Add(GameMediator.Instance.RegisterEventHandler(sysPackage, GameEventNames.EVENT_LEVEL_BUILDER_BEFORE_START, TAG, (evtName, param) => {
        this.RemoveAllSoundCollData();
        this._AddInternalSoundCollData();
        return false;
      }));
    }
    private void OnDestroy() {
      GameMediator.Instance?.UnRegisterEventHandler(GameEventNames.EVENT_LEVEL_BUILDER_BEFORE_START, _MyEventHandlers[0]);
    }

    #region 碰撞数据控制

    //内部函数
    private void _AddInternalSoundCollData() 
    {
      _CollIDNames.Add("Nome", 0);
      AddSoundCollData(0, new BallSoundCollData() {
        MinSpeed = 0,
        MaxSpeed = 0,
        SleepAfterwards = 0,
        SpeedThreadhold = 0,
        HasRollSound = false,
        RollSoundName = null,
        HitSoundName = null
      });
      AddSoundCollData(GetSoundCollIDByName("Stone"), new BallSoundCollData() {
        MinSpeed = 5,
        MaxSpeed = 30,
        SleepAfterwards = 0.6f,
        SpeedThreadhold = 20,
        HasRollSound = true,
        RollSoundName = "Stone",
        HitSoundName = "Stone"
      });
      AddSoundCollData(GetSoundCollIDByName("Wood"), new BallSoundCollData() {
        MinSpeed = 5,
        MaxSpeed = 30,
        SleepAfterwards = 0.6f,
        SpeedThreadhold = 20f,
        HasRollSound = true,
        RollSoundName = "Wood",
        HitSoundName = "Wood"
      });
      AddSoundCollData(GetSoundCollIDByName("Metal"), new BallSoundCollData() {
        MinSpeed = 5,
        MaxSpeed = 30,
        SleepAfterwards = 0.6f,
        SpeedThreadhold = 20,
        HasRollSound = true,
        RollSoundName = "Metal",
        HitSoundName = "Metal"
      });
    }
    private void RemoveAllSoundCollData() 
    {
      _CurrentCollId = 1;
      _CollIDNames.Clear();
      _SoundCollData.Clear();
    }

    /// <summary>
    /// 添加球碰撞声音组
    /// </summary>
    /// <param name="colId">自定义碰撞层ID，为防止重复，请使用 GetSoundCollIDByName 使用名称获取ID</param>
    /// <param name="data">碰撞数据</param>
    public void AddSoundCollData(int colId, BallSoundCollData data) 
    {
      if (_SoundCollData.ContainsKey(colId)) 
      {
        Log.W(TAG, $"AddSoundCollData failed because SoundCollData id: {colId} already added");
        return;
      }
      _SoundCollData[colId] = data;
    }
    /// <summary>
    /// 移除球碰撞声音组
    /// </summary>
    /// <param name="colId">（注意，不会移除激活中球的声音组，需要等到球下一次激活时生效，因此不建议在游戏中使用此函数）</param>
    public void RemoveSoundCollData(int colId) 
    {
      _SoundCollData.Remove(colId);
    }
    /// <summary>
    /// 通过名称分配一个可用的声音组ID, 如果名称存在，则返回同样的ID
    /// </summary>
    /// <param name="name">自定义声音组名称</param>
    /// <returns></returns>
    public int GetSoundCollIDByName(string name) 
    {
      if (_CollIDNames.TryGetValue(name, out var id))
        return id;
      else
      {
        id = _CurrentCollId;
        _CurrentCollId++;
        _CollIDNames[name] = id;
        return id;
      }
    }
    /// <summary>
    /// 强制停止所有声音
    /// </summary>
    public void StopAllSound() {
      Log.D(TAG, "StopAllSound");
      foreach(var value in soundableBallWorkDatas) {
        foreach(var sound in value.Value._CurrentPlayingRollSounds) {
          sound.volume = 0;
          sound.pitch = 1;
        }
      }
    }

    #endregion

    #region 声音数据控制

    private class BallHitSoundItem {
      public BallHitSoundItem(AudioSource sound1, AudioSource sound2) {
        this.sound1 = sound1;
        this.sound2 = sound2;
      }
      public AudioSource sound2;
      public AudioSource sound1;
      public bool isSound1 = true;
    }

    private class SoundableBallWorkData {
      public Dictionary<string, BallHitSoundItem> _HitSoundSounds = new Dictionary<string, BallHitSoundItem>();
      public Dictionary<string, AudioSource> _RollSoundSounds = new Dictionary<string, AudioSource>();
      public LinkedList<AudioSource> _CurrentPlayingRollSounds = new LinkedList<AudioSource>();
      public HashSet<AudioSource> _CurrentPlayingRollSoundsHashSet = new HashSet<AudioSource>();
      public SpeedMeter _SpeedMeter = null;
    }

    private Dictionary<int, SoundableBallWorkData> soundableBallWorkDatas = new Dictionary<int, SoundableBallWorkData>();

    //加载球的撞击和滚动声音
    private SoundableBallWorkData InitSoundableBallWorkData(int instanceId, Ball.BallSoundConfig _SoundConfig) {
      SoundableBallWorkData data = null;
      if (soundableBallWorkDatas.TryGetValue(instanceId, out data))
        return data;
      data = new SoundableBallWorkData();
      //加载球的撞击和滚动声音
      var SoundManager = GameSoundManager.Instance;
      foreach (var item in _SoundConfig.HitSound.Names) {
        if (!string.IsNullOrEmpty(item.Value)) {
          var key = item.Key;
          var sound2 = SoundManager.RegisterSoundPlayer(GameSoundType.BallEffect, item.Value, false, true,  $"{gameObject.name}SoundHit{key}2");
          var sound1 = SoundManager.RegisterSoundPlayer(GameSoundType.BallEffect, item.Value, false, true, $"{gameObject.name}SoundHit{key}1");
          data._HitSoundSounds[key] = new BallHitSoundItem(sound1, sound2);
        }
      }
      foreach (var item in _SoundConfig.RollSound.Names) {
        if (!string.IsNullOrEmpty(item.Value)) {
          var sound = SoundManager.RegisterSoundPlayer(GameSoundType.BallEffect, item.Value, true, true, $"{gameObject.name}SoundRoll{item.Key}");
          sound.loop = true;
          sound.playOnAwake = true;
          sound.volume = 0;
          sound.Play();
          data._RollSoundSounds[item.Key] = sound;
        }
      }
      soundableBallWorkDatas.Add(instanceId, data);
      return data;
    }

    #endregion

    #region 球碰撞声音滚动声音控制函数

    /// <summary>
    /// 为指定物理刚体添加声音处理函数
    /// </summary>
    public void AddSoundableBall(PhysicsObject rigidbody, Ball.BallSoundConfig _SoundConfig) 
    {
      //添加或者获取已有碰撞数据
      SoundableBallWorkData soundableBallWorkData = InitSoundableBallWorkData(rigidbody.gameObject.GetInstanceID(), _SoundConfig);

      //添加声音层工具侦听
      foreach(var value in _SoundCollData) {
        rigidbody.AddCollDetection(value.Key, value.Value.MinSpeed, value.Value.MaxSpeed, value.Value.SleepAfterwards, value.Value.SpeedThreadhold);
        if (value.Value.HasRollSound)
          rigidbody.AddContractDetection(value.Key, _SoundConfig.RollSound.TimeDelayStart, _SoundConfig.RollSound.TimeDelayEnd);
      }
      //添加回调
      rigidbody.EnableCollisionEvent = true;
      rigidbody.EnableCollisionDetection();
      rigidbody.EnableContractEventCallback();
      //撞击处理回调
      rigidbody.OnPhysicsCollDetection = (_, col_id, speed_precent) => {
        var currentSoundCollData = _SoundCollData[col_id];
        if (currentSoundCollData != null && !string.IsNullOrEmpty(currentSoundCollData.HitSoundName)) 
        {
          //获取当前层与目标层的碰撞声音
          soundableBallWorkData._HitSoundSounds.TryGetValue(currentSoundCollData.HitSoundName, out var sound);
          //如果没有碰撞声音，则尝试使用 All 声音
          if (sound == null) 
            soundableBallWorkData._HitSoundSounds.TryGetValue("All", out sound); 

          if (sound != null) 
          {
            //这里是切换了两个sound的播放，因为碰撞声音很可能
            //一个没有播放完成另一个就来了
            if (sound.isSound1) 
            {
              sound.isSound1 = false;
              sound.sound1.volume = speed_precent;
              if (!sound.sound1.isPlaying) 
                sound.sound1.Play();
            }
            else
            {
              sound.isSound1 = true;
              sound.sound2.volume = speed_precent;
              if (!sound.sound2.isPlaying)
                sound.sound2.Play();
            }
          }
        }
      };
      
      //滚动声音相关
      if (_SoundConfig.HasRollSound)
      {
        //接触开始处理回调
        rigidbody.OnPhysicsContactOn = (_, col_id) => {
          var currentSoundCollData = _SoundCollData[col_id];
          if (currentSoundCollData != null && currentSoundCollData.HasRollSound)
          {
            //获取当前层与目标层的碰撞声音
            soundableBallWorkData._RollSoundSounds.TryGetValue(currentSoundCollData.RollSoundName, out var sound);
            //如果没有碰撞声音，则尝试使用 All 声音
            if (sound == null) 
              soundableBallWorkData._RollSoundSounds.TryGetValue("All", out sound); 
            //加入正在播放声音中
            if (sound != null && !soundableBallWorkData._CurrentPlayingRollSoundsHashSet.Contains(sound)) {
              soundableBallWorkData._CurrentPlayingRollSoundsHashSet.Add(sound);
              soundableBallWorkData._CurrentPlayingRollSounds.AddLast(sound);
            }
          }
        };
        //接触结束处理回调
        rigidbody.OnPhysicsContactOff = (_, col_id) => {
          var currentSoundCollData = _SoundCollData[col_id];
          if (currentSoundCollData != null && currentSoundCollData.HasRollSound)
          {
            //获取当前层与目标层的碰撞声音
            soundableBallWorkData._RollSoundSounds.TryGetValue(currentSoundCollData.RollSoundName, out var sound);
            //如果没有碰撞声音，则尝试使用 All 声音
            if (sound == null) 
              soundableBallWorkData._RollSoundSounds.TryGetValue("All", out sound); 
            //从正在播放声音中移除
            if (sound != null && soundableBallWorkData._CurrentPlayingRollSoundsHashSet.Contains(sound)) {
              soundableBallWorkData._CurrentPlayingRollSoundsHashSet.Remove(sound);
              soundableBallWorkData._CurrentPlayingRollSounds.Remove(sound);
              sound.volume = 0;
              sound.pitch = 1;
            }
          }
        };
      
        //添加速度计组件，以计算滚动声音相关效果
        soundableBallWorkData._SpeedMeter = rigidbody.gameObject.GetComponent<SpeedMeter>();
        if (soundableBallWorkData._SpeedMeter == null)
          soundableBallWorkData._SpeedMeter = rigidbody.gameObject.AddComponent<SpeedMeter>();
        soundableBallWorkData._SpeedMeter.Enabled = true;
        soundableBallWorkData._SpeedMeter.Callback = (meter) => {
          HandlerBallRollSpeedChange(meter.NowAbsoluteSpeed, _SoundConfig, soundableBallWorkData);
        };
      }
    }
    /// <summary>
    /// 移除指定物理刚体添加声音处理函数
    /// </summary>
    public void RemoveSoundableBall(PhysicsObject rigidbody, Ball.BallSoundConfig _SoundConfig) 
    {
      SoundableBallWorkData soundableBallWorkData = InitSoundableBallWorkData(rigidbody.gameObject.GetInstanceID(), _SoundConfig);

      //移除回调
      rigidbody.OnPhysicsCollDetection = null;
      rigidbody.OnPhysicsContactOn = null;
      rigidbody.OnPhysicsContactOff = null;
      if (rigidbody.IsPhysicalized) {
        rigidbody.DeleteAllCollDetection();
        rigidbody.DeleteAllContractDetection();
        rigidbody.DisableCollisionDetection();
      }
      rigidbody.EnableCollisionEvent = false;
      //删除速度计组件
      if (soundableBallWorkData._SpeedMeter != null) {
        Object.Destroy(soundableBallWorkData._SpeedMeter);
        soundableBallWorkData._SpeedMeter = null;
      }
      //移除声音层工具侦听
      var node = soundableBallWorkData._CurrentPlayingRollSounds.First;
      while(node != null) {
        var value = node.Value;
        value.volume = 0;
        value.pitch = 0;

        node = node.Next;
      }
      soundableBallWorkData._CurrentPlayingRollSounds.Clear();
      soundableBallWorkDatas.Remove(rigidbody.gameObject.GetInstanceID());
    }
    /// <summary>
    /// 强制停止球的所有声音
    /// </summary>
    public void StopSoundableBallAllSound(PhysicsObject rigidbody, Ball.BallSoundConfig _SoundConfig)
    {
      SoundableBallWorkData soundableBallWorkData = InitSoundableBallWorkData(rigidbody.gameObject.GetInstanceID(), _SoundConfig);
      var node = soundableBallWorkData._CurrentPlayingRollSounds.First;
      while(node != null) {
        var value = node.Value;
        value.volume = 0;
        value.pitch = 1;
        node = node.Next;
      }
    }

    /// <summary>
    /// 滚动声音音量与速度处理
    /// </summary>
    private void HandlerBallRollSpeedChange(float speed, Ball.BallSoundConfig _SoundConfig, SoundableBallWorkData soundableBallWorkData)
    {
      float vol = Mathf.Min(1, _SoundConfig.RollSound.VolumeBase + BallRollSpeedVolumeCruve.Evaluate(speed / _SoundConfig.RollSoundSpeedReference));
      float pit = Mathf.Min(1, _SoundConfig.RollSound.PitchBase + (speed * _SoundConfig.RollSound.PitchFactor));

      if (GameManager.DebugMode)
        _SoundConfig._SoundManagerDebugString.Clear();

      //将音量设置到正在播放的声音中
      var node = soundableBallWorkData._CurrentPlayingRollSounds.First;
      while(node != null) {
        var value = node.Value;

        if (GameManager.DebugMode)
          _SoundConfig._SoundManagerDebugString.AppendLine(string.Format("> [{0}] {1:F2} {2:F2}", value.name, vol, pit));

        value.volume = vol;
        value.pitch = pit;

        node = node.Next;
      }
    }
  
    #endregion
  }
}