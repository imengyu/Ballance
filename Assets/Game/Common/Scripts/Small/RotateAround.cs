using Ballance2.Services;
using UnityEngine;
using UnityEngine.Animations;

/*
 * Copyright (c) 2023  mengyu
 * 
 * 模块名：     
 * CamFollow.cs
 * 
 * 用途：
 * 物体绕指定对象轴旋转脚本
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game
{
  /// <summary>
  /// 物体绕指定对象轴旋转脚本
  /// </summary>
  public class RotateAround : MonoBehaviour
  {
    public float Speed = 0.1f;
    public Vector3 Axis = Vector3.up;
    public Transform Around = null;
    public bool Rotate = true;

    private void Update() {
      if (Rotate && Around != null)
        transform.RotateAround(Around.position, Axis, Time.deltaTime * this.Speed);
    }
  } 
}