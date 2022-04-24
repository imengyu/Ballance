using System.Collections;
using System.Collections.Generic;
using Ballance2.Services;
using SLua;
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
  [CustomLuaClass]
  [LuaApiDescription("速度计")]
  [LuaApiNotes("速度计组件，用于测量物体的移动速度。")]
  public class SpeedMeter : MonoBehaviour
  {
    [Tooltip("是否要开始测速")]
    [LuaApiDescription("是否要开始测速")]
    public bool Enabled {
      get { return _Enabled; }
      set {
        _Enabled = value;
        if(_Enabled) StartUpdate();
        else StopUpdate();
      }
    }
    [Tooltip("测速的目标，如果为空，则对当前物体测速")]
    [LuaApiDescription("测速的目标，如果为空，则对当前物体测速")]
    public Transform Target;
    [Tooltip("相对速度计算的最大值")]
    [LuaApiDescription("相对速度计算的最大值")]
    public float MaxSpeed = 1;
    [Tooltip("相对速度计算的最小值")]
    [LuaApiDescription("相对速度计算的最小值")]
    public float MinSpeed = 0;

    [Tooltip("相对速度 0-1")]
    [LuaApiDescription("相对速度 0-1, 即 (NowAbsoluteSpeed - MinSpeed) / (MaxSpeed - MinSpeed)")]
    public float NowRelativeSpeed;
    [Tooltip("绝对速度 m/s ")]
    [LuaApiDescription("绝对速度 m/s ")]
    public float NowAbsoluteSpeed;

    private bool LastPosIsNull = true;
    private Vector3 LastPos;
    private bool CheckOnce = false;
    private SpeedMeterDelegate CheckOnceCallback = null;
    private bool _Enabled = false;

    [LuaApiDescription("检查回调, 通过设置这个回调，可以每帧获取一次速度值")]
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

    private void _FixedUpdate() {
      if(Enabled && Target != null) {
        if(LastPosIsNull) {

          var nowPos = Target.position;
          var len = (nowPos - LastPos).sqrMagnitude / Time.fixedDeltaTime;

          NowAbsoluteSpeed = len;
          NowRelativeSpeed = (NowAbsoluteSpeed - MinSpeed) / (MaxSpeed - MinSpeed);

          if(Callback != null)
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
    
    [LuaApiDescription("手动调用检查一次速度")]
    [LuaApiParamDescription("callback", "速度计算回调")]
    public void TestOnce(SpeedMeterDelegate callback) {
      Enabled = true;
      LastPosIsNull = true;
      CheckOnce = true;
      CheckOnceCallback = callback;
    }
  }  
  [CustomLuaClass]
  public delegate void SpeedMeterDelegate(SpeedMeter meter);

}
