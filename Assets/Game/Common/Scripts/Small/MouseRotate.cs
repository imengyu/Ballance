using Ballance2.Utils;
using UnityEngine;

namespace Ballance2.Game
{
  /// <summary>
  /// 鼠标旋转物体小脚本.此脚本功能是让鼠标按住并旋转物体。
  /// </summary>
  public class MouseRotate : MonoBehaviour
  {
    private float OffsetX = 0;
    private float OffsetY = 0;

    public Camera Camera;
    /// <summary>
    /// 是否可以缩放
    /// </summary>
    [Tooltip("是否可以缩放")]
    public bool CanScale = false;
    public float ScaleMinZ = -1000f;
    public float ScaleMaxZ = -0.01f;

    /// <summary>
    /// 缩放速度
    /// </summary>
    [Tooltip("缩放速度")]
    public float ScaleSpeed = 6f;
    /// <summary>
    /// 旋转速度
    /// </summary>
    [Tooltip("旋转速度")]
    public float Speed = 6f;//旋转速度
    /// <summary>
    /// 指定是否可以旋转X轴
    /// </summary>
    [Tooltip("指定是否可以旋转X轴")]
    public bool CanRotateX = true;
    /// <summary>
    /// 指定是否可以旋转Y轴
    /// </summary>
    [Tooltip("指定是否可以旋转Y轴")]
    public bool CanRotateY = true;

    void Update()
    {
      if (Input.GetMouseButton(0))
      {
        if (CanRotateX)
          OffsetX = Input.GetAxis("Mouse X");
        if (CanRotateY)
          OffsetY = Input.GetAxis("Mouse Y");
        transform.Rotate(new Vector3(OffsetY, -OffsetX, 0) * Speed, Space.World);
      }
      if (CanScale)
      {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        float speed = ScaleSpeed;
        float z = Mathf.Abs(Camera.transform.localPosition.z);
        if (z < 1)
          speed = ScaleSpeed / 10000;
        if (z < 5)
          speed = ScaleSpeed / 1000;
        if (z < 8)
          speed = ScaleSpeed / 100;
        if (z < 10)
          speed = ScaleSpeed / 10;
        else if (z > 100)
          speed = ScaleSpeed * 10;

        if (scrollWheelInput > 0f)
          Camera.transform.localPosition = new Vector3(0, 0, CommonUtils.LimitNumber(Camera.transform.localPosition.z + speed, ScaleMinZ, ScaleMaxZ));
        else if (scrollWheelInput < 0f)
          Camera.transform.localPosition = new Vector3(0, 0, CommonUtils.LimitNumber(Camera.transform.localPosition.z - speed, ScaleMinZ, ScaleMaxZ));

      }
    }
  }
}
