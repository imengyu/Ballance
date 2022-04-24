using System.Collections;
using System.Collections.Generic;
using Ballance2.Services.InputManager;
using SLua;
using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * DistanceChecker.cs
 * 
 * 用途：
 * 距离检查器，用于测量两个物体的距离，以在指定范围内触发自定义事件。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game
{
  [CustomLuaClass]
  [LuaApiDescription("距离检查器")]
  [LuaApiNotes("距离检查器，用于测量两个物体的距离，以在指定范围内触发自定义事件。")]
  public class DistanceChecker : MonoBehaviour
  {
    [Tooltip("物体1")]
    [LuaApiDescription("物体1")]
    public Transform Object1;

    [Tooltip("物体2")]
    [LuaApiDescription("物体2")]
    public Transform Object2;

    [Tooltip("检查距离")]
    [LuaApiDescription("检查距离")]
    public float Diatance;

    [LuaApiDescription("是否开启检查")]
    public bool CheckEnabled = false;
    [LuaApiDescription("每隔多少Tick进行一次检查")]
    public int CheckTickMin = 60;
    [LuaApiDescription("最大检查Tick")]
    public int CheckTickMax = 300;

    [LuaApiDescription("进入范围事件")]
    public GameObjectDelegate OnEnterRange;
    [LuaApiDescription("离开范围事件")]
    public GameObjectDelegate OnLeaveRange;

    private int CheckTickNow = 0;
    private bool LastEnter = false;

    void FixedUpdate()
    {
      if(CheckTickNow > 0) 
        CheckTickNow--;
      else {
        if(CheckEnabled && Object1 != null && Object2 != null) {
          var distance = (Object1.position - Object2.position).sqrMagnitude;
          if(distance <= Diatance) {
            if(!LastEnter) {
              OnEnterRange?.Invoke(gameObject);
              LastEnter = true;
            }
            CheckTickNow = CheckTickMin;
          } else {
            if(LastEnter) {
              OnLeaveRange?.Invoke(gameObject);
              LastEnter = false;
            }
            CheckTickNow = (int)Mathf.Lerp((float)CheckTickMin, (float)CheckTickMax, ((distance - Diatance) / (float)Diatance));
          }
        }
      }
    }
  }

}
