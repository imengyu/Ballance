using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * SmoothFly.cs
 * 
 * 用途：
 * 平滑移动脚本，可将物体平滑移动至指定目标。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game.Utils
{    
  [SLua.CustomLuaClass]
  public enum SmoothFlyType {
    [LuaApiDescription("SmoothDamp")]
    SmoothDamp,
    [LuaApiDescription("直接移动")]
    Lerp
  }
  [LuaApiDescription("平滑移动脚本")]
  [SLua.CustomLuaClass]
  public class SmoothFly : MonoBehaviour {

    [SLua.CustomLuaClass]
    public delegate void CallbackDelegate(SmoothFly fly);

    [LuaApiDescription("平滑移动目标变换")]
    public Transform TargetTransform;
    [LuaApiDescription("平滑移动目标位置")]
    public Vector3 TargetPos = Vector3.zero;
    [LuaApiDescription("是否开启平滑移动")]
    public bool Fly = false;
    [LuaApiDescription("平滑移动时间")]
    public float Time = 1.0f;
    [LuaApiDescription("最大速度")]
    public float MaxSpeed = 0;
    [LuaApiDescription("当前的速度")]
    public Vector3 CurrentVelocity = Vector3.zero;
    [LuaApiDescription("达到目标时停止")]
    public bool StopWhenArrival = false;
    [LuaApiDescription("达到目标判断阈值")]
    public float ArrivalDiatance = 0.1f;
    [LuaApiDescription("达到目标时回调")]
    public CallbackDelegate ArrivalCallback;
    [LuaApiDescription("飞行类型")]
    public SmoothFlyType Type = SmoothFlyType.SmoothDamp;

    private float LerpTick = 0;
    private Vector3 LerpStart = Vector3.zero;
    private Vector3 LerpEnd = Vector3.zero;
    
    private void Update() {
      if(Fly) {
        if(Type == SmoothFlyType.SmoothDamp) {
          var targetPos = GetPos();
          if(MaxSpeed <= 0)
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref CurrentVelocity, Time);
          else
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref CurrentVelocity, Time, MaxSpeed);

          if((transform.position - targetPos).sqrMagnitude < ArrivalDiatance) 
            Arrival();
          
        } else if(Type == SmoothFlyType.Lerp) {
          if(LerpTick == 0) {
            LerpStart = transform.position;
            LerpEnd = GetPos();
          }
          if(LerpTick <= Time){
            transform.position = Vector3.Lerp(LerpStart,LerpEnd , LerpTick);
            LerpTick += UnityEngine.Time.deltaTime;
          } else {
            LerpTick = 0;
            Arrival();
          }
        }
      }
    }
    private Vector3 GetPos() {
      return TargetTransform != null ? TargetTransform.position : TargetPos;
    }
    private void Arrival() {
      if(StopWhenArrival) Fly = false;
      ArrivalCallback?.Invoke(this);
    }
  }
}