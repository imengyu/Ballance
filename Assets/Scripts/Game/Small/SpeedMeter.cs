using System.Collections;
using System.Collections.Generic;
using Ballance2.LuaHelpers;
using SLua;
using UnityEngine;

namespace Ballance2.Game
{
  [CustomLuaClass]
  [LuaApiDescription("速度计")]
  public class SpeedMeter : MonoBehaviour
  {
    [LuaApiDescription("是否要开始测速")]
    public bool Enabled = true;
    [LuaApiDescription("目标")]
    public Transform Target;
    [LuaApiDescription("相对速度计算的最大值")]
    public float MaxSpeed = 1;
    [LuaApiDescription("相对速度计算的最小值")]
    public float MinSpeed = 0;

    [LuaApiDescription("相对速度 0-1")]
    public float NowRelativeSpeed;
    [LuaApiDescription("绝对速度 m/s ")]
    public float NowAbsoluteSpeed;

    private bool LastPosIsNull = true;
    private Vector3 LastPos;
    private bool CheckOnce = false;
    private SpeedMeterDelegate CheckOnceCallback = null;

    private void Start() {
      if(Target == null)
        Target = transform;
    }

    private void FixedUpdate() {
      if(Enabled && Target != null) {
        if(LastPosIsNull) {
          var nowPos = Target.position;
          var len = (nowPos - LastPos).sqrMagnitude;

          NowAbsoluteSpeed = len;
          NowRelativeSpeed = (NowAbsoluteSpeed - MinSpeed) / (MaxSpeed - MinSpeed);

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
    
    [LuaApiDescription("检查一次")]
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
