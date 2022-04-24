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
  [LuaApiDescription("指定 平滑移动脚本 SmoothFly 平滑移动的类型")]
  public enum SmoothFlyType {
    [LuaApiDescription("使用 SmoothDamp 函数平滑移动")]
    SmoothDamp,
    [LuaApiDescription("使用 Lerp 曲线直接移动")]
    Lerp
  }
  [SLua.CustomLuaClass]
  [LuaApiDescription("平滑移动脚本")]
  [LuaApiNotes("平滑移动脚本，可将物体平滑移动至指定目标", @"
你可以先在需要平滑移动的物体上绑定此脚本，然后设置平滑移动目标，开启，随后在 `ArrivalCallback` 中就可以接收移动完成事件，例如：
```lua
local smoothFly = gameObject:AddComponent(SmoothFly)
smoothFly.TargetTransform = 移动目标
smoothFly.Fly = true --开启移动
smoothFly.ArrivalDiatance = function() 
  --移动结束时执行某些功能
end
```
")]
  public class SmoothFly : MonoBehaviour {

    [SLua.CustomLuaClass]
    public delegate void CallbackDelegate(SmoothFly fly);

    [LuaApiDescription("设置平滑移动目标变换")]
    [Tooltip("设置平滑移动目标变换")]
    public Transform TargetTransform;
    [LuaApiDescription("设置平滑移动目标位置")]
    [Tooltip("设置平滑移动目标位置")]
    public Vector3 TargetPos = Vector3.zero;
    [LuaApiDescription("设置是否开启平滑移动")]
    [Tooltip("设置是否开启平滑移动")]
    public bool Fly = false;
    [LuaApiDescription("设置平滑移动时间（秒）")]
    [Tooltip("设置平滑移动时间（秒）")]
    public float Time = 1.0f;
    [LuaApiDescription("设置最大速度")]
    [Tooltip("设置最大速度")]
    public float MaxSpeed = 0;
    [LuaApiDescription("获取当前的速度")]
    [Tooltip("获取当前的速度")]
    public Vector3 CurrentVelocity = Vector3.zero;
    [LuaApiDescription("设置是否达到目标时停止")]
    [Tooltip("设置是否达到目标时停止")]
    public bool StopWhenArrival = false;
    [LuaApiDescription("设置达到目标判断阈值")]
    [Tooltip("设置达到目标判断阈值")]
    public float ArrivalDiatance = 0.1f;
    [LuaApiDescription("设置达到目标时的事件回调")]
    public CallbackDelegate ArrivalCallback;
    [LuaApiDescription("设置平滑移动的类型")]
    [Tooltip("设置平滑移动的类型")]
    public SmoothFlyType Type = SmoothFlyType.SmoothDamp;

    private float LerpTick = 0;
    private Vector3 LerpStart = Vector3.zero;
    private Vector3 LerpEnd = Vector3.zero;
    
    private void FixedUpdate() {
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