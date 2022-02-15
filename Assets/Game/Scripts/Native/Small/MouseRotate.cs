using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ballance2.Game
{
  [SLua.CustomLuaClass]
  [LuaApiDescription("鼠标旋转物体小脚本")]
  public class MouseRotate : MonoBehaviour
  {
    private float OffsetX = 0;
    private float OffsetY = 0;

    [Tooltip("旋转速度")]
    public float Speed = 6f;//旋转速度
    [Tooltip("是否可以旋转X轴")]
    public bool CanRotateX = true;
    [Tooltip("是否可以旋转Y轴")]
    public bool CanRotateY = true;

    void Update()
    {
      if (Input.GetMouseButton(0))
      {
        if(CanRotateX)
          OffsetX = Input.GetAxis("Mouse X");
        if(CanRotateY)
          OffsetY= Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(OffsetY, -OffsetX, 0) * Speed, Space.World);
      }
    }
  }
}
