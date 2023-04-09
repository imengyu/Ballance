using Ballance2.Services;
using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * SpeedMeter.cs
 * 
 * 用途：
 * 速度计组件，用于测量物体的移动速度。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game
{
  /// <summary>
  /// 速度计组件，用于测量物体的移动速度。
  /// </summary>
  public class SpeedMeter : MonoBehaviour
  {
    [Tooltip("是否要开始测速")]
    /// <summary>
    /// 是否要开始测速
    /// </summary>
    /// <value></value>
    public bool Enabled {
      get { return _Enabled; }
      set {
        _Enabled = value;
        if(_Enabled) StartUpdate();
        else StopUpdate();
      }
    }
    [Tooltip("测速的目标，如果为空，则对当前物体测速")]
    /// <summary>
    /// 测速的目标，如果为空，则对当前物体测速
    /// </summary>
    /// <value></value>
    public Transform Target;
    [Tooltip("相对速度计算的最大值")]
    /// <summary>
    /// 相对速度计算的最大值
    /// </summary>
    /// <value></value>
    public float MaxSpeed = 1;
    [Tooltip("相对速度计算的最小值")]
    /// <summary>
    /// 相对速度计算的最小值
    /// </summary>
    /// <value></value>
    public float MinSpeed = 0;
    [Tooltip("相对速度计算的最小值")]
    /// <summary>
    /// 用于 Callback 延迟调用，每 FixedUpdateTick % CallbackDelayMod == 0 时调用一次 Callback
    /// </summary>
    /// <value></value>
    public int CallbackDelayMod = 26;

    [Tooltip("相对速度 0-1")]
    /// <summary>
    /// 相对速度 0-1, 即 (NowAbsoluteSpeed - MinSpeed) / (MaxSpeed - MinSpeed)
    /// </summary>
    /// <value></value>
    public float NowRelativeSpeed;
    [Tooltip("绝对速度 m/s ")]
    /// <summary>
    /// 绝对速度 m/s 
    /// /// </summary>
    /// <value></value>
    public float NowAbsoluteSpeed;

    private bool LastPosIsNull = true;
    private Vector3 LastPos;
    private bool CheckOnce = false;
    private SpeedMeterDelegate CheckOnceCallback = null;
    private bool _Enabled = false;

    /// <summary>
    /// 检查回调, 通过设置这个回调，可以每帧获取一次速度值
    /// </summary>
    public SpeedMeterDelegate Callback = null;

    private void Start() {
      if(Target == null)
        Target = transform;
      if(_Enabled) 
        StartUpdate();
    }
    private void OnDestroy() {
      StopUpdate();
    }

    private GameTimeMachine.GameTimeMachineTimeTicket fixedUpdateTicket = null;
    
    private void StartUpdate() {
      if (fixedUpdateTicket == null)
        fixedUpdateTicket = GameManager.GameTimeMachine.RegisterFixedUpdate(_FixedUpdate, 30);
    }
    private void StopUpdate() {
      if (fixedUpdateTicket != null)
      {
        fixedUpdateTicket.Unregister();
        fixedUpdateTicket = null;
      }
    }
    private void OnEnable() {
      if (fixedUpdateTicket != null) fixedUpdateTicket.Enable();
    }
    private void OnDisable() {
      if (fixedUpdateTicket != null) fixedUpdateTicket.Disable();
    }

    private void _FixedUpdate() {
      if(Enabled && Target != null) {
        if(LastPosIsNull) {

          var nowPos = Target.position;
          var len = (nowPos - LastPos).sqrMagnitude / Time.fixedDeltaTime;

          NowAbsoluteSpeed = len;
          NowRelativeSpeed = (NowAbsoluteSpeed - MinSpeed) / (MaxSpeed - MinSpeed);

          if(Callback != null && GameTimeMachine.FixedUpdateTick % CallbackDelayMod == 0)
            Callback.Invoke(this);
          if(CheckOnce) {
            CheckOnceCallback?.Invoke(this);
            CheckOnce = false;
            Enabled = false;
          }
          
          LastPos = nowPos;
        } else {
          LastPosIsNull = false;
          LastPos = Target.position;
        }
      }
    }
    
    /// <summary>
    /// 手动调用检查一次速度
    /// </summary>
    /// <param name="callback">速度计算回调</param>
    public void TestOnce(SpeedMeterDelegate callback) {
      Enabled = true;
      LastPosIsNull = true;
      CheckOnce = true;
      CheckOnceCallback = callback;
    }
  }  

  public delegate void SpeedMeterDelegate(SpeedMeter meter);

}
