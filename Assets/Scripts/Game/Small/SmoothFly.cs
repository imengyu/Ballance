using Ballance2.LuaHelpers;
using UnityEngine;

namespace Ballance2.Game.Utils
{
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
    public float MaxSpeed = 10000;
    [LuaApiDescription("当前的速度")]
    public Vector3 CurrentVelocity = Vector3.zero;
    [LuaApiDescription("达到目标时停止")]
    public bool StopWhenArrival = false;
    [LuaApiDescription("达到目标判断阈值")]
    public float ArrivalDiatance = 0.1f;
    [LuaApiDescription("达到目标时回调")]
    public CallbackDelegate ArrivalCallback;
    
    private void Update() {
      if(Fly) {
        if(TargetTransform != null) {
          transform.position = Vector3.SmoothDamp(transform.position, TargetTransform.position, ref CurrentVelocity, Time, MaxSpeed);
          if((transform.position - TargetTransform.position).sqrMagnitude < ArrivalDiatance) Arrival();
        }
        else {
          transform.position = Vector3.SmoothDamp(transform.position, TargetPos, ref CurrentVelocity, Time, MaxSpeed);
          if((transform.position - TargetPos).sqrMagnitude < ArrivalDiatance) Arrival();
        }
      }
    }
    private void Arrival() {
      if(StopWhenArrival) Fly = false;
      ArrivalCallback?.Invoke(this);
    }
  }
}