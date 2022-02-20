using UnityEngine;

/*
 * Copyright (c) 2021  mengyu
 * 
 * 模块名：     
 * LookAt.cs
 * 
 * 用途：
 * 物体看物体工具脚本。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game
{
  [LuaApiDescription("物体看物体工具脚本。")]
  [SLua.CustomLuaClass]
  public class LookAt : MonoBehaviour {
    
    [LuaApiDescription("目标物体")]
    [Tooltip("目标物体")]
    public Transform Target;
    [LuaApiDescription("是否更新X")]
    [Tooltip("是否更新X")]
    public bool EnableX = true;
    [LuaApiDescription("是否更新Y")]
    [Tooltip("是否更新Y")]
    public bool EnableY = true;
    [LuaApiDescription("是否更新Z")]
    [Tooltip("是否更新Z")]
    public bool EnableZ = true;

    private void Update() {
      if(Target != null) {
        if(EnableX && EnableY && EnableZ)
          transform.LookAt(Target);
        else {
          Vector3 old = transform.eulerAngles;
          transform.LookAt(Target);

          if(EnableX) old.x = transform.eulerAngles.x;
          if(EnableY) old.y = transform.eulerAngles.y;
          if(EnableZ) old.z = transform.eulerAngles.z;

          transform.eulerAngles = old;
        }
      }
    }
  }
}