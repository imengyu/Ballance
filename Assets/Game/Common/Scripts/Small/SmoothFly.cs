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
  /// <summary>
  /// 指定 平滑移动脚本 SmoothFly 平滑移动的类型
  /// </summary>
  public enum SmoothFlyType {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    SmoothDamp,
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Lerp
  }
  /// <summary>
  /// 平滑移动脚本，可将物体平滑移动至指定目标
  /// </summary>
  /// <remarks>
  /// 你可以先在需要平滑移动的物体上绑定此脚本，然后设置平滑移动目标，开启，随后在 `ArrivalCallback` 中就可以接收移动完成事件，例如：
  /// ```csharp
  /// var smoothFly = gameObject.AddComponent(SmoothFly);
  /// smoothFly.TargetTransform = 移动目标;
  /// smoothFly.Fly = true; //开启移动
  /// smoothFly.ArrivalDiatance = () => {
  ///   --移动结束时执行某些功能
  /// };
  /// ```
  /// </remarks>
  public class SmoothFly : MonoBehaviour {

    public delegate void CallbackDelegate(SmoothFly fly);

    /// <summary>
    /// 设置平滑移动目标变换
    /// </summary>
    [Tooltip("设置平滑移动目标变换")]
    public Transform TargetTransform;
    /// <summary>
    /// 设置平滑移动目标位置
    /// </summary>
    [Tooltip("设置平滑移动目标位置")]
    public Vector3 TargetPos = Vector3.zero;
    /// <summary>
    /// 设置是否开启平滑移动
    /// </summary>
    [Tooltip("设置是否开启平滑移动")]
    public bool Fly = false;
    /// <summary>
    /// 设置平滑移动时间（秒）
    /// </summary>
    [Tooltip("设置平滑移动时间（秒）")]
    public float Time = 1.0f;
    /// <summary>
    /// 设置最大速度
    /// </summary>
    [Tooltip("设置最大速度")]
    public float MaxSpeed = 0;
    /// <summary>
    /// 获取当前的速度
    /// </summary>
    [Tooltip("获取当前的速度")]
    public Vector3 CurrentVelocity = Vector3.zero;
    /// <summary>
    /// 设置是否达到目标时停止
    /// </summary>
    [Tooltip("设置是否达到目标时停止")]
    public bool StopWhenArrival = false;
    /// <summary>
    /// 设置达到目标判断阈值
    /// </summary>
    [Tooltip("设置达到目标判断阈值")]
    public float ArrivalDiatance = 0.1f;
    /// <summary>
    /// 设置达到目标时的事件回调
    /// </summary>
    public CallbackDelegate ArrivalCallback;
    /// <summary>
    /// 设置平滑移动的类型
    /// </summary>
    [Tooltip("设置平滑移动的类型")]
    public SmoothFlyType Type = SmoothFlyType.SmoothDamp;

    private float LerpTick = 0;
    private Vector3 LerpStart = Vector3.zero;
    private Vector3 LerpEnd = Vector3.zero;
    
    private void FixedUpdate() {
      if(Fly) {
        switch (Type) {
          case SmoothFlyType.SmoothDamp: {
            var targetPos = GetPos();
            if(MaxSpeed <= 0)
              transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref CurrentVelocity, Time);
            else
              transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref CurrentVelocity, Time, MaxSpeed);

            if((transform.position - targetPos).sqrMagnitude < ArrivalDiatance) 
              Arrival();
            break;
          } 
          case SmoothFlyType.Lerp: {
            if(LerpTick == 0) {
              LerpStart = transform.position;
            }
            if(LerpTick <= Time){
              transform.position = Vector3.Lerp(LerpStart, GetPos(), LerpTick);
              LerpTick += UnityEngine.Time.deltaTime;
            } else {
              LerpTick = 0;
              Arrival();
            }
            break;
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