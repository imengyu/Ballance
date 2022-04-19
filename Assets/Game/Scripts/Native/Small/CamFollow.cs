using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * CamFollow.cs
 * 
 * 用途：
 * 摄像机跟随脚本，用于Ballance球摄像机跟随的核心内容。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game
{
  [LuaApiDescription("摄像机跟随脚本")]
  [LuaApiNotes("摄像机跟随脚本，用于Ballance球摄像机跟随的核心内容。")]
  [SLua.CustomLuaClass]
  public class CamFollow : MonoBehaviour
  {
    [LuaApiDescription("指定摄像机跟随球是否开启")]
    [Tooltip("指定摄像机跟随球是否开启")]

    public bool Follow = false;
    [LuaApiDescription("指定摄像机看着球是否开启")]
    [Tooltip("指定摄像机看着球是否开启")]
    public bool Look = false;

    [LuaApiDescription("指定当前跟踪的目标")]
    public Transform Target
    {
      get { return _Target; }
      set
      {
        _Target = value;
        if (_Target != null)
        {
          LookSmoothTarget.transform.position = _Target.position;
          transform.position = _Target.position;
        }
      }
    }
    [LuaApiDescription("指定当前平滑移动跟踪的目标")]
    public Transform LookSmoothTarget = null;

    [LuaApiDescription("指定当前跟踪的目标")]
    [Tooltip("指定当前跟踪的目标")]
    public Transform CameraTransform = null;

    [LuaApiDescription("摄像机平滑移动的时间")]
    [Tooltip("摄像机平滑移动的时间")]
    public float SmoothTime = 0.2f;

    [LuaApiDescription("摄像机平滑移动的时间")]
    [Tooltip("摄像机平滑移动的时间")]
    public float SmoothTimeY = 0.5f;

    [LuaApiDescription("摄像机查看跟随锚点平滑移动的时间")]
    [Tooltip("摄像机查看跟随锚点平滑移动的时间")]
    public float SmoothToTargetTime = 0.1f;

    [Tooltip("指定当前跟踪的目标")]
    [SerializeField]
    private Transform _Target = null;
    private float smoothyVelocity = 0;
    private Vector3 smoothTargetVelocity = Vector3.zero;
    private Vector3 cameraVelocity = Vector3.zero;

    private void FixedUpdate()
    {
      if (Target != null)
      {
        if (Look)
        {
          LookSmoothTarget.transform.position = Vector3.SmoothDamp(
            LookSmoothTarget.transform.position, 
            Target.position, 
            ref smoothTargetVelocity, 
            SmoothToTargetTime);

          if (CameraTransform != null)
            CameraTransform.LookAt(LookSmoothTarget);
        }
        if (Follow)
        {
          var v = Vector3.SmoothDamp(transform.position, Target.position, ref cameraVelocity, SmoothTime);
          transform.position = new Vector3(
              v.x,
              Mathf.SmoothDamp(transform.position.y, Target.position.y, ref smoothyVelocity, SmoothTimeY),
              v.z);
        }
      }
    }
  }
}